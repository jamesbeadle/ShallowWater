import { MeshStandardMaterial } from 'three';
import { patchMaterial } from './materialPatches.js';

const grit = /* glsl */ `
    float coarse = fractalAt(vWorldPosition * 0.18);
    float fine = noiseAt(vWorldPosition * 3.1);
    diffuseColor.rgb *= 0.72 + 0.4 * coarse + 0.16 * fine;
`;

export function createTerrainMaterial() {
    const material = new MeshStandardMaterial({ vertexColors: true, roughness: 0.96, metalness: 0 });
    return patchMaterial(material, { colour: grit });
}
