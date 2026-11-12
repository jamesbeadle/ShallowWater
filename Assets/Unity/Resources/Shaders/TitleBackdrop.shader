Shader "Hidden/Shallow Water/Title Backdrop"
{
    Properties
    {
        _MainTex ("Picture behind", 2D) = "black" {}
        _Night ("Night, as painted", Vector) = (0.043, 0.024, 0.020, 1)
        _Umber ("Umber, as painted", Vector) = (0.110, 0.039, 0.024, 1)
        _Oxblood ("Oxblood, as painted", Vector) = (0.353, 0.051, 0.024, 1)
        _Brick ("Brick, as painted", Vector) = (0.561, 0.141, 0.063, 1)
        _Ember ("Ember, as painted", Vector) = (0.851, 0.412, 0.184, 1)
        _Sunset ("Sunset, as painted", Vector) = (0.910, 0.510, 0.247, 1)
        _Glow ("Glow, as painted", Vector) = (0.965, 0.694, 0.400, 1)
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"
            #include "TitleScene.cginc"

            float4 _MainTex_TexelSize;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 corner : TEXCOORD0;
            };

            v2f vert (appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.corner = v.texcoord;
                return o;
            }

            float2 UprightCorner(float2 corner)
            {
                #if UNITY_UV_STARTS_AT_TOP
                if (_MainTex_TexelSize.y < 0.0) corner.y = 1.0 - corner.y;
                #endif
                return corner;
            }

            float3 LightFrom(float3 painted)
            {
                #ifdef UNITY_COLORSPACE_GAMMA
                return painted;
                #else
                return float3(GammaToLinearSpaceExact(painted.r), GammaToLinearSpaceExact(painted.g), GammaToLinearSpaceExact(painted.b));
                #endif
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 corner = UprightCorner(i.corner);
                float aspect = _MainTex_TexelSize.z / _MainTex_TexelSize.w;
                float pixel = 1.0 / _MainTex_TexelSize.w;
                float3 painted = DuskPicture(float2(corner.x * aspect, corner.y), aspect, pixel, _Time.y);
                return fixed4(LightFrom(saturate(painted)), 1.0);
            }
            ENDCG
        }
    }
    FallBack Off
}
