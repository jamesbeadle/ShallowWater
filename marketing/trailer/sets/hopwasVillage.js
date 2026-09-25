import { Group, Vector3 } from 'three';
import { buildHumpbackBridge } from '../props/bridges/humpbackBridge.js';
import { buildHouse } from '../props/buildings/house.js';
import { buildPubSign } from '../props/buildings/pubSign.js';
import { buildNarrowboat, placeBoat } from '../props/narrowboat/narrowboat.js';
import { Liveries } from '../props/narrowboat/livery.js';
import { centreZ, headingAt, pointBeside } from '../world/canal.js';
import { HopwasBridge, Moorings, Pub } from './hopwasPlaces.js';

const BridgePlan = {
    halfLength: 17, width: 5.4, footing: -1.8, approachHeight: 0.7, crownHeight: 5.3,
    archCentre: 1.2, archHalfSpan: 5.2, springing: 1.3, archRise: 2.5,
};
const cottage = { width: 8, depth: 5.5, storeys: 2, storeyHeight: 2.6, pitch: 0.48, bays: 2 };
const Cottages = [
    { x: -12, across: 34, turn: Math.PI / 2 }, { x: -12, across: 46, turn: Math.PI / 2 }, { x: 12, across: 40, turn: -Math.PI / 2 },
    { x: 12, across: 60, turn: -Math.PI / 2 }, { x: -12, across: 76, turn: Math.PI / 2 }, { x: 62, across: 24, turn: Math.PI },
    { x: 84, across: 26, turn: Math.PI }, { x: -34, across: 23, turn: Math.PI },
];

function placeOnGround(object, groundAt, x, across, turn) {
    object.position.set(x, groundAt(x, across) - 0.15, centreZ(x) + across);
    object.rotation.set(0, turn + headingAt(x), 0);
    return object;
}

function buildHouses(groundAt, materials, litShare) {
    const houses = Cottages.map(({ x, across, turn }, index) => {
        const house = buildHouse({ ...cottage, litShare, seed: index + 3 }, materials);
        return placeOnGround(house, groundAt, x, across, turn);
    });
    const pub = buildHouse({ width: 13, depth: 7, storeys: 2, storeyHeight: 2.9, pitch: 0.5, bays: 4, litShare: litShare * 1.5, seed: 41 }, materials);
    houses.push(placeOnGround(pub, groundAt, Pub.x, Pub.across, Math.PI));
    houses.push(placeOnGround(buildPubSign('TAME OTTER'), groundAt, Pub.x - 8, Pub.across - 11, Math.PI / 2));
    return houses;
}

function mooredBoats() {
    const halcyonBerth = pointBeside(Moorings.halcyon, Moorings.offset);
    const patienceBerth = pointBeside(Moorings.patience, Moorings.offset);
    const halcyon = placeBoat(buildNarrowboat({ livery: Liveries.halcyon }), halcyonBerth, headingAt(Moorings.halcyon));
    const patience = placeBoat(buildNarrowboat({ livery: Liveries.patience, isMotor: false }), patienceBerth, headingAt(Moorings.patience) + Math.PI);
    return { halcyon, patience };
}

export function buildHopwasVillage({ groundAt, materials, litShare = 0, hasHalcyonMoored = true }) {
    const village = new Group();
    const bridge = buildHumpbackBridge(BridgePlan, materials);
    bridge.position.copy(pointBeside(HopwasBridge.x, 0));
    bridge.rotation.set(0, headingAt(HopwasBridge.x) - Math.PI / 2, 0);
    village.add(bridge, ...buildHouses(groundAt, materials, litShare));
    const { halcyon, patience } = mooredBoats();
    village.add(patience.boat);
    if (hasHalcyonMoored) {
        village.add(halcyon.boat);
    }
    return { village, bridgeTop: new Vector3(HopwasBridge.x, BridgePlan.crownHeight, centreZ(HopwasBridge.x)) };
}
