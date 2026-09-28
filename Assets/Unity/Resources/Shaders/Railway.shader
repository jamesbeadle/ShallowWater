Shader "Shallow Water/Railway"
{
    Properties
    {
        _Ballast ("Ballast", Color) = (0.38, 0.35, 0.32, 1)
        _BallastGrain ("Ballast grain", Color) = (0.24, 0.22, 0.2, 1)
        _Sleeper ("Sleepers", Color) = (0.2, 0.15, 0.11, 1)
        _Rail ("Rails", Color) = (0.36, 0.3, 0.26, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.5
        #include "Noise.cginc"

        #define SLEEPER_SPACING 0.7
        #define SLEEPER_SHARE 0.36
        #define SLEEPER_HALF_LENGTH 1.3
        #define RAIL_FROM_THE_MIDDLE 0.7525
        #define RAIL_HALF_WIDTH 0.035
        #define STONES_PER_METRE 22.0

        fixed4 _Ballast;
        fixed4 _BallastGrain;
        fixed4 _Sleeper;
        fixed4 _Rail;

        struct Input
        {
            float2 surfacePlace;
        };

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.surfacePlace = v.texcoord.xy;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float along = IN.surfacePlace.x;
            float across = abs(IN.surfacePlace.y);
            float2 stonePlace = IN.surfacePlace * STONES_PER_METRE;
            float stones = lerp(0.5, ValueNoise(stonePlace), PatternDetail(stonePlace));
            float3 ballast = lerp(_Ballast.rgb, _BallastGrain.rgb, stones);
            float2 trackPlace = float2(along / SLEEPER_SPACING, across / RAIL_HALF_WIDTH);
            float detail = PatternDetail(trackPlace);
            bool isSleeper = frac(along / SLEEPER_SPACING) < SLEEPER_SHARE && across < SLEEPER_HALF_LENGTH;
            bool isRail = abs(across - RAIL_FROM_THE_MIDDLE) < RAIL_HALF_WIDTH;
            float3 track = isSleeper ? _Sleeper.rgb : ballast;
            track = isRail ? _Rail.rgb : track;
            float3 average = lerp(ballast, _Sleeper.rgb, across < SLEEPER_HALF_LENGTH ? SLEEPER_SHARE : 0.0);
            o.Albedo = lerp(average, track, detail);
            o.Smoothness = isRail ? 0.7 * detail : 0.08;
            o.Metallic = isRail ? 0.8 * detail : 0.0;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
