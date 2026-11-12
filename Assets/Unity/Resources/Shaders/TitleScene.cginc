#ifndef SHALLOW_WATER_TITLE_SCENE_INCLUDED
#define SHALLOW_WATER_TITLE_SCENE_INCLUDED

#include "TitleSky.cginc"
#include "TitleBank.cginc"
#include "TitleBridge.cginc"
#include "TitleBoat.cginc"
#include "TitleWater.cginc"
#include "TitleChain.cginc"
#include "TitlePrint.cginc"

#define FOCUS_FROM_THE_RIGHT 0.80
#define FOCUS_NEVER_LEFT_OF 0.5
#define REFLECTION_SOFTNESS 2.5

float3 Ashore(float2 scene, float pixel, float seconds)
{
    float3 picture = DuskSky(scene, pixel, seconds);
    picture = DistanceOver(picture, scene, pixel);
    picture = BridgeOver(picture, scene, pixel);
    picture = BoatOver(picture, scene, pixel, seconds);
    return FrontWoodOver(picture, scene, pixel);
}

float3 DuskPicture(float2 frame, float aspect, float pixel, float seconds)
{
    float focus = max(aspect - FOCUS_FROM_THE_RIGHT, aspect * FOCUS_NEVER_LEFT_OF);
    float2 scene = float2(frame.x - focus, frame.y);
    float isWater = step(scene.y, HORIZON);
    float2 seen = lerp(scene, Reflected(scene, seconds), isWater);
    float3 picture = Ashore(seen, pixel * lerp(1.0, REFLECTION_SOFTNESS, isWater), seconds);
    picture = lerp(picture, WaterFrom(picture, scene, seconds), isWater);
    picture = ChainOver(picture, scene, pixel);
    return Printed(picture, frame, aspect);
}

#endif
