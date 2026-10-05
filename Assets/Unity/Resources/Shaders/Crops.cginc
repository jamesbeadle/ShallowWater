#ifndef SHALLOW_WATER_CROPS_INCLUDED
#define SHALLOW_WATER_CROPS_INCLUDED

#define FULL_TURN 6.2831853
#define RIDGE_AND_FURROW_METRES 7.0
#define RIDGE_SWAY_METRES 2.5
#define RIDGE_SWAY_LENGTH_METRES 70.0
#define TURF_PATCH_METRES 9.0
#define BLADE_METRES 0.04
#define TUFT_METRES 0.7

struct CropSurface
{
    float3 colour;
    float height;
    float smoothness;
};

CropSurface Made(float3 colour, float height, float smoothness)
{
    CropSurface made;
    made.colour = colour;
    made.height = height;
    made.smoothness = smoothness;
    return made;
}

float Detailed(float2 placeInCells, float near, float far)
{
    return lerp(far, near, PatternDetail(placeInCells));
}

float3 Soil(float2 ground)
{
    float patches = Fbm(ground / 7.0);
    float3 soil = lerp(_Ground.rgb, _GroundWorn.rgb, smoothstep(0.3, 0.75, patches));
    float2 crumbPlace = ground / 0.05;
    float crumbs = Detailed(crumbPlace, ValueNoise(crumbPlace), 0.5);
    return soil * lerp(0.85, 1.12, crumbs);
}

float3 Turf(float2 ground, out float blades)
{
    float patches = Fbm(ground / TURF_PATCH_METRES);
    float3 turf = lerp(_Ground.rgb, _GroundWorn.rgb, smoothstep(0.35, 0.8, patches));
    float2 bladePlace = ground / BLADE_METRES;
    blades = Detailed(bladePlace, ValueNoise(bladePlace), 0.5);
    return turf * lerp(0.88, 1.1, blades);
}

CropSurface Grass(float2 place, float2 ground)
{
    float blades;
    float3 turf = Turf(ground, blades);
    float2 tuftPlace = ground / TUFT_METRES;
    float tuftDetail = PatternDetail(tuftPlace);
    float clumping = ValueNoise(tuftPlace) * 0.6 + ValueNoise(tuftPlace * 3.1 + 5.3) * 0.25 + Fbm(ground / 4.0) * 0.15;
    float tufts = smoothstep(1.0 - _Cover, 1.2 - _Cover, clumping);
    float2 clumpPlace = ground / 0.22;
    float clumps = Detailed(clumpPlace, ValueNoise(clumpPlace), 0.5);
    turf *= lerp(0.9, 1.08, clumps);
    float2 strawPlace = ground / 0.05;
    float deadStems = step(0.8, ValueNoise(strawPlace)) * PatternDetail(strawPlace);
    float3 colour = lerp(turf, _Growth.rgb, lerp(_Cover * 0.5, tufts * 0.75, tuftDetail));
    colour = lerp(colour, _Accent.rgb, deadStems * tufts * 0.3);
    float sway = sin(place.x / RIDGE_SWAY_LENGTH_METRES) * RIDGE_SWAY_METRES;
    float ridge = 0.5 - 0.5 * cos((place.y + sway) / RIDGE_AND_FURROW_METRES * FULL_TURN);
    float hasRidges = saturate(_Relief * 4.0);
    colour *= lerp(1.0, lerp(0.9, 1.04, ridge), hasRidges);
    float height = ridge * _Relief + tufts * 0.015 * tuftDetail + (clumps - 0.5) * 0.008 + (blades - 0.5) * 0.006;
    return Made(colour, height, 0.08);
}

CropSurface HedgeBottom(float2 place, float2 ground)
{
    CropSurface grass = Grass(place, ground);
    float fromTheHedge = smoothstep(0.7, 1.7, place.y);
    float2 leafPlace = ground / 0.03;
    float fallenLeaves = step(0.55, ValueNoise(leafPlace)) * PatternDetail(leafPlace) * (1.0 - fromTheHedge);
    float3 underTheHedge = lerp(_GroundWorn.rgb * 0.5, _Accent.rgb * 0.55, fallenLeaves * 0.8);
    grass.colour = lerp(underTheHedge, grass.colour, fromTheHedge * 0.7 + 0.3);
    return grass;
}

CropSurface Footpath(float2 place, float2 ground)
{
    float wander = (ValueNoise(float2(place.x / 7.0, 3.3)) - 0.5) * 0.3;
    float halfBare = 0.26 + 0.12 * ValueNoise(float2(place.x / 4.0, 8.1));
    float offCentre = abs(place.y - wander);
    float2 tuftPlace = ground / 0.15;
    float tuftEdge = Detailed(tuftPlace, ValueNoise(tuftPlace), 0.5) - 0.5;
    float bare = 1.0 - smoothstep(halfBare * 0.6, halfBare + tuftEdge * 0.14, offCentre);
    float trodden = 1.0 - smoothstep(0.0, halfBare * 0.6, offCentre);
    float3 dust = lerp(_Ground.rgb, _GroundWorn.rgb, saturate(trodden * 0.6 + (Fbm(ground / 1.5) - 0.5) * 0.4));
    float2 stonePlace = ground / 0.04;
    dust = lerp(dust, _Accent.rgb, step(0.88, ValueNoise(stonePlace)) * PatternDetail(stonePlace));
    float2 bladePlace = ground / BLADE_METRES;
    float blades = Detailed(bladePlace, ValueNoise(bladePlace), 0.5);
    float3 turf = _Growth.rgb * lerp(0.86, 1.1, blades) * lerp(0.92, 1.06, Fbm(ground / TURF_PATCH_METRES));
    float dusted = 1.0 - smoothstep(halfBare, halfBare + 0.3, offCentre);
    turf = lerp(turf, dust, dusted * 0.25);
    float3 colour = lerp(turf, dust, bare);
    float height = -bare * _Relief + (1.0 - bare) * tuftEdge * 0.03;
    return Made(colour, height, 0.06);
}

#endif
