#ifndef SHALLOW_WATER_CASTLE_INCLUDED
#define SHALLOW_WATER_CASTLE_INCLUDED

#include "Paint.cginc"

#define CASTLE_SKY_LOW float3(0.78, 0.8, 0.8)
#define CASTLE_SKY_HIGH float3(0.4, 0.5, 0.66)
#define CASTLE_HILLS float3(0.28, 0.4, 0.22)
#define CASTLE_WATER float3(0.32, 0.42, 0.56)
#define CASTLE_WALLS float3(0.82, 0.77, 0.64)
#define CASTLE_ROOFS float3(0.6, 0.2, 0.14)
#define CASTLE_OPENINGS float3(0.12, 0.1, 0.1)
#define DIAMOND_RED float3(0.5, 0.16, 0.12)
#define DIAMOND_CREAM float3(0.62, 0.58, 0.46)
#define DIAMOND_GREEN float3(0.16, 0.28, 0.17)
#define DIAMOND_EDGE float3(0.1, 0.08, 0.06)

float InBox(float2 place, float2 low, float2 high)
{
    return step(low.x, place.x) * step(place.x, high.x) * step(low.y, place.y) * step(place.y, high.y);
}

float UnderRoof(float2 place, float2 eaves, float halfWidth, float height)
{
    float rise = height * (1.0 - abs(place.x - eaves.x) / halfWidth);
    return step(eaves.y, place.y) * step(place.y, eaves.y + rise);
}

float3 CastleScene(float2 place)
{
    float3 colour = lerp(CASTLE_SKY_LOW, CASTLE_SKY_HIGH, saturate(place.y));
    float hills = 0.32 + 0.07 * sin(place.x * 7.0 + 1.3) + 0.03 * sin(place.x * 17.0);
    colour = place.y < hills ? CASTLE_HILLS : colour;
    float ripples = step(0.5, frac(place.y * 40.0 + sin(place.x * 20.0)));
    colour = place.y < 0.16 ? CASTLE_WATER * (0.9 + 0.2 * ripples) : colour;
    float keep = InBox(place, float2(0.38, 0.24), float2(0.62, 0.62));
    float towers = max(InBox(place, float2(0.24, 0.24), float2(0.34, 0.72)), InBox(place, float2(0.66, 0.24), float2(0.76, 0.72)));
    float battlements = InBox(place, float2(0.38, 0.62), float2(0.62, 0.67)) * step(0.5, frac(place.x * 30.0));
    colour = max(max(keep, towers), battlements) > 0.0 ? CASTLE_WALLS : colour;
    float roofs = max(UnderRoof(place, float2(0.29, 0.72), 0.07, 0.16), UnderRoof(place, float2(0.71, 0.72), 0.07, 0.16));
    colour = roofs > 0.0 ? CASTLE_ROOFS : colour;
    float windows = keep * InBox(place, float2(0.0, 0.44), float2(1.0, 0.52)) * step(0.4, frac(place.x * 14.0)) * step(frac(place.x * 14.0), 0.6);
    float door = InBox(place, float2(0.47, 0.24), float2(0.53, 0.35));
    colour = max(windows, door) > 0.0 ? CASTLE_OPENINGS : colour;
    return Painted(colour);
}

float3 Diamonds(float2 place, float size)
{
    float2 grid = place / size;
    float2 turned = float2(grid.x + grid.y, grid.x - grid.y);
    float2 cell = floor(turned);
    float2 inside = frac(turned);
    float kind = fmod(abs(cell.x + cell.y), 3.0);
    float3 colour = kind < 0.5 ? DIAMOND_RED : (kind < 1.5 ? DIAMOND_CREAM : DIAMOND_GREEN);
    float nearestEdge = min(min(inside.x, 1.0 - inside.x), min(inside.y, 1.0 - inside.y));
    return Painted(nearestEdge < 0.06 ? DIAMOND_EDGE : colour);
}

#endif
