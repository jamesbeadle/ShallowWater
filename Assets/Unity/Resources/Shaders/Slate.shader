Shader "Shallow Water/Slate"
{
    Properties
    {
        _Colour ("Slate colour", Color) = (0.26, 0.27, 0.3, 1)
        _Variation ("Second slate colour", Color) = (0.2, 0.21, 0.24, 1)
        _Moss ("Moss and lichen", Color) = (0.4, 0.42, 0.2, 1)
        _Smoothness ("Smoothness", Range(0, 1)) = 0.3
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

        #define SLATE_WIDTH 0.3
        #define ROW_UP_THE_SLOPE 0.22
        #define JOINT_WIDTH 0.03
        #define LAP_SHADOW_FROM 0.82
        #define MOSS_REACH_UP_THE_SLOPE 5.0

        fixed4 _Colour;
        fixed4 _Variation;
        fixed4 _Moss;
        float _Smoothness;

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
            float row = floor(roof.y / ROW_UP_THE_SLOPE);
            float2 place = float2(roof.x / SLATE_WIDTH + frac(row * 0.5), roof.y / ROW_UP_THE_SLOPE);
            float2 inside = frac(place);
            float3 slate = lerp(_Colour.rgb, _Variation.rgb, Hash21(floor(place)));
            float lapShadow = lerp(1.0, 0.62, smoothstep(LAP_SHADOW_FROM, 1.0, inside.y));
            float joint = inside.x < JOINT_WIDTH ? 0.55 : 1.0;
            float3 laid = slate * lapShadow * joint;
            float3 average = lerp(_Colour.rgb, _Variation.rgb, 0.5) * 0.88;
            float detail = PatternDetail(place);
            float3 colour = lerp(average, laid, detail);
            float mossPatches = smoothstep(0.62, 0.85, Fbm(IN.worldPos.xz * 0.6));
            float nearTheEaves = 1.0 - saturate(roof.y / MOSS_REACH_UP_THE_SLOPE);
            colour = lerp(colour, _Moss.rgb, mossPatches * nearTheEaves * 0.5);
            o.Albedo = colour;
            o.Smoothness = _Smoothness * (1.0 - mossPatches * 0.8);
            float relief = (0.006 * (1.0 - inside.y) - (inside.x < JOINT_WIDTH ? 0.003 : 0.0)) * detail;
            o.Normal = WORLD_TO_TANGENT(IN, Raised(IN.worldPos, VERTEX_NORMAL(IN), relief));
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
