#ifndef SHALLOW_WATER_TITLE_BRIDGE_INCLUDED
#define SHALLOW_WATER_TITLE_BRIDGE_INCLUDED

#include "TitleSky.cginc"

#define BRIDGE_HALF_SPAN 0.37
#define PARAPET_AT_THE_ENDS 0.185
#define PARAPET_HUMP 0.085
#define HUMP_WIDTH 0.05
#define PIER_HALF_WIDTH 0.016
#define PIER_RISE 0.018
#define ARCH_RADII float2(0.135, 0.165)
#define RING_THICKNESS 0.016
#define COURSE_BELOW_THE_TOP 0.04
#define TOWPATH_FROM 0.075
#define TOWPATH_RISE 0.028
#define RIM_THICKNESS 0.0035
#define RIM_REACH 0.09
#define SOFFIT_DEPTH 0.011
#define COPING_THICKNESS 0.003
#define WING_REACH 0.2
#define WING_FOOT 0.035

float ParapetTop(float along)
{
    return HORIZON + PARAPET_AT_THE_ENDS + PARAPET_HUMP * exp(-along * along / HUMP_WIDTH);
}

float WingTop(float beyond)
{
    float falling = 1.0 - smoothstep(0.0, WING_REACH, beyond);
    return HORIZON + WING_FOOT + (PARAPET_AT_THE_ENDS - WING_FOOT) * falling * falling;
}

float GapToTheWall(float2 scene)
{
    float beyond = abs(scene.x) - BRIDGE_HALF_SPAN;
    float wall = max(scene.y - ParapetTop(scene.x), beyond);
    float wing = max(scene.y - WingTop(beyond), -beyond);
    float2 pierCentre = float2(BRIDGE_HALF_SPAN, HORIZON + (PARAPET_AT_THE_ENDS + PIER_RISE) * 0.5);
    float2 pierHalfSize = float2(PIER_HALF_WIDTH, (PARAPET_AT_THE_ENDS + PIER_RISE) * 0.5);
    float piers = GapToBox(float2(abs(scene.x), scene.y), pierCentre, pierHalfSize);
    return min(min(wall, wing), piers);
}

float GapToTheOpening(float2 scene)
{
    return GapToEllipse(scene, float2(0.0, HORIZON), ARCH_RADII);
}

float GapToTheTowpath(float2 scene)
{
    return max(TOWPATH_FROM - scene.x, scene.y - HORIZON - TOWPATH_RISE);
}

float3 BridgeOver(float3 behind, float2 scene, float pixel)
{
    float wall = GapToTheWall(scene);
    float opening = GapToTheOpening(scene);
    float towpath = GapToTheTowpath(scene);
    float masonry = Covered(max(wall, -max(opening, -towpath)), pixel);
    float upTheWall = saturate((scene.y - HORIZON) / (PARAPET_AT_THE_ENDS + PARAPET_HUMP));
    float3 brick = lerp(lerp(_Night.rgb, _Umber.rgb, 0.6), lerp(_Umber.rgb, _Oxblood.rgb, 0.22), upTheWall * upTheWall);
    float ring = Covered(abs(opening - RING_THICKNESS * 0.5) - RING_THICKNESS * 0.5, pixel);
    float course = Covered(abs(scene.y - ParapetTop(scene.x) + COURSE_BELOW_THE_TOP) - 0.002, pixel) * step(abs(scene.x), BRIDGE_HALF_SPAN);
    float coping = Covered(abs(towpath + COPING_THICKNESS * 0.5) - COPING_THICKNESS * 0.5, pixel) * step(opening, 0.0) * step(HORIZON + TOWPATH_RISE * 0.5, scene.y);
    brick = lerp(brick, lerp(_Umber.rgb, _Oxblood.rgb, 0.4), max(max(ring, course), coping) * 0.6);
    float towardsTheSun = exp(-(scene.x - SUN_CENTRE.x) * (scene.x - SUN_CENTRE.x) / RIM_REACH);
    float rim = Covered(abs(wall + RIM_THICKNESS * 0.5) - RIM_THICKNESS * 0.5, pixel) * step(abs(scene.x), BRIDGE_HALF_SPAN - PIER_HALF_WIDTH);
    brick = lerp(brick, _Ember.rgb, rim * towardsTheSun * 0.9);
    float3 picture = lerp(behind, brick, masonry);
    float soffit = Covered(abs(opening + SOFFIT_DEPTH * 0.5) - SOFFIT_DEPTH * 0.5, pixel) * step(HORIZON + TOWPATH_RISE, scene.y);
    return lerp(picture, lerp(_Umber.rgb, _Brick.rgb, 0.45), soffit);
}

#endif
