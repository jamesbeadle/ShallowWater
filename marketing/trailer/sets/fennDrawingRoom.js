import { BoxGeometry, ConeGeometry, CylinderGeometry, ExtrudeGeometry, Group, Mesh, MeshStandardMaterial, PlaneGeometry, PointLight, Shape } from 'three';
import { mergeGeometries } from 'three/addons/utils/BufferGeometryUtils.js';
import { paintTexture } from '../textures/canvasTexture.js';
import { srgb } from '../world/colours.js';

const Room = { halfWidth: 2.3, depth: 4.8, height: 3.9, front: -0.5 };
const Chandelier = { colour: srgb(1.0, 0.7, 0.42), intensity: 16, reach: 10, decay: 1.3, height: 3.2, back: 3.2 };
const Drape = { width: 0.75, folds: 5, swing: 0.06, thickness: 0.05, colour: srgb(0.32, 0.05, 0.05) };
const Sash = { bar: 0.035, frame: 0.07, depth: 0.05, colour: srgb(0.85, 0.82, 0.76) };

function paintStripes(context, width, height) {
    [0, 1, 2, 3].forEach((stripe) => {
        context.fillStyle = stripe % 2 === 0 ? 'rgb(214, 186, 132)' : 'rgb(186, 150, 96)';
        context.fillRect((stripe * width) / 4, 0, width / 4, height);
    });
}

function shellOf(opening, floor) {
    const papered = paintTexture(64, 16, paintStripes);
    papered.repeat.set(Room.halfWidth * 4, 1);
    const walls = new MeshStandardMaterial({ map: papered, roughness: 0.85 });
    const boards = new MeshStandardMaterial({ color: srgb(0.16, 0.09, 0.05), roughness: 0.45 });
    const middleZ = Room.front - Room.depth / 2;
    const plane = (width, height, material, [x, y, z], [turnX, turnY]) => {
        const surface = new Mesh(new PlaneGeometry(width, height), material);
        surface.position.set(x, y, z);
        surface.rotation.set(turnX, turnY, 0);
        return surface;
    };
    const centre = opening.x;
    return [
        plane(Room.halfWidth * 2, Room.height, walls, [centre, floor + Room.height / 2, Room.front - Room.depth], [0, 0]),
        plane(Room.depth, Room.height, walls, [centre - Room.halfWidth, floor + Room.height / 2, middleZ], [0, Math.PI / 2]),
        plane(Room.depth, Room.height, walls, [centre + Room.halfWidth, floor + Room.height / 2, middleZ], [0, -Math.PI / 2]),
        plane(Room.halfWidth * 2, Room.depth, boards, [centre, floor, middleZ], [-Math.PI / 2, 0]),
        plane(Room.halfWidth * 2, Room.depth, walls, [centre, floor + Room.height, middleZ], [Math.PI / 2, 0]),
    ];
}

function drapeGeometry(height) {
    const outline = new Shape();
    const steps = Drape.folds * 8;
    const waveAt = (step) => Math.sin((step / steps) * Math.PI * 2 * Drape.folds) * Drape.swing;
    outline.moveTo(0, waveAt(0));
    for (let step = 1; step <= steps; step += 1) {
        outline.lineTo((step / steps) * Drape.width, waveAt(step));
    }
    for (let step = steps; step >= 0; step -= 1) {
        outline.lineTo((step / steps) * Drape.width, waveAt(step) - Drape.thickness);
    }
    const geometry = new ExtrudeGeometry(outline, { depth: height, bevelEnabled: false });
    geometry.rotateX(-Math.PI / 2);
    return geometry;
}

function curtainsAround(opening) {
    const velvet = new MeshStandardMaterial({ color: Drape.colour, roughness: 0.8 });
    const height = opening.height + 0.6;
    return [-1, 1].map((side) => {
        const drape = new Mesh(drapeGeometry(height), velvet);
        const inner = opening.x + side * (opening.width / 2 - 0.12);
        drape.position.set(side < 0 ? inner - Drape.width : inner, opening.bottom - 0.2, Room.front - 0.15);
        return drape;
    });
}

function sashBars(opening) {
    const { x, bottom, width, height } = opening;
    const bar = (barWidth, barHeight, barX, barY) => {
        const piece = new BoxGeometry(barWidth, barHeight, Sash.depth);
        piece.translate(barX, barY, 0);
        return piece;
    };
    const verticals = [-0.5, -1 / 6, 1 / 6, 0.5].map((across) => bar(across ** 2 > 0.2 ? Sash.frame : Sash.bar, height, x + across * width, bottom + height / 2));
    const horizontals = [0, 0.25, 0.5, 0.75, 1].map((up) => bar(width, up % 1 === 0 ? Sash.frame : Sash.bar, x, bottom + up * height));
    const frame = new Mesh(mergeGeometries([...verticals, ...horizontals]), new MeshStandardMaterial({ color: Sash.colour, roughness: 0.5 }));
    frame.position.setZ(-0.28);
    return frame;
}

function standardLamp(opening, floor) {
    const shade = new Mesh(new ConeGeometry(0.28, 0.32, 20, 1, true), new MeshStandardMaterial({ color: srgb(0.4, 0.3, 0.18), emissive: Chandelier.colour, emissiveIntensity: 2.4 }));
    shade.position.set(opening.x + 1.5, floor + 1.55, Room.front - Room.depth + 0.6);
    const stem = new Mesh(new CylinderGeometry(0.018, 0.03, 1.4, 8), new MeshStandardMaterial({ color: srgb(0.3, 0.22, 0.1), metalness: 0.8, roughness: 0.3 }));
    stem.position.set(opening.x + 1.5, floor + 0.7, Room.front - Room.depth + 0.6);
    return [shade, stem];
}

export function buildDrawingRoom(opening, floor) {
    const room = new Group();
    const light = new PointLight(Chandelier.colour, Chandelier.intensity, Chandelier.reach, Chandelier.decay);
    light.position.set(opening.x - 0.3, floor + Chandelier.height, Room.front - Chandelier.back);
    room.add(...shellOf(opening, floor), ...curtainsAround(opening), sashBars(opening), ...standardLamp(opening, floor), light);
    return { room, light };
}
