#ifndef SHALLOW_WATER_OPENINGS_INCLUDED
#define SHALLOW_WATER_OPENINGS_INCLUDED

#include "Noise.cginc"

#define WINDOW_HALF_WIDTH 0.45
#define GROUND_FLOOR_WINDOW float2(0.9, 2.1)
#define UPPER_FLOOR_WINDOW float2(3.1, 4.2)
#define DOOR_HALF_WIDTH 0.45
#define DOOR_HEIGHTS float2(0.12, 2.2)
#define FRAME_WIDTH 0.07
#define GLAZING_BAR_WIDTH 0.03
#define SILL_DEPTH 0.08
#define SILL_OVERHANG 0.08
#define CURTAIN_FROM_THE_MIDDLE 0.22

fixed4 _Frame;
fixed4 _Glass;
fixed4 _Curtain;
fixed4 _Door;
fixed4 _Sill;

struct Opening
{
    float3 colour;
    float smoothness;
    float coverage;
};

float InsideBy(float2 fromCentre, float2 halfSize)
{
    float2 edge = halfSize - abs(fromCentre);
    return min(edge.x, edge.y);
}

Opening Covering(Opening under, Opening over)
{
    Opening uppermost = under;
    if (over.coverage > 0.0)
    {
        uppermost = over;
    }
    return uppermost;
}

Opening Sill(float2 wallPlace, float bottom)
{
    Opening sill;
    bool isUnderTheWindow = abs(wallPlace.x) < WINDOW_HALF_WIDTH + SILL_OVERHANG;
    bool isInTheSill = wallPlace.y < bottom && wallPlace.y > bottom - SILL_DEPTH;
    sill.colour = _Sill.rgb;
    sill.smoothness = 0.2;
    sill.coverage = (isUnderTheWindow && isInTheSill) ? 1.0 : 0.0;
    return sill;
}

Opening Window(float2 wallPlace, float2 heights)
{
    float2 middle = float2(0.0, (heights.x + heights.y) * 0.5);
    float2 fromCentre = wallPlace - middle;
    float inside = InsideBy(fromCentre, float2(WINDOW_HALF_WIDTH, (heights.y - heights.x) * 0.5));
    bool isFrame = inside < FRAME_WIDTH;
    bool isBar = abs(fromCentre.x) < GLAZING_BAR_WIDTH || abs(fromCentre.y) < GLAZING_BAR_WIDTH * 1.5;
    bool isPainted = isFrame || isBar;
    bool isCurtain = abs(fromCentre.x) > CURTAIN_FROM_THE_MIDDLE;
    float3 glass = isCurtain ? lerp(_Glass.rgb, _Curtain.rgb, 0.55) : _Glass.rgb;
    Opening window;
    window.colour = isPainted ? _Frame.rgb : glass;
    window.smoothness = isPainted ? 0.45 : 0.92;
    window.coverage = inside > 0.0 ? 1.0 : 0.0;
    Opening sill = Sill(wallPlace, heights.x);
    return Covering(window, sill);
}

Opening Door(float2 wallPlace)
{
    float2 middle = float2(0.0, (DOOR_HEIGHTS.x + DOOR_HEIGHTS.y) * 0.5);
    float2 fromCentre = wallPlace - middle;
    float inside = InsideBy(fromCentre, float2(DOOR_HALF_WIDTH, (DOOR_HEIGHTS.y - DOOR_HEIGHTS.x) * 0.5));
    bool isFrame = inside < FRAME_WIDTH;
    bool isPanelLine = abs(fromCentre.x) < 0.02 || abs(fromCentre.y - 0.2) < 0.02;
    bool isStep = wallPlace.y < DOOR_HEIGHTS.x && abs(wallPlace.x) < DOOR_HALF_WIDTH + SILL_OVERHANG;
    Opening door;
    door.colour = isFrame ? _Frame.rgb : _Door.rgb * (isPanelLine ? 0.6 : 1.0);
    door.colour = isStep ? _Sill.rgb : door.colour;
    door.smoothness = 0.5;
    door.coverage = (inside > 0.0 || isStep) ? 1.0 : 0.0;
    return door;
}


#endif
