Shader "Shallow Water/Sky"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.5
            #include "UnityCG.cginc"
            #include "Sky.cginc"

            #define SUN_DISC_EDGE float2(0.99955, 0.99985)
            #define SUN_DISC_BRIGHTNESS 18.0
            #define GROUND_HAZE_DARKENING 0.8

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 direction : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.direction = v.vertex.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 direction = normalize(i.direction);
                float3 sky = SkyColour(direction);
                float towardsTheSun = dot(direction, _SunDirection.xyz);
                float disc = smoothstep(SUN_DISC_EDGE.x, SUN_DISC_EDGE.y, towardsTheSun);
                sky += _SunGlow.rgb * disc * SUN_DISC_BRIGHTNESS;
                float belowTheHorizon = saturate(-direction.y * 6.0);
                sky = lerp(sky, _SkyHaze.rgb * GROUND_HAZE_DARKENING, belowTheHorizon);
                return fixed4(sky, 1.0);
            }
            ENDCG
        }
    }
    FallBack Off
}
