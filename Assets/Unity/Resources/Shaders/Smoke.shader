Shader "Shallow Water/Smoke"
{
    Properties
    {
        _Colour ("Smoke colour", Color) = (0.55, 0.55, 0.56, 1)
        _Life ("Life in seconds", Float) = 8
        _Puffs ("Puffs alive at once", Float) = 12
        _Rise ("Rise in metres per second", Float) = 0.45
        _StartSize ("Size when made, in metres", Float) = 0.12
        _Grow ("Growth in metres per second", Float) = 0.28
        _Opacity ("Opacity", Range(0, 1)) = 0.35
        _Drift ("Drift in the wind, metres per second", Vector) = (0.35, 0, 0.15, 0)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        LOD 200

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma target 3.5
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Noise.cginc"

            #define WOBBLE_METRES 0.12
            #define FADE_IN_SHARE 0.08
            #define FADE_OUT_FROM_SHARE 0.35

            fixed4 _Colour;
            float _Life;
            float _Puffs;
            float _Rise;
            float _StartSize;
            float _Grow;
            float _Opacity;
            float4 _Drift;
            float4 _BoatVelocity;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 corner : TEXCOORD0;
                float2 puff : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };

            v2f vert (appdata_full v)
            {
                float index = v.texcoord1.x;
                float share = frac(_Time.y / _Life + index / _Puffs);
                float age = share * _Life;
                float3 emitter = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0)).xyz;
                float3 wobble = float3(sin(age * 1.7 + index * 2.1), 0.0, cos(age * 1.3 + index * 1.3)) * WOBBLE_METRES * age;
                float3 centre = emitter + float3(0.0, _Rise * age, 0.0) + (_Drift.xyz - _BoatVelocity.xyz) * age + wobble;
                float size = _StartSize + _Grow * age;
                float2 corner = v.texcoord.xy - 0.5;
                float3 across = UNITY_MATRIX_V[0].xyz;
                float3 upward = UNITY_MATRIX_V[1].xyz;
                float3 world = centre + (across * corner.x + upward * corner.y) * size;
                v2f o;
                o.pos = mul(UNITY_MATRIX_VP, float4(world, 1.0));
                o.corner = corner;
                float fade = smoothstep(0.0, FADE_IN_SHARE, share) * (1.0 - smoothstep(FADE_OUT_FROM_SHARE, 1.0, share));
                o.puff = float2(index + share * 3.0, fade);
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float reach = length(i.corner) * 2.0;
                float soft = saturate(1.0 - reach);
                float billow = ValueNoise(i.corner * 3.0 + i.puff.x * 7.0) * 0.5 + ValueNoise(i.corner * 7.0 - i.puff.x * 3.0) * 0.5;
                float alpha = soft * soft * (0.45 + 0.55 * billow) * _Opacity * i.puff.y;
                float3 light = ShadeSH9(float4(0.0, 1.0, 0.0, 1.0)) + _LightColor0.rgb * 0.6;
                fixed4 colour = fixed4(_Colour.rgb * light, alpha);
                UNITY_APPLY_FOG(i.fogCoord, colour);
                return colour;
            }
            ENDCG
        }
    }
    FallBack Off
}
