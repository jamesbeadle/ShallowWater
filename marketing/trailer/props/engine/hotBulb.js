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
    float nearFlame = 1.0 - smoothstep(0.0, 0.26, distance(vWorldPosition, hotSpot));
    float mottle = fractalAt(vWorldPosition * 160.0 + vec3(0.0, time * 0.3, 0.0));
    float warmth = glow * (0.3 + 0.7 * nearFlame * nearFlame) * (0.88 + 0.24 * mottle);
    outgoingLight += vec3(0.75, 0.08, 0.015) * warmth * warmth * 1.4 + vec3(1.0, 0.42, 0.08) * pow(warmth, 4.0) * 2.2;
`;

const scale = /* glsl */ `
    diffuseColor.rgb *= 0.88 + 0.18 * fractalAt(vWorldPosition * 300.0);
`;

export function buildHotBulb(time) {
    const heatUniforms = { hotSpot: { value: Bulb.centre.clone() }, glow: { value: 0.5 } };
    const material = new MeshStandardMaterial({ color: srgb(0.09, 0.075, 0.07), roughness: 0.62, metalness: 0.55 });
    patchMaterial(material, { time, uniforms: heatUniforms, declarations, colour: scale, light: heat });
    return { bulb: turned(bulbProfile, material), heat: heatUniforms };
}
