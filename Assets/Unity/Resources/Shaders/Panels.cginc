#ifndef SHALLOW_WATER_PANELS_INCLUDED
#define SHALLOW_WATER_PANELS_INCLUDED

#include "Joinery.cginc"

#define FANLIGHT 0.4
#define TRANSOM 0.055
#define STILE 0.11
#define MUNTIN 0.05
#define MOULDING 0.03
#define BOARD 0.11
#define KNOB_RADIUS 0.028
#define LETTERBOX float2(0.13, 0.025)
#define STRAP_HEIGHT 0.035
#define LEDGED_DOOR 1.5

fixed4 _Trim;
fixed4 _Furniture;

Joinery Fanlight(float2 place, float2 size, float leafTop)
{
    bool isBar = IsOnABar(place.x, size.x, 3.0, GLAZING_BAR);
    bool isTransom = place.y < leafTop + TRANSOM;
    bool isFrame = InsideBy(place, size) < BOX_FRAME;
    Joinery glass;
    glass.colour = lerp(_Room.rgb, _Glass.rgb, 0.5);
    glass.smoothness = 0.9;
    glass.relief = -0.012;
    if (isBar || isTransom || isFrame) return Paintwork(_Trim.rgb, place, 0.0);
    return glass;
}

float PanelDepth(float2 inLeaf, float2 leaf, float pattern)
{
    float4 rows = pattern > 0.5 ? float4(0.08, 0.38, 0.46, 0.8) : float4(0.1, 0.4, 0.5, 0.93);
    float share = inLeaf.y / leaf.y;
    bool isInARow = (share > rows.x && share < rows.y) || (share > rows.z && share < rows.w) || (pattern > 0.5 && share > 0.86 && share < 0.95);
    float fromTheMiddle = abs(inLeaf.x - leaf.x * 0.5);
    bool isBetweenTheStiles = fromTheMiddle > MUNTIN && inLeaf.x > STILE && inLeaf.x < leaf.x - STILE;
    float inset = min(min(inLeaf.x - STILE, leaf.x - STILE - inLeaf.x), fromTheMiddle - MUNTIN);
    float bevel = saturate(inset / MOULDING);
    return (isInARow && isBetweenTheStiles) ? -0.006 - 0.008 * bevel : 0.0;
}

float BoardDepth(float2 inLeaf, float2 leaf)
{
    bool isJoint = frac(inLeaf.x / BOARD) < 0.06;
    return isJoint ? -0.005 : 0.0;
}

bool IsFurniture(float2 inLeaf, float2 leaf, float pattern)
{
    bool isKnob = length(inLeaf - float2(leaf.x - STILE * 0.8, leaf.y * 0.45)) < KNOB_RADIUS;
    float2 fromTheLetterbox = abs(inLeaf - float2(leaf.x * 0.5, leaf.y * 0.44));
    bool isLetterbox = pattern < 1.5 && fromTheLetterbox.x < LETTERBOX.x && fromTheLetterbox.y < LETTERBOX.y;
    bool isStrap = pattern > LEDGED_DOOR && inLeaf.x < leaf.x * 0.7 && (abs(inLeaf.y - leaf.y * 0.2) < STRAP_HEIGHT || abs(inLeaf.y - leaf.y * 0.82) < STRAP_HEIGHT);
    return isKnob || isLetterbox || isStrap;
}

Joinery Doorway(float2 place, float2 size, float pattern, float paint)
{
    bool hasFanlight = pattern < LEDGED_DOOR;
    float leafTop = hasFanlight ? size.y - FANLIGHT : size.y;
    if (place.y > leafTop) return Fanlight(place, size, leafTop);
    if (InsideBy(place, float2(size.x, leafTop + BOX_FRAME)) < BOX_FRAME) return Paintwork(_Trim.rgb, place, 0.0);
    float2 inLeaf = place - BOX_FRAME;
    float2 leaf = float2(size.x - BOX_FRAME * 2.0, leafTop - BOX_FRAME);
    float depth = pattern > LEDGED_DOOR ? BoardDepth(inLeaf, leaf) : PanelDepth(inLeaf, leaf, pattern);
    Joinery door = Paintwork(PaintOf(paint), place, depth - 0.01);
    Joinery furniture = Paintwork(_Furniture.rgb, place, 0.004);
    furniture.smoothness = 0.6;
    if (IsFurniture(inLeaf, leaf, pattern)) return furniture;
    return door;
}

#endif
