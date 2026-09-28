#ifndef SHALLOW_WATER_GRIME_INCLUDED
#define SHALLOW_WATER_GRIME_INCLUDED

#include "Noise.cginc"

#define SOOT_RISES_FROM 0.9
#define SOOT_THICKEST_AT 1.9
#define CHIPS_PER_METRE 34.0
#define LUMINANCE_WEIGHTS float3(0.3, 0.59, 0.11)

float Sootiness(float3 aboard, float2 place, float soot)
{
    float high = smoothstep(SOOT_RISES_FROM, SOOT_THICKEST_AT, aboard.y);
    float streaks = Fbm(float2(place.x * 9.0, place.y * 1.1));
    float patches = Fbm(aboard.xz * 1.7 + aboard.y);
    return saturate(soot * (0.35 + 0.65 * high) * (0.55 + 0.9 * streaks) * (0.7 + 0.6 * patches));
}

float3 Faded(float3 colour, float fade)
{
    float grey = dot(colour, LUMINANCE_WEIGHTS);
    return lerp(colour, grey * float3(1.02, 1.0, 0.96) + 0.015, fade);
}

float Chipped(float2 place, float wear, float edgeCloseness)
{
    float2 chipPlace = place * CHIPS_PER_METRE;
    float chips = ValueNoise(chipPlace) * 0.65 + ValueNoise(chipPlace * 2.7) * 0.35;
    float threshold = 1.0 - wear * (0.25 + 0.75 * edgeCloseness);
    return smoothstep(threshold, threshold + 0.03, chips) * PatternDetail(chipPlace);
}

#endif
