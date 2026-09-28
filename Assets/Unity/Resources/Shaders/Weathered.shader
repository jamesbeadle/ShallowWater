Shader "Shallow Water/Weathered"
{
    Properties
    {
        _Colour ("Colour", Color) = (0.5, 0.5, 0.5, 1)
        _Worn ("Worn colour", Color) = (0.4, 0.4, 0.4, 1)
        _Grain ("Grain colour", Color) = (0.3, 0.3, 0.3, 1)
        _PatchMetres ("Patch size in metres", Float) = 6
        _GrainMetres ("Grain size in metres", Float) = 0.15
        _GrainAmount ("Grain amount", Range(0, 1)) = 0.3
        _Smoothness ("Smoothness", Range(0, 1)) = 0.1
        _Metallic ("Metallic", Range(0, 1)) = 0
        _Aboard ("Moves with its object", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows addshadow
        #pragma multi_compile_instancing
        #pragma target 3.5
        #include "Noise.cginc"

        fixed4 _Colour;
        fixed4 _Worn;
        fixed4 _Grain;
        float _PatchMetres;
        float _GrainMetres;
        float _GrainAmount;
        float _Smoothness;
        float _Metallic;
        float _Aboard;

        struct Input
        {
            float3 worldPos;
            float3 worldNormal;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 aboardPosition = mul(unity_WorldToObject, float4(IN.worldPos, 1.0)).xyz;
            float3 aboardNormal = mul((float3x3)unity_WorldToObject, IN.worldNormal);
            float3 position = lerp(IN.worldPos, aboardPosition, _Aboard);
            float3 normal = lerp(IN.worldNormal, aboardNormal, _Aboard);
            float2 place = FacePlace(position, normal);
            float patches = Fbm(place / _PatchMetres);
            float3 colour = lerp(_Colour.rgb, _Worn.rgb, smoothstep(0.3, 0.75, patches));
            float2 grainPlace = place / _GrainMetres;
            float grain = lerp(0.5, ValueNoise(grainPlace), PatternDetail(grainPlace));
            float2 finePlace = grainPlace * 3.7;
            float fine = lerp(0.5, ValueNoise(finePlace), PatternDetail(finePlace));
            colour = lerp(colour, _Grain.rgb, _GrainAmount * (grain * 0.7 + fine * 0.3));
            o.Albedo = colour;
            o.Smoothness = _Smoothness * lerp(0.6, 1.3, patches);
            o.Metallic = _Metallic;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
