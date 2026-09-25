import { AdditiveBlending, Mesh, PlaneGeometry, ShaderMaterial } from 'three';
import { shaderNoise } from '../world/shaderNoise.js';
import { keepOutOfDepth } from './depthVeil.js';

const vertexShader = /* glsl */ `
    varying vec3 vWorld;
    void main() {
        vWorld = (modelMatrix * vec4(position, 1.0)).xyz;
        gl_Position = projectionMatrix * viewMatrix * vec4(vWorld, 1.0);
    }`;

const fragmentShader = /* glsl */ `
    uniform vec3 glowColour, lamp;
    uniform float time, strength, reach, grain, seed;
    varying vec3 vWorld;
    ${shaderNoise}
    void main() {
        vec3 fromLamp = vWorld - lamp;
        float falloff = exp(-dot(fromLamp, fromLamp) / (reach * reach));
        vec3 drifting = vWorld * grain + vec3(time * 0.05, -time * 0.09, seed);
        float curl = fractalAt(drifting + fractalAt(drifting * 1.7) * 1.3);
        float wisps = smoothstep(0.5, 0.9, curl);
        vec3 haze = glowColour * (falloff * (0.25 + 1.4 * wisps) + 0.015 * wisps);
        gl_FragColor = vec4(haze * strength, 1.0);
    }`;

export function createLampHaze({ size, lamp, colour, reach, grain, seed, time }) {
    const strength = { value: 1 };
    const uniforms = {
        glowColour: { value: colour }, lamp: { value: lamp }, time, strength,
        reach: { value: reach }, grain: { value: grain }, seed: { value: seed },
    };
    const material = new ShaderMaterial({ vertexShader, fragmentShader, uniforms, blending: AdditiveBlending, transparent: true, depthWrite: false });
    const haze = keepOutOfDepth(new Mesh(new PlaneGeometry(size[0], size[1]), material));
    return { haze, strength };
}
