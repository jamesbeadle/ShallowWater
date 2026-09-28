Shader "Shallow Water/Paintwork"
{
    Properties
    {
        _Frame ("Frame paint", Color) = (0.42, 0.12, 0.09, 1)
        _Panel ("Panel paint", Color) = (0.12, 0.22, 0.15, 1)
        _Line ("Coach line", Color) = (0.78, 0.68, 0.42, 1)
        _Lettering ("Lettering", Color) = (0.82, 0.74, 0.55, 1)
        _LetterShade ("Letter shading", Color) = (0.35, 0.1, 0.07, 1)
        _Primer ("Primer under chipped paint", Color) = (0.36, 0.2, 0.14, 1)
        _SootColour ("Soot and coal dust", Color) = (0.05, 0.045, 0.04, 1)
        _FirstPanel ("First panel, from and to along", Vector) = (0, 1, 0, 0)
        _SecondPanel ("Second panel, from and to along", Vector) = (2, 3, 0, 0)
        _PanelHeights ("Panel foot and top", Vector) = (0.85, 1.6, 0, 0)
        _Decoration ("Decoration", Float) = 0
        _Soot ("Soot", Range(0, 1)) = 0.5
        _Fade ("Fade", Range(0, 1)) = 0.35
        _Wear ("Wear", Range(0, 1)) = 0.35
        _Smoothness ("Smoothness", Range(0, 1)) = 0.45
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
        #include "Grime.cginc"
        #include "Artwork.cginc"

        #define LINE_INSIDE_THE_PANEL 0.045
        #define LINE_HALF_WIDTH 0.011
        #define BEADING_WIDTH 0.018
        #define BEADING_HEIGHT 0.002
        #define EDGE_REACH 0.08
        #define CHIP_DEPTH 0.0012
        #define CABIN_SIDE 1.0
        #define BACK_DOORS 2.0
        #define DIAMONDS 3.0
        #define CAN 4.0
        #define DIAMOND_METRES 0.2

        fixed4 _Frame;
        fixed4 _Panel;
        fixed4 _Line;
        fixed4 _Lettering;
        fixed4 _LetterShade;
        fixed4 _Primer;
        fixed4 _SootColour;
        float4 _FirstPanel;
        float4 _SecondPanel;
        float4 _PanelHeights;
        float _Decoration;
        float _Soot;
        float _Fade;
        float _Wear;
        float _Smoothness;

        struct Input
        {
            float2 surfacePlace;
            float3 aboard;
            float3 worldPos;
            float3 worldNormal;
            INTERNAL_DATA
        };

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.surfacePlace = v.texcoord.xy;
            o.aboard = v.vertex.xyz;
        }

        float InsidePanel(float2 place, float2 along)
        {
            float lengthways = min(place.x - along.x, along.y - place.x);
            float upright = min(place.y - _PanelHeights.x, _PanelHeights.y - place.y);
            return min(lengthways, upright);
        }

        bool IsDecoration(float kind)
        {
            return abs(_Decoration - kind) < 0.5;
        }

        Artwork Decoration(float2 place, float3 aboard, float firstInside, float secondInside)
        {
            Artwork artwork = Blank();
            float side = aboard.x < 0.0 ? -1.0 : 1.0;
            float middle = (_FirstPanel.x + _FirstPanel.y) * 0.5;
            if (IsDecoration(CABIN_SIDE) && firstInside > 0.0) artwork = NameAndRoses(place, side, middle, _Lettering.rgb, _LetterShade.rgb);
            if (IsDecoration(CABIN_SIDE) && secondInside > 0.0) artwork = EngineRoomSide(place);
            if (IsDecoration(BACK_DOORS) && firstInside > 0.0) artwork = DoorPainting(place, _FirstPanel.xy, _PanelHeights.xy);
            if (IsDecoration(BACK_DOORS) && secondInside > 0.0) artwork = DoorPainting(place, _SecondPanel.xy, _PanelHeights.xy);
            if (IsDecoration(DIAMONDS)) artwork = Laid(artwork, float4(Diamonds(place, DIAMOND_METRES), 1.0), 0.0);
            if (IsDecoration(CAN)) artwork = CanPainting(place, _Panel.rgb, _Frame.rgb);
            return artwork;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 place = IN.surfacePlace;
            float firstInside = InsidePanel(place, _FirstPanel.xy);
            float secondInside = InsidePanel(place, _SecondPanel.xy);
            float inside = max(firstInside, secondInside);
            float3 paint = inside > 0.0 ? _Panel.rgb : _Frame.rgb;
            paint = abs(inside - LINE_INSIDE_THE_PANEL) < LINE_HALF_WIDTH ? _Line.rgb : paint;
            float relief = BEADING_HEIGHT * (1.0 - smoothstep(0.0, BEADING_WIDTH, abs(inside)));
            Artwork artwork = Decoration(place, IN.aboard, firstInside, secondInside);
            paint = lerp(paint, artwork.colour, artwork.coverage);
            relief += artwork.relief;
            float edgeCloseness = saturate(1.0 - abs(inside) / EDGE_REACH);
            float chip = Chipped(place, _Wear, edgeCloseness);
            paint = lerp(Faded(paint, _Fade), _Primer.rgb, chip);
            float soot = Sootiness(IN.aboard, place, _Soot);
            o.Albedo = lerp(paint, _SootColour.rgb, soot);
            o.Smoothness = _Smoothness * (1.0 - soot * 0.75) * (1.0 - chip * 0.6);
            o.Normal = WORLD_TO_TANGENT(IN, Raised(IN.worldPos, VERTEX_NORMAL(IN), relief - chip * CHIP_DEPTH));
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
