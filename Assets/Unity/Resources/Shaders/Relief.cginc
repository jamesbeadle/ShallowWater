#ifndef SHALLOW_WATER_RELIEF_INCLUDED
#define SHALLOW_WATER_RELIEF_INCLUDED

#define VERTEX_NORMAL(IN) WorldNormalVector(IN, float3(0, 0, 1))
#define WORLD_TO_TANGENT(IN, worldNormal) normalize(mul(float3x3(WorldNormalVector(IN, float3(1, 0, 0)), WorldNormalVector(IN, float3(0, 1, 0)), WorldNormalVector(IN, float3(0, 0, 1))), worldNormal))

float3 Raised(float3 position, float3 normal, float heightMetres)
{
    float3 acrossTheScreen = ddx(position);
    float3 downTheScreen = ddy(position);
    float3 firstPerpendicular = cross(downTheScreen, normal);
    float3 secondPerpendicular = cross(normal, acrossTheScreen);
    float determinant = dot(acrossTheScreen, firstPerpendicular);
    float3 gradient = sign(determinant) * (ddx(heightMetres) * firstPerpendicular + ddy(heightMetres) * secondPerpendicular);
    return normalize(abs(determinant) * normal - gradient);
}

#endif
