#ifndef SHALLOW_WATER_TITLE_SKY_INCLUDED
#define SHALLOW_WATER_TITLE_SKY_INCLUDED

#include "Noise.cginc"
#include "TitleShapes.cginc"

#define HORIZON 0.30
#define SUN_CENTRE float2(0.12, 0.60)
#define SUN_RADIUS 0.19
#define SUN_GLOW_REACH 0.16
#define SUN_GLOW_STRENGTH 0.42
#define CLOUD_STRETCH 1.35
#define CLOUD_BANDS 15.0
#define CLOUD_DRIFT 0.006
#define CLOUD_EDGE float2(0.54, 0.62)
#define CLOUD_SHADE 0.55

float4 _Night;
float4 _Umber;
float4 _Oxblood;
float4 _Brick;
float4 _Ember;
float4 _Sunset;
float4 _Glow;

float3 SkyTone(float height)
{
    float3 low = lerp(_Brick.rgb, _Ember.rgb, 0.55);
    float3 sky = lerp(low, _Brick.rgb, smoothstep(0.0, 0.28, height));
    sky = lerp(sky, _Oxblood.rgb, smoothstep(0.18, 0.6, height));
    return lerp(sky, _Night.rgb, smoothstep(0.6, 1.6, height));
}

float3 SunTone(float2 fromTheSun)
{
    float lift = saturate(fromTheSun.y / SUN_RADIUS * 0.5 + 0.5);
    float3 body = lerp(_Brick.rgb, _Ember.rgb, smoothstep(0.0, 0.45, lift));
    body = lerp(body, _Sunset.rgb, smoothstep(0.35, 0.9, lift));
    float core = 1.0 - saturate(length(fromTheSun - float2(0.0, SUN_RADIUS * 0.25)) / SUN_RADIUS);
    return lerp(body, _Glow.rgb, core * core * 0.55);
}

float CloudBars(float2 scene, float seconds)
{
    float2 stretched = float2(scene.x * CLOUD_STRETCH + seconds * CLOUD_DRIFT, scene.y * CLOUD_BANDS);
    float streaks = Fbm(stretched) * 0.7 + Fbm(stretched * float2(2.3, 1.7) + 9.0) * 0.3;
    float aboveTheHaze = smoothstep(HORIZON + 0.03, HORIZON + 0.16, scene.y);
    return smoothstep(CLOUD_EDGE.x, CLOUD_EDGE.y, streaks) * aboveTheHaze;
}

float3 DuskSky(float2 scene, float pixel, float seconds)
{
    float height = (scene.y - HORIZON) / (1.0 - HORIZON);
    float3 sky = SkyTone(saturate(height));
    float2 fromTheSun = scene - SUN_CENTRE;
    float reach = length(fromTheSun);
    float glow = exp(-reach * reach / (SUN_GLOW_REACH * SUN_GLOW_REACH));
    sky += _Ember.rgb * glow * SUN_GLOW_STRENGTH;
    sky = lerp(sky, SunTone(fromTheSun), Covered(reach - SUN_RADIUS, pixel));
    float3 cloud = sky * (1.0 - CLOUD_SHADE) + _Oxblood.rgb * 0.12;
    return lerp(sky, cloud, CloudBars(scene, seconds));
}

#endif
