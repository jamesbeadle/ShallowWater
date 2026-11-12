#ifndef SHALLOW_WATER_TITLE_BANK_INCLUDED
#define SHALLOW_WATER_TITLE_BANK_INCLUDED

#include "TitleSky.cginc"
#include "TitleWood.cginc"

#define RIDGE_RISE 0.05
#define RIDGE_ROLL 0.045
#define RIDGE_HAZE 0.3
#define HEDGE_FROM 0.4
#define HEDGE_RISE 0.03
#define POPLAR_HALF_WIDTH 0.016
#define POLE_THICKNESS 0.0045
#define POLE_TOP 0.215
#define POLES_STAND_AT float3(0.47, 0.7, 0.96)
#define ARM_HALF_WIDTH 0.02
#define ARMS_HANG float2(0.014, 0.034)
#define WIRE_SAG 0.02

float RidgeTop(float along)
{
    float roll = Fbm(float2(along * 1.1 + 3.0, 0.5));
    float crowns = ValueNoise(float2(along * 70.0, 1.5));
    return HORIZON + RIDGE_RISE + RIDGE_ROLL * roll + 0.004 * crowns;
}

float HedgeTop(float along)
{
    float bushes = Fbm(float2(along * 9.0 + 5.0, 4.0));
    float twigs = ValueNoise(float2(along * 80.0, 2.0));
    float standing = smoothstep(HEDGE_FROM, HEDGE_FROM + 0.05, along);
    return HORIZON + 0.012 + standing * (HEDGE_RISE * bushes + 0.006 * twigs);
}

float GapToPoplar(float2 scene, float along, float height)
{
    float sway = 0.06 * ValueNoise(float2(scene.y * 60.0, along * 31.0)) - 0.03;
    float2 radii = float2(POPLAR_HALF_WIDTH * (1.0 + sway * 4.0), height * 0.5);
    return GapToEllipse(scene, float2(along, HORIZON + height * 0.5), radii);
}

float GapToPole(float2 scene, float along)
{
    float top = HORIZON + POLE_TOP;
    float post = GapToBox(scene, float2(along, (HORIZON + top) * 0.5), float2(POLE_THICKNESS * 0.5, (top - HORIZON) * 0.5));
    float upperArm = GapToBox(scene, float2(along, top - ARMS_HANG.x), float2(ARM_HALF_WIDTH, 0.0022));
    float lowerArm = GapToBox(scene, float2(along, top - ARMS_HANG.y), float2(ARM_HALF_WIDTH * 0.8, 0.0022));
    return min(post, min(upperArm, lowerArm));
}

float GapToWire(float2 scene, float from, float to, float drop)
{
    float share = saturate((scene.x - from) / (to - from));
    float hang = HORIZON + POLE_TOP - drop - WIRE_SAG * 4.0 * share * (1.0 - share);
    float within = max(from - scene.x, scene.x - to);
    return max(abs(scene.y - hang) - 0.0007, within);
}

float GapToTheWires(float2 scene, float drop)
{
    float3 poles = POLES_STAND_AT;
    return min(GapToWire(scene, poles.x, poles.y, drop), GapToWire(scene, poles.y, poles.z, drop));
}

float GapToTheLine(float2 scene)
{
    float3 poles = POLES_STAND_AT;
    float2 arms = ARMS_HANG;
    float standing = min(GapToPole(scene, poles.x), GapToPole(scene, poles.y));
    float strung = min(GapToTheWires(scene, arms.x), GapToTheWires(scene, arms.y));
    return min(standing, strung);
}

float3 DistanceOver(float3 behind, float2 scene, float pixel)
{
    float3 ridge = lerp(lerp(_Oxblood.rgb, _Brick.rgb, 0.35), behind, RIDGE_HAZE);
    float3 picture = lerp(behind, ridge, Covered(scene.y - RidgeTop(scene.x), pixel));
    picture = BackWoodOver(picture, scene, pixel);
    picture = lerp(picture, _Umber.rgb, Covered(GapToTheLine(scene), pixel));
    float trees = min(GapToPoplar(scene, 0.56, 0.2), GapToPoplar(scene, 0.6, 0.165));
    trees = min(trees, scene.y - HedgeTop(scene.x));
    return lerp(picture, _Umber.rgb, Covered(trees, pixel));
}

#endif
