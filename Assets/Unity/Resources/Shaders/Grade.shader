Shader "Hidden/Shallow Water/Grade"
{
    Properties
    {
        _MainTex ("Picture", 2D) = "white" {}
        _Exposure ("Exposure", Float) = 0.85
        _Warmth ("Warmth", Vector) = (1.04, 1.0, 0.94, 1)
        _Saturation ("Saturation", Float) = 0.92
        _Vignette ("Vignette", Range(0, 1)) = 0.28
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            #define LUMINANCE float3(0.2126, 0.7152, 0.0722)
            #define VIGNETTE_FROM 0.35
            #define VIGNETTE_TO 0.95
            #define VIGNETTE_WIDENING 1.4

            sampler2D _MainTex;
            float _Exposure;
            float4 _Warmth;
            float _Saturation;
            float _Vignette;

            float3 FilmicToneFrom(float3 light)
            {
                float3 numerator = light * (2.51 * light + 0.03);
                float3 denominator = light * (2.43 * light + 0.59) + 0.14;
                return saturate(numerator / denominator);
            }

            fixed4 frag (v2f_img i) : SV_Target
            {
                float3 light = tex2D(_MainTex, i.uv).rgb * _Exposure * _Warmth.rgb;
                float luminance = dot(light, LUMINANCE);
                light = max(0.0, lerp(luminance.xxx, light, _Saturation));
                float3 picture = FilmicToneFrom(light);
                float2 fromTheMiddle = (i.uv - 0.5) * float2(1.0, 0.8) * VIGNETTE_WIDENING;
                float vignette = 1.0 - _Vignette * smoothstep(VIGNETTE_FROM, VIGNETTE_TO, length(fromTheMiddle));
                return fixed4(picture * vignette, 1.0);
            }
            ENDCG
        }
    }
    FallBack Off
}
