#ifndef SHALLOW_WATER_TITLE_BOAT_INCLUDED
#define SHALLOW_WATER_TITLE_BOAT_INCLUDED

#include "TitleSky.cginc"

#define BOAT_FOOT float2(-0.03, HORIZON)
#define METRE 0.05
#define CABIN_LEAN 0.1
#define CHIMNEY_ACROSS (-0.62)
#define CHIMNEY_TOP 2.76
#define SMOKE_HEADING float2(0.5547, 0.8321)
#define SMOKE_SPREAD 0.26
#define SMOKE_REACH float2(0.1, 0.42)
#define SMOKE_CURL 0.016
#define SMOKE_RISING 0.3
#define SMOKE_STRENGTH 0.4
#define DOORWAY_GLOW 1.0

float GapToTheHull(float2 deck)
{
    float counter = GapToEllipse(deck, float2(0.0, 0.3), float2(1.1, 0.32));
    float lean = 1.0 + CABIN_LEAN * saturate((deck.y - 0.5) / 1.5);
    float cabin = GapToBox(float2(deck.x * lean, deck.y), float2(0.0, 1.22), float2(0.85, 0.66)) - 0.1;
    float chimney = GapToBox(deck, float2(CHIMNEY_ACROSS, 2.36), float2(0.075, 0.37));
    float cowl = GapToBox(deck, float2(CHIMNEY_ACROSS, CHIMNEY_TOP - 0.03), float2(0.1, 0.028));
    float can = GapToBox(deck, float2(-0.3, 2.07), float2(0.07, 0.1)) - 0.02;
    return min(min(counter, cabin), min(min(chimney, cowl), can));
}

float GapToTheSteerer(float2 deck)
{
    float coat = GapToBox(deck, float2(0.0, 1.52), float2(0.18, 0.52)) - 0.07;
    float shoulders = GapToEllipse(deck, float2(0.0, 2.02), float2(0.29, 0.13));
    float legs = min(GapToStroke(deck, float2(-0.09, 1.3), float2(-0.12, 0.66), 0.15), GapToStroke(deck, float2(0.09, 1.3), float2(0.12, 0.66), 0.15));
    float arm = min(GapToStroke(deck, float2(0.24, 2.0), float2(0.38, 1.64), 0.12), GapToStroke(deck, float2(0.38, 1.64), float2(0.3, 1.4), 0.1));
    float tiller = GapToStroke(deck, float2(0.3, 1.4), float2(0.06, 0.98), 0.06);
    float head = min(GapToDisc(deck, float2(0.0, 2.33), 0.11), GapToEllipse(deck, float2(0.0, 2.19), float2(0.13, 0.09)));
    float cap = min(GapToEllipse(deck, float2(0.01, 2.43), float2(0.16, 0.06)), GapToEllipse(deck, float2(0.13, 2.4), float2(0.07, 0.028)));
    return min(min(min(coat, shoulders), min(legs, arm)), min(tiller, min(head, cap)));
}

float Smoke(float2 scene, float seconds)
{
    float2 chimneyTop = BOAT_FOOT + float2(CHIMNEY_ACROSS, CHIMNEY_TOP) * METRE;
    float2 drift = Turned(scene - chimneyTop, SMOKE_HEADING);
    float curl = sin(drift.x * 17.0 - seconds * 0.7) + 0.6 * sin(drift.x * 41.0 - seconds * 1.3);
    float across = drift.y + curl * SMOKE_CURL * saturate(drift.x * 4.0);
    float width = 0.005 + max(drift.x, 0.0) * SMOKE_SPREAD;
    float plume = exp(-across * across / (width * width));
    float thinning = smoothstep(0.0, 0.015, drift.x) * (1.0 - smoothstep(SMOKE_REACH.x, SMOKE_REACH.y, drift.x));
    float puffs = Fbm(float2(drift.x * 7.0 - seconds * SMOKE_RISING, across * 12.0 + seconds * 0.04));
    return plume * thinning * smoothstep(0.15, 0.8, puffs);
}

float3 BoatOver(float3 behind, float2 scene, float pixel, float seconds)
{
    float2 deck = (scene - BOAT_FOOT) / METRE;
    float deckPixel = pixel / METRE;
    float3 picture = lerp(behind, _Night.rgb, Covered(GapToTheHull(deck), deckPixel));
    float doorway = Covered(GapToBox(deck, float2(0.0, 1.22), float2(0.3, 0.64)), deckPixel);
    float3 lamplight = lerp(_Sunset.rgb, _Glow.rgb, saturate((deck.y - 0.6) / 1.3)) * DOORWAY_GLOW;
    picture = lerp(picture, lamplight, doorway);
    picture = lerp(picture, _Night.rgb, Covered(GapToTheSteerer(deck), deckPixel));
    float3 haze = lerp(_Brick.rgb, _Ember.rgb, 0.2);
    return lerp(picture, haze, Smoke(scene, seconds) * SMOKE_STRENGTH);
}

#endif
