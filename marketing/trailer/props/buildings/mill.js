import { BoxGeometry, Group, Mesh } from 'three';
import { applyWorldUvs } from '../../world/worldUvs.js';
import { createRandom } from '../../world/random.js';
import { windowPanes } from './houseParts.js';

const Parapet = { height: 0.5, thickness: 0.45 };
const MillWindow = { width: 1.5, height: 2.2, sill: 0.8, standOff: 0.03 };

function brickBox(width, height, depth, material) {
    const mesh = new Mesh(applyWorldUvs(new BoxGeometry(width, height, depth), 2.2), material);
    mesh.castShadow = true;
    mesh.receiveShadow = true;
    return mesh;
}

function parapets(length, depth, top, material) {
    const sides = [
        [length, Parapet.thickness, 0, depth / 2], [length, Parapet.thickness, 0, -depth / 2],
        [Parapet.thickness, depth, length / 2, 0], [Parapet.thickness, depth, -length / 2, 0],
    ];
    return sides.map(([width, deep, x, z]) => {
        const wall = brickBox(width, Parapet.height, deep, material);
        wall.position.set(x, top + Parapet.height / 2, z);
        return wall;
    });
}

function millWindows(plan) {
    const placements = [];
    for (let storey = 0; storey < plan.storeys; storey += 1) {
        for (let bay = 0; bay < plan.bays; bay += 1) {
            const x = -plan.length / 2 + ((bay + 0.5) * plan.length) / plan.bays;
            const y = storey * plan.storeyHeight + MillWindow.sill + MillWindow.height / 2;
            placements.push({ x, y, z: plan.depth / 2 + MillWindow.standOff, turn: 0, storey, bay });
            placements.push({ x, y, z: -plan.depth / 2 - MillWindow.standOff, turn: Math.PI, storey, bay });
        }
    }
    return placements;
}

export function buildMill(plan, materials) {
    const mill = new Group();
    const height = plan.storeys * plan.storeyHeight;
    const body = brickBox(plan.length, height, plan.depth, materials.brick);
    body.position.setY(height / 2);
    const roof = new Mesh(new BoxGeometry(plan.length - 0.2, 0.3, plan.depth - 0.2), materials.darkGlass);
    roof.position.setY(height + 0.15);
    mill.add(body, roof, ...parapets(plan.length, plan.depth, height, materials.brick));
    const random = createRandom(plan.seed ?? 7);
    const placements = millWindows(plan);
    const isLit = placements.map((placement) => plan.isWindowLit?.(placement) ?? random() < (plan.litShare ?? 0));
    const lit = placements.filter((placement, index) => isLit[index]);
    const unlit = placements.filter((placement, index) => !isLit[index]);
    const litMaterial = plan.wakingMaterials ? plan.wakingMaterials[0] : materials.litWindow;
    [[unlit, materials.window], [lit, litMaterial]].filter(([group]) => group.length).forEach(([group, material]) => {
        mill.add(new Mesh(windowPanes(group, MillWindow), material));
    });
    return { mill, roofHeight: height };
}
