#ifndef SHALLOW_WATER_WAKE_INCLUDED
#define SHALLOW_WATER_WAKE_INCLUDED

#include "Ripples.cginc"

#define KELVIN_SPREAD 0.354
#define WASH_WIDENING 0.16
#define WASH_WIDTH 0.6
#define WASH_LENGTH 25.0
#define ARM_LENGTH 45.0
#define ARM_SHARPNESS 3.0
#define ALONGSIDE_SHARPNESS 3.0

float4 _BoatPlace;
float4 _BoatHeading;
float4 _BoatVelocity;
float4 _BoatSize;

struct Wake
{
    float2 slope;
    float churn;
    float foam;
};

float2 BoatFrame(float2 ground)
{
    float2 forward = _BoatHeading.xy;
    float2 starboard = float2(forward.y, -forward.x);
    float2 relative = ground - _BoatPlace.xy;
    return float2(dot(relative, forward), dot(relative, starboard));
}

Wake WakeAt(float2 ground, float time)
{
    float2 frame = BoatFrame(ground);
    float speed = _BoatVelocity.w;
    float behindTheStern = -_BoatSize.x - frame.x;
    float washWidth = WASH_WIDTH + max(behindTheStern, 0.0) * WASH_WIDENING;
    float wash = speed * step(0.0, behindTheStern) * saturate(1.0 - abs(frame.y) / washWidth) * saturate(1.0 - behindTheStern / WASH_LENGTH);
    float behindTheBow = _BoatSize.x - frame.x;
    float armOffset = abs(abs(frame.y) - (_BoatSize.y + max(behindTheBow, 0.0) * KELVIN_SPREAD));
    float arm = speed * step(0.0, behindTheBow) * exp(-armOffset * armOffset * ARM_SHARPNESS) * saturate(1.0 - behindTheBow / ARM_LENGTH);
    float beside = max(abs(frame.y) - _BoatSize.y, 0.0) * ALONGSIDE_SHARPNESS;
    float alongside = speed * step(0.0, behindTheBow) * step(0.0, frame.x + _BoatSize.x) * exp(-beside * beside);
    float2 forward = _BoatHeading.xy;
    Wake wake;
    wake.slope = NoiseSlope(ground * 5.0 - forward * time * 1.5) * 0.1 * wash + NoiseSlope(ground * 2.2 + time * 0.4) * 0.12 * (arm + alongside);
    wake.churn = wash;
    wake.foam = saturate(wash * 0.5 + arm * 0.3 + alongside * 0.35) * smoothstep(0.45, 0.75, ValueNoise(ground * 6.0 - forward * time));
    return wake;
}

#endif
