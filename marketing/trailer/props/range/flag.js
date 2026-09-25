import { CylinderGeometry, DoubleSide, Group, Mesh, MeshStandardMaterial, PlaneGeometry } from 'three';
import { srgb } from '../../world/colours.js';
import { patchMaterial } from '../../world/materialPatches.js';

const Flag = { width: 1.9, height: 1.25, segments: 16, poleHeight: 9, poleRadius: 0.07 };

const flutter = /* glsl */ `
    float along = clamp(position.x / ${Flag.width.toFixed(2)}, 0.0, 1.0);
    float wave = sin(position.x * 3.2 - time * 7.5 + position.y * 0.8) + 0.5 * sin(position.x * 6.1 - time * 11.0);
    transformed.z += wave * 0.16 * along;
    transformed.y -= along * along * 0.12;
`;

function createFlagMaterial(time) {
    const material = new MeshStandardMaterial({ color: srgb(0.72, 0.05, 0.035), roughness: 0.8, side: DoubleSide });
    return patchMaterial(material, { time, vertex: flutter });
}

export function buildRangeFlag(time) {
    const flagpole = new Group();
    const whitewash = new MeshStandardMaterial({ color: srgb(0.8, 0.78, 0.72) });
    const pole = new Mesh(new CylinderGeometry(Flag.poleRadius * 0.7, Flag.poleRadius, Flag.poleHeight, 8), whitewash);
    pole.position.setY(Flag.poleHeight / 2);
    const cloth = new PlaneGeometry(Flag.width, Flag.height, Flag.segments, Flag.segments / 2);
    cloth.translate(Flag.width / 2, 0, 0);
    const flag = new Mesh(cloth, createFlagMaterial(time));
    flag.position.set(Flag.poleRadius, Flag.poleHeight - Flag.height / 2 - 0.1, 0);
    [pole, flag].forEach((part) => Object.assign(part, { castShadow: true }));
    flagpole.add(pole, flag);
    return flagpole;
}
