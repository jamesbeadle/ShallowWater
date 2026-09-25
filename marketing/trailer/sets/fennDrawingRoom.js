import { BoxGeometry, ConeGeometry, CylinderGeometry, ExtrudeGeometry, Group, Mesh, MeshBasicMaterial, PlaneGeometry, Shape } from 'three';
import { mergeGeometries } from 'three/addons/utils/BufferGeometryUtils.js';
import { paintTexture } from '../textures/canvasTexture.js';
import { srgb } from '../world/colours.js';

const Room = { halfWidth: 2.3, depth: 4.8, height: 3.9, front: -0.5 };
const Lamplit = { backWall: srgb(1.25, 0.95, 0.66), sideWall: srgb(0.8, 0.58, 0.38), ceiling: srgb(0.55, 0.4, 0.28), floor: srgb(0.12, 0.07, 0.04), shade: srgb(3.2, 2.2, 1.3) };
const Drape = { width: 0.75, folds: 5, swing: 0.06, thickness: 0.05, colour: srgb(0.22, 0.035, 0.03) };
const Sash = { bar: 0.035, frame: 0.07, depth: 0.05, colour: srgb(0.1, 0.1, 0.12) };

function paintStripes(context, width, height) {
    [0, 1, 2, 3].forEach((stripe) => {
        context.fillStyle = stripe % 2 === 0 ? 'rgb(214, 186, 132)' : 'rgb(170, 134, 84)';
        context.fillRect((stripe * width) / 4, 0, width / 4, height);
    });
}

function wallpaper(glow) {
    const papered = paintTexture(64, 16, paintStripes);
    papered.repeat.set(Room.halfWidth * 4, 1);
    return new MeshBasicMaterial({ map: papered, color: glow });
}

function plane(width, height, material, [x, y, z], [turnX, turnY]) {
    const surface = new Mesh(new PlaneGeometry(width, height), material);
    surface.position.set(x, y, z);
    surface.rotation.set(turnX, turnY, 0);
    return surface;
}

function shellOf({ x }, floor) {
    const middleZ = Room.front - Room.depth / 2;
    const middleY = floor + Room.height / 2;
    const side = wallpaper(Lamplit.sideWall);
    return [
        plane(Room.halfWidth * 2, Room.height, wallpaper(Lamplit.backWall), [x, middleY, Room.front - Room.depth], [0, 0]),
        plane(Room.depth, Room.height, side, [x - Room.halfWidth, middleY, middleZ], [0, Math.PI / 2]),
        plane(Room.depth, Room.height, side, [x + Room.halfWidth, middleY, middleZ], [0, -Math.PI / 2]),
        plane(Room.halfWidth * 2, Room.depth, new MeshBasicMaterial({ color: Lamplit.floor }), [x, floor, middleZ], [-Math.PI / 2, 0]),
        plane(Room.halfWidth * 2, Room.depth, new MeshBasicMaterial({ color: Lamplit.ceiling }), [x, floor + Room.height, middleZ], [Math.PI / 2, 0]),
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
    const velvet = new MeshBasicMaterial({ color: Drape.colour });
    return [-1, 1].map((side) => {
        const drape = new Mesh(drapeGeometry(opening.height + 0.6), velvet);
        const inner = opening.x + side * (opening.width / 2 - 0.12);
        drape.position.set(side < 0 ? inner - Drape.width : inner, opening.bottom - 0.2, Room.front - 0.15);
        return drape;
    });
}

function sashBars({ x, bottom, width, height }) {
    const bar = (barWidth, barHeight, barX, barY) => {
        const piece = new BoxGeometry(barWidth, barHeight, Sash.depth);
        piece.translate(barX, barY, 0);
        return piece;
    };
    const verticals = [-0.5, -1 / 6, 1 / 6, 0.5].map((across) => bar(Sash.bar, height, x + across * width, bottom + height / 2));
    const horizontals = [0, 0.25, 0.5, 0.75, 1].map((up) => bar(width, Sash.bar, x, bottom + up * height));
    const frame = new Mesh(mergeGeometries([...verticals, ...horizontals]), new MeshBasicMaterial({ color: Sash.colour }));
    frame.position.setZ(-0.28);
    return frame;
}

function standardLamp({ x }, floor) {
    const shade = new Mesh(new ConeGeometry(0.28, 0.32, 20, 1, true), new MeshBasicMaterial({ color: Lamplit.shade }));
    shade.position.set(x + 1.5, floor + 1.55, Room.front - Room.depth + 0.6);
    const stem = new Mesh(new CylinderGeometry(0.018, 0.03, 1.4, 8), new MeshBasicMaterial({ color: srgb(0.2, 0.14, 0.07) }));
    stem.position.set(x + 1.5, floor + 0.7, Room.front - Room.depth + 0.6);
    return [shade, stem];
}

export function buildDrawingRoom(opening, floor) {
    const room = new Group();
    room.add(...shellOf(opening, floor), ...curtainsAround(opening), sashBars(opening), ...standardLamp(opening, floor));
    return room;
}
