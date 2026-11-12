#ifndef SHALLOW_WATER_TITLE_WOOD_INCLUDED
#define SHALLOW_WATER_TITLE_WOOD_INCLUDED

#include "TitleSky.cginc"

#define WOOD_FOOT 0.24
#define WOOD_STEEPNESS 2.6
#define HILL_FOOT 0.03
#define BACK_WOOD float3(0.42, 0.07, 2.0)
#define FRONT_WOOD float3(0.22, 0.06, 8.0)
#define CROWN_JITTER 0.8
#define CROWN_GIRTH float2(0.55, 0.45)
#define CROWN_LIFT 0.6
#define LEAF_GRAIN 90.0
#define LEAF_DEPTH 0.012

float Standing(float along)
{
    return smoothstep(WOOD_FOOT - 0.03, WOOD_FOOT + 0.12, -along);
}

float HillTop(float along, float rise)
{
    float uphill = 1.0 - exp(-max(-along - WOOD_FOOT, 0.0) * WOOD_STEEPNESS);
    return HORIZON + Standing(along) * (HILL_FOOT + rise * uphill);
}

float GapToCrown(float2 scene, float index, float3 wood)
{
    float2 chance = Hash22(float2(index, wood.z));
    float along = (index + 0.5 + (chance.x - 0.5) * CROWN_JITTER) * wood.y;
    float radius = wood.y * (CROWN_GIRTH.x + CROWN_GIRTH.y * chance.y) * Standing(along);
    float2 centre = float2(along, HillTop(along, wood.x) + radius * CROWN_LIFT);
    return GapToDisc(scene, centre, radius);
}

float GapToTheWood(float2 scene, float3 wood)
{
    float index = floor(scene.x / wood.y);
    float nearest = min(GapToCrown(scene, index - 1.0, wood), GapToCrown(scene, index + 1.0, wood));
    float crowns = min(nearest, GapToCrown(scene, index, wood));
    float hill = scene.y - HillTop(scene.x, wood.x);
    float leaves = (ValueNoise(scene * LEAF_GRAIN) - 0.5) * LEAF_DEPTH;
    return min(crowns, hill) + leaves;
}

float3 BackWoodOver(float3 behind, float2 scene, float pixel)
{
    float3 backWood = lerp(_Umber.rgb, _Oxblood.rgb, 0.3);
    return lerp(behind, backWood, Covered(GapToTheWood(scene, BACK_WOOD), pixel));
}

float3 FrontWoodOver(float3 behind, float2 scene, float pixel)
{
    float3 frontWood = lerp(_Night.rgb, _Umber.rgb, 0.5);
    return lerp(behind, frontWood, Covered(GapToTheWood(scene, FRONT_WOOD), pixel));
}

#endif
