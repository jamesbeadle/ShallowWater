#ifndef SHALLOW_WATER_SKY_INCLUDED
#define SHALLOW_WATER_SKY_INCLUDED

#include "Noise.cginc"

float4 _SunDirection;
float4 _SkyZenith;
float4 _SkyHorizon;
float4 _SkyHaze;
float4 _SunGlow;
float4 _CloudLight;
float4 _CloudShade;
float _CloudCover;
float _StarShare;

#define CLOUD_DRIFT float2(0.004, 0.0015)
#define CLOUD_SCALE 1.7
#define CLOUD_DECK 0.12
#define STAR_CELLS 600.0
#define STAR_RARITY 0.9985
#define STAR_BRIGHTNESS 1.2
#define STAR_TWINKLE_SPEED 3.0

float3 SkyGradient(float3 direction)
{
    float up = saturate(direction.y);
    float3 sky = lerp(_SkyHorizon.rgb, _SkyZenith.rgb, pow(up, 0.45));
    float nearTheHorizon = pow(1.0 - up, 8.0);
    sky = lerp(sky, _SkyHaze.rgb, nearTheHorizon);
    float towardsTheSun = saturate(dot(direction, _SunDirection.xyz));
    float glow = pow(towardsTheSun, 8.0) * 0.35 + pow(towardsTheSun, 64.0) * 0.6;
    return sky + _SunGlow.rgb * glow;
}

float3 Stars(float3 direction)
{
    float chance = Hash31(floor(direction * STAR_CELLS));
    float isAStar = step(STAR_RARITY, chance);
    float twinkle = 0.6 + 0.4 * sin(_Time.y * STAR_TWINKLE_SPEED + chance * 400.0);
    float aboveTheHaze = smoothstep(0.05, 0.25, direction.y);
    return isAStar * twinkle * aboveTheHaze * _StarShare * STAR_BRIGHTNESS;
}

float3 CloudsOver(float3 sky, float3 direction)
{
    float up = saturate(direction.y);
    float2 cloudPlace = direction.xz / (up + CLOUD_DECK) * CLOUD_SCALE + _Time.y * CLOUD_DRIFT;
    float density = Fbm(cloudPlace);
    float thickness = smoothstep(1.0 - _CloudCover, 1.25 - _CloudCover, density);
    float lit = saturate(1.0 - Fbm(cloudPlace + _SunDirection.xz * 0.15) + 0.35);
    float towardsTheSun = saturate(dot(direction, _SunDirection.xyz));
    float3 cloud = lerp(_CloudShade.rgb, _CloudLight.rgb, lit) + _SunGlow.rgb * pow(towardsTheSun, 12.0) * 0.5;
    float aboveTheHaze = smoothstep(0.02, 0.22, up);
    return lerp(sky, cloud, thickness * aboveTheHaze);
}

float3 SkyColour(float3 direction)
{
    float3 sky = SkyGradient(direction) + Stars(direction);
    return CloudsOver(sky, direction);
}

#endif
