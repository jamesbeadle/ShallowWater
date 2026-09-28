Shader "Shallow Water/Livery"
{
    Properties
    {
        _Frame ("Frame paint", Color) = (0.07, 0.22, 0.14, 1)
        _Panel ("Panel paint", Color) = (0.52, 0.09, 0.07, 1)
        _Line ("Coach line", Color) = (0.87, 0.78, 0.5, 1)
        _FirstPanel ("First panel, from and to along", Vector) = (0, 1, 0, 0)
        _SecondPanel ("Second panel, from and to along", Vector) = (2, 3, 0, 0)
        _PanelHeights ("Panel foot and top", Vector) = (0.85, 1.6, 0, 0)
        _Smoothness ("Smoothness", Range(0, 1)) = 0.55
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.5
        #include "Noise.cginc"

        #define LINE_INSIDE_THE_PANEL 0.045
        #define LINE_HALF_WIDTH 0.011

        fixed4 _Frame;
        fixed4 _Panel;
        fixed4 _Line;
        float4 _FirstPanel;
        float4 _SecondPanel;
        float4 _PanelHeights;
        float _Smoothness;

        struct Input
        {
            float2 surfacePlace;
        };

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.surfacePlace = v.texcoord.xy;
        }

        float InsidePanel(float2 place, float2 along)
        {
            float lengthways = min(place.x - along.x, along.y - place.x);
            float upright = min(place.y - _PanelHeights.x, _PanelHeights.y - place.y);
            return min(lengthways, upright);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 place = IN.surfacePlace;
            float inside = max(InsidePanel(place, _FirstPanel.xy), InsidePanel(place, _SecondPanel.xy));
            bool isLine = abs(inside - LINE_INSIDE_THE_PANEL) < LINE_HALF_WIDTH;
            float3 paint = inside > 0.0 ? _Panel.rgb : _Frame.rgb;
            paint = isLine ? _Line.rgb : paint;
            float grime = Fbm(place * 3.0);
            o.Albedo = paint * lerp(0.85, 1.05, grime);
            o.Smoothness = _Smoothness * lerp(0.7, 1.1, grime);
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
