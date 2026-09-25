import { CylinderGeometry, Group } from 'three';
import { mergeGeometries } from 'three/addons/utils/BufferGeometryUtils.js';
import { castingOf, turnedGeometry } from './turnedParts.js';

const Wheel = { radius: 0.36, rimDepth: 0.075, rimWidth: 0.1, hubRadius: 0.075, hubWidth: 0.14, spokes: 6 };
const Spoke = { innerRadius: 0.03, outerRadius: 0.019, sides: 10 };

function rimProfile() {
    const outer = Wheel.radius;
    const inner = Wheel.radius - Wheel.rimDepth;
    const half = Wheel.rimWidth / 2;
    return [[inner + 0.01, -half], [outer - 0.01, -half], [outer, -half + 0.012], [outer, half - 0.012],
        [outer - 0.01, half], [inner + 0.01, half], [inner, half - 0.012], [inner, -half + 0.012], [inner + 0.01, -half]];
}

function hubProfile() {
    const half = Wheel.hubWidth / 2;
    const hub = Wheel.hubRadius;
    return [[0, -half], [hub * 0.8, -half], [hub, -half * 0.6], [hub, half * 0.6], [hub * 0.8, half], [0, half]];
}

function spokeGeometry(index) {
    const length = Wheel.radius - Wheel.rimDepth - Wheel.hubRadius + 0.03;
    const spoke = new CylinderGeometry(Spoke.outerRadius, Spoke.innerRadius, length, Spoke.sides);
    spoke.scale(1, 1, 0.7);
    spoke.translate(0, Wheel.hubRadius - 0.015 + length / 2, 0);
    spoke.rotateY(Math.PI / 2);
    spoke.rotateX((index / Wheel.spokes) * Math.PI * 2);
    return spoke;
}

export function buildFlywheel(materials) {
    const wheel = new Group();
    const rim = turnedGeometry(rimProfile(), 64);
    rim.rotateZ(Math.PI / 2);
    const hub = turnedGeometry(hubProfile(), 24);
    hub.rotateZ(Math.PI / 2);
    const spokes = Array.from({ length: Wheel.spokes }, (unused, index) => spokeGeometry(index));
    wheel.add(castingOf(rim, materials.iron), castingOf(mergeGeometries([hub, ...spokes]), materials.paint));
    return wheel;
}
