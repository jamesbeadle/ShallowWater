import { BoxGeometry, CylinderGeometry, Group } from 'three';
import { RoundedBoxGeometry } from 'three/addons/geometries/RoundedBoxGeometry.js';
import { buildPipework } from './enginePipework.js';
import { buildFlywheel } from './flywheel.js';
import { Bulb, buildHotBulb } from './hotBulb.js';
import { buildNamePlate } from './namePlate.js';
import { castingOf, nutRing, placed, turned } from './turnedParts.js';

export const Crank = { height: 0.42, flywheelX: -0.49 };

const jacketProfile = [[0, 0.6], [0.2, 0.6], [0.2, 0.625], [0.165, 0.64], [0.16, 0.66], [0.158, 0.9], [0.17, 0.915], [0.19, 0.92], [0.19, 0.945], [0, 0.945]];
const bulbFlangeProfile = [[0, 1.012], [0.108, 1.012], [0.108, 1.034], [0, 1.034]];
const injectorProfile = [[0, 1.26], [0.02, 1.26], [0.02, 1.29], [0.012, 1.3], [0.012, 1.33], [0, 1.33]];

function bottomEnd(materials) {
    const bottom = new Group();
    const bearers = [-0.22, 0.22].map((z) => placed(castingOf(new BoxGeometry(1.5, 0.11, 0.1), materials.timber), [0, 0.055, z]));
    const bed = placed(castingOf(new RoundedBoxGeometry(0.92, 0.05, 0.58, 2, 0.012), materials.paint), [0, 0.14, 0]);
    const crankcase = placed(castingOf(new RoundedBoxGeometry(0.62, 0.44, 0.46, 3, 0.035), materials.paint), [0, 0.38, 0]);
    const door = placed(turned([[0, 0], [0.09, 0], [0.095, 0.012], [0.08, 0.02], [0, 0.022]], materials.iron), [-0.08, 0.34, 0.228], [Math.PI / 2, 0, 0]);
    const doorRing = nutRing({ count: 6, ringRadius: 0.075, height: 0.02, nutRadius: 0.011, material: materials.iron });
    const doorNuts = placed(doorRing, [-0.08, 0.34, 0.228], [Math.PI / 2, 0, 0]);
    const boss = placed(castingOf(new CylinderGeometry(0.09, 0.1, 0.1, 24), materials.paint), [-0.34, Crank.height, 0], [0, 0, Math.PI / 2]);
    const shaft = placed(castingOf(new CylinderGeometry(0.038, 0.038, 0.22, 16), materials.iron), [-0.46, Crank.height, 0], [0, 0, Math.PI / 2]);
    const plate = placed(buildNamePlate(), [0.06, 0.52, 0.232]);
    bottom.add(...bearers, bed, crankcase, door, doorNuts, boss, shaft, plate);
    return bottom;
}

function topEnd(materials, time) {
    const top = new Group();
    const head = placed(castingOf(new RoundedBoxGeometry(0.34, 0.07, 0.34, 2, 0.015), materials.paint), [0, 0.978, 0]);
    const headNuts = nutRing({ count: 4, ringRadius: 0.19, height: 1.024, nutRadius: 0.02, material: materials.iron });
    headNuts.rotation.set(0, Math.PI / 4, 0);
    const bulbNuts = nutRing({ count: 6, ringRadius: 0.092, height: 1.04, nutRadius: 0.012, material: materials.iron });
    const { bulb, heat } = buildHotBulb(time);
    top.add(turned(jacketProfile, materials.paint), head, headNuts, turned(bulbFlangeProfile, materials.iron), bulbNuts, bulb);
    top.add(turned(injectorProfile, materials.brass));
    return { top, heat };
}

export function buildBolinder(materials, time) {
    const engine = new Group();
    const { top, heat } = topEnd(materials, time);
    const flywheel = placed(buildFlywheel(materials), [Crank.flywheelX, Crank.height, 0]);
    const { pipes, exhaustJoint } = buildPipework(materials);
    engine.add(bottomEnd(materials), top, flywheel, pipes);
    return { engine, flywheel, heat, bulbCentre: Bulb.centre, bulbRadius: Bulb.radius, exhaustJoint };
}
