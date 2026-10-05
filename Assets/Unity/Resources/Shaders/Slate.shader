Shader "Shallow Water/Slate"
{
    Properties
    {
        _Colour ("Slate or tile colour", Color) = (0.25, 0.26, 0.29, 1)
        _Variation ("Second colour", Color) = (0.2, 0.21, 0.25, 1)
        _Moss ("Moss and lichen", Color) = (0.38, 0.4, 0.2, 1)
        _MossAmount ("Moss amount", Range(0, 1)) = 0.5
        _Smoothness ("Smoothness", Range(0, 1)) = 0.3
        _UnitWidth ("Width of a slate or tile, in metres", Float) = 0.3
        _Gauge ("Course up the slope, in metres", Float) = 0.22
        _Lap ("Step at each lap, in metres", Float) = 0.006
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.5
        #include "Noise.cginc"
        #include "Relief.cginc"

        #define JOINT_WIDTH 0.06
        #define LAP_SHADOW_FROM 0.78
        #define MOSS_REACH_UP_THE_SLOPE 4.0

        fixed4 _Colour;
        fixed4 _Variation;
        fixed4 _Moss;
        float _MossAmount;
        float _Smoothness;
        float _UnitWidth;
        float _Gauge;
        float _Lap;

        struct Input
        {
            float3 worldPos;
            float2 surfacePlace;
            float3 worldNormal;
            INTERNAL_DATA
        };

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.surfacePlace = v.texcoord.xy;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 roof = IN.surfacePlace;
            float row = floor(roof.y / _Gauge);
            float wobble = (Hash21(float2(row, 3.1)) - 0.5) * 0.12;
            float2 place = float2(roof.x / _UnitWidth + frac(row * 0.5) + wobble, roof.y / _Gauge);
            float2 inside = frac(place);
            float pick = Hash21(floor(place));
            float3 unit = lerp(_Colour.rgb, _Variation.rgb, pick) * (0.9 + 0.2 * Hash21(floor(place) + 9.3));
            float lapShadow = lerp(1.0, 0.6, smoothstep(LAP_SHADOW_FROM, 1.0, inside.y));
            float joint = inside.x < JOINT_WIDTH ? 0.55 : 1.0;
            float camber = 1.0 - 0.12 * pow(abs(inside.x - 0.5) * 2.0, 2.0);
            float3 laid = unit * lapShadow * joint * camber;
            float3 average = lerp(_Colour.rgb, _Variation.rgb, 0.5) * 0.86;
            float detail = PatternDetail(place);
            float3 colour = lerp(average, laid, detail);
            float mossPatches = smoothstep(0.58, 0.85, Fbm(IN.worldPos.xz * 0.6 + pick * 0.3));
            float nearTheEaves = 1.0 - saturate(roof.y / MOSS_REACH_UP_THE_SLOPE);
            float lichen = step(0.93, Hash21(floor(place) + 4.7)) * 0.5;
            float growth = saturate(mossPatches * (0.4 + nearTheEaves) + lichen) * _MossAmount;
            colour = lerp(colour, _Moss.rgb, growth * 0.7);
            float streaks = ValueNoise(float2(roof.x * 3.0, roof.y * 0.3));
            colour *= lerp(1.0, 0.8, smoothstep(0.6, 0.9, streaks));
            o.Albedo = colour;
            o.Smoothness = _Smoothness * (1.0 - growth);
            float relief = (_Lap * (1.0 - inside.y) - (inside.x < JOINT_WIDTH ? _Lap * 0.5 : 0.0) + (camber - 1.0) * _Lap) * detail;
            o.Normal = WORLD_TO_TANGENT(IN, Raised(IN.worldPos, VERTEX_NORMAL(IN), relief));
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
