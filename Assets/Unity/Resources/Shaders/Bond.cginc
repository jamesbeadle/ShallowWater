#ifndef SHALLOW_WATER_BOND_INCLUDED
#define SHALLOW_WATER_BOND_INCLUDED

#include "Noise.cginc"

#define STRETCHER 0.225
#define HEADER 0.1125
#define FLEMISH_MODULE 0.3375
#define COURSE 0.075
#define SOLDIER 0.075
#define JOINT 0.01

struct Laid
{
    float2 place;
    float id;
    float isHeader;
    float isJoint;
};

Laid Flemish(float2 wall)
{
    float course = floor(wall.y / COURSE);
    float shifted = wall.x + frac(course * 0.5) * FLEMISH_MODULE;
    float inModule = frac(shifted / FLEMISH_MODULE) * FLEMISH_MODULE;
    Laid laid;
    laid.isHeader = inModule >= STRETCHER ? 1.0 : 0.0;
    float along = laid.isHeader > 0.5 ? inModule - STRETCHER : inModule;
    float up = frac(wall.y / COURSE) * COURSE;
    laid.isJoint = (along < JOINT || up < JOINT) ? 1.0 : 0.0;
    laid.id = Hash21(float2(floor(shifted / FLEMISH_MODULE) * 2.0 + laid.isHeader, course));
    laid.place = float2(wall.x / HEADER, wall.y / COURSE);
    return laid;
}

Laid Stretchers(float2 wall)
{
    float course = floor(wall.y / COURSE);
    float shifted = wall.x / STRETCHER + frac(course * 0.5);
    Laid laid;
    laid.isHeader = 0.0;
    laid.isJoint = (frac(shifted) * STRETCHER < JOINT || frac(wall.y / COURSE) * COURSE < JOINT) ? 1.0 : 0.0;
    laid.id = Hash21(float2(floor(shifted), course));
    laid.place = float2(shifted * 2.0, wall.y / COURSE);
    return laid;
}

Laid Soldiers(float2 wall)
{
    float shifted = wall.x / SOLDIER;
    Laid laid;
    laid.isHeader = 0.0;
    laid.isJoint = frac(shifted) * SOLDIER < JOINT ? 1.0 : 0.0;
    laid.id = Hash21(float2(floor(shifted), 7.0));
    laid.place = float2(shifted, wall.y / COURSE);
    return laid;
}

#endif
