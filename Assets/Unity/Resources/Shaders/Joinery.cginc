#ifndef SHALLOW_WATER_JOINERY_INCLUDED
#define SHALLOW_WATER_JOINERY_INCLUDED

#include "Noise.cginc"

#define BOX_FRAME 0.055
#define GLAZING_BAR 0.018

fixed4 _Paint0;
fixed4 _Paint1;
fixed4 _Paint2;
fixed4 _Paint3;
fixed4 _Glass;
fixed4 _Room;

struct Joinery
{
    float3 colour;
    float smoothness;
    float relief;
};

struct JoineryInput
{
    float3 worldPos;
    float3 worldNormal;
    float2 surfacePlace;
    float4 fitting;
    INTERNAL_DATA
};

void JoineryVertex(inout appdata_full v, out JoineryInput o)
{
    UNITY_INITIALIZE_OUTPUT(JoineryInput, o);
    o.surfacePlace = v.texcoord.xy;
    o.fitting = v.texcoord1;
}

float3 PaintOf(float paint)
{
    float3 colour = _Paint0.rgb;
    colour = paint > 0.5 ? _Paint1.rgb : colour;
    colour = paint > 1.5 ? _Paint2.rgb : colour;
    colour = paint > 2.5 ? _Paint3.rgb : colour;
    return colour;
}

float InsideBy(float2 place, float2 size)
{
    float2 edges = min(place, size - place);
    return min(edges.x, edges.y);
}

bool IsOnABar(float along, float length, float divisions, float halfBar)
{
    float spacing = length / divisions;
    float fromABar = abs(frac(along / spacing + 0.5) - 0.5) * spacing;
    bool isInside = along > halfBar && along < length - halfBar;
    return isInside && fromABar < halfBar && divisions > 1.0;
}

Joinery Paintwork(float3 paint, float2 place, float relief)
{
    Joinery painted;
    float wear = smoothstep(0.62, 0.8, Fbm(place * 9.0));
    painted.colour = paint * lerp(1.0, 0.72, wear) * (0.93 + 0.07 * ValueNoise(place * 30.0));
    painted.smoothness = lerp(0.42, 0.18, wear);
    painted.relief = relief;
    return painted;
}

float3 OldGlassNormal(float3 normal, float3 position)
{
    float2 ripple = float2(ValueNoise(position.xy * 9.0 + position.z * 7.0), ValueNoise(position.zy * 9.0 + 3.7)) - 0.5;
    return normalize(normal + float3(ripple.x, ripple.y * 0.5, ripple.y) * 0.06);
}

#endif
