Shader "Shallow Water/Hull Plates"
{
    Properties
    {
        _Tar ("Tar", Color) = (0.045, 0.045, 0.045, 1)
        _DustyTar ("Dusty tar", Color) = (0.12, 0.11, 0.1, 1)
        _Rust ("Rust", Color) = (0.3, 0.15, 0.08, 1)
        _Weed ("Weed at the waterline", Color) = (0.12, 0.14, 0.07, 1)
        _Smoothness ("Smoothness", Range(0, 1)) = 0.35
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

        #define KEEL -0.65
        #define STRAKE_HEIGHT 0.34
        #define PLATE_LENGTH 1.83
        #define BUTT_STAGGER 0.61
        #define LAP_WIDTH 0.03
        #define LAP_HEIGHT 0.004
        #define RIVET_PITCH 0.055
        #define RIVET_ABOVE_THE_LAP 0.015
        #define RIVET_RADIUS 0.007
        #define RIVET_HEIGHT 0.003
        #define BUTT_GAP 0.006
        #define WEED_FROM -0.12
        #define WEED_TO 0.07

        fixed4 _Tar;
        fixed4 _DustyTar;
        fixed4 _Rust;
        fixed4 _Weed;
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

        float PlateRelief(float along, float height)
        {
            float fromTheKeel = height - KEEL;
            float strake = floor(fromTheKeel / STRAKE_HEIGHT);
            float aboveTheLap = fromTheKeel - strake * STRAKE_HEIGHT;
            float lap = LAP_HEIGHT * (1.0 - saturate(aboveTheLap / LAP_WIDTH));
            float2 rivetPlace = float2((frac(along / RIVET_PITCH) - 0.5) * RIVET_PITCH, aboveTheLap - RIVET_ABOVE_THE_LAP);
            float rivet = RIVET_HEIGHT * saturate(1.0 - dot(rivetPlace, rivetPlace) / (RIVET_RADIUS * RIVET_RADIUS));
            float butt = frac((along + strake * BUTT_STAGGER) / PLATE_LENGTH) * PLATE_LENGTH;
            float isButt = step(butt, BUTT_GAP);
            float detail = PatternDetail(float2(along / RIVET_PITCH, height / RIVET_PITCH));
            return (lap + rivet * detail) - isButt * LAP_HEIGHT;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float along = IN.surfacePlace.x;
            float height = IN.surfacePlace.y;
            float dust = Fbm(float2(along * 0.7, height * 2.5));
            float3 colour = lerp(_Tar.rgb, _DustyTar.rgb, smoothstep(0.45, 0.8, dust));
            float streaks = smoothstep(0.62, 0.85, Fbm(float2(along * 7.0, height * 0.8))) * smoothstep(0.4, 0.7, Fbm(float2(along * 1.3, 3.1)));
            colour = lerp(colour, _Rust.rgb, streaks * 0.6);
            float scrapes = smoothstep(0.78, 0.92, ValueNoise(float2(along * 0.9, height * 45.0))) * PatternDetail(float2(along, height * 45.0));
            colour = lerp(colour, _DustyTar.rgb * 1.6, scrapes * 0.5);
            float weed = smoothstep(WEED_FROM, WEED_FROM + 0.05, height) * (1.0 - smoothstep(WEED_TO - 0.04, WEED_TO, height));
            colour = lerp(colour, _Weed.rgb, weed * (0.6 + 0.4 * dust));
            o.Albedo = colour;
            o.Smoothness = lerp(_Smoothness * (1.0 - dust * 0.5), 0.75, weed);
            float relief = PlateRelief(along, height) + weed * 0.002 * ValueNoise(float2(along * 30.0, height * 30.0));
            o.Normal = WORLD_TO_TANGENT(IN, Raised(IN.worldPos, VERTEX_NORMAL(IN), relief));
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
