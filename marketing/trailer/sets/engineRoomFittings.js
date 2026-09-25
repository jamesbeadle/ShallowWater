import { BoxGeometry, CylinderGeometry, Group, Mesh, MeshStandardMaterial, TorusGeometry } from 'three';
import { placed, turned } from '../props/engine/turnedParts.js';
import { createPlankTexture } from '../textures/plankTexture.js';
import { srgb } from '../world/colours.js';
import { Room, SideDoor } from './engineRoom.js';

const Door = { thickness: 0.035, openAngle: 0.3 };
const Shelf = { x: 0.85, height: 1.02, length: 0.62, depth: 0.16 };
const oilCanProfile = [[0, 0], [0.055, 0], [0.058, 0.01], [0.058, 0.07], [0.03, 0.1], [0.012, 0.12], [0, 0.12]];
const jarProfile = [[0, 0], [0.04, 0], [0.042, 0.1], [0.03, 0.115], [0.03, 0.125], [0, 0.125]];

function doorAjar() {
    const width = SideDoor.to - SideDoor.from;
    const height = SideDoor.head - SideDoor.sill;
    const boards = createPlankTexture({ tone: [30, 52, 36], seed: 8 });
    boards.repeat.set(width / 0.9, height / 0.9);
    const leaf = new Mesh(new BoxGeometry(width, height, Door.thickness), new MeshStandardMaterial({ map: boards, roughness: 0.6 }));
    leaf.position.set(-width / 2, height / 2, Door.thickness / 2);
    const hinge = placed(new Group(), [SideDoor.to, SideDoor.sill, Room.far], [0, Door.openAngle, 0]);
    hinge.add(leaf);
    return hinge;
}

function shelfWithTins() {
    const shelf = new Group();
    const timber = new MeshStandardMaterial({ color: srgb(0.16, 0.1, 0.06), roughness: 0.7 });
    const tinPlate = new MeshStandardMaterial({ color: srgb(0.35, 0.33, 0.3), roughness: 0.35, metalness: 0.9 });
    const paintedTin = new MeshStandardMaterial({ color: srgb(0.45, 0.08, 0.05), roughness: 0.45, metalness: 0.5 });
    const board = new Mesh(new BoxGeometry(Shelf.length, 0.025, Shelf.depth), timber);
    const oilCan = placed(turned(oilCanProfile, tinPlate, 20), [-0.2, 0.0125, 0]);
    const spout = placed(new Mesh(new CylinderGeometry(0.004, 0.007, 0.16, 8), tinPlate), [-0.15, 0.16, 0.02], [0, 0, -0.9]);
    const paraffin = placed(new Mesh(new BoxGeometry(0.14, 0.2, 0.1), paintedTin), [0.02, 0.1125, 0]);
    const jar = placed(turned(jarProfile, new MeshStandardMaterial({ color: srgb(0.3, 0.26, 0.12), roughness: 0.1, metalness: 0.1 }), 16), [0.2, 0.0125, 0.01]);
    shelf.add(board, oilCan, spout, paraffin, jar);
    return placed(shelf, [Shelf.x, Shelf.height, Room.far + Shelf.depth / 2]);
}

function coiledRope() {
    const rope = new MeshStandardMaterial({ color: srgb(0.42, 0.36, 0.26), roughness: 1 });
    const coil = new Group();
    [0, 1, 2, 3].forEach((turn) => {
        const loop = new Mesh(new TorusGeometry(0.16 - turn * 0.012, 0.018, 8, 28), rope);
        coil.add(placed(loop, [0, turn * 0.03, 0], [Math.PI / 2, 0, 0]));
    });
    return placed(coil, [1.0, 0.02, -0.55]);
}

export function fitOutEngineRoom() {
    const fittings = [doorAjar(), shelfWithTins(), coiledRope()];
    fittings.forEach((fitting) => fitting.traverse((part) => Object.assign(part, { castShadow: true, receiveShadow: true })));
    return fittings;
}
