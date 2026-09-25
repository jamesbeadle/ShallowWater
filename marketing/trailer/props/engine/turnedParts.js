import { CatmullRomCurve3, CylinderGeometry, LatheGeometry, Mesh, TubeGeometry, Vector2, Vector3 } from 'three';
import { mergeGeometries } from 'three/addons/utils/BufferGeometryUtils.js';

const Smoothness = { turnedSides: 32, pipeSteps: 48, pipeSides: 10, nutSides: 6 };

function shaded(mesh) {
    mesh.castShadow = true;
    mesh.receiveShadow = true;
    return mesh;
}

export function turnedGeometry(profile, sides = Smoothness.turnedSides) {
    return new LatheGeometry(profile.map(([radius, height]) => new Vector2(radius, height)), sides);
}

export function turned(profile, material, sides) {
    return shaded(new Mesh(turnedGeometry(profile, sides), material));
}

export function pipe(points, radius, material) {
    const curve = new CatmullRomCurve3(points.map(([x, y, z]) => new Vector3(x, y, z)));
    return shaded(new Mesh(new TubeGeometry(curve, Smoothness.pipeSteps, radius, Smoothness.pipeSides), material));
}

export function nutRing({ count, ringRadius, height, nutRadius, material }) {
    const nuts = Array.from({ length: count }, (unused, index) => {
        const angle = (index / count) * Math.PI * 2;
        const nut = new CylinderGeometry(nutRadius, nutRadius, nutRadius * 1.2, Smoothness.nutSides);
        nut.translate(Math.cos(angle) * ringRadius, height, Math.sin(angle) * ringRadius);
        return nut;
    });
    return shaded(new Mesh(mergeGeometries(nuts), material));
}

export function placed(mesh, [x, y, z], [turnX, turnY, turnZ] = [0, 0, 0]) {
    mesh.position.set(x, y, z);
    mesh.rotation.set(turnX, turnY, turnZ);
    return mesh;
}

export function castingOf(geometry, material) {
    return shaded(new Mesh(geometry, material));
}
