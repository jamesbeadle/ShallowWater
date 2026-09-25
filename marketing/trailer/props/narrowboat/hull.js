import { BufferGeometry, DoubleSide, Float32BufferAttribute, Mesh, MeshStandardMaterial } from 'three';
import { srgb } from '../../world/colours.js';
import { Boat } from './dimensions.js';

const Lofting = { stations: 60, sectionPoints: 18, boxiness: 4 };

export function halfBeamAt(x) {
    const fromStern = x + Boat.length / 2;
    const toBow = Boat.length / 2 - x;
    const bowTaper = 1 - Math.max(0, 1 - toBow / Boat.bowLength) ** 1.7;
    const sternTaper = 0.62 + 0.38 * Math.min(fromStern / Boat.sternLength, 1) ** 0.6;
    return (Boat.beam / 2) * Math.min(bowTaper, sternTaper);
}

export function sheerAt(x) {
    const bowward = Math.max(0, 1 - (Boat.length / 2 - x) / Boat.bowLength);
    const sternward = Math.max(0, 1 - (x + Boat.length / 2) / Boat.sternLength);
    return Boat.depth + Boat.bowRise * bowward ** 2 + Boat.sternRise * sternward ** 2;
}

function sectionPoint(halfBeam, sheer, fraction) {
    const angle = Math.PI + fraction * Math.PI;
    const across = Math.sign(Math.cos(angle)) * Math.abs(Math.cos(angle)) ** (2 / Lofting.boxiness);
    const down = Math.abs(Math.sin(angle)) ** (2 / Lofting.boxiness);
    return [halfBeam * across, sheer * (1 - down)];
}

function hullGrid() {
    const positions = [];
    for (let station = 0; station <= Lofting.stations; station += 1) {
        const x = -Boat.length / 2 + (Boat.length * station) / Lofting.stations;
        const halfBeam = halfBeamAt(x);
        const sheer = sheerAt(x);
        for (let point = 0; point <= Lofting.sectionPoints; point += 1) {
            const [z, y] = sectionPoint(halfBeam, sheer, point / Lofting.sectionPoints);
            positions.push(x, y - Boat.draft, z);
        }
    }
    return positions;
}

function hullIndices() {
    const rowLength = Lofting.sectionPoints + 1;
    const indices = [];
    for (let station = 0; station < Lofting.stations; station += 1) {
        for (let point = 0; point < Lofting.sectionPoints; point += 1) {
            const corner = station * rowLength + point;
            indices.push(corner, corner + rowLength, corner + 1, corner + 1, corner + rowLength, corner + rowLength + 1);
        }
    }
    return indices;
}

export function buildHull() {
    const geometry = new BufferGeometry();
    geometry.setAttribute('position', new Float32BufferAttribute(hullGrid(), 3));
    geometry.setIndex(hullIndices());
    geometry.computeVertexNormals();
    const material = new MeshStandardMaterial({ color: srgb(0.035, 0.032, 0.03), roughness: 0.42, metalness: 0.05, side: DoubleSide });
    const hull = new Mesh(geometry, material);
    hull.castShadow = true;
    hull.receiveShadow = true;
    return hull;
}

