import { blend, srgb } from '../world/colours.js';
import { fractalNoise, smoothstep } from '../world/noise.js';
import { Channel, Wharf, distanceToWater } from './fazeleyPlaces.js';

const Ground = {
    bed: srgb(0.08, 0.07, 0.06),
    coping: srgb(0.42, 0.4, 0.36),
    towpath: srgb(0.36, 0.32, 0.27),
    cobbles: srgb(0.24, 0.22, 0.21),
    grass: srgb(0.26, 0.29, 0.16),
    yard: srgb(0.3, 0.29, 0.27),
};

export function fazeleyHeight(x, z) {
    const fromWater = distanceToWater(x, z);
    if (fromWater < 0) {
        return Channel.bed + (Channel.bankTop - Channel.bed) * smoothstep(-1.2, 0, fromWater) ** 3;
    }
    const lumps = 0.25 * fractalNoise(x / 40, z / 40);
    return Channel.bankTop + lumps * smoothstep(3, 12, fromWater);
}

function landColour(x, z) {
    const isYard = x > Wharf.xFrom - Wharf.yardFrom && x < Wharf.xTo && z < 0 && z > Wharf.zFrom;
    const patch = fractalNoise(x / 25 + 5, z / 25);
    const town = blend(Ground.cobbles, Ground.grass, smoothstep(0.5, 0.7, patch));
    return isYard ? Ground.yard : town;
}

export function fazeleyColour(x, z) {
    const fromWater = distanceToWater(x, z);
    if (fromWater < 0) {
        return Ground.bed;
    }
    const isTowpath = z > 0 && fromWater < 3.2;
    const land = isTowpath ? Ground.towpath : landColour(x, z);
    return blend(Ground.coping, land, smoothstep(0, Channel.coping, fromWater));
}
