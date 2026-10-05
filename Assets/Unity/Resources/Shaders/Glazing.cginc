#ifndef SHALLOW_WATER_GLAZING_INCLUDED
#define SHALLOW_WATER_GLAZING_INCLUDED

#include "Joinery.cginc"

#define SASH_RAIL 0.035
#define BOTTOM_RAIL 0.06
#define MEETING_RAIL 0.028
#define MULLION 0.035
#define GROUND_FLOOR_BELOW 1.6
#define NETS_UP_THE_GLASS 0.62

fixed4 _Net;
fixed4 _Curtain0;
fixed4 _Curtain1;

float3 BehindTheGlass(float2 place, float2 size, float paint, float sillHeight)
{
    float3 room = _Room.rgb * (0.7 + 0.6 * ValueNoise(place * 3.0));
    bool isGroundFloor = sillHeight < GROUND_FLOOR_BELOW;
    float lace = 0.72 + 0.28 * step(0.45, frac(place.x * 45.0) * frac(place.y * 45.0 + 0.3));
    float hem = size.y * NETS_UP_THE_GLASS + 0.025 * sin(place.x * 60.0);
    float3 netted = place.y < hem ? lerp(room, _Net.rgb * lace, 0.8) : room;
    float drawnBack = size.x * 0.2 * (0.8 + 0.4 * frac(sillHeight * 3.7 + paint));
    bool isCurtain = place.x < drawnBack || place.x > size.x - drawnBack;
    float3 curtain = frac(paint * 0.5 + sillHeight) > 0.5 ? _Curtain0.rgb : _Curtain1.rgb;
    float folds = 0.75 + 0.25 * sin(place.x * 70.0);
    float3 upstairs = isCurtain ? curtain * folds : room;
    return isGroundFloor ? netted : upstairs;
}

Joinery Glass(float2 place, float2 size, float paint, float sillHeight)
{
    Joinery glass;
    float grime = Fbm(place * 6.0) * 0.25;
    glass.colour = lerp(BehindTheGlass(place, size, paint, sillHeight) * 0.75 + _Glass.rgb * 0.25, float3(0.35, 0.33, 0.3), grime);
    glass.smoothness = 0.93 - grime;
    glass.relief = -0.012;
    return glass;
}

bool IsSashTimber(float2 place, float2 size, float pattern)
{
    float meeting = size.y * 0.5;
    bool isUpper = place.y > meeting;
    float2 inSash = isUpper ? float2(place.x, place.y - meeting) : place;
    float sashHeight = isUpper ? size.y - meeting : meeting;
    bool isRail = abs(place.y - meeting) < MEETING_RAIL || place.y < BOX_FRAME + BOTTOM_RAIL;
    float columns = pattern > 0.5 ? 3.0 : 2.0;
    float rows = pattern > 0.5 ? 2.0 : 1.0;
    bool isBar = IsOnABar(inSash.x, size.x, columns, GLAZING_BAR) || IsOnABar(inSash.y, sashHeight, rows, GLAZING_BAR);
    return isRail || isBar || InsideBy(place, size) < BOX_FRAME + SASH_RAIL;
}

bool IsCasementTimber(float2 place, float2 size)
{
    bool isMullion = abs(place.x - size.x * 0.5) < MULLION;
    bool isBar = IsOnABar(place.y, size.y, 3.0, GLAZING_BAR);
    return isMullion || isBar || InsideBy(place, size) < BOX_FRAME + SASH_RAIL;
}

bool IsFourPaneTimber(float2 place, float2 size)
{
    bool isBar = IsOnABar(place.x, size.x, 2.0, GLAZING_BAR) || IsOnABar(place.y, size.y, 2.0, GLAZING_BAR);
    return isBar || InsideBy(place, size) < BOX_FRAME + SASH_RAIL;
}

Joinery Sashes(float2 place, float2 size, float pattern, float paint, float sillHeight)
{
    bool isTimber = pattern < 1.5 ? IsSashTimber(place, size, pattern) : IsCasementTimber(place, size);
    isTimber = pattern > 2.5 ? IsFourPaneTimber(place, size) : isTimber;
    bool isBoxFrame = InsideBy(place, size) < BOX_FRAME;
    bool isUpperSash = pattern < 1.5 && place.y > size.y * 0.5;
    float timberRelief = isBoxFrame ? 0.0 : (isUpperSash ? -0.004 : -0.009);
    if (isTimber) return Paintwork(PaintOf(paint), place, timberRelief);
    return Glass(place, size, paint, sillHeight);
}

#endif
