import { Color, Vector3 } from 'three';
import { patchMaterial } from '../../world/materialPatches.js';

const declarations = /* glsl */ `
    uniform vec3 rimColour;
    uniform vec3 rimDirection;
    uniform float rimStrength;
`;

const rim = /* glsl */ `
    vec3 surfaceNormal = normalize(normal);
    float grazing = 1.0 - clamp(dot(surfaceNormal, normalize(vViewPosition)), 0.0, 1.0);
    vec3 towardLight = normalize((viewMatrix * vec4(rimDirection, 0.0)).xyz);
    float lightSide = 0.15 + 0.85 * max(dot(surfaceNormal, towardLight), 0.0);
    outgoingLight += rimColour * rimStrength * pow(grazing, 2.5) * lightSide;
`;

export function createRim(colour = new Color(1, 0.75, 0.5), strength = 0, direction = new Vector3(0, 1, 0)) {
    return { rimColour: { value: colour }, rimStrength: { value: strength }, rimDirection: { value: direction } };
}

export function withRim(material, rimUniforms) {
    return patchMaterial(material, { uniforms: rimUniforms, declarations, light: rim });
}
