import { BufferGeometry, DoubleSide, Float32BufferAttribute, InstancedMesh, Matrix4, MeshStandardMaterial, Quaternion, Vector3 } from 'three';
import { between, createRandom, pick } from './random.js';

const Frond = { count: 6, length: 0.75, width: 0.16, droop: 0.35 };
const upward = new Vector3(0, 1, 0);

function frondTriangles(index) {
    const angle = (index / Frond.count) * Math.PI * 2;
    const reach = [Math.cos(angle) * Frond.length, Frond.length * (1 - Frond.droop), Math.sin(angle) * Frond.length];
    const side = [-Math.sin(angle) * Frond.width, 0, Math.cos(angle) * Frond.width];
    const middle = reach.map((value, axis) => value * 0.5 + side[axis]);
    return [0, 0, 0, ...middle, ...reach, 0, 0, 0, ...reach, ...reach.map((value, axis) => value * 0.5 - side[axis])];
}

function shadesFor(positions) {
    const colours = [];
    for (let heightIndex = 1; heightIndex < positions.length; heightIndex += 3) {
        const shade = 0.4 + positions[heightIndex] / Frond.length;
        colours.push(shade, shade, shade);
    }
    return colours;
}

function tuftGeometry() {
    const positions = Array.from({ length: Frond.count }, (unused, index) => frondTriangles(index)).flat();
    const colours = shadesFor(positions);
    const geometry = new BufferGeometry();
    geometry.setAttribute('position', new Float32BufferAttribute(positions, 3));
    geometry.setAttribute('color', new Float32BufferAttribute(colours, 3));
    geometry.computeVertexNormals();
    return geometry;
}

export function scatterGroundCover({ count, pointAt, colours, seed, size = [0.7, 1.3] }) {
    const random = createRandom(seed);
    const material = new MeshStandardMaterial({ vertexColors: true, roughness: 0.9, side: DoubleSide });
    const tufts = new InstancedMesh(tuftGeometry(), material, count);
    for (let index = 0; index < count; index += 1) {
        const scale = between(random, size[0], size[1]);
        const turn = new Quaternion().setFromAxisAngle(upward, between(random, 0, Math.PI * 2));
        tufts.setMatrixAt(index, new Matrix4().compose(pointAt(random), turn, new Vector3(scale, scale, scale)));
        tufts.setColorAt(index, pick(random, colours));
    }
    tufts.receiveShadow = true;
    tufts.computeBoundingSphere();
    return tufts;
}
