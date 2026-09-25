import { CylinderGeometry, Group, Mesh } from 'three';
import { applyWorldUvs } from '../../world/worldUvs.js';

const Stack = { baseRadius: 2.3, topRadius: 1.35, capRadius: 1.6, capHeight: 1.2 };

export function buildChimneyStack(height, materials) {
    const chimney = new Group();
    const shaft = new Mesh(applyWorldUvs(new CylinderGeometry(Stack.topRadius, Stack.baseRadius, height, 24, 1, true), 2.2), materials.brick);
    shaft.position.setY(height / 2);
    const cap = new Mesh(new CylinderGeometry(Stack.capRadius, Stack.capRadius, Stack.capHeight, 24), materials.stone);
    cap.position.setY(height - Stack.capHeight / 2);
    [shaft, cap].forEach((part) => Object.assign(part, { castShadow: true, receiveShadow: true }));
    chimney.add(shaft, cap);
    return { chimney, top: height };
}
