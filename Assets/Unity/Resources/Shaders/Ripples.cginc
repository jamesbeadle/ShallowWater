#ifndef SHALLOW_WATER_RIPPLES_INCLUDED
#define SHALLOW_WATER_RIPPLES_INCLUDED

#include "Noise.cginc"

#define FULL_TURN 6.2831853
#define RIPPLES_FADE_METRES 260.0
#define SLOPE_STEP 0.07

float2 Wave(float2 ground, float2 direction, float wavelength, float speed, float time)
{
    float phase = (dot(ground, direction) - time * speed) * FULL_TURN / wavelength;
    return direction * cos(phase);
}

float2 NoiseSlope(float2 place)
{
    float here = ValueNoise(place);
    float east = ValueNoise(place + float2(SLOPE_STEP, 0.0));
    float north = ValueNoise(place + float2(0.0, SLOPE_STEP));
    return float2(east - here, north - here) / SLOPE_STEP;
}

float2 RippleSlope(float2 ground, float time)
{
    float2 slope = Wave(ground, normalize(float2(0.83, 0.55)), 2.3, 0.35, time) * 0.035;
    slope += Wave(ground, normalize(float2(-0.42, 0.91)), 1.4, 0.28, time) * 0.03;
    slope += Wave(ground, normalize(float2(0.97, -0.25)), 0.9, 0.22, time) * 0.025;
    slope += Wave(ground, normalize(float2(-0.7, -0.71)), 0.55, 0.18, time) * 0.02;
    slope += NoiseSlope(ground * 1.7 + time * float2(0.11, 0.07)) * 0.028;
    slope += NoiseSlope(ground * 4.3 - time * float2(0.05, 0.13)) * 0.016;
    return slope;
}

#endif
