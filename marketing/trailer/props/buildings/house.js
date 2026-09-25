import { Group, Mesh, PlaneGeometry } from 'three';
import { createRandom } from '../../world/random.js';
import { chimneyStack, roofOf, wallsWithGables, windowPanes } from './houseParts.js';

const WindowSize = { width: 0.95, height: 1.4, sill: 0.95, standOff: 0.03 };
const Door = { width: 1.0, height: 2.1 };

function windowPlacements({ width, depth, storeys, storeyHeight, bays }) {
    const placements = [];
    for (let storey = 0; storey < storeys; storey += 1) {
        for (let bay = 0; bay < bays; bay += 1) {
            const x = -width / 2 + ((bay + 0.5) * width) / bays;
            const y = storey * storeyHeight + WindowSize.sill + WindowSize.height / 2;
            const isDoorway = storey === 0 && bay === Math.floor(bays / 2);
            const front = { x, y, z: depth / 2 + WindowSize.standOff, turn: 0 };
            const back = { x, y, z: -depth / 2 - WindowSize.standOff, turn: Math.PI };
            placements.push(...(isDoorway ? [back] : [front, back]));
        }
    }
    return placements;
}

function frontDoor({ width, depth, bays }, material) {
    const middleBay = Math.floor(bays / 2);
    const door = new Mesh(new PlaneGeometry(Door.width, Door.height), material);
    door.position.set(-width / 2 + ((middleBay + 0.5) * width) / bays, Door.height / 2, depth / 2 + WindowSize.standOff);
    return door;
}

function splitByLight(placements, litShare, seed) {
    const random = createRandom(seed);
    const lit = placements.filter(() => random() < litShare);
    return { lit, unlit: placements.filter((placement) => !lit.includes(placement)) };
}

export function buildHouse(plan, materials) {
    const shape = { ...plan, wallHeight: plan.storeys * plan.storeyHeight, roofRise: plan.depth * plan.pitch };
    const house = new Group();
    house.add(wallsWithGables(shape, materials.brick), roofOf(shape, materials.slate), frontDoor(shape, materials.door));
    const chimneys = plan.chimneys ?? [-1, 1];
    chimneys.forEach((side) => house.add(chimneyStack(shape, (side * plan.width) / 2 - side * 0.5, materials)));
    const { lit, unlit } = splitByLight(windowPlacements(shape), plan.litShare ?? 0, plan.seed ?? 1);
    const waking = plan.wakingMaterials;
    const litMaterial = waking ? waking[(plan.seed ?? 0) % waking.length] : materials.litWindow;
    const panes = [[unlit, materials.window], [lit, litMaterial]];
    panes.filter(([placements]) => placements.length).forEach(([placements, material]) => {
        house.add(new Mesh(windowPanes(placements, WindowSize), material));
    });
    return house;
}
