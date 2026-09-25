import { MeshStandardMaterial, Vector3 } from 'three';
import { srgb } from '../../world/colours.js';
import { patchMaterial } from '../../world/materialPatches.js';
import { turned } from './turnedParts.js';

const bulbProfile = [[0, 1.015], [0.07, 1.015], [0.075, 1.05], [0.1, 1.075], [0.122, 1.11], [0.13, 1.15], [0.126, 1.19],
    [0.108, 1.225], [0.078, 1.25], [0.04, 1.265], [0, 1.27]];

export const Bulb = { centre: new Vector3(0, 1.15, 0), radius: 0.13 };

const declarations = /* glsl */ `
    uniform vec3 hotSpot;
    uniform float glow;
`;

const heat = /* glsl */ `
    float nearFlame = 1.0 - smoothstep(0.0, 0.24, distance(vWorldPosition, hotSpot));
    float mottle = fractalAt(vWorldPosition * 90.0 + vec3(0.0, time * 0.3, 0.0));
    float flakes = smoothstep(0.45, 0.75, noiseAt(vWorldPosition * 120.0));
    float warmth = glow * (0.12 + 0.88 * pow(nearFlame, 1.5)) * (0.92 + 0.16 * mottle) * (1.0 - 0.25 * flakes);
    outgoingLight += vec3(0.5, 0.03, 0.006) * warmth * warmth * 2.2 + vec3(0.9, 0.2, 0.03) * pow(warmth, 4.0) * 1.1;
`;

const scale = /* glsl */ `
    diffuseColor.rgb *= 0.9 + 0.12 * noiseAt(vWorldPosition * 300.0);
`;

export function buildHotBulb(time) {
    const heatUniforms = { hotSpot: { value: Bulb.centre.clone() }, glow: { value: 0.5 } };
    const material = new MeshStandardMaterial({ color: srgb(0.08, 0.066, 0.06), roughness: 0.78, metalness: 0.45 });
    patchMaterial(material, { time, uniforms: heatUniforms, declarations, colour: scale, light: heat });
    return { bulb: turned(bulbProfile, material), heat: heatUniforms };
}
