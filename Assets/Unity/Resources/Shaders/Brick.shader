Shader "Shallow Water/Brick"
{
    Properties
    {
        _Colour ("Brick colour", Color) = (0.52, 0.29, 0.21, 1)
        _Variation ("Second brick colour", Color) = (0.43, 0.22, 0.16, 1)
        _Mortar ("Lime mortar", Color) = (0.6, 0.56, 0.49, 1)
        _Headers ("Burnt headers", Color) = (0.27, 0.2, 0.22, 1)
        _Plinth ("Plinth", Color) = (0.17, 0.17, 0.2, 1)
        _PlinthTop ("Top of the plinth, world height", Float) = -100
        _Limewash ("Limewash", Color) = (0.86, 0.84, 0.76, 1)
        _IsLimewashed ("Limewashed", Float) = 0
        _IsArch ("Bricks on end, for arches", Float) = 0
        _Soot ("Soot", Color) = (0.12, 0.11, 0.1, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.5
        #include "Noise.cginc"
        #include "Relief.cginc"
        #include "Bond.cginc"

        fixed4 _Colour;
        fixed4 _Variation;
        fixed4 _Mortar;
        fixed4 _Headers;
        fixed4 _Plinth;
        float _PlinthTop;
        fixed4 _Limewash;
        float _IsLimewashed;
        float _IsArch;
        fixed4 _Soot;

        struct Input
        {
            float3 worldPos;
            float3 worldNormal;
            INTERNAL_DATA
        };

        Laid LaidAt(float3 position, float3 normal, bool isPlinth)
        {
            bool isLevel = abs(normal.y) > 0.5;
            float2 wall = isLevel ? position.xz : float2(AlongTheFace(position, normal), position.y);
            if (_IsArch > 0.5) return Soldiers(wall);
            if (isPlinth || isLevel) return Stretchers(wall);
            return Flemish(wall);
        }

        float3 BrickFace(Laid laid, bool isPlinth)
        {
            float3 brick = lerp(_Colour.rgb, _Variation.rgb, laid.id);
            float burnt = laid.isHeader * step(0.35, frac(laid.id * 7.31));
            brick = lerp(brick, _Headers.rgb, burnt * 0.8);
            brick = isPlinth ? _Plinth.rgb * (0.85 + 0.3 * laid.id) : brick;
            brick *= 0.86 + 0.26 * ValueNoise(laid.place * float2(3.0, 1.5));
            return laid.isJoint > 0.5 ? _Mortar.rgb : brick;
        }

        float3 Weathered(float3 colour, float3 position, float3 normal)
        {
            float streaks = ValueNoise(float2(AlongTheFace(position, normal) * 2.5, position.y * 0.25));
            float soot = smoothstep(0.55, 0.95, streaks) * 0.35 + Fbm(position.xz * 0.6 + position.y * 0.2) * 0.15;
            float damp = 1.0 - smoothstep(_PlinthTop, _PlinthTop + 0.5, position.y);
            colour = lerp(colour, _Soot.rgb, soot);
            return colour * lerp(1.0, 0.78, damp * step(-50.0, _PlinthTop));
        }

        float3 Limewashed(float3 brick, float3 position, bool isPlinth)
        {
            float flaking = smoothstep(0.62, 0.7, Fbm(position.xy * 1.7 + position.zy * 1.3));
            float grime = Fbm(position.xz * 0.9 + position.y * 0.4);
            float3 wash = _Limewash.rgb * lerp(0.82, 1.0, grime);
            float3 washed = lerp(wash, brick, flaking * 0.85);
            return isPlinth ? _Plinth.rgb : washed;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 normal = VERTEX_NORMAL(IN);
            bool isPlinth = IN.worldPos.y < _PlinthTop;
            Laid laid = LaidAt(IN.worldPos, normal, isPlinth);
            float detail = PatternDetail(laid.place);
            float3 average = lerp(lerp(_Colour.rgb, _Variation.rgb, 0.5), _Mortar.rgb, 0.2);
            average = isPlinth ? _Plinth.rgb : average;
            float3 brick = lerp(average, BrickFace(laid, isPlinth), detail);
            brick = _IsLimewashed > 0.5 ? Limewashed(brick, IN.worldPos, isPlinth) : brick;
            o.Albedo = Weathered(brick, IN.worldPos, normal);
            o.Smoothness = laid.isJoint > 0.5 ? 0.05 : 0.14;
            float washedOver = _IsLimewashed > 0.5 ? 0.4 : 1.0;
            float relief = ((laid.isJoint > 0.5 ? -0.007 : 0.0) + ValueNoise(laid.place * 4.0) * 0.002) * detail * washedOver;
            o.Normal = WORLD_TO_TANGENT(IN, Raised(IN.worldPos, normal, relief));
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
