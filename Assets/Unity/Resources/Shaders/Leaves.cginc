#ifndef SHALLOW_WATER_LEAVES_INCLUDED
#define SHALLOW_WATER_LEAVES_INCLUDED

#define LEAF_FULL_TURN 6.2831853
#define LEAF_LENGTH_CELLS 1.12
#define CARD_SHUFFLE 41.0
#define NEEDLES_IN_A_TUFT 14.0
#define NEEDLE_WIDTH 0.028
#define OAK_LOBES 4.5
#define HAZEL_TEETH 14.0
#define SHARPEST_LEAVES 0.2
#define BLURRIEST_LEAVES 0.55
#define GROWN_EDGE_RAGGEDNESS 0.3
#define GROWN_EDGE 0.08
#define SUB_CLUMPS 6
#define SUB_CLUMP_SPREAD 0.95
#define SMALLEST_SUB_CLUMP 0.28
#define SUB_CLUMP_RANGE 0.22
#define OUTLINE_SOFTNESS 0.3
#define SPRAYS_PER_LEAF 0.45

struct LeafCover
{
    float amount;
    float tone;
    float rib;
};

float2 Turned(float2 place, float radians)
{
    float sine = sin(radians);
    float cosine = cos(radians);
    return float2(cosine * place.x - sine * place.y, sine * place.x + cosine * place.y);
}

float OakLeaf(float2 leaf)
{
    float along = leaf.x + 0.5;
    float body = sqrt(saturate(along * (1.0 - along))) * 0.6 * (0.7 + 0.5 * along);
    float lobes = 1.0 - 0.32 * pow(abs(sin(along * 3.14159 * OAK_LOBES)), 0.6);
    return body * lobes - abs(leaf.y);
}

float Leaflet(float2 leaf, float breadth)
{
    float along = leaf.x + 0.5;
    return sqrt(saturate(along * (1.0 - along))) * breadth - abs(leaf.y);
}

float BirchLeaf(float2 leaf)
{
    float along = leaf.x + 0.5;
    return saturate(along * 1.7) * saturate((1.0 - along) * 1.3) * 0.5 - abs(leaf.y);
}

float NeedleTuft(float2 leaf)
{
    float reach = length(leaf);
    float angle = atan2(leaf.y, leaf.x);
    float spacing = LEAF_FULL_TURN / NEEDLES_IN_A_TUFT;
    float nearest = round(angle / spacing) * spacing;
    float across = abs(reach * sin(angle - nearest));
    return min(NEEDLE_WIDTH - across, 0.5 - reach);
}

float HazelLeaf(float2 leaf)
{
    float along = leaf.x + 0.5;
    float body = sqrt(saturate(along * (1.0 - along))) * 0.72 * saturate((1.0 - along) * 3.0);
    return body * (1.0 - 0.06 * abs(sin(along * 3.14159 * HAZEL_TEETH))) - abs(leaf.y);
}

float LeafInside(float2 leaf, float shape)
{
    if (shape < 0.5) return OakLeaf(leaf);
    if (shape < 1.5) return Leaflet(leaf, 0.3);
    if (shape < 2.5) return BirchLeaf(leaf);
    if (shape < 3.5) return NeedleTuft(leaf);
    return HazelLeaf(leaf);
}

void SubClumpsOf(float seed, out float3 discs[SUB_CLUMPS])
{
    for (int clump = 0; clump < SUB_CLUMPS; clump++)
    {
        float index = float(clump);
        float2 salt = float2(seed * 97.0 + index, index * 3.1);
        float2 centre = (Hash22(salt) - 0.5) * SUB_CLUMP_SPREAD;
        discs[clump] = float3(centre, SMALLEST_SUB_CLUMP + SUB_CLUMP_RANGE * Hash21(salt + 7.7));
    }
}

float Clumped(float2 corner, float3 discs[SUB_CLUMPS])
{
    float clumped = 0.0;
    for (int clump = 0; clump < SUB_CLUMPS; clump++)
    {
        clumped = max(clumped, 1.0 - length(corner - discs[clump].xy) / discs[clump].z);
    }
    return clumped;
}

float CardOutline(float2 corner, float3 discs[SUB_CLUMPS])
{
    return smoothstep(0.0, OUTLINE_SOFTNESS, Clumped(corner, discs));
}

float Sprayed(float2 corner, float3 discs[SUB_CLUMPS], float sprays)
{
    return CardOutline(corner, discs) * (0.55 + 0.9 * sprays);
}

LeafCover LeafIn(float2 place, float2 neighbour, float3 discs[SUB_CLUMPS], float seed, float halfLeavesAcross, float2 shapeAndBlur)
{
    float2 jitter = Hash22(neighbour);
    float2 centre = neighbour + 0.15 + 0.7 * jitter;
    float2 centreOnTheCard = (centre - seed * CARD_SHUFFLE) / halfLeavesAcross;
    float isGrown = step(GROWN_EDGE, Clumped(centreOnTheCard, discs) + (jitter.y - 0.5) * GROWN_EDGE_RAGGEDNESS);
    float size = LEAF_LENGTH_CELLS * (0.8 + 0.4 * jitter.x);
    float2 leaf = Turned(place - centre, Hash21(neighbour + 9.1) * LEAF_FULL_TURN) / size;
    float inside = LeafInside(leaf, shapeAndBlur.x) * size;
    LeafCover cover;
    cover.amount = saturate(inside / max(shapeAndBlur.y, 1e-4) + 0.5) * isGrown;
    cover.tone = Hash21(neighbour + 2.7);
    cover.rib = saturate(inside * 8.0);
    return cover;
}

LeafCover LeavesOnACard(float2 corner, float seed, float shape, float halfLeavesAcross)
{
    float2 place = corner * halfLeavesAcross + seed * CARD_SHUFFLE;
    float2 cell = floor(place);
    float blur = max(fwidth(place.x), fwidth(place.y));
    float3 discs[SUB_CLUMPS];
    SubClumpsOf(seed, discs);
    LeafCover cover = (LeafCover)0;
    for (int north = -1; north <= 1; north++)
    {
        for (int east = -1; east <= 1; east++)
        {
            LeafCover leaf = LeafIn(place, cell + float2(east, north), discs, seed, halfLeavesAcross, float2(shape, blur));
            bool isOnTop = leaf.amount > cover.amount;
            cover.tone = isOnTop ? leaf.tone : cover.tone;
            cover.rib = isOnTop ? leaf.rib : cover.rib;
            cover.amount = max(cover.amount, leaf.amount);
        }
    }
    float sprays = ValueNoise(place * SPRAYS_PER_LEAF);
    float farAway = smoothstep(SHARPEST_LEAVES, BLURRIEST_LEAVES, blur);
    cover.amount = lerp(cover.amount, smoothstep(0.35, 0.6, Sprayed(corner, discs, sprays)), farAway);
    cover.tone = lerp(cover.tone, frac(sprays * 3.7 + seed), farAway);
    cover.rib = lerp(cover.rib, sprays, farAway);
    return cover;
}

float LeafShadow(float2 corner, float seed, float halfLeavesAcross)
{
    float2 place = corner * halfLeavesAcross + seed * CARD_SHUFFLE;
    float3 discs[SUB_CLUMPS];
    SubClumpsOf(seed, discs);
    return Sprayed(corner, discs, ValueNoise(place * SPRAYS_PER_LEAF));
}

#endif
