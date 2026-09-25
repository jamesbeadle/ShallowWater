export const shaderNoise = /* glsl */ `
    float hashOf(vec3 point) {
        point = fract(point * 0.3183099 + 0.1);
        point *= 17.0;
        return fract(point.x * point.y * point.z * (point.x + point.y + point.z));
    }

    float noiseAt(vec3 point) {
        vec3 cell = floor(point);
        vec3 blend = fract(point);
        blend = blend * blend * (3.0 - 2.0 * blend);
        float lowerNear = mix(hashOf(cell), hashOf(cell + vec3(1.0, 0.0, 0.0)), blend.x);
        float lowerFar = mix(hashOf(cell + vec3(0.0, 1.0, 0.0)), hashOf(cell + vec3(1.0, 1.0, 0.0)), blend.x);
        float upperNear = mix(hashOf(cell + vec3(0.0, 0.0, 1.0)), hashOf(cell + vec3(1.0, 0.0, 1.0)), blend.x);
        float upperFar = mix(hashOf(cell + vec3(0.0, 1.0, 1.0)), hashOf(cell + vec3(1.0, 1.0, 1.0)), blend.x);
        return mix(mix(lowerNear, lowerFar, blend.y), mix(upperNear, upperFar, blend.y), blend.z);
    }

    float fractalAt(vec3 point) {
        return 0.5 * noiseAt(point) + 0.25 * noiseAt(point * 2.07) + 0.125 * noiseAt(point * 4.13) + 0.125 * noiseAt(point * 8.3);
    }
`;
