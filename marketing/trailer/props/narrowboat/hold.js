import { BufferGeometry, CylinderGeometry, DoubleSide, Float32BufferAttribute, Mesh, MeshStandardMaterial, Quaternion, Vector3 } from 'three';
import { mergeGeometries } from 'three/addons/utils/BufferGeometryUtils.js';
import { srgb } from '../../world/colours.js';
import { Boat, Hold } from './dimensions.js';
import { halfBeamAt, sheerAt } from './hull.js';

const stations = 40;
const upward = new Vector3(0, 1, 0);

function tentEdges(x) {
    const gunwale = sheerAt(x) - Boat.draft;
    const halfWidth = halfBeamAt(x) * 0.97;
    return { left: new Vector3(x, gunwale, -halfWidth), apex: new Vector3(x, gunwale + Hold.plankHeight, 0), right: new Vector3(x, gunwale, halfWidth) };
}

function tentGeometry(from, to) {
    const positions = [];
    for (let station = 0; station < stations; station += 1) {
        const near = tentEdges(from + ((to - from) * station) / stations);
        const far = tentEdges(from + ((to - from) * (station + 1)) / stations);
        [[near.left, near.apex, far.left, far.left, near.apex, far.apex], [near.apex, near.right, far.apex, far.apex, near.right, far.right]]
            .flat()
            .forEach((point) => positions.push(point.x, point.y, point.z));
    }
    const geometry = new BufferGeometry();
    geometry.setAttribute('position', new Float32BufferAttribute(positions, 3));
    geometry.computeVertexNormals();
    return geometry;
}

function stringBetween(start, end) {
    const length = start.distanceTo(end);
    const rope = new CylinderGeometry(0.012, 0.012, length, 5);
    rope.translate(0, length / 2, 0);
    const direction = new Vector3().subVectors(end, start).normalize();
    rope.applyQuaternion(new Quaternion().setFromUnitVectors(upward, direction));
    rope.translate(start.x, start.y + 0.012, start.z);
    return rope;
}

function stringsGeometry(from, to) {
    const ropes = [];
    for (let x = from + 0.3; x < to; x += Hold.stringSpacing) {
        const { left, apex, right } = tentEdges(x);
        ropes.push(stringBetween(left, apex), stringBetween(right, apex));
    }
    return mergeGeometries(ropes);
}

export function buildHold(holdStart, holdEnd) {
    const cloth = new MeshStandardMaterial({ color: srgb(0.07, 0.068, 0.062), roughness: 0.93, side: DoubleSide });
    const rope = new MeshStandardMaterial({ color: srgb(0.78, 0.74, 0.64), roughness: 0.9 });
    const tent = new Mesh(tentGeometry(holdStart, holdEnd), cloth);
    const strings = new Mesh(stringsGeometry(holdStart, holdEnd), rope);
    [tent, strings].forEach((mesh) => Object.assign(mesh, { castShadow: true, receiveShadow: true }));
    return [tent, strings];
}
