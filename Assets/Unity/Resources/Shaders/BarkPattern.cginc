#ifndef SHALLOW_WATER_BARK_PATTERN_INCLUDED
#define SHALLOW_WATER_BARK_PATTERN_INCLUDED

#define BARK_FULL_TURN 6.2831853
#define PATTERN_NETTED 0.5
#define PATTERN_PAPERY 1.5
#define PATTERN_PLATED 2.5

float3 AroundTheLimb(float metresAlong, float turn, float furrowsAround, float furrowMetres)
{
    float reach = furrowsAround / BARK_FULL_TURN;
    return float3(cos(turn) * reach, sin(turn) * reach, metresAlong / furrowMetres);
}

float Furrowed(float3 place)
{
    float plates = ValueNoise3(place) * 0.65 + ValueNoise3(place * float3(2.1, 2.1, 1.3) + 7.0) * 0.35;
    float cracks = smoothstep(0.02, 0.08, abs(frac(place.z * 2.3 + plates * 1.6) - 0.5));
    return smoothstep(0.4, 0.5, plates) * lerp(0.7, 1.0, cracks);
}

float Netted(float3 place)
{
    float net = ValueNoise3(place * float3(1.0, 1.0, 1.6));
    float lines = smoothstep(0.03, 0.1, abs(net - 0.5)) * smoothstep(0.03, 0.1, abs(frac(net * 2.0 + 0.25) - 0.5));
    return lines;
}

float Papery(float3 place)
{
    float lenticels = ValueNoise3(place * float3(0.6, 0.6, 9.0));
    float isDash = smoothstep(0.7, 0.76, lenticels);
    float patches = smoothstep(0.72, 0.8, ValueNoise3(place * float3(0.35, 0.35, 0.5) + 3.0));
    return 1.0 - max(isDash, patches);
}

float Plated(float3 place)
{
    float plates = ValueNoise3(place * float3(0.8, 0.8, 0.7));
    return smoothstep(0.3, 0.45, plates) * smoothstep(0.02, 0.08, abs(frac(place.z * 1.2 + plates) - 0.5));
}

float RidgeAt(float3 place, float pattern)
{
    if (pattern > PATTERN_PLATED) return Plated(place);
    if (pattern > PATTERN_PAPERY) return Papery(place);
    if (pattern > PATTERN_NETTED) return Netted(place);
    return Furrowed(place);
}

#endif
