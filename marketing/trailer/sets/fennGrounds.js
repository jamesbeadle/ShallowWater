import { Mesh, MeshStandardMaterial, PlaneGeometry, Vector3 } from 'three';
import { srgb } from '../world/colours.js';
import { patchMaterial } from '../world/materialPatches.js';
import { createTreeline } from '../world/treeline.js';

const Lawn = { size: 700, detail: 1 };
const PoolCount = 6;

const declarations = /* glsl */ `
    uniform vec3 gravelColour, lawnColour, poolColour;
    uniform vec3 pools[${PoolCount}];
`;

const surface = /* glsl */ `
    vec2 place = vWorldPosition.xz;
    float sweep = 1.0 - smoothstep(8.6, 9.3, length(place - vec2(0.0, 11.5)));
    float avenue = (1.0 - smoothstep(2.7, 3.3, abs(place.x))) * step(11.5, place.y);
    float apron = (1.0 - smoothstep(3.6, 4.2, place.y)) * step(-1.0, place.y);
    float gravel = max(max(sweep, avenue), apron);
    float grit = noiseAt(vWorldPosition * 16.0);
    float tufts = noiseAt(vWorldPosition * 0.45);
    vec3 lawn = lawnColour * (0.65 + 0.6 * tufts);
    vec3 stones = gravelColour * (0.7 + 0.5 * grit);
    diffuseColor.rgb = mix(lawn, stones, gravel);
`;

const pooledLight = /* glsl */ `
    float pooled = 0.0;
    for (int index = 0; index < ${PoolCount}; index++) {
        vec2 offset = (vWorldPosition.xz - pools[index].xz) * vec2(0.55, 0.3);
        pooled += exp(-dot(offset, offset)) * pools[index].y;
    }
    outgoingLight += poolColour * diffuseColor.rgb * pooled;
`;

export function buildLawnAndDrive(poolPlaces) {
    const material = new MeshStandardMaterial({ color: srgb(1, 1, 1), roughness: 0.95, metalness: 0 });
    const uniforms = {
        gravelColour: { value: srgb(0.42, 0.39, 0.34) }, lawnColour: { value: srgb(0.07, 0.1, 0.06) }, poolColour: { value: srgb(1.0, 0.62, 0.3) },
        pools: { value: poolPlaces.slice(0, PoolCount) },
    };
    patchMaterial(material, { uniforms, declarations, colour: surface, light: pooledLight });
    const ground = new Mesh(new PlaneGeometry(Lawn.size, Lawn.size, Lawn.detail, Lawn.detail), material);
    ground.rotation.set(-Math.PI / 2, 0, 0);
    ground.receiveShadow = true;
    return ground;
}

const Treelines = [
    { width: 150, height: 24, colour: srgb(0.018, 0.024, 0.03), seed: 5, trees: 15, place: [0, -40] },
    { width: 70, height: 20, colour: srgb(0.02, 0.026, 0.03), seed: 9, trees: 7, place: [-50, -6] },
    { width: 60, height: 19, colour: srgb(0.02, 0.026, 0.03), seed: 13, trees: 6, place: [52, -8] },
    { width: 900, height: 16, colour: srgb(0.05, 0.055, 0.07), seed: 21, trees: 70, place: [0, -220], tallest: 0.8 },
];

export function plantTreelines() {
    return Treelines.map(createTreeline);
}

export const poolsBeforeWindows = (openings) => openings.map(({ x }) => new Vector3(x, 1, 3.2));
