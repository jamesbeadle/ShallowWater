#ifndef SHALLOW_WATER_TITLE_WATER_INCLUDED
#define SHALLOW_WATER_TITLE_WATER_INCLUDED

#include "TitleSky.cginc"

#define WATER_NEARNESS 0.035
#define RIPPLE_GRAIN float2(2.6, 1.9)
#define RIPPLE_FLOW float2(0.03, 0.22)
#define RIPPLE_SWAY 0.1
#define RIPPLE_SCATTER float2(0.12, 1.1)
#define STREAK_GRAIN float2(0.7, 9.0)
#define STREAK_SHADE 0.5
#define WATER_DEEPENING float2(0.22, 0.6)
#define GLINT_GRAIN float2(1.6, 15.0)
#define GLINT_EDGE float2(0.7, 0.8)
#define GLINT_STRENGTH 0.7
#define GLINT_PATH float2(0.035, 0.75)
#define SHORE_MIST 0.3
#define SHORE_MIST_REACH 70.0

float2 WaterGround(float2 scene)
{
    float depth = max(HORIZON - scene.y, 0.0);
    return float2(scene.x, 1.0) / (depth + WATER_NEARNESS);
}

float2 Reflected(float2 scene, float seconds)
{
    float depth = max(HORIZON - scene.y, 0.0);
    float2 ground = WaterGround(scene) * RIPPLE_GRAIN + seconds * RIPPLE_FLOW;
    float sway = ValueNoise(ground) - 0.5;
    float scatter = ValueNoise(ground * float2(0.6, 2.3) + 31.0) - 0.5;
    float scattering = RIPPLE_SCATTER.x + RIPPLE_SCATTER.y * depth;
    return float2(scene.x + sway * RIPPLE_SWAY * depth, HORIZON + depth * (1.0 + scatter * scattering));
}

float3 WaterFrom(float3 reflected, float2 scene, float seconds)
{
    float depth = max(HORIZON - scene.y, 0.0);
    float deepening = lerp(WATER_DEEPENING.x, WATER_DEEPENING.y, smoothstep(0.0, HORIZON, depth));
    float3 water = lerp(reflected, _Night.rgb, deepening);
    float streaks = ValueNoise(WaterGround(scene) * STREAK_GRAIN + seconds * float2(0.02, 0.12));
    water *= 1.0 - STREAK_SHADE * smoothstep(0.35, 0.7, streaks) * smoothstep(0.0, 0.08, depth);
    float2 ground = WaterGround(scene) * GLINT_GRAIN + seconds * float2(0.03, 0.7);
    float glints = smoothstep(GLINT_EDGE.x, GLINT_EDGE.y, ValueNoise(ground));
    float fromThePath = (scene.x - SUN_CENTRE.x) / (GLINT_PATH.x + depth * GLINT_PATH.y);
    float onThePath = exp(-fromThePath * fromThePath) * smoothstep(0.03, HORIZON, depth);
    water += lerp(_Sunset.rgb, _Glow.rgb, 0.5) * glints * onThePath * GLINT_STRENGTH;
    float3 mist = lerp(_Brick.rgb, _Ember.rgb, 0.35);
    return lerp(water, mist, exp(-depth * SHORE_MIST_REACH) * SHORE_MIST);
}

#endif
