#ifndef SHALLOW_WATER_NOISE_INCLUDED
#define SHALLOW_WATER_NOISE_INCLUDED

#define SMOOTHEST_FADE 1.5
#define FAR_AWAY_CELL 8.0

float Hash21(float2 place)
{
    place = frac(place * float2(123.34, 456.21));
    place += dot(place, place + 45.32);
    return frac(place.x * place.y);
}

float2 Hash22(float2 place)
{
    float first = Hash21(place);
    return float2(first, Hash21(place + first + 17.17));
}

float Hash31(float3 place)
{
    place = frac(place * float3(0.1031, 0.1030, 0.0973));
    place += dot(place, place.yxz + 33.33);
    return frac((place.x + place.y) * place.z);
}

float ValueNoise(float2 place)
{
    float2 cell = floor(place);
    float2 inside = frac(place);
    float2 eased = inside * inside * (3.0 - 2.0 * inside);
    float south = lerp(Hash21(cell), Hash21(cell + float2(1, 0)), eased.x);
    float north = lerp(Hash21(cell + float2(0, 1)), Hash21(cell + float2(1, 1)), eased.x);
    return lerp(south, north, eased.y);
}

float ValueNoise3(float3 place)
{
    float3 cell = floor(place);
    float3 inside = frac(place);
    float3 eased = inside * inside * (3.0 - 2.0 * inside);
    float lowSouth = lerp(Hash31(cell), Hash31(cell + float3(1, 0, 0)), eased.x);
    float lowNorth = lerp(Hash31(cell + float3(0, 0, 1)), Hash31(cell + float3(1, 0, 1)), eased.x);
    float highSouth = lerp(Hash31(cell + float3(0, 1, 0)), Hash31(cell + float3(1, 1, 0)), eased.x);
    float highNorth = lerp(Hash31(cell + float3(0, 1, 1)), Hash31(cell + float3(1, 1, 1)), eased.x);
    return lerp(lerp(lowSouth, lowNorth, eased.z), lerp(highSouth, highNorth, eased.z), eased.y);
}

float Fbm(float2 place)
{
    float sum = 0.0;
    float amplitude = 0.5;
    for (int octave = 0; octave < 4; octave++)
    {
        sum += amplitude * ValueNoise(place);
        place = place * 2.03 + 17.1;
        amplitude *= 0.5;
    }
    return sum / 0.9375;
}

float3 Cells(float2 place)
{
    float2 cell = floor(place);
    float2 inside = frac(place);
    float nearest = FAR_AWAY_CELL;
    float second = FAR_AWAY_CELL;
    float2 nearestCell = cell;
    for (int north = -1; north <= 1; north++)
    {
        for (int east = -1; east <= 1; east++)
        {
            float2 neighbour = float2(east, north);
            float2 toSeed = neighbour + Hash22(cell + neighbour) - inside;
            float distanceSquared = dot(toSeed, toSeed);
            bool isNearest = distanceSquared < nearest;
            second = isNearest ? nearest : min(second, distanceSquared);
            nearestCell = isNearest ? cell + neighbour : nearestCell;
            nearest = isNearest ? distanceSquared : nearest;
        }
    }
    return float3(sqrt(second) - sqrt(nearest), Hash21(nearestCell), sqrt(nearest));
}

float PatternDetail(float2 placeInCells)
{
    float2 change = fwidth(placeInCells);
    return saturate(1.0 - max(change.x, change.y) * SMOOTHEST_FADE);
}

float2 FacePlace(float3 position, float3 normal)
{
    float3 facing = abs(normal);
    bool isLevel = facing.y > max(facing.x, facing.z);
    bool facesEastOrWest = facing.x > facing.z;
    float2 upright = facesEastOrWest ? position.zy : position.xy;
    return isLevel ? position.xz : upright;
}

float AlongTheFace(float3 position, float3 normal)
{
    float2 across = normalize(float2(-normal.z, normal.x) + float2(1e-5, 0));
    return dot(position.xz, across);
}

#endif
