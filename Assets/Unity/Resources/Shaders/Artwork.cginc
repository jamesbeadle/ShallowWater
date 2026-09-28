#ifndef SHALLOW_WATER_ARTWORK_INCLUDED
#define SHALLOW_WATER_ARTWORK_INCLUDED

#include "Signwriting.cginc"
#include "Roses.cginc"
#include "Castle.cginc"

#define LETTER_HEIGHT 0.26
#define NAME_FOOT 1.09
#define SHADE_OFFSET float2(0.07, -0.07)
#define ROSES_FROM_THE_MIDDLE 1.02
#define ROSES_HEIGHT 1.22
#define ROSE_SIZE 0.085
#define ENGINE_DOORS float3(-6.1, -5.6, -5.1)
#define ENGINE_DOOR_HEIGHTS float2(0.9, 1.55)
#define DOOR_GAP 0.009
#define KNOBS float3(-5.66, -5.54, 1.22)
#define KNOB_RADIUS 0.018
#define PORTHOLE float3(-4.72, 1.25, 0.1)
#define PORTHOLE_RIM 0.022
#define TARNISHED_BRASS float3(0.36, 0.29, 0.15)
#define PORTHOLE_GLASS float3(0.06, 0.07, 0.08)
#define CASTLE_ABOVE 0.52
#define DOOR_ROSES_HEIGHT 0.26
#define DOOR_ROSE_SIZE 0.07
#define CAN_BANDS float4(0.03, 0.06, 0.24, 0.27)
#define CAN_SHOULDER 0.28
#define CAN_ROSES float3(0.2, 0.61, 0.15)
#define CAN_ROSE_SIZE 0.05

struct Artwork
{
    float3 colour;
    float coverage;
    float relief;
};

Artwork Blank()
{
    Artwork artwork;
    artwork.colour = float3(0.0, 0.0, 0.0);
    artwork.coverage = 0.0;
    artwork.relief = 0.0;
    return artwork;
}

Artwork Laid(Artwork under, float4 over, float relief)
{
    Artwork artwork = under;
    if (over.a > 0.0)
    {
        artwork.colour = over.rgb;
        artwork.coverage = 1.0;
        artwork.relief = relief;
    }
    return artwork;
}

Artwork NameAndRoses(float2 place, float side, float middle, float3 lettering, float3 shade)
{
    float2 letterPlace = float2((place.x - middle) * side, place.y - NAME_FOOT) / LETTER_HEIGHT + float2(NAME_WIDTH * 0.5, 0.0);
    float fill = SparrowName(letterPlace);
    float shading = SparrowName(letterPlace - SHADE_OFFSET);
    Artwork artwork = Blank();
    artwork = Laid(artwork, float4(shade, shading < 0.0 ? 1.0 : 0.0), 0.0003);
    artwork = Laid(artwork, float4(lettering, fill < 0.0 ? 1.0 : 0.0), 0.0005);
    float2 rosePlace = float2((place.x - middle) * side, place.y);
    artwork = Laid(artwork, RoseCluster(rosePlace, float2(-ROSES_FROM_THE_MIDDLE, ROSES_HEIGHT), ROSE_SIZE), 0.0006);
    return Laid(artwork, RoseCluster(rosePlace, float2(ROSES_FROM_THE_MIDDLE, ROSES_HEIGHT), ROSE_SIZE), 0.0006);
}

Artwork EngineRoomSide(float2 place)
{
    float onTheDoors = step(ENGINE_DOORS.x, place.x) * step(place.x, ENGINE_DOORS.z) * step(ENGINE_DOOR_HEIGHTS.x, place.y) * step(place.y, ENGINE_DOOR_HEIGHTS.y);
    float3 fromTheEdges = abs(place.xxx - ENGINE_DOORS);
    float2 fromTheTops = abs(place.yy - ENGINE_DOOR_HEIGHTS);
    float isGap = onTheDoors * step(min(min(fromTheEdges.x, fromTheEdges.y), min(fromTheEdges.z, min(fromTheTops.x, fromTheTops.y))), DOOR_GAP);
    float isKnob = step(min(length(place - KNOBS.xz), length(place - KNOBS.yz)), KNOB_RADIUS);
    float fromThePorthole = length(place - PORTHOLE.xy);
    float isRim = step(PORTHOLE.z - PORTHOLE_RIM, fromThePorthole) * step(fromThePorthole, PORTHOLE.z);
    float isGlass = step(fromThePorthole, PORTHOLE.z - PORTHOLE_RIM);
    Artwork artwork = Blank();
    artwork = Laid(artwork, float4(Painted(float3(0.05, 0.05, 0.05)), isGap), -0.004);
    artwork = Laid(artwork, float4(Painted(TARNISHED_BRASS), max(isKnob, isRim)), 0.008);
    return Laid(artwork, float4(Painted(PORTHOLE_GLASS), isGlass), -0.004);
}

Artwork DoorPainting(float2 place, float2 door, float2 heights)
{
    float2 inDoor = float2((place.x - door.x) / (door.y - door.x), (place.y - heights.x) / (heights.y - heights.x));
    float isCastle = step(CASTLE_ABOVE + 0.04, inDoor.y) * step(inDoor.y, 0.95) * step(0.08, inDoor.x) * step(inDoor.x, 0.92);
    float2 scenePlace = float2((inDoor.x - 0.08) / 0.84, (inDoor.y - CASTLE_ABOVE - 0.04) / (0.95 - CASTLE_ABOVE - 0.04));
    Artwork artwork = Blank();
    artwork = Laid(artwork, float4(CastleScene(scenePlace), isCastle), 0.0004);
    float2 roseCentre = float2((door.x + door.y) * 0.5, heights.x + DOOR_ROSES_HEIGHT);
    return Laid(artwork, RoseCluster(place, roseCentre, DOOR_ROSE_SIZE), 0.0006);
}

Artwork CanPainting(float2 place, float3 body, float3 bands)
{
    float isBand = max(step(CAN_BANDS.x, place.y) * step(place.y, CAN_BANDS.y), step(CAN_BANDS.z, place.y) * step(place.y, CAN_BANDS.w));
    float isShoulder = step(CAN_SHOULDER, place.y);
    Artwork artwork = Blank();
    artwork = Laid(artwork, float4(body, 1.0), 0.0);
    artwork = Laid(artwork, float4(bands, max(isBand, isShoulder)), 0.001);
    artwork = Laid(artwork, RoseCluster(place, CAN_ROSES.xz, CAN_ROSE_SIZE), 0.0005);
    return Laid(artwork, RoseCluster(place, CAN_ROSES.yz, CAN_ROSE_SIZE), 0.0005);
}

#endif
