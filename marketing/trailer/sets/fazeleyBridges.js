import { Group } from 'three';
import { buildHumpbackBridge } from '../props/bridges/humpbackBridge.js';
import { A5Bridge, RovingBridge } from './fazeleyPlaces.js';

export const RoadBridgePlan = {
    halfLength: 24, width: 11, footing: -1.8, approachHeight: 0.9, crownHeight: 5.6,
    archCentre: 1.0, archHalfSpan: 6.0, springing: 1.0, archRise: 2.9,
};
export const FootBridgePlan = {
    halfLength: 13, width: 3, footing: -1.8, approachHeight: 0.8, crownHeight: 5.0,
    archCentre: 0.5, archHalfSpan: 5.0, springing: 1.2, archRise: 2.6,
};

function spanning(plan, x, materials) {
    const bridge = buildHumpbackBridge(plan, materials);
    bridge.position.set(x, 0, 0);
    bridge.rotation.set(0, -Math.PI / 2, 0);
    return bridge;
}

export function buildFazeleyBridges(materials) {
    const bridges = new Group();
    bridges.add(spanning(RoadBridgePlan, A5Bridge.x, materials), spanning(FootBridgePlan, RovingBridge.x, materials));
    return bridges;
}
