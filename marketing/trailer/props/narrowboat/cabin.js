import { DoubleSide, ExtrudeGeometry, Mesh, MeshStandardMaterial, PlaneGeometry, Shape } from 'three';
import { paintCabinSide } from './livery.js';
import { paintRosesAndCastles } from './rosesAndCastles.js';
import { Boat, Cabin, EngineRoom, freeboard, sternX } from './dimensions.js';
import { halfBeamAt } from './hull.js';
import { rgb } from '../../textures/canvasTexture.js';

function crossSection(halfWidth, height) {
    const base = freeboard();
    const section = new Shape();
    section.moveTo(-halfWidth, base);
    section.lineTo(-halfWidth, base + height);
    section.quadraticCurveTo(0, base + height + Cabin.camber * 2, halfWidth, base + height);
    section.lineTo(halfWidth, base);
    return section;
}

function box(start, length, height, material) {
    const halfWidth = halfBeamAt(start + length / 2) - Cabin.inset;
    const geometry = new ExtrudeGeometry(crossSection(halfWidth, height), { depth: length, bevelEnabled: false, curveSegments: 8 });
    geometry.rotateY(Math.PI / 2);
    geometry.translate(start, 0, 0);
    const mesh = new Mesh(geometry, material);
    mesh.castShadow = true;
    mesh.receiveShadow = true;
    return { mesh, halfWidth };
}

function sidePanels(start, halfWidth, material) {
    return [-1, 1].map((side) => {
        const panel = new Mesh(new PlaneGeometry(Cabin.length - 0.25, Cabin.height - 0.18), material);
        panel.position.set(start + Cabin.length / 2, freeboard() + Cabin.height / 2, side * (halfWidth + 0.006));
        panel.rotateY(side > 0 ? 0 : Math.PI);
        return panel;
    });
}

function sternDoors(start, halfWidth) {
    const texture = paintRosesAndCastles();
    texture.repeat.set(2, 1);
    const paintwork = new MeshStandardMaterial({ map: texture, roughness: 0.55, side: DoubleSide });
    const doors = new Mesh(new PlaneGeometry(halfWidth * 1.5, Cabin.height - 0.1), paintwork);
    doors.position.set(start - 0.006, freeboard() + Cabin.height / 2, 0);
    doors.rotateY(-Math.PI / 2);
    return doors;
}

export function buildCabins(livery) {
    const paint = new MeshStandardMaterial({ color: rgb(...livery.field), roughness: 0.55 });
    const cabinStart = sternX() + Cabin.sternDeck;
    const cabin = box(cabinStart, Cabin.length, Cabin.height, paint);
    const engineRoom = box(cabinStart + Cabin.length, EngineRoom.length, EngineRoom.height, paint);
    const sideMaterial = new MeshStandardMaterial({ map: paintCabinSide(livery), roughness: 0.5 });
    const parts = [cabin.mesh, engineRoom.mesh, ...sidePanels(cabinStart, cabin.halfWidth, sideMaterial), sternDoors(cabinStart, cabin.halfWidth)];
    return { parts, cabinStart, holdStart: cabinStart + Cabin.length + EngineRoom.length, holdEnd: Boat.length / 2 - 1.4 };
}
