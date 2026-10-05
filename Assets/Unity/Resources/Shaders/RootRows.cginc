#ifndef SHALLOW_WATER_ROOT_ROWS_INCLUDED
#define SHALLOW_WATER_ROOT_ROWS_INCLUDED

#define ROWS_LIFTED_TOGETHER 12.0
#define LIFTING_STRETCH_METRES 400.0
#define LEAF_LOBE_METRES 0.25
#define HEAP_METRES 6.0
#define PLANT_SHARE_OF_ROW 0.55

float IsLifted(float rowIndex, float along)
{
    float band = floor(rowIndex / ROWS_LIFTED_TOGETHER);
    float stretch = floor(along / LIFTING_STRETCH_METRES);
    return step(Hash21(float2(band, stretch) + 0.37), _Lifted);
}

float3 LiftedGround(float2 place, float2 ground, float3 soil)
{
    float2 heapPlace = float2(place.x / HEAP_METRES, place.y / (HEAP_METRES * 0.75));
    float2 heapCell = floor(heapPlace);
    float2 fromTheHeap = (frac(heapPlace) - 0.5 - (Hash22(heapCell) - 0.5) * 0.5) * float2(HEAP_METRES, HEAP_METRES * 0.75);
    float isNearAHeap = 1.0 - smoothstep(0.25, 0.45, length(fromTheHeap));
    float2 rootPlace = ground / 0.09;
    float roots = step(0.35, ValueNoise(rootPlace)) * isNearAHeap * PatternDetail(rootPlace);
    float2 topsPlace = ground / 0.2;
    float wiltedTops = step(0.72, ValueNoise(topsPlace)) * PatternDetail(topsPlace);
    float3 lifted = lerp(soil, _Growth.rgb * 0.6, wiltedTops * 0.7);
    return lerp(lifted, _Accent.rgb, roots);
}

float Rosette(float2 place, float rowIndex, float fromTheRow, out float bulge)
{
    float plantMetres = _RowMetres * PLANT_SHARE_OF_ROW;
    float plantPlace = place.x / plantMetres;
    float2 plantCell = float2(floor(plantPlace), rowIndex);
    float2 jitter = Hash22(plantCell) - 0.5;
    float2 fromThePlant = float2((frac(plantPlace) - 0.5 - jitter.x * 0.3) * plantMetres, (fromTheRow - jitter.y * 0.1) * _RowMetres * 0.5);
    float reach = _Cover * _RowMetres * 0.55 * lerp(0.75, 1.15, Hash21(plantCell + 0.5));
    float2 leafPlace = place / 0.07;
    float raggedEdge = (ValueNoise(leafPlace) - 0.5) * 0.08;
    float distance = length(fromThePlant) + raggedEdge;
    bulge = saturate(1.0 - distance / reach);
    return 1.0 - smoothstep(reach * 0.85, reach, distance);
}

CropSurface RootRows(float2 place, float2 ground)
{
    float3 soil = Soil(ground);
    float rowPlace = place.y / _RowMetres;
    float rowIndex = floor(rowPlace);
    float rowDetail = PatternDetail(float2(rowPlace, place.x / (_RowMetres * PLANT_SHARE_OF_ROW)));
    float fromTheRow = abs(frac(rowPlace) - 0.5) * 2.0;
    float bulge;
    float canopy = Rosette(place, rowIndex, fromTheRow, bulge);
    float2 leafPlace = ground / 0.06;
    float veins = Detailed(leafPlace, ValueNoise(leafPlace), 0.5);
    float3 leaves = _Growth.rgb * lerp(0.82, 1.12, veins) * lerp(0.7, 1.1, bulge);
    float2 crownPlace = ground / 0.05;
    float crowns = step(0.7, ValueNoise(crownPlace)) * step(0.75, bulge) * PatternDetail(crownPlace);
    leaves = lerp(leaves, _Accent.rgb, crowns * 0.45);
    float lifted = IsLifted(rowIndex, place.x);
    float3 near = lerp(lerp(soil, leaves, canopy), LiftedGround(place, ground, soil), lifted);
    float3 far = lerp(lerp(soil, _Growth.rgb, _Cover * 0.8), soil, lifted);
    float3 colour = lerp(far, near, rowDetail);
    float height = canopy * (1.0 - lifted) * _Relief * lerp(0.4, 1.0, bulge) * rowDetail;
    return Made(colour, height, lerp(0.07, 0.22, canopy));
}

CropSurface PotatoRidges(float2 place, float2 ground)
{
    float3 soil = Soil(ground);
    float rowPlace = place.y / _RowMetres;
    float rowIndex = floor(rowPlace);
    float rowDetail = PatternDetail(float2(rowPlace, 0.0));
    float crest = 0.5 + 0.5 * cos((frac(rowPlace) - 0.5) * FULL_TURN);
    float lifted = IsLifted(rowIndex, place.x);
    float profile = crest * lerp(1.0, 0.2, lifted);
    float3 ridges = lerp(_Ground.rgb * 0.8, _GroundWorn.rgb, profile);
    float2 haulmPlace = ground / 0.15;
    float haulm = step(1.0 - _Cover, ValueNoise(haulmPlace)) * step(0.6, crest) * (1.0 - lifted);
    float3 near = lerp(lerp(ridges, soil, 0.3), _Growth.rgb, haulm * PatternDetail(haulmPlace));
    float2 tuberPlace = ground / 0.07;
    float tubers = step(0.9, ValueNoise(tuberPlace)) * lifted * PatternDetail(tuberPlace);
    near = lerp(near, _Accent.rgb, tubers);
    float3 far = lerp(lerp(soil, _Growth.rgb, _Cover * 0.3 * (1.0 - lifted)), soil, 0.3);
    float3 colour = lerp(far, near, rowDetail);
    float height = profile * _Relief * rowDetail;
    return Made(colour, height, 0.08);
}

#endif
