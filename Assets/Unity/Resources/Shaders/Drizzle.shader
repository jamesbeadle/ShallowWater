Shader "Shallow Water/Drizzle"
{
    Properties
    {
        _Colour ("Drizzle colour", Color) = (0.62, 0.63, 0.64, 1)
        _Box ("Box of air round the eye, in metres", Vector) = (26, 14, 26, 0)
        _Fall ("Fall in metres per second", Float) = 4.5
        _Length ("Streak length in metres", Float) = 0.35
        _Width ("Streak width in metres", Float) = 0.006
        _Drift ("Drift in the wind, metres per second", Vector) = (0.6, 0, 0.25, 0)
        _Opacity ("Opacity", Range(0, 1)) = 0.22
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

            #define NEAREST_METRES 0.6

            fixed4 _Colour;
            float4 _Box;
            float _Fall;
            float _Length;
            float _Width;
            float4 _Drift;
            float _Opacity;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 corner : TEXCOORD0;
                float fade : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };

            float3 PlaceInTheBox(float index)
            {
                float3 seed = float3(Hash21(float2(index, 1.7)), Hash21(float2(index, 5.3)), Hash21(float2(index, 9.1)));
                float3 fallen = seed * _Box.xyz + (_Drift.xyz - float3(0.0, _Fall, 0.0)) * _Time.y;
                float3 fromTheEye = frac((fallen - _WorldSpaceCameraPos) / _Box.xyz) - 0.5;
                return fromTheEye * _Box.xyz;
            }

            v2f vert (appdata_full v)
            {
                float index = v.texcoord1.x;
                float3 fromTheEye = PlaceInTheBox(index);
                float3 centre = _WorldSpaceCameraPos + fromTheEye;
                float3 falling = normalize(float3(_Drift.x, -_Fall, _Drift.z));
                float3 sideways = normalize(cross(falling, centre - _WorldSpaceCameraPos));
                float2 corner = v.texcoord.xy - 0.5;
                float3 world = centre + falling * corner.y * _Length + sideways * corner.x * _Width;
                v2f o;
                o.pos = mul(UNITY_MATRIX_VP, float4(world, 1.0));
                o.corner = corner;
                float reach = length(fromTheEye / (_Box.xyz * 0.5));
                o.fade = smoothstep(0.0, NEAREST_METRES, length(fromTheEye)) * (1.0 - smoothstep(0.6, 1.0, reach));
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float alongTheStreak = 1.0 - abs(i.corner.y) * 2.0;
                float alpha = alongTheStreak * _Opacity * i.fade;
                float3 light = ShadeSH9(float4(0.0, 1.0, 0.0, 1.0)) + _LightColor0.rgb * 0.4;
                fixed4 colour = fixed4(_Colour.rgb * light, alpha);
                UNITY_APPLY_FOG(i.fogCoord, colour);
                return colour;
            }
            ENDCG
        }
    }
    FallBack Off
}
