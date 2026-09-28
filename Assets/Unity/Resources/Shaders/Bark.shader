Shader "Shallow Water/Bark"
{
    Properties
    {
        _Colour ("Bark", Color) = (0.36, 0.33, 0.28, 1)
        _Fissure ("Deep in the fissures", Color) = (0.12, 0.1, 0.08, 1)
        _Lichen ("Lichen and green algae", Color) = (0.42, 0.46, 0.34, 1)
        _Moss ("Moss at the foot", Color) = (0.2, 0.28, 0.08, 1)
        _Upper ("Upper bark", Color) = (0.36, 0.33, 0.28, 1)
        _Pattern ("Pattern: 0 furrowed, 1 netted, 2 papery, 3 plated", Float) = 0
        _FurrowsAround ("Furrows around a limb", Float) = 12
        _FurrowMetres ("Length of a plate", Float) = 0.6
        _Relief ("Depth of the fissures", Float) = 0.015
        _UpperFromMetres ("Where the upper bark begins", Float) = 100
        _FootMetres ("Height of the dark rough foot", Float) = 0
        _Lichens ("Lichen amount", Range(0, 1)) = 0.3
        _Sway ("Sway in the wind", Float) = 0.12
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert addshadow
        #pragma multi_compile_instancing
        #pragma target 3.5
        #include "Noise.cginc"
        #include "Relief.cginc"
        #include "Wind.cginc"
        #include "BarkPattern.cginc"

        #define AVERAGE_RIDGE 0.6
        #define MOSS_TOP_METRES 1.4
        #define UPPER_BLEND_METRES 2.0

        fixed4 _Colour;
        fixed4 _Fissure;
        fixed4 _Lichen;
        fixed4 _Moss;
        fixed4 _Upper;
        float _Pattern;
        float _FurrowsAround;
        float _FurrowMetres;
        float _Relief;
        float _UpperFromMetres;
        float _FootMetres;
        float _Lichens;
        float _Sway;

        struct Input
        {
            float2 surfacePlace;
            float3 treePlace;
            float3 worldPos;
            float3 worldNormal;
            INTERNAL_DATA
        };

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.surfacePlace = v.texcoord.xy;
            o.treePlace = v.vertex.xyz;
            v.vertex.xyz += Swayed(v.vertex.xyz, TreeSeed(), _Sway);
        }

        float3 BarkColour(float3 place, float ridge, float height)
        {
            float tone = 0.85 + 0.3 * ValueNoise3(place * 0.23 + 11.0);
            float3 bark = lerp(_Fissure.rgb, _Colour.rgb, ridge) * tone;
            float isRoughFoot = 1.0 - smoothstep(_FootMetres * 0.6, _FootMetres, height);
            bark = lerp(bark, lerp(_Fissure.rgb, _Fissure.rgb * 1.8, Furrowed(place * 1.3)), isRoughFoot);
            float isUpper = smoothstep(_UpperFromMetres - UPPER_BLEND_METRES, _UpperFromMetres + UPPER_BLEND_METRES, height);
            bark = lerp(bark, _Upper.rgb * tone * lerp(0.8, 1.1, ridge), isUpper);
            float lichen = smoothstep(0.55, 0.75, ValueNoise3(place * 0.5 + 5.0)) * _Lichens * ridge;
            return lerp(bark, _Lichen.rgb, lichen);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 place = AroundTheLimb(IN.surfacePlace.x, IN.surfacePlace.y, _FurrowsAround, _FurrowMetres);
            float height = IN.treePlace.y;
            float detail = PatternDetail(float2(place.z, IN.surfacePlace.y * _FurrowsAround / BARK_FULL_TURN));
            float ridge = lerp(AVERAGE_RIDGE, RidgeAt(place, _Pattern), detail);
            float3 vertexNormal = VERTEX_NORMAL(IN);
            float isMossy = (1.0 - smoothstep(0.1, MOSS_TOP_METRES, height)) * saturate(0.3 + vertexNormal.z * 0.7);
            float moss = isMossy * smoothstep(0.35, 0.65, ValueNoise3(place * 0.7 + 9.0));
            o.Albedo = lerp(BarkColour(place, ridge, height), _Moss.rgb, moss);
            o.Occlusion = lerp(0.55, 1.0, ridge);
            o.Smoothness = 0.08;
            o.Normal = WORLD_TO_TANGENT(IN, Raised(IN.worldPos, vertexNormal, ridge * _Relief));
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
