Shader "Shallow Water/Rope"
{
    Properties
    {
        _Colour ("Rope colour", Color) = (0.62, 0.58, 0.5, 1)
        _Dark ("Between the strands", Color) = (0.3, 0.27, 0.22, 1)
        _Twist ("Twists per metre", Float) = 30
        _Strands ("Strands around, per metre of girth", Float) = 30
        _StrandDepth ("Strand depth in metres", Float) = 0.002
        _Smoothness ("Smoothness", Range(0, 1)) = 0.1
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

        #define FULL_TURN 6.2831853

        fixed4 _Colour;
        fixed4 _Dark;
        float _Twist;
        float _Strands;
        float _StrandDepth;
        float _Smoothness;

        struct Input
        {
            float2 surfacePlace;
            float3 worldPos;
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
            float2 place = IN.surfacePlace;
            float2 strandPlace = float2(place.x * _Twist + place.y * _Strands, place.x);
            float detail = PatternDetail(strandPlace);
            float strand = lerp(0.5, sin(strandPlace.x * FULL_TURN) * 0.5 + 0.5, detail);
            float dirt = Fbm(place * float2(3.0, 20.0));
            o.Albedo = lerp(_Dark.rgb, _Colour.rgb, strand) * lerp(0.7, 1.05, dirt);
            o.Smoothness = _Smoothness;
            o.Normal = WORLD_TO_TANGENT(IN, Raised(IN.worldPos, VERTEX_NORMAL(IN), strand * _StrandDepth));
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
