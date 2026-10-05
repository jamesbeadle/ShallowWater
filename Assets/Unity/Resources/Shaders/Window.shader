Shader "Shallow Water/Window"
{
    Properties
    {
        _Color ("Seen only if this shader cannot run", Color) = (0.08, 0.09, 0.1, 1)
        _Paint0 ("Cream", Color) = (0.84, 0.79, 0.66, 1)
        _Paint1 ("White lead", Color) = (0.88, 0.87, 0.82, 1)
        _Paint2 ("Brunswick green", Color) = (0.11, 0.22, 0.15, 1)
        _Paint3 ("Brown", Color) = (0.28, 0.17, 0.1, 1)
        _Glass ("Glass", Color) = (0.09, 0.11, 0.13, 1)
        _Room ("The room behind", Color) = (0.05, 0.04, 0.035, 1)
        _Net ("Net curtains", Color) = (0.8, 0.79, 0.74, 1)
        _Curtain0 ("Curtains", Color) = (0.42, 0.16, 0.13, 1)
        _Curtain1 ("Other curtains", Color) = (0.3, 0.33, 0.22, 1)
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
        #include "Joinery.cginc"
        #include "Glazing.cginc"

        struct Input
        {
            float2 surfacePlace;
            float4 fitting;
            float3 worldPos;
            float3 worldNormal;
            INTERNAL_DATA
        };

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.surfacePlace = v.texcoord.xy;
            o.fitting = v.texcoord1;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 normal = VERTEX_NORMAL(IN);
            float2 place = IN.surfacePlace;
            float2 size = IN.fitting.xy;
            float sillHeight = IN.worldPos.y - place.y;
            Joinery window = Sashes(place, size, IN.fitting.z, IN.fitting.w, sillHeight);
            bool isGlass = window.smoothness > 0.8;
            float detail = PatternDetail(place * 20.0);
            o.Albedo = window.colour;
            o.Smoothness = window.smoothness;
            float3 bent = Raised(IN.worldPos, normal, window.relief * detail);
            o.Normal = WORLD_TO_TANGENT(IN, isGlass ? OldGlassNormal(bent, IN.worldPos) : bent);
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
