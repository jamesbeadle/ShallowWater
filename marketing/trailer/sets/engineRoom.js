import { BoxGeometry, ExtrudeGeometry, Group, Mesh, MeshStandardMaterial, Path, PlaneGeometry, Shape } from 'three';
import { createPlankTexture } from '../textures/plankTexture.js';
import { createRivetedSteel } from '../textures/rivetedSteelTexture.js';
import { srgb } from '../world/colours.js';

export const Room = { fore: -1.6, aft: 1.3, far: -1.0, near: 1.3, roof: 1.6 };
export const SideDoor = { from: -0.32, to: 0.2, sill: 0.74, head: 1.42 };
const Carlines = { positions: [-1.05, -0.15, 0.75], width: 0.09, depth: 0.1 };
const metresPerPlate = 1.25;
const metresPerBoardRepeat = 1.2;

function steelMaterial() {
    const { map, bumpMap } = createRivetedSteel();
    [map, bumpMap].forEach((texture) => texture.repeat.set(1 / metresPerPlate, 1 / metresPerPlate));
    return new MeshStandardMaterial({ map, bumpMap, bumpScale: 1.2, roughness: 0.62, metalness: 0.35 });
}

function sideWithDoor(material) {
    const outline = new Shape();
    outline.moveTo(Room.fore, 0);
    [[Room.aft, 0], [Room.aft, Room.roof], [Room.fore, Room.roof]].forEach(([x, y]) => outline.lineTo(x, y));
    const doorway = new Path();
    doorway.moveTo(SideDoor.from, SideDoor.sill);
    [[SideDoor.to, SideDoor.sill], [SideDoor.to, SideDoor.head], [SideDoor.from, SideDoor.head]].forEach(([x, y]) => doorway.lineTo(x, y));
    outline.holes.push(doorway);
    const wall = new Mesh(new ExtrudeGeometry(outline, { depth: 0.04, bevelEnabled: false }), material);
    wall.position.set(0, 0, Room.far - 0.04);
    return wall;
}

function bulkhead(material, x, turn) {
    const depth = Room.near - Room.far;
    const panel = new Mesh(new PlaneGeometry(depth, Room.roof), material);
    panel.position.set(x, Room.roof / 2, (Room.near + Room.far) / 2);
    panel.rotation.set(0, turn, 0);
    return panel;
}

function levelPlane(material, height, turn) {
    const plane = new Mesh(new PlaneGeometry(Room.aft - Room.fore, Room.near - Room.far), material);
    plane.rotation.set(turn, 0, 0);
    plane.position.set((Room.aft + Room.fore) / 2, height, (Room.near + Room.far) / 2);
    return plane;
}

function floorAndRoof() {
    const boards = createPlankTexture();
    boards.repeat.set((Room.aft - Room.fore) / metresPerBoardRepeat, (Room.near - Room.far) / metresPerBoardRepeat);
    const floor = levelPlane(new MeshStandardMaterial({ map: boards, roughness: 0.55, metalness: 0 }), 0, -Math.PI / 2);
    const timber = new MeshStandardMaterial({ color: srgb(0.09, 0.06, 0.04), roughness: 0.8 });
    const beams = Carlines.positions.map((x) => {
        const beam = new Mesh(new BoxGeometry(Carlines.width, Carlines.depth, Room.near - Room.far), timber);
        beam.position.set(x, Room.roof - Carlines.depth / 2, (Room.near + Room.far) / 2);
        return beam;
    });
    return [floor, levelPlane(timber, Room.roof, Math.PI / 2), ...beams];
}

export function buildEngineRoom() {
    const room = new Group();
    const steel = steelMaterial();
    room.add(sideWithDoor(steel), bulkhead(steel, Room.fore, Math.PI / 2), bulkhead(steel, Room.aft, -Math.PI / 2), ...floorAndRoof());
    room.traverse((part) => Object.assign(part, { receiveShadow: true }));
    return room;
}
