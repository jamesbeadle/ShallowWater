import { Group, MeshPhysicalMaterial, MeshStandardMaterial } from 'three';
import { srgb } from '../../world/colours.js';
import { buildSaloonBody } from './saloonBody.js';
import { buildSaloonTrim } from './saloonTrim.js';
import { buildWheel, mountWheels } from './saloonWheels.js';

function saloonFinishes() {
    return {
        paint: new MeshPhysicalMaterial({ color: srgb(0.012, 0.012, 0.014), roughness: 0.32, metalness: 0.2, clearcoat: 1, clearcoatRoughness: 0.05 }),
        chrome: new MeshStandardMaterial({ color: srgb(0.86, 0.86, 0.88), roughness: 0.12, metalness: 1 }),
        glass: new MeshStandardMaterial({ color: srgb(0.015, 0.018, 0.022), roughness: 0.05, metalness: 0.7 }),
        rubber: new MeshStandardMaterial({ color: srgb(0.025, 0.025, 0.025), roughness: 0.85 }),
        grille: new MeshStandardMaterial({ color: srgb(0.02, 0.02, 0.02), roughness: 0.6, metalness: 0.5 }),
        lens: new MeshStandardMaterial({ color: srgb(0.5, 0.48, 0.4), roughness: 0.1, metalness: 0.3 }),
    };
}

export function buildSaloon() {
    const finishes = saloonFinishes();
    const wheel = buildWheel(finishes);
    const saloon = new Group();
    saloon.add(...buildSaloonBody(finishes), ...mountWheels(wheel), buildSaloonTrim(finishes, wheel));
    return saloon;
}
