#ifndef SHALLOW_WATER_TITLE_PRINT_INCLUDED
#define SHALLOW_WATER_TITLE_PRINT_INCLUDED

#include "Noise.cginc"

#define GRAIN_CELLS 1080.0
#define GRAIN_STRENGTH 0.07
#define PAPER_FIBRES float2(140.0, 46.0)
#define FIBRE_STRENGTH 0.06
#define VIGNETTE 0.4
#define VIGNETTE_EDGE float2(0.32, 0.78)
#define WORDS_SCRIM 0.45
#define WORDS_SCRIM_EDGE float2(0.5, 1.05)

float3 Printed(float3 picture, float2 frame, float aspect)
{
    float grain = Hash21(floor(frame * GRAIN_CELLS)) - 0.5;
    float fibres = Fbm(frame * PAPER_FIBRES) - 0.5;
    picture *= 1.0 + grain * GRAIN_STRENGTH + fibres * FIBRE_STRENGTH;
    float2 fromTheMiddle = (frame / float2(aspect, 1.0) - 0.5) * float2(1.0, 0.85);
    picture *= 1.0 - VIGNETTE * smoothstep(VIGNETTE_EDGE.x, VIGNETTE_EDGE.y, length(fromTheMiddle));
    float underTheWords = 1.0 - smoothstep(WORDS_SCRIM_EDGE.x, WORDS_SCRIM_EDGE.y, frame.x);
    return picture * (1.0 - WORDS_SCRIM * underTheWords);
}

#endif
