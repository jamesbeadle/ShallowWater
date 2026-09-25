import { Group } from 'three';
import { buildRifle } from '../rifle.js';
import { pronePose } from './poses.js';

const Lying = { lift: 0.13, rifleAlong: 1.28, rifleSide: 0.12, rifleHeight: 0.24 };

export function layProne(rig, position, heading, { hasScope = true } = {}) {
    pronePose(rig);
    const holder = new Group();
    holder.add(rig.root);
    const rifle = buildRifle({ hasScope });
    rifle.rotation.set(0, -Math.PI / 2, 0.06);
    rifle.position.set(Lying.rifleSide, Lying.rifleHeight, Lying.rifleAlong);
    holder.add(rifle);
    holder.position.copy(position).setY(position.y + Lying.lift);
    holder.rotation.set(0, heading, 0);
    return { holder, rifle };
}
