import { Color, MeshStandardMaterial } from 'three';
import { createBrickTexture } from '../../textures/brickTexture.js';
import { createSlateTexture } from '../../textures/slateTexture.js';
import { createWindowGlow, createWindowTexture } from '../../textures/windowTexture.js';
import { srgb } from '../../world/colours.js';

export const BrickTones = {
    staffordshireRed: [150, 72, 50],
    sootyRed: [112, 58, 44],
    blueBrick: [70, 66, 72],
    stuccoCream: [196, 186, 160],
};

export function createBuildingMaterials({ brickTone = BrickTones.staffordshireRed, glowColour = srgb(1.0, 0.72, 0.4) } = {}) {
    const glow = createWindowGlow();
    return {
        brick: new MeshStandardMaterial({ map: createBrickTexture({ tone: brickTone }), roughness: 0.95 }),
        slate: new MeshStandardMaterial({ map: createSlateTexture(), roughness: 0.62, metalness: 0.05 }),
        stone: new MeshStandardMaterial({ color: srgb(0.55, 0.52, 0.46), roughness: 0.9 }),
        pot: new MeshStandardMaterial({ color: srgb(0.45, 0.24, 0.16), roughness: 0.85 }),
        door: new MeshStandardMaterial({ color: srgb(0.12, 0.16, 0.12), roughness: 0.6 }),
        window: new MeshStandardMaterial({ map: createWindowTexture(), roughness: 0.25, metalness: 0.1 }),
        litWindow: new MeshStandardMaterial({ map: createWindowTexture(), emissiveMap: glow, emissive: glowColour, emissiveIntensity: 2.2, roughness: 0.3 }),
        darkGlass: new MeshStandardMaterial({ color: new Color(0.01, 0.012, 0.015), roughness: 0.15, metalness: 0.3 }),
    };
}
