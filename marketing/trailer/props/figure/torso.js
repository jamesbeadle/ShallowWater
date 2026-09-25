import { CapsuleGeometry, LatheGeometry, Mesh, SphereGeometry, Vector2 } from 'three';
import { Girth, Proportions } from './proportions.js';

const Coat = { waist: 0.18, hem: 0.26, length: 0.6, skirtLength: 0.8 };
const Chest = { depth: 0.66 };
const torsoOutline = [[0.001, -0.06], [0.15, -0.05], [0.16, 0.08], [0.175, 0.25], [0.2, 0.38], [0.19, 0.45], [0.12, 0.5], [0.05, 0.52]];

function shaded(mesh) {
    mesh.castShadow = true;
    return mesh;
}

export function chestMesh(material) {
    const outline = torsoOutline.map(([radius, height]) => new Vector2(radius, height * (Proportions.torso / 0.5)));
    const geometry = new LatheGeometry(outline, 20);
    geometry.scale(1, 1, Chest.depth);
    const chest = shaded(new Mesh(geometry, material));
    [-1, 1].forEach((side) => {
        const shoulder = shaded(new Mesh(new SphereGeometry(Girth.upperArm * 1.02, 12, 10), material));
        shoulder.position.set(side * Proportions.shoulderSpread * 0.94, Proportions.torso - 0.08, 0);
        chest.add(shoulder);
    });
    return chest;
}

export function hipsMesh(material) {
    const geometry = new CapsuleGeometry(0.125, 0.1, 4, 12);
    geometry.rotateZ(Math.PI / 2);
    geometry.scale(1, 1, 0.75);
    geometry.translate(0, -0.03, 0);
    return shaded(new Mesh(geometry, material));
}

export function neckMesh(material) {
    const geometry = new CapsuleGeometry(Girth.neck, Proportions.neck, 2, 8);
    geometry.translate(0, Proportions.neck / 2, 0);
    return new Mesh(geometry, material);
}

export function coatSkirt(material, isLong) {
    const length = isLong ? Coat.skirtLength : Coat.length;
    const outline = [new Vector2(Coat.waist, 0.12), new Vector2(Coat.waist * 1.08, -0.1), new Vector2(Coat.hem, -length)];
    const geometry = new LatheGeometry(outline, 18);
    geometry.scale(1, 1, Chest.depth + 0.14);
    return shaded(new Mesh(geometry, material));
}
