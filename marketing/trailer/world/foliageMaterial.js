import { DoubleSide, MeshStandardMaterial } from 'three';
import { patchMaterial } from './materialPatches.js';

const declarations = /* glsl */ `
    uniform vec3 sunDirection;
    uniform vec3 sunColour;
    const float raggedness = 0.5;
`;

const nearLeaves = /* glsl */ `
    vec3 viewDirection = normalize(cameraPosition - vWorldPosition);
    float clumps = 0.62 * noiseAt(vWorldPosition * 1.4) + 0.38 * noiseAt(vWorldPosition * 3.3);
    float silhouette = 1.0 - abs(dot(normalize(vNormal), normalize(vViewPosition)));
    float ragged = silhouette * silhouette * (0.4 + clumps) + (1.0 - clumps) * 0.18;
    if (ragged > raggedness) discard;
    diffuseColor.rgb *= 0.55 + 0.75 * clumps;
`;

const farLeaves = /* glsl */ `
    vec3 viewDirection = normalize(cameraPosition - vWorldPosition);
    float clumps = noiseAt(vWorldPosition * 0.9);
    diffuseColor.rgb *= 0.6 + 0.65 * clumps;
`;

const translucency = /* glsl */ `
    float backlight = pow(max(dot(-viewDirection, sunDirection), 0.0), 5.0);
    outgoingLight += sunColour * diffuseColor.rgb * backlight * (0.35 + clumps) * 1.6;
`;

export function createFoliageMaterial(sun, isNear) {
    const material = new MeshStandardMaterial({ vertexColors: true, roughness: 0.85, metalness: 0, side: DoubleSide });
    const uniforms = { sunDirection: { value: sun.direction }, sunColour: { value: sun.colour } };
    const colour = isNear ? nearLeaves : farLeaves;
    return patchMaterial(material, { uniforms, declarations, colour, light: translucency });
}

export function createBarkMaterial() {
    return new MeshStandardMaterial({ vertexColors: true, roughness: 0.95, metalness: 0 });
}
