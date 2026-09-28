Shader "Shallow Water/Hedge"
{
    Properties
    {
        _Colour ("Leaf colour", Color) = (0.2, 0.3, 0.11, 1)
        _Autumn ("Turned leaves", Color) = (0.45, 0.35, 0.12, 1)
        _Gaps ("Shade between the leaves", Color) = (0.06, 0.08, 0.04, 1)
        _Berries ("Berries", Color) = (0.55, 0.07, 0.05, 1)
        _BerryAmount ("Berry amount", Range(0, 0.3)) = 0.08
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.5
        #include "Noise.cginc"

        #define CLUMPS_PER_METRE 3.1
        #define LEAVES_PER_METRE 11.0
        #define BERRIES_PER_METRE 14.0
        #define TURNING_PER_METRE 0.6

        fixed4 _Colour;
        fixed4 _Autumn;
        fixed4 _Gaps;
        fixed4 _Berries;
        float _BerryAmount;

        struct Input
        {
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 place = IN.worldPos;
            float detail = PatternDetail(place.xz * LEAVES_PER_METRE + place.y * LEAVES_PER_METRE);
            float clumps = ValueNoise3(place * CLUMPS_PER_METRE);
            float leaves = lerp(0.5, ValueNoise3(place * LEAVES_PER_METRE), detail);
            float turning = smoothstep(0.45, 0.8, ValueNoise3(place * TURNING_PER_METRE));
            float3 leaf = lerp(_Colour.rgb, _Autumn.rgb, turning) * (0.75 + 0.5 * leaves);
            float isLeafy = smoothstep(0.25, 0.6, clumps);
            float3 colour = lerp(_Gaps.rgb, leaf, lerp(0.75, isLeafy, detail));
            float isBerry = step(1.0 - _BerryAmount, Hash31(floor(place * BERRIES_PER_METRE))) * isLeafy * detail;
            o.Albedo = lerp(colour, _Berries.rgb, isBerry);
            o.Occlusion = lerp(0.55, 1.0, isLeafy);
            o.Smoothness = 0.08 + isBerry * 0.5;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
