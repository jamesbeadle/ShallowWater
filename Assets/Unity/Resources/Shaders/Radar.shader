Shader "Hidden/Shallow Water/Radar"
{
    Properties
    {
        _MainTex ("Map", 2D) = "white" {}
        _Centre ("Centre on the map", Vector) = (0.5, 0.5, 0, 0)
        _Reach ("Map reached by the rim", Vector) = (0.03, 0.03, 0, 0)
        _Heading ("Heading of the view in radians", Float) = 0
        _Pointing ("Player's heading against the view in radians", Float) = 0
        _Paper ("Beyond the map", Color) = (0.82, 0.78, 0.68, 1)
        _Rim ("Rim", Color) = (0.12, 0.12, 0.16, 1)
        _Arrow ("Player", Color) = (0.93, 0.9, 0.82, 1)
        _Outline ("Player outline", Color) = (0.1, 0.08, 0.06, 1)
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            #define RIM_INNER 0.92
            #define ARROW_TIP 0.11
            #define ARROW_TAIL -0.07
            #define ARROW_HALF_WIDTH 0.075
            #define ARROW_OUTLINE 0.025

            sampler2D _MainTex;
            float4 _Centre;
            float4 _Reach;
            float _Heading;
            float _Pointing;
            float4 _Paper;
            float4 _Rim;
            float4 _Arrow;
            float4 _Outline;

            float2 Turned(float2 place, float angle)
            {
                float across = cos(angle);
                float along = sin(angle);
                return float2(place.x * across + place.y * along, -place.x * along + place.y * across);
            }

            float3 MapUnder(float2 disc)
            {
                float2 eastNorth = Turned(disc, _Heading);
                float2 mapPlace = _Centre.xy + eastNorth * _Reach.xy;
                float isOnTheSheet = all(mapPlace == saturate(mapPlace));
                float3 map = tex2D(_MainTex, mapPlace).rgb;
                return lerp(_Paper.rgb, map, isOnTheSheet);
            }

            float ArrowDistance(float2 disc)
            {
                float2 place = Turned(disc, -_Pointing);
                float behind = ARROW_TAIL - place.y;
                float taper = ARROW_HALF_WIDTH * (ARROW_TIP - place.y) / (ARROW_TIP - ARROW_TAIL);
                float beside = abs(place.x) - taper;
                return max(behind, beside);
            }

            fixed4 frag (v2f_img i) : SV_Target
            {
                float2 disc = i.uv * 2.0 - 1.0;
                float outwards = length(disc);
                float edge = fwidth(outwards);
                float3 colour = MapUnder(disc);
                float rim = smoothstep(RIM_INNER - edge, RIM_INNER, outwards);
                colour = lerp(colour, _Rim.rgb, rim);
                float arrow = ArrowDistance(disc);
                colour = lerp(colour, _Outline.rgb, 1.0 - smoothstep(ARROW_OUTLINE - edge, ARROW_OUTLINE, arrow));
                colour = lerp(colour, _Arrow.rgb, 1.0 - smoothstep(-edge, 0.0, arrow));
                float inside = 1.0 - smoothstep(1.0 - edge, 1.0, outwards);
                return fixed4(colour, inside);
            }
            ENDCG
        }
    }
    FallBack Off
}
