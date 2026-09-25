import { InstancedMesh, Matrix4, Quaternion, Vector3 } from 'three';
import { createBarkMaterial, createFoliageMaterial } from './foliageMaterial.js';
import { canopyGeometryOf, trunkGeometryOf } from './treeGeometry.js';
import { TreeKinds } from './treeKinds.js';
import { createRandom, pick } from './random.js';
import { keepFromReflections } from './layers.js';

const upward = new Vector3(0, 1, 0);
const chunkSize = 160;

function placementMatrix(placement) {
    const turn = new Quaternion().setFromAxisAngle(upward, placement.turn);
    const size = new Vector3(placement.scale, placement.scale * placement.stretch, placement.scale);
    const position = new Vector3(placement.x, placement.y, placement.z);
    return new Matrix4().compose(position, turn, size);
}

function instancesOf(geometry, material, placements, shouldCastShadow) {
    const mesh = new InstancedMesh(geometry, material, placements.length);
    placements.forEach((placement, index) => mesh.setMatrixAt(index, placementMatrix(placement)));
    mesh.castShadow = shouldCastShadow;
    mesh.receiveShadow = true;
    return mesh;
}

function groupKey(placement) {
    const column = Math.floor(placement.x / chunkSize);
    const row = Math.floor(placement.z / chunkSize);
    return `${placement.kind}|${placement.detail}|${column}|${row}`;
}

function plantGroup(placements, materials) {
    const [first] = placements;
    const kind = TreeKinds[first.kind];
    const seed = Math.abs(Math.round(first.x * 7 + first.z * 13));
    const random = createRandom(seed);
    const isNear = first.detail === 'near';
    const foliage = isNear ? materials.nearFoliage : materials.farFoliage;
    const canopies = instancesOf(canopyGeometryOf(first.kind, first.detail, seed), foliage, placements, isNear);
    placements.forEach((placement, index) => canopies.setColorAt(index, pick(random, kind.colours)));
    const trunks = kind.trunk ? [instancesOf(trunkGeometryOf(first.kind), materials.bark, placements, isNear)] : [];
    const meshes = [canopies, ...trunks];
    meshes.forEach((mesh) => mesh.computeBoundingSphere());
    return isNear ? meshes : meshes.map(keepFromReflections);
}

export function plantForest(placements, sun) {
    const materials = { nearFoliage: createFoliageMaterial(sun, true), farFoliage: createFoliageMaterial(sun, false), bark: createBarkMaterial() };
    const groups = Map.groupBy(placements, groupKey);
    return [...groups.values()].flatMap((group) => plantGroup(group, materials));
}
