Shader "Shallow Water/Water"
{
    Properties
    {
        _Deep ("Deep water", Color) = (0.1, 0.13, 0.09, 1)
        _Shallow ("Silted water by the banks", Color) = (0.28, 0.25, 0.16, 1)
        _BankReflection ("Banks seen in the water", Color) = (0.1, 0.16, 0.07, 1)
        _Leaves ("Fallen leaves", Color) = (0.62, 0.36, 0.12, 1)
        _HalfWidth ("Half width in metres", Float) = 6
        _Ripple ("Ripple strength", Float) = 1
        _LeafAmount ("Fallen leaf amount", Range(0, 0.5)) = 0.06
        _Churned ("Silt churned up by the propeller", Color) = (0.3, 0.24, 0.15, 1)
        _Foam ("Foam", Color) = (0.72, 0.7, 0.62, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
        LOD 200

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            #pragma multi_compile_fog
            #pragma target 3.5
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            #include "Sky.cginc"
            #include "Wake.cginc"

            #define WATER_REFLECTANCE 0.02
            #define GLINT_SHARPNESS 900.0
            #define GLINT_STRENGTH 6.0
            #define LEAVES_PER_METRE 2.8
            #define LEAF_SIZE 0.12

            fixed4 _Deep;
            fixed4 _Shallow;
            fixed4 _BankReflection;
            fixed4 _Leaves;
            float _HalfWidth;
            float _Ripple;
            float _LeafAmount;
            fixed4 _Churned;
            fixed4 _Foam;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float2 surfacePlace : TEXCOORD1;
                SHADOW_COORDS(2)
                UNITY_FOG_COORDS(3)
            };

            v2f vert (appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.surfacePlace = v.texcoord.xy;
                TRANSFER_SHADOW(o)
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            float3 Reflection(float3 reflected, float nearTheBank)
            {
                float3 upward = float3(reflected.x, abs(reflected.y), reflected.z);
                return lerp(SkyColour(upward), _BankReflection.rgb, nearTheBank);
            }

            float3 Leaves(float3 colour, float3 light, float2 ground, float nearTheBank)
            {
                float2 leafPlace = ground * LEAVES_PER_METRE;
                float3 cells = Cells(leafPlace);
                float amount = _LeafAmount * lerp(0.5, 2.0, nearTheBank);
                float isLeaf = step(cells.z, LEAF_SIZE) * step(cells.y, amount) * PatternDetail(leafPlace);
                float3 leaf = lerp(_Leaves.rgb, _Deep.rgb * 2.5, frac(cells.y * 37.0) * 0.6) * light;
                return lerp(colour, leaf, isLeaf);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 toTheEye = _WorldSpaceCameraPos - i.worldPos;
                float eyeDistance = length(toTheEye);
                float3 view = toTheEye / eyeDistance;
                float calm = saturate(eyeDistance / RIPPLES_FADE_METRES);
                Wake wake = WakeAt(i.worldPos.xz, _Time.y);
                float2 slope = RippleSlope(i.worldPos.xz, _Time.y) * _Ripple * (1.0 - calm * 0.85) + wake.slope;
                float3 normal = normalize(float3(-slope.x, 1.0, -slope.y));
                float3 sun = _WorldSpaceLightPos0.xyz;
                float shadow = SHADOW_ATTENUATION(i);
                float nearTheBank = smoothstep(0.45, 1.0, abs(i.surfacePlace.y) / _HalfWidth);
                float3 light = ShadeSH9(float4(normal, 1.0)) + _LightColor0.rgb * saturate(dot(normal, sun)) * shadow;
                float3 body = lerp(lerp(_Deep.rgb, _Shallow.rgb, nearTheBank * 0.6), _Churned.rgb, wake.churn * 0.7) * light;
                float facing = saturate(dot(normal, view));
                float fresnel = WATER_REFLECTANCE + (1.0 - WATER_REFLECTANCE) * pow(1.0 - facing, 5.0);
                float3 reflected = reflect(-view, normal);
                float3 colour = lerp(body, Reflection(reflected, nearTheBank * 0.7), fresnel);
                float glint = pow(saturate(dot(reflected, sun)), GLINT_SHARPNESS) * GLINT_STRENGTH;
                colour += _LightColor0.rgb * glint * shadow;
                colour = Leaves(colour, light, i.worldPos.xz, nearTheBank);
                colour = lerp(colour, _Churned.rgb * light, wake.churn * 0.35);
                colour = lerp(colour, _Foam.rgb * light, wake.foam);
                UNITY_APPLY_FOG(i.fogCoord, colour);
                return fixed4(colour, 1.0);
            }
            ENDCG
        }
    }
    FallBack "Legacy Shaders/VertexLit"
}
