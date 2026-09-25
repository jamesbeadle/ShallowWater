import { MeshStandardMaterial } from 'three';
import { srgb } from '../../world/colours.js';
import { patchMaterial } from '../../world/materialPatches.js';

const RoughnessChunk = '#include <roughnessmap_fragment>';

const declarations = /* glsl */ `
    uniform vec3 oilColour;
    uniform vec3 wornColour;
    uniform float grimeScale;
`;

const grime = /* glsl */ `
    float wear = fractalAt(vWorldPosition * grimeScale);
    float runs = noiseAt(vec3(vWorldPosition.x * 70.0, vWorldPosition.y * 5.0, vWorldPosition.z * 70.0));
    float oiled = smoothstep(0.45, 0.8, runs * 0.55 + wear * 0.6);
    float rubbed = smoothstep(0.8, 0.95, fractalAt(vWorldPosition * grimeScale * 2.3 + 7.0));
    diffuseColor.rgb *= 0.45 + 0.9 * wear;
    diffuseColor.rgb = mix(diffuseColor.rgb, wornColour, rubbed * 0.45);
    diffuseColor.rgb = mix(diffuseColor.rgb, oilColour, oiled * 0.75);
`;

const sheen = /* glsl */ `
    roughnessFactor = clamp(roughnessFactor * (1.3 - 1.05 * oiled) - rubbed * 0.15, 0.05, 1.0);
`;

function withOilSheen(material) {
    const { onBeforeCompile, customProgramCacheKey } = material;
    material.onBeforeCompile = (shader) => {
        onBeforeCompile(shader);
        shader.fragmentShader = shader.fragmentShader.replace(RoughnessChunk, `${RoughnessChunk}\n${sheen}`);
    };
    material.customProgramCacheKey = () => `${customProgramCacheKey()}${sheen}`;
    return material;
}

export function createGrimyMetal({ colour, roughness, metalness, worn = srgb(0.5, 0.48, 0.44), grimeScale = 9 }) {
    const material = new MeshStandardMaterial({ color: colour, roughness, metalness });
    const uniforms = { oilColour: { value: srgb(0.012, 0.011, 0.009) }, wornColour: { value: worn }, grimeScale: { value: grimeScale } };
    return withOilSheen(patchMaterial(material, { uniforms, declarations, colour: grime }));
}

export function createEngineMaterials() {
    return {
        paint: createGrimyMetal({ colour: srgb(0.09, 0.19, 0.12), roughness: 0.42, metalness: 0.15, worn: srgb(0.34, 0.33, 0.31) }),
        iron: createGrimyMetal({ colour: srgb(0.05, 0.048, 0.045), roughness: 0.4, metalness: 0.7, worn: srgb(0.55, 0.53, 0.5), grimeScale: 12 }),
        brass: createGrimyMetal({ colour: srgb(0.78, 0.56, 0.24), roughness: 0.24, metalness: 1, worn: srgb(0.95, 0.78, 0.45), grimeScale: 14 }),
        copper: createGrimyMetal({ colour: srgb(0.66, 0.34, 0.2), roughness: 0.32, metalness: 1, worn: srgb(0.85, 0.5, 0.3), grimeScale: 16 }),
        glass: new MeshStandardMaterial({ color: srgb(0.35, 0.3, 0.18), roughness: 0.05, metalness: 0.2 }),
        timber: createGrimyMetal({ colour: srgb(0.2, 0.12, 0.07), roughness: 0.75, metalness: 0, worn: srgb(0.3, 0.2, 0.12), grimeScale: 6 }),
    };
}
