import { Group, Vector3 } from 'three';
import { RoundedBoxGeometry } from 'three/addons/geometries/RoundedBoxGeometry.js';
import { castingOf, pipe, placed, turned } from './turnedParts.js';

const Runs = {
    exhaust: [[-0.1, 0.86, -0.1], [-0.22, 0.87, -0.19], [-0.29, 0.99, -0.25], [-0.3, 1.3, -0.28], [-0.3, 1.75, -0.28]],
    waterOut: [[0.15, 0.74, 0.04], [0.27, 0.75, 0.08], [0.34, 0.62, 0.12], [0.36, 0.25, 0.15], [0.42, 0.04, 0.18]],
    waterIn: [[-0.12, 0.66, 0.13], [-0.2, 0.6, 0.24], [-0.22, 0.4, 0.3], [-0.3, 0.1, 0.34]],
    fuel: [[0.2, 0.76, -0.17], [0.22, 0.95, -0.2], [0.16, 1.14, -0.18], [0.06, 1.28, -0.07], [0.012, 1.3, -0.01]],
    oiler: [[-0.19, 0.82, 0.1], [-0.17, 0.76, 0.1], [-0.15, 0.72, 0.08]],
};
const Bore = { exhaust: 0.042, water: 0.016, fuel: 0.005, oiler: 0.004 };
const lubricatorProfile = [[0, 0], [0.03, 0], [0.03, 0.018], [0.02, 0.024], [0.02, 0.07], [0.03, 0.076], [0.03, 0.09], [0.012, 0.1], [0, 0.1]];
const sightGlassProfile = [[0.023, 0.026], [0.023, 0.068]];
const flangeProfile = [[0, 0], [0.07, 0], [0.07, 0.018], [0, 0.018]];

function fuelPump(materials) {
    const pump = new Group();
    pump.add(castingOf(new RoundedBoxGeometry(0.075, 0.12, 0.075, 2, 0.012), materials.iron));
    pump.add(placed(turned([[0, 0], [0.022, 0], [0.022, 0.03], [0.01, 0.04], [0, 0.04]], materials.brass), [0, 0.06, 0]));
    return placed(pump, [0.2, 0.66, -0.17]);
}

function lubricator(materials) {
    const oiler = new Group();
    oiler.add(turned(lubricatorProfile, materials.brass), turned(sightGlassProfile, materials.glass));
    return placed(oiler, [-0.2, 0.82, 0.1]);
}

export function buildPipework(materials) {
    const pipes = new Group();
    const exhaustFlange = placed(turned(flangeProfile, materials.iron), [-0.29, 1.08, -0.26]);
    pipes.add(pipe(Runs.exhaust, Bore.exhaust, materials.iron), exhaustFlange);
    pipes.add(pipe(Runs.waterOut, Bore.water, materials.copper), pipe(Runs.waterIn, Bore.water, materials.copper));
    pipes.add(pipe(Runs.fuel, Bore.fuel, materials.copper), pipe(Runs.oiler, Bore.oiler, materials.copper));
    pipes.add(fuelPump(materials), lubricator(materials));
    return { pipes, exhaustJoint: new Vector3(-0.29, 1.1, -0.24) };
}
