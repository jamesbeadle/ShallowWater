#ifndef SHALLOW_WATER_TITLE_CHAIN_INCLUDED
#define SHALLOW_WATER_TITLE_CHAIN_INCLUDED

#include "TitleSky.cginc"

#define CHAIN_MIDDLE (-0.2)
#define CHAIN_HALF_SPAN 0.9
#define CHAIN_LOWEST 0.085
#define CHAIN_SAG 0.1
#define LINK_PITCH 0.031
#define LINK_HALF_SIZE float2(0.02, 0.0105)
#define LINK_WIRE 0.0046
#define POST_HALF_SIZE float2(0.021, 0.105)
#define POST_CAP_HALF_SIZE float2(0.026, 0.011)

float ChainHeight(float along)
{
    float share = (along - CHAIN_MIDDLE) / CHAIN_HALF_SPAN;
    return CHAIN_LOWEST + CHAIN_SAG * share * share;
}

float GapToLink(float2 scene, float index)
{
    float along = (index + 0.5) * LINK_PITCH;
    float slope = 2.0 * CHAIN_SAG * (along - CHAIN_MIDDLE) / (CHAIN_HALF_SPAN * CHAIN_HALF_SPAN);
    float2 fromTheLink = Turned(scene - float2(along, ChainHeight(along)), normalize(float2(1.0, slope)));
    float corner = LINK_HALF_SIZE.y - LINK_WIRE * 0.5;
    float flatLink = abs(GapToBox(fromTheLink, float2(0.0, 0.0), LINK_HALF_SIZE - corner) - corner + LINK_WIRE * 0.5) - LINK_WIRE * 0.5;
    float edgeLink = GapToBox(fromTheLink, float2(0.0, 0.0), float2(LINK_HALF_SIZE.x - LINK_WIRE, 0.0)) - LINK_WIRE * 0.62;
    float isOnEdge = step(0.5, frac(index * 0.5));
    return lerp(flatLink, edgeLink, isOnEdge);
}

float GapToTheChain(float2 scene)
{
    float index = floor(scene.x / LINK_PITCH);
    float links = min(GapToLink(scene, index - 1.0), min(GapToLink(scene, index), GapToLink(scene, index + 1.0)));
    float between = abs(scene.x - CHAIN_MIDDLE) - CHAIN_HALF_SPAN;
    return max(links, between);
}

float GapToThePosts(float2 scene)
{
    float2 mirrored = float2(abs(scene.x - CHAIN_MIDDLE), scene.y);
    float post = GapToBox(mirrored, float2(CHAIN_HALF_SPAN + POST_HALF_SIZE.x, POST_HALF_SIZE.y), POST_HALF_SIZE);
    float2 capCentre = float2(CHAIN_HALF_SPAN + POST_HALF_SIZE.x, POST_HALF_SIZE.y * 2.0 + POST_CAP_HALF_SIZE.y);
    return min(post, GapToBox(mirrored, capCentre, POST_CAP_HALF_SIZE) - 0.003);
}

float3 ChainOver(float3 behind, float2 scene, float pixel)
{
    float ironwork = min(GapToTheChain(scene), GapToThePosts(scene));
    return lerp(behind, _Night.rgb * 0.6, Covered(ironwork, pixel));
}

#endif
