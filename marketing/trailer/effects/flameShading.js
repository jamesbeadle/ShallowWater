import { shaderNoise } from '../world/shaderNoise.js';

const declarations = /* glsl */ `
    uniform float time, strength, reach;
    uniform vec3 coreColour, tongueColour;
    varying float vAlong;
    varying float vFacing;
    varying vec3 vLocal;
    ${shaderNoise}
`;

const plume = /* glsl */ `
    ${declarations}
    void main() {
        float churn = fractalAt(vec3(vLocal.x * 26.0, vLocal.y * 15.0 - time * 26.0, vLocal.z * 26.0));
        float edge = pow(vFacing, 1.4) * (1.0 - smoothstep(0.82, 1.0, vAlong));
        float tips = smoothstep(0.5, 0.95, vAlong) * smoothstep(0.52, 0.78, churn);
        vec3 blue = coreColour * (0.3 + 0.7 * churn);
        vec3 light = mix(blue, tongueColour * 2.2, tips) * edge * (0.45 + 0.9 * churn);
        gl_FragColor = vec4(light * strength, 1.0);
    }`;

const core = /* glsl */ `
    ${declarations}
    void main() {
        float shimmer = 0.8 + 0.4 * noiseAt(vec3(vLocal.y * 60.0 - time * 40.0, vLocal.x * 80.0, time));
        float cone = pow(vFacing, 2.2) * (1.0 - smoothstep(0.55, 1.0, vAlong));
        gl_FragColor = vec4(coreColour * cone * shimmer * 3.2 * strength, 1.0);
    }`;

const splash = /* glsl */ `
    ${declarations}
    void main() {
        vec3 around = normalize(vLocal);
        float polar = acos(clamp(around.y, -1.0, 1.0));
        vec2 ring = normalize(around.xz + vec2(0.0001)) * 2.6;
        float streaks = fractalAt(vec3(ring, polar * 3.0 - time * 10.0));
        float spread = smoothstep(0.05, 0.45, polar) * (1.0 - smoothstep(0.9, 1.6, polar));
        float sheet = spread * smoothstep(0.45, 0.8, streaks) * (pow(1.0 - vFacing, 1.3) + 0.15);
        vec3 colour = mix(coreColour * 0.9, tongueColour * 1.8, smoothstep(0.45, 1.5, polar));
        gl_FragColor = vec4(colour * sheet * strength * 2.2, 1.0);
    }`;

export const flameFragments = { plume, core, splash };
