import { Group } from 'three';
import { buildChimneyStack } from '../props/buildings/chimneyStack.js';
import { buildChurch } from '../props/buildings/churchTower.js';
import { buildHouse } from '../props/buildings/house.js';
import { buildMill } from '../props/buildings/mill.js';
import { Church, Mill, MillChimney, Terraces, TollHouse, Wharf } from './fazeleyPlaces.js';

const bankHeight = 0.62;
const Terrace = { bayWidth: 4.8, depth: 7, storeys: 2, storeyHeight: 2.7, pitch: 0.45 };

function placed(object, x, z, turn = 0) {
    object.position.set(x, bankHeight, z);
    object.rotation.set(0, turn, 0);
    return object;
}

function terraces(materials, litShare, windowPlan) {
    return Terraces.map(({ x, z, bays, turn }, index) => {
        const plan = { ...Terrace, width: bays * Terrace.bayWidth, bays, litShare, seed: 100 + index, ...windowPlan };
        return placed(buildHouse(plan, materials), x, z, turn);
    });
}

function warehouse(materials, litShare) {
    const plan = {
        width: Wharf.xTo - Wharf.xFrom, depth: Wharf.zTo - Wharf.zFrom,
        storeys: 3, storeyHeight: 3.4, pitch: 0.32, bays: 10, litShare, chimneys: [], seed: 61,
    };
    return placed(buildHouse(plan, materials), (Wharf.xFrom + Wharf.xTo) / 2, (Wharf.zFrom + Wharf.zTo) / 2, Math.PI);
}

export function buildFazeleyBuildings(materials, { litShare = 0, millPlan = {}, windowPlan = {} } = {}) {
    const town = new Group();
    const millLength = Mill.xTo - Mill.xFrom;
    const plan = { length: millLength, depth: Mill.zTo - Mill.zFrom, storeys: Mill.storeys, storeyHeight: Mill.storeyHeight, bays: 22, litShare };
    const mill = buildMill({ ...plan, ...millPlan }, materials);
    town.add(placed(mill.mill, (Mill.xFrom + Mill.xTo) / 2, (Mill.zFrom + Mill.zTo) / 2));
    const stack = buildChimneyStack(MillChimney.height, materials);
    town.add(placed(stack.chimney, MillChimney.x, MillChimney.z));
    const church = buildChurch(materials);
    town.add(placed(church.church, Church.x, Church.z, Math.PI / 2));
    const toll = buildHouse({ width: 7, depth: 5.5, storeys: 1, storeyHeight: 3, pitch: 0.55, bays: 2, litShare, seed: 71 }, materials);
    town.add(placed(toll, TollHouse.x, TollHouse.z, Math.PI), warehouse(materials, litShare), ...terraces(materials, litShare, windowPlan));
    return { town, millRoof: mill.roofHeight + bankHeight, chimneyTop: stack.top + bankHeight };
}
