import { MeshStandardMaterial } from 'three';
import { createWindowGlow, createWindowTexture } from '../../textures/windowTexture.js';
import { srgb } from '../../world/colours.js';

const Waking = { groups: 9, firstAt: 64.05, spacing: 0.19, rampSeconds: 0.12, brightness: 2.6 };

export function createWakingWindows() {
    const map = createWindowTexture();
    const glow = createWindowGlow();
    const lamplight = srgb(1.0, 0.72, 0.4);
    const unlit = { map, emissiveMap: glow, emissive: lamplight, emissiveIntensity: 0, roughness: 0.3 };
    return Array.from({ length: Waking.groups }, () => new MeshStandardMaterial(unlit));
}

export function wakeWindows(materials, time) {
    materials.forEach((material, index) => {
        const since = time - (Waking.firstAt + index * Waking.spacing);
        material.emissiveIntensity = Waking.brightness * Math.min(Math.max(since / Waking.rampSeconds, 0), 1);
    });
}
