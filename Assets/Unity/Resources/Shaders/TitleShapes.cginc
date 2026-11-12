#ifndef SHALLOW_WATER_TITLE_SHAPES_INCLUDED
#define SHALLOW_WATER_TITLE_SHAPES_INCLUDED

float Covered(float gap, float pixel)
{
    return 1.0 - smoothstep(-pixel, pixel, gap);
}

float GapToDisc(float2 place, float2 centre, float radius)
{
    return length(place - centre) - radius;
}

float GapToBox(float2 place, float2 centre, float2 halfSize)
{
    float2 beyond = abs(place - centre) - halfSize;
    return length(max(beyond, float2(0.0, 0.0))) + min(max(beyond.x, beyond.y), 0.0);
}

float GapToEllipse(float2 place, float2 centre, float2 radii)
{
    float2 scaled = (place - centre) / radii;
    return (length(scaled) - 1.0) * min(radii.x, radii.y);
}

float GapToStroke(float2 place, float2 start, float2 finish, float thickness)
{
    float2 along = finish - start;
    float2 toPlace = place - start;
    float share = saturate(dot(toPlace, along) / dot(along, along));
    return length(toPlace - along * share) - thickness * 0.5;
}

float2 Turned(float2 place, float2 heading)
{
    return float2(dot(place, heading), dot(place, float2(-heading.y, heading.x)));
}

#endif
