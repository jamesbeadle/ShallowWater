Shader "Shallow Water/Door"
{
    Properties
    {
        _Color ("Seen only if this shader cannot run", Color) = (0.08, 0.09, 0.1, 1)
        _Paint0 ("Brunswick green", Color) = (0.1, 0.22, 0.14, 1)
        _Paint1 ("Maroon", Color) = (0.36, 0.09, 0.09, 1)
        _Paint2 ("Black", Color) = (0.06, 0.06, 0.07, 1)
        _Paint3 ("Grained brown", Color) = (0.32, 0.19, 0.1, 1)
        _Trim ("Frame and fanlight", Color) = (0.84, 0.79, 0.66, 1)
        _Glass ("Glass", Color) = (0.09, 0.11, 0.13, 1)
        _Room ("The hall behind", Color) = (0.05, 0.04, 0.035, 1)
        _Furniture ("Knocker, knob and letterbox", Color) = (0.3, 0.24, 0.12, 1)
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
        #include "Panels.cginc"

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
            Joinery door = Doorway(place, IN.fitting.xy, IN.fitting.z, IN.fitting.w);
            float detail = PatternDetail(place * 20.0);
            o.Albedo = door.colour;
            o.Smoothness = door.smoothness;
            o.Normal = WORLD_TO_TANGENT(IN, Raised(IN.worldPos, normal, door.relief * detail));
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
