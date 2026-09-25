import { BoxGeometry, CapsuleGeometry, Group, Mesh, SphereGeometry } from 'three';

const Smoothness = { capSegments: 4, radialSegments: 10 };

export function hangingLimb(length, radius, material) {
    const geometry = new CapsuleGeometry(radius, length, Smoothness.capSegments, Smoothness.radialSegments);
    geometry.translate(0, -length / 2, 0);
    const limb = new Mesh(geometry, material);
    limb.castShadow = true;
    return limb;
}

export function joint(x, y, z) {
    const pivot = new Group();
    pivot.position.set(x, y, z);
    return pivot;
}

export function boot(length, material) {
    const geometry = new BoxGeometry(0.1, 0.09, length);
    geometry.translate(0, -0.045, length * 0.3);
    const shoe = new Mesh(geometry, material);
    shoe.castShadow = true;
    return shoe;
}

export function hand(material) {
    const geometry = new SphereGeometry(0.048, 10, 8);
    geometry.scale(0.8, 1.15, 0.6);
    geometry.translate(0, -0.05, 0);
    const palm = new Mesh(geometry, material);
    palm.castShadow = true;
    return palm;
}
