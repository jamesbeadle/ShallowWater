#ifndef SHALLOW_WATER_ROSES_INCLUDED
#define SHALLOW_WATER_ROSES_INCLUDED

#include "Paint.cginc"

#define ROSE_RED float3(0.66, 0.12, 0.1)
#define ROSE_PINK float3(0.86, 0.5, 0.48)
#define ROSE_YELLOW float3(0.86, 0.7, 0.3)
#define LEAF_GREEN float3(0.2, 0.36, 0.14)
#define LEAF_VEIN float3(0.62, 0.7, 0.34)
#define PETAL_STROKE float3(0.95, 0.9, 0.82)

float4 Rose(float2 place, float2 centre, float radius, float3 petals)
{
    float2 fromCentre = (place - centre) / radius;
    float reach = length(fromCentre);
    float turned = atan2(fromCentre.y, fromCentre.x);
    float rings = frac(reach * 2.6 + 0.12 * sin(turned * 5.0));
    bool isStroke = rings > 0.55 && rings < 0.8 && fromCentre.y > -0.35;
    float3 colour = lerp(petals * 0.35, petals, smoothstep(0.05, 0.45, reach));
    colour = isStroke ? lerp(colour, PETAL_STROKE, 0.55) : colour;
    return float4(Painted(colour), reach < 1.0 ? 1.0 : 0.0);
}

float4 Leaf(float2 place, float2 centre, float2 pointing, float2 size)
{
    float2 fromCentre = place - centre;
    float2 local = float2(dot(fromCentre, pointing), dot(fromCentre, float2(-pointing.y, pointing.x))) / size;
    float reach = length(local);
    bool isVein = abs(local.y) < 0.08 && local.x > -0.9;
    float3 colour = isVein ? LEAF_VEIN : LEAF_GREEN * lerp(0.8, 1.15, local.y * 0.5 + 0.5);
    return float4(Painted(colour), reach < 1.0 ? 1.0 : 0.0);
}

float4 Over(float4 under, float4 over)
{
    return over.a > 0.0 ? over : under;
}

float4 RoseCluster(float2 place, float2 centre, float size)
{
    float4 cluster = float4(0.0, 0.0, 0.0, 0.0);
    cluster = Over(cluster, Leaf(place, centre + float2(-1.3, 0.55) * size, normalize(float2(-1.0, 0.4)), float2(0.75, 0.3) * size));
    cluster = Over(cluster, Leaf(place, centre + float2(1.35, 0.6) * size, normalize(float2(1.0, 0.5)), float2(0.75, 0.3) * size));
    cluster = Over(cluster, Leaf(place, centre + float2(0.0, 1.2) * size, float2(0.0, 1.0), float2(0.6, 0.26) * size));
    cluster = Over(cluster, Rose(place, centre + float2(-0.85, -0.35) * size, 0.62 * size, ROSE_PINK));
    cluster = Over(cluster, Rose(place, centre + float2(0.9, -0.3) * size, 0.58 * size, ROSE_YELLOW));
    return Over(cluster, Rose(place, centre, 0.8 * size, ROSE_RED));
}

#endif
