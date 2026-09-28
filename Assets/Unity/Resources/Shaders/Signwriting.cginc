#ifndef SHALLOW_WATER_SIGNWRITING_INCLUDED
#define SHALLOW_WATER_SIGNWRITING_INCLUDED

#define FULL_TURN_RADIANS 6.2831853
#define STROKE_HALF_WIDTH 0.075
#define NAME_WIDTH 5.41

float StrokeTo(float2 place, float2 from, float2 to)
{
    float2 fromStart = place - from;
    float2 run = to - from;
    float share = saturate(dot(fromStart, run) / dot(run, run));
    return length(fromStart - run * share);
}

float ArcTo(float2 place, float2 centre, float radius, float fromRadians, float sweepRadians)
{
    float2 fromCentre = place - centre;
    float turned = atan2(fromCentre.y, fromCentre.x) - fromRadians;
    turned -= FULL_TURN_RADIANS * floor(turned / FULL_TURN_RADIANS);
    float2 start = centre + radius * float2(cos(fromRadians), sin(fromRadians));
    float endRadians = fromRadians + sweepRadians;
    float2 end = centre + radius * float2(cos(endRadians), sin(endRadians));
    float toTheEnds = min(length(place - start), length(place - end));
    return turned <= sweepRadians ? abs(length(fromCentre) - radius) : toTheEnds;
}

float LetterS(float2 place)
{
    float upper = ArcTo(place, float2(0.33, 0.705), 0.22, 0.35, 4.36);
    float lower = ArcTo(place, float2(0.33, 0.295), 0.22, 3.49, 4.36);
    return min(upper, lower);
}

float LetterP(float2 place)
{
    float stem = StrokeTo(place, float2(0.075, 0.075), float2(0.075, 0.925));
    float bars = min(StrokeTo(place, float2(0.075, 0.925), float2(0.35, 0.925)), StrokeTo(place, float2(0.075, 0.475), float2(0.35, 0.475)));
    float bowl = ArcTo(place, float2(0.35, 0.7), 0.225, -1.5708, 3.1416);
    return min(stem, min(bars, bowl));
}

float LetterA(float2 place)
{
    float left = StrokeTo(place, float2(0.07, 0.075), float2(0.36, 0.925));
    float right = StrokeTo(place, float2(0.36, 0.925), float2(0.65, 0.075));
    return min(min(left, right), StrokeTo(place, float2(0.2, 0.36), float2(0.52, 0.36)));
}

float LetterR(float2 place)
{
    return min(LetterP(place), StrokeTo(place, float2(0.3, 0.475), float2(0.62, 0.075)));
}

float LetterO(float2 place)
{
    float2 radii = float2(0.285, 0.425);
    float2 fromCentre = (place - float2(0.36, 0.5)) / radii;
    return abs(length(fromCentre) - 1.0) * min(radii.x, radii.y);
}

float LetterW(float2 place)
{
    float outer = min(StrokeTo(place, float2(0.05, 0.925), float2(0.24, 0.075)), StrokeTo(place, float2(0.66, 0.075), float2(0.85, 0.925)));
    float inner = min(StrokeTo(place, float2(0.24, 0.075), float2(0.45, 0.7)), StrokeTo(place, float2(0.45, 0.7), float2(0.66, 0.075)));
    return min(outer, inner);
}

float SparrowName(float2 place)
{
    float nearest = LetterS(place);
    nearest = min(nearest, LetterP(place - float2(0.72, 0.0)));
    nearest = min(nearest, LetterA(place - float2(1.42, 0.0)));
    nearest = min(nearest, LetterR(place - float2(2.2, 0.0)));
    nearest = min(nearest, LetterR(place - float2(2.94, 0.0)));
    nearest = min(nearest, LetterO(place - float2(3.68, 0.0)));
    nearest = min(nearest, LetterW(place - float2(4.46, 0.0)));
    return nearest - STROKE_HALF_WIDTH;
}

#endif
