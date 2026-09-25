import { MeshStandardMaterial } from 'three';
import { srgb } from '../../world/colours.js';
import { createRim, withRim } from './rimLight.js';

function cloth(red, green, blue, roughness = 1) {
    return new MeshStandardMaterial({ color: srgb(red, green, blue), roughness, metalness: 0 });
}

export function createClothing(overrides = {}, rim = createRim()) {
    const clothing = {
        coat: cloth(0.13, 0.12, 0.105),
        jumper: cloth(0.2, 0.18, 0.15),
        trousers: cloth(0.085, 0.08, 0.075),
        cap: cloth(0.2, 0.17, 0.13),
        skin: cloth(0.56, 0.38, 0.3, 0.7),
        boots: cloth(0.05, 0.04, 0.035, 0.45),
        ...overrides,
    };
    Object.values(clothing).forEach((material) => withRim(material, rim));
    return { ...clothing, rim };
}

export function cadetClothing(rim) {
    const khaki = cloth(0.34, 0.31, 0.2);
    return createClothing({ coat: khaki, jumper: khaki, trousers: cloth(0.3, 0.27, 0.18), cap: cloth(0.3, 0.27, 0.17) }, rim);
}

export const Wardrobe = {
    runner: { top: 'jumper', hasCoat: false, hasCap: true },
    workman: { top: 'coat', hasCoat: true, hasCap: true },
    gentleman: { top: 'coat', hasCoat: true, hasCap: false, hasHat: true },
    lady: { top: 'coat', hasCoat: true, hasCap: false, hasHat: true, hasSkirt: true },
    constable: { top: 'coat', hasCoat: true, hasCap: false, hasSkirt: true, headwear: 'helmet' },
};
