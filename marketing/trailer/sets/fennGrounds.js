import { Mesh, MeshStandardMaterial, PlaneGeometry, Vector3 } from 'three';
import { srgb } from '../world/colours.js';
import { patchMaterial } from '../world/materialPatches.js';
import { between, createRandom } from '../world/random.js';

const Lawn = { size: 700, detail: 1 };
const PoolCount = 6;
const porticoClearance = 4.8;

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
    float tufts = fractalAt(vWorldPosition * 0.6);
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

const Woods = [
    { kind: 'oak', count: 16, x: [-45, 45], z: [-40, -18], scale: [1.3, 1.8] },
    { kind: 'oak', count: 7, x: [-42, -17], z: [-14, 12], scale: [1.2, 1.7] },
    { kind: 'pine', count: 3, x: [-30, -20], z: [-8, 6], scale: [1.4, 1.7] },
    { kind: 'oak', count: 7, x: [17, 44], z: [-14, 10], scale: [1.2, 1.7] },
    { kind: 'bush', count: 10, x: [-16, 16], z: [-1, 1.5], scale: [0.9, 1.3] },
];

function clearOfThePortico(x) {
    return Math.sign(x) * Math.max(Math.abs(x), porticoClearance);
}

export function treePlacements() {
    const random = createRandom(1754);
    return Woods.flatMap((wood) => Array.from({ length: wood.count }, () => {
        const x = clearOfThePortico(between(random, wood.x[0], wood.x[1]));
        const z = between(random, wood.z[0], wood.z[1]);
        const scale = between(random, wood.scale[0], wood.scale[1]);
        return { kind: wood.kind, detail: 'near', x, y: 0, z, scale, stretch: between(random, 0.9, 1.15), turn: random() * Math.PI * 2 };
    }));
}

export const poolsBeforeWindows = (openings) => openings.map(({ x }) => new Vector3(x, 1, 3.2));
