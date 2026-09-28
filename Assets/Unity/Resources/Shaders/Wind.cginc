#ifndef SHALLOW_WATER_WIND_INCLUDED
#define SHALLOW_WATER_WIND_INCLUDED

#define WIND_FULL_TURN 6.2831853
#define GUST_SPEED 1.1
#define GUST_WANDER 0.7
#define GUST_SWELL_SPEED 0.37
#define CALMEST_GUST 0.6
#define SWAY_HEIGHT_METRES 18.0

float TreeSeed()
{
    float3 root = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
    return Hash21(root.xz);
}

float3 Swayed(float3 place, float seed, float swayMetres)
{
    float lift = saturate(place.y / SWAY_HEIGHT_METRES);
    float phase = seed * WIND_FULL_TURN + _Time.y * GUST_SPEED;
    float swell = lerp(CALMEST_GUST, 1.0, 0.5 + 0.5 * sin(_Time.y * GUST_SWELL_SPEED + seed * WIND_FULL_TURN));
    return float3(sin(phase), 0, cos(phase * GUST_WANDER)) * lift * lift * swayMetres * swell;
}

#endif
