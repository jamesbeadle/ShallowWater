Shader "Shallow Water/Stone"
{
    Properties
    {
        _Colour ("Stone colour", Color) = (0.6, 0.6, 0.55, 1)
        _Variation ("Second stone colour", Color) = (0.5, 0.5, 0.45, 1)
        _Mortar ("Mortar colour", Color) = (0.4, 0.4, 0.37, 1)
        _StoneMetres ("Stone size in metres", Float) = 0.8
        _MortarWidth ("Mortar width, in stones", Range(0, 0.3)) = 0.06
        _Smoothness ("Smoothness", Range(0, 1)) = 0.15
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

        fixed4 _Colour;
        fixed4 _Variation;
        fixed4 _Mortar;
        float _StoneMetres;
        float _MortarWidth;
        float _Smoothness;

        struct Input
        {
            float3 worldPos;
            float3 worldNormal;
            INTERNAL_DATA
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 vertexNormal = VERTEX_NORMAL(IN);
            float2 place = FacePlace(IN.worldPos, vertexNormal) / _StoneMetres;
            float3 cells = Cells(place);
            float detail = PatternDetail(place);
            float3 averageStone = lerp(_Colour.rgb, _Variation.rgb, 0.5);
            float3 stone = lerp(_Colour.rgb, _Variation.rgb, cells.y);
            float2 mottlePlace = place * 6.3;
            float mottle = lerp(0.5, ValueNoise(mottlePlace), PatternDetail(mottlePlace));
            stone *= 0.82 + 0.36 * mottle;
            float isMortar = 1.0 - smoothstep(_MortarWidth, _MortarWidth * 1.8, cells.x);
            float3 colour = lerp(stone, _Mortar.rgb, isMortar);
            float dirt = Fbm(IN.worldPos.xz * 0.35);
            colour *= lerp(0.8, 1.05, dirt);
            o.Albedo = lerp(averageStone * lerp(0.8, 1.05, dirt), colour, detail);
            o.Smoothness = _Smoothness * (1.0 - isMortar * detail);
            float relief = (saturate(cells.x * 4.0) * 0.004 - isMortar * 0.008 + (mottle - 0.5) * 0.003) * detail;
            o.Normal = WORLD_TO_TANGENT(IN, Raised(IN.worldPos, vertexNormal, relief));
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
