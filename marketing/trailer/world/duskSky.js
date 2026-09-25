import { BackSide, Mesh, ShaderMaterial, SphereGeometry, Vector3 } from 'three';
import { srgb } from './colours.js';
import { shaderNoise } from './shaderNoise.js';

const Dome = { radius: 3000, widthSegments: 48, heightSegments: 24 };
export const Dusk = {
    zenith: srgb(0.035, 0.06, 0.16),
    middle: srgb(0.12, 0.2, 0.38),
    horizon: srgb(0.52, 0.46, 0.5),
    afterglow: srgb(0.95, 0.5, 0.25),
    afterglowDirection: new Vector3(0.75, 0.04, -0.66).normalize(),
};

const vertexShader = /* glsl */ `
    varying vec3 vDirection;
    void main() {
        vDirection = normalize(position);
        gl_Position = projectionMatrix * modelViewMatrix * vec4(position, 1.0);
    }`;

const fragmentShader = /* glsl */ `
    uniform vec3 zenith, middle, horizon, afterglow, afterglowDirection;
    varying vec3 vDirection;
    ${shaderNoise}
    void main() {
        vec3 direction = normalize(vDirection);
        float height = max(direction.y, 0.0);
        vec3 sky = mix(horizon, middle, smoothstep(0.0, 0.22, height));
        sky = mix(sky, zenith, smoothstep(0.18, 0.75, height));
        float towardGlow = max(dot(direction, afterglowDirection), 0.0);
        sky += afterglow * pow(towardGlow, 5.0) * (1.0 - smoothstep(0.0, 0.3, height)) * 0.6;
        vec2 streakPlace = direction.xz / (direction.y + 0.12);
        float streaks = fractalAt(vec3(streakPlace.x * 0.6, streakPlace.y * 2.6, 3.0));
        float cloud = smoothstep(0.55, 0.8, streaks) * smoothstep(0.03, 0.12, height) * (1.0 - smoothstep(0.25, 0.5, height));
        sky = mix(sky, horizon * 0.55 + afterglow * 0.25 * towardGlow, cloud * 0.55);
        float star = step(0.9965, fractalAt(direction * 420.0)) * smoothstep(0.2, 0.6, height);
        gl_FragColor = vec4(sky + vec3(star * 0.6), 1.0);
    }`;

export function createDuskSky() {
    const uniforms = Object.fromEntries(Object.entries(Dusk).map(([name, value]) => [name, { value }]));
    const material = new ShaderMaterial({ vertexShader, fragmentShader, uniforms, side: BackSide, depthWrite: false, fog: false });
    const sky = new Mesh(new SphereGeometry(Dome.radius, Dome.widthSegments, Dome.heightSegments), material);
    sky.renderOrder = -1;
    return sky;
}
