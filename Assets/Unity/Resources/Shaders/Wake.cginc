#ifndef SHALLOW_WATER_WAKE_INCLUDED
#define SHALLOW_WATER_WAKE_INCLUDED

#include "Ripples.cginc"
#include "Kelvin.cginc"

#define WASH_WIDTH 0.7
#define WASH_WIDENING 0.35
#define CHURN_LIFE_SECONDS 25.0
#define FOAM_LIFE_SECONDS 3.0
#define BREAKING_SPEED 1.8
#define CUSHION_HEIGHT 0.1
#define CUSHION_LENGTH 1.2
#define CUSHION_WIDTH 1.4
#define TOP_SPEED 3.0
#define STIRRED_SLOPE 0.05
#define STREAKS_ACROSS 3.0
#define STREAKS_ALONG 0.4

float4 _BoatPlace;
float4 _BoatWash;

struct Wake
{
    float2 slope;
    float churn;
    float foam;
    float swell;
};

float2 BoatFrame(float2 ground)
{
    float2 forward = _BoatHeading.xy;
    float2 starboard = float2(forward.y, -forward.x);
    float2 relative = ground - _BoatPlace.xy;
    return float2(dot(relative, forward), dot(relative, starboard));
}

TrailPoint AlongTheHull(float2 frame)
{
    TrailPoint hull;
    hull.behind = _BoatSize.x - frame.x;
    hull.side = abs(frame.y);
    hull.age = 0.0;
    hull.speed = abs(_BoatWash.y);
    hull.thrust = _BoatWash.x;
    hull.backward = -_BoatHeading.xy;
    hull.outward = float2(_BoatHeading.y, -_BoatHeading.x) * (frame.y < 0.0 ? -1.0 : 1.0);
    return hull;
}

float3 BowCushion(float2 frame)
{
    float speedShare = saturate(abs(_BoatWash.y) / TOP_SPEED);
    float ahead = (frame.x - _BoatSize.x) / CUSHION_LENGTH;
    float aside = frame.y / CUSHION_WIDTH;
    float bump = exp(-ahead * ahead - aside * aside) * CUSHION_HEIGHT * speedShare * speedShare;
    return float3(bump, -2.0 * ahead / CUSHION_LENGTH * bump, -2.0 * aside / CUSHION_WIDTH * bump);
}

Wake WakeAt(float2 ground, float time)
{
    float2 frame = BoatFrame(ground);
    TrailPoint trail = AlongTheHull(frame);
    if (frame.x < -_BoatSize.x) trail = OnTheTrail(ground);
    float3 waves = KelvinWaves(trail) * step(0.0, trail.behind);
    float3 cushion = BowCushion(frame);
    float2 forward = _BoatHeading.xy;
    float2 starboard = float2(forward.y, -forward.x);
    float washWidth = WASH_WIDTH + trail.age * WASH_WIDENING;
    float inTheWash = exp(-(trail.side * trail.side) / (washWidth * washWidth)) * step(2.0 * _BoatSize.x, trail.behind);
    float stirred = abs(trail.thrust) * inTheWash;
    float freshlyStirred = stirred * exp(-trail.age / FOAM_LIFE_SECONDS);
    float2 streakPlace = float2(trail.side * STREAKS_ACROSS, trail.behind * STREAKS_ALONG + time * 0.3);
    float froth = smoothstep(0.45, 0.8, ValueNoise(streakPlace) * 0.65 + ValueNoise(streakPlace * 2.7 + 5.0) * 0.35);
    float breaking = saturate(trail.speed - BREAKING_SPEED) * Breaking(trail) * step(0.0, trail.behind);
    Wake wake;
    wake.slope = trail.backward * waves.y + trail.outward * waves.z + forward * cushion.y + starboard * cushion.z;
    wake.slope += NoiseSlope(ground * 1.6 - forward * time * 0.8) * STIRRED_SLOPE * freshlyStirred;
    wake.churn = stirred * exp(-trail.age / CHURN_LIFE_SECONDS);
    wake.foam = saturate((freshlyStirred * 0.7 + breaking * 0.8 + cushion.x / CUSHION_HEIGHT * 0.8) * froth);
    wake.swell = abs(waves.x) + cushion.x;
    return wake;
}

#endif
