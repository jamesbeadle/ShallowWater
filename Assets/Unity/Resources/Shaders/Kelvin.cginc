#ifndef SHALLOW_WATER_KELVIN_INCLUDED
#define SHALLOW_WATER_KELVIN_INCLUDED

#define WAKE_SAMPLES 24
#define KELVIN_SPREAD 0.354
#define WAVELENGTH_PER_SPEED_SQUARED 0.64
#define SHORTEST_WAVELENGTH 1.2
#define LONGEST_WAVELENGTH 8.0
#define WAVE_HEIGHT_PER_SPEED_SQUARED 0.015
#define WAVE_LIFE_SECONDS 7.0
#define TRANSVERSE_SHARE 0.6
#define ARM_WIDTH 1.2
#define ARM_WIDENING 0.08
#define DIVERGENT_ACROSS 0.94
#define DIVERGENT_ALONG 0.34
#define DIVERGENT_SCALE 1.3
#define SHORTEST_RUN 0.5
#define FULL_WAVE 6.2831853

float4 _WakePath[WAKE_SAMPLES];
float4 _WakeThrust[WAKE_SAMPLES];
float _WakeSamples;
float4 _BoatHeading;
float4 _BoatSize;

struct TrailPoint
{
    float behind;
    float side;
    float age;
    float speed;
    float thrust;
    float2 backward;
    float2 outward;
};

TrailPoint OnTheTrail(float2 ground)
{
    TrailPoint nearest = (TrailPoint)0;
    float nearestDistance = 1e6;
    float travelled = _BoatSize.x * 2.0;
    for (int segment = 0; segment < WAKE_SAMPLES - 1; segment++)
    {
        if (float(segment + 1) >= _WakeSamples) break;
        float4 newer = _WakePath[segment];
        float4 older = _WakePath[segment + 1];
        float2 run = older.xy - newer.xy;
        float runLength = max(length(run), 0.001);
        float2 backward = runLength > SHORTEST_RUN ? run / runLength : -_BoatHeading.xy;
        float share = saturate(dot(ground - newer.xy, run) / (runLength * runLength));
        float2 off = ground - (newer.xy + run * share);
        float offLength = length(off);
        if (offLength < nearestDistance)
        {
            nearestDistance = offLength;
            nearest.behind = travelled + share * runLength;
            nearest.side = offLength;
            nearest.age = lerp(newer.z, older.z, share);
            nearest.speed = lerp(newer.w, older.w, share);
            nearest.thrust = lerp(_WakeThrust[segment].x, _WakeThrust[segment + 1].x, share);
            nearest.backward = backward;
            nearest.outward = off / max(offLength, 0.001);
        }
        travelled += runLength;
    }
    return nearest;
}

float3 KelvinWaves(TrailPoint trail)
{
    float wavelength = clamp(trail.speed * trail.speed * WAVELENGTH_PER_SPEED_SQUARED, SHORTEST_WAVELENGTH, LONGEST_WAVELENGTH);
    float waveNumber = FULL_WAVE / wavelength;
    float arm = _BoatSize.y + trail.behind * KELVIN_SPREAD;
    float amplitude = WAVE_HEIGHT_PER_SPEED_SQUARED * trail.speed * trail.speed * exp(-trail.age / WAVE_LIFE_SECONDS);
    float inside = smoothstep(arm + 1.0, arm - 1.5, trail.side) * TRANSVERSE_SHARE;
    float transversePhase = waveNumber * trail.behind;
    float offTheArm = (trail.side - arm) / (ARM_WIDTH + trail.behind * ARM_WIDENING);
    float envelope = exp(-offTheArm * offTheArm);
    float divergentNumber = waveNumber * DIVERGENT_SCALE;
    float divergentPhase = divergentNumber * (trail.side * DIVERGENT_ACROSS + trail.behind * DIVERGENT_ALONG);
    float height = cos(transversePhase) * inside + cos(divergentPhase) * envelope;
    float alongSlope = -waveNumber * sin(transversePhase) * inside - divergentNumber * DIVERGENT_ALONG * sin(divergentPhase) * envelope;
    float acrossSlope = -divergentNumber * DIVERGENT_ACROSS * sin(divergentPhase) * envelope;
    return float3(height, alongSlope, acrossSlope) * amplitude;
}

float Breaking(TrailPoint trail)
{
    float wavelength = clamp(trail.speed * trail.speed * WAVELENGTH_PER_SPEED_SQUARED, SHORTEST_WAVELENGTH, LONGEST_WAVELENGTH);
    float divergentNumber = FULL_WAVE / wavelength * DIVERGENT_SCALE;
    float crest = cos(divergentNumber * (trail.side * DIVERGENT_ACROSS + trail.behind * DIVERGENT_ALONG));
    float arm = _BoatSize.y + trail.behind * KELVIN_SPREAD;
    float offTheArm = (trail.side - arm) / (ARM_WIDTH + trail.behind * ARM_WIDENING);
    return exp(-offTheArm * offTheArm) * saturate(crest * 2.0 - 1.0) * exp(-trail.age / WAVE_LIFE_SECONDS);
}

#endif
