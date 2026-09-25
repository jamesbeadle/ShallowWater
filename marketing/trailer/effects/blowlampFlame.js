import { AdditiveBlending, DoubleSide, Group, LatheGeometry, Mesh, ShaderMaterial, SphereGeometry, Vector2 } from 'three';
import { shaderNoise } from '../world/shaderNoise.js';
import { srgb } from '../world/colours.js';
import { keepOutOfDepth } from './depthVeil.js';
import { flameFragments } from './flameShading.js';

const Jet = { rings: 28, sides: 28, splashDetail: 36, coreLength: 0.35, coreWidth: 0.45 };
const Colours = { core: srgb(0.28, 0.48, 1.0), tongue: srgb(1.0, 0.44, 0.12) };

const vertexShader = /* glsl */ `
    uniform float reach, time, ragged;
    varying float vAlong;
    varying float vFacing;
    varying vec3 vLocal;
    ${shaderNoise}
    void main() {
        vAlong = clamp(position.y / reach, 0.0, 1.0);
        float lick = noiseAt(vec3(position.x * 40.0, position.y * 14.0 - time * 30.0, position.z * 40.0)) - 0.35;
        vec3 displaced = position + normal * lick * ragged * vAlong;
        vLocal = displaced;
        vec4 viewPosition = modelViewMatrix * vec4(displaced, 1.0);
        vFacing = abs(dot(normalize(normalMatrix * normal), normalize(-viewPosition.xyz)));
        gl_Position = projectionMatrix * viewPosition;
    }`;

function flameMaterial(fragmentShader, uniforms, ragged) {
    const shaded = { ...uniforms, ragged: { value: ragged } };
    const blending = { blending: AdditiveBlending, transparent: true, depthWrite: false, side: DoubleSide };
    return new ShaderMaterial({ vertexShader, fragmentShader, uniforms: shaded, ...blending });
}

function jetProfile(reach, radius) {
    return Array.from({ length: Jet.rings + 1 }, (unused, ring) => {
        const along = ring / Jet.rings;
        const width = radius * (0.22 + 0.78 * Math.sqrt(along)) * (1 - along ** 5 * 0.85);
        return new Vector2(width, along * reach);
    });
}

function jetOf(reach, radius, material) {
    return new Mesh(new LatheGeometry(jetProfile(reach, radius), Jet.sides), material);
}

export function createBlowlampFlame({ reach, radius, splashRadius, time }) {
    const strength = { value: 1 };
    const uniforms = { time, strength, reach: { value: reach }, coreColour: { value: Colours.core }, tongueColour: { value: Colours.tongue } };
    const coreUniforms = { ...uniforms, reach: { value: reach * Jet.coreLength } };
    const jet = new Group();
    jet.add(jetOf(reach, radius, flameMaterial(flameFragments.plume, uniforms, radius * 0.9)));
    jet.add(jetOf(reach * Jet.coreLength, radius * Jet.coreWidth, flameMaterial(flameFragments.core, coreUniforms, radius * 0.1)));
    const splashShape = new SphereGeometry(splashRadius, Jet.splashDetail, Jet.splashDetail / 2);
    const splash = new Mesh(splashShape, flameMaterial(flameFragments.splash, { ...uniforms, reach: { value: splashRadius } }, splashRadius * 0.25));
    [jet, splash].forEach(keepOutOfDepth);
    return { jet, splash, strength };
}
