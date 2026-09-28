#ifndef SHALLOW_WATER_PAINT_INCLUDED
#define SHALLOW_WATER_PAINT_INCLUDED

float3 Painted(float3 pickedColour)
{
#ifdef UNITY_COLORSPACE_GAMMA
    return pickedColour;
#else
    return pow(pickedColour, 2.2);
#endif
}

#endif
