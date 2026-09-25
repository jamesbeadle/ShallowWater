import { MeshStandardMaterial } from 'three';
import { srgb } from '../../world/colours.js';
import { patchMaterial } from '../../world/materialPatches.js';

const declarations = /* glsl */ `
    uniform float courseHeight, rusticationTop;
`;

const weathering = /* glsl */ `
    float weather = fractalAt(vWorldPosition * vec3(0.8, 0.3, 0.8));
    float runs = noiseAt(vec3(vWorldPosition.x * 5.0, vWorldPosition.y * 0.35, vWorldPosition.z * 5.0));
    float coursePhase = fract(vWorldPosition.y / courseHeight);
    float groove = (1.0 - smoothstep(0.0, 0.06, coursePhase)) * (1.0 - step(rusticationTop, vWorldPosition.y));
    diffuseColor.rgb *= (0.84 + 0.22 * weather) * (1.0 - 0.5 * groove) * (1.0 - 0.2 * smoothstep(0.62, 0.92, runs));
`;

export function createStucco({ colour = srgb(0.74, 0.7, 0.6), courseHeight = 0.45, rusticationTop = 0 } = {}) {
    const material = new MeshStandardMaterial({ color: colour, roughness: 0.92, metalness: 0 });
    const uniforms = { courseHeight: { value: courseHeight }, rusticationTop: { value: rusticationTop } };
    return patchMaterial(material, { uniforms, declarations, colour: weathering });
}
