#ifndef SHALLOW_WATER_TILLAGE_INCLUDED
#define SHALLOW_WATER_TILLAGE_INCLUDED

#define LAND_METRES 22.0
#define STALK_METRES 0.035
#define FURROW_WANDER_METRES 3.0

float RowLine(float rowPlace, float halfWidth)
{
    return 1.0 - smoothstep(halfWidth, halfWidth + 0.12, abs(frac(rowPlace) - 0.5));
}

CropSurface Stubble(float2 place, float2 ground)
{
    float3 soil = Soil(ground);
    float rowPlace = place.y / _RowMetres;
    float rowDetail = PatternDetail(float2(rowPlace, place.x / STALK_METRES));
    float2 stalkPlace = float2(place.x / STALK_METRES, place.y / STALK_METRES);
    float stalks = step(0.5, ValueNoise(stalkPlace)) * PatternDetail(stalkPlace);
    float stubble = RowLine(rowPlace, 0.05) * lerp(0.6, stalks, PatternDetail(stalkPlace));
    float2 litterPlace = ground / 0.04;
    float litter = step(0.6, ValueNoise(litterPlace + 3.1)) * PatternDetail(litterPlace);
    float undergrowth = saturate(_Cover * 1.6 * lerp(0.6, 1.2, Fbm(ground / 6.0)));
    float2 leafPlace = ground / 0.03;
    float leaves = step(1.0 - undergrowth, ValueNoise(leafPlace));
    float3 straw = _Growth.rgb * lerp(0.82, 1.08, ValueNoise(ground / 0.3));
    float3 between = lerp(soil, straw, litter * 0.7);
    between = lerp(between, _Accent.rgb, lerp(undergrowth * 0.7, leaves, PatternDetail(leafPlace)));
    float3 near = lerp(between, straw, stubble);
    float3 far = lerp(lerp(soil, straw, 0.6), _Accent.rgb, undergrowth * 0.45);
    float3 colour = lerp(far, near, rowDetail);
    float height = (stubble + undergrowth * 0.3) * _Relief * rowDetail;
    return Made(colour, height, 0.07);
}

CropSurface Plough(float2 place, float2 ground)
{
    float wander = (ValueNoise(float2(place.x / FURROW_WANDER_METRES, 0.5)) - 0.5) * 0.4;
    float slicePlace = place.y / _RowMetres + wander;
    float slice = frac(slicePlace);
    float sliceDetail = PatternDetail(float2(slicePlace, 0.0));
    float lump = smoothstep(0.0, 0.25, slice) * (1.0 - smoothstep(0.35, 1.0, slice));
    float landPlace = frac(place.y / LAND_METRES);
    float fromTheOpenFurrow = min(landPlace, 1.0 - landPlace) * LAND_METRES;
    float openFurrow = 1.0 - smoothstep(0.15, 0.55, fromTheOpenFurrow);
    float2 clodPlace = ground / 0.12;
    float clods = Detailed(clodPlace, ValueNoise(clodPlace), 0.5);
    float drying = Fbm(ground / 12.0) - 0.5;
    float3 near = lerp(_Ground.rgb, _GroundWorn.rgb, saturate(lump * 0.8 + (clods - 0.5) * 0.5 + drying * 0.6));
    float2 trashPlace = ground / 0.08;
    float trash = step(1.0 - _Cover, ValueNoise(trashPlace)) * PatternDetail(trashPlace) * (1.0 - lump);
    near = lerp(near, _Growth.rgb, trash);
    float3 far = lerp(_Ground.rgb, _GroundWorn.rgb, saturate(0.45 + drying * 0.5));
    float3 colour = lerp(far, near, sliceDetail);
    colour = lerp(colour, _Ground.rgb * 0.7, openFurrow);
    float height = (lump * sliceDetail + (clods - 0.5) * 0.4 - openFurrow * 1.5) * _Relief;
    return Made(colour, height, lerp(0.18, 0.08, lump));
}

CropSurface Drilled(float2 place, float2 ground)
{
    float3 soil = Soil(ground);
    float rowPlace = place.y / _RowMetres;
    float rowDetail = PatternDetail(float2(rowPlace, place.x / STALK_METRES));
    float drill = RowLine(rowPlace, 0.04);
    float shoots = step(0.38, ValueNoise(float2(place.x / 0.03, floor(rowPlace) * 5.3)));
    float braird = drill * shoots * saturate(_Cover * 2.0);
    float3 near = lerp(soil * lerp(1.0, 0.85, drill), _Growth.rgb, braird);
    float3 far = lerp(soil, _Growth.rgb, _Cover * 0.35);
    float3 colour = lerp(far, near, rowDetail);
    float height = (braird - drill * 0.5) * _Relief * rowDetail;
    return Made(colour, height, 0.09);
}

#endif
