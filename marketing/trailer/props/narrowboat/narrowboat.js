import { Group } from 'three';
import { buildCabins } from './cabin.js';
import { bowFender, chimney, chimneyTop, exhaust, exhaustMouth, steeringSpot, tiller } from './fittings.js';
import { buildHold } from './hold.js';
import { buildHull } from './hull.js';
import { Liveries } from './livery.js';

export function buildNarrowboat({ livery = Liveries.halcyon, isMotor = true } = {}) {
    const boat = new Group();
    const cabins = buildCabins(livery);
    boat.add(buildHull(), ...cabins.parts, ...buildHold(cabins.holdStart, cabins.holdEnd), chimney(cabins.cabinStart), tiller(), bowFender());
    if (isMotor) {
        boat.add(exhaust(cabins.holdStart));
    }
    return { boat, exhaustMouth: exhaustMouth(cabins.holdStart), steeringSpot: steeringSpot(), chimneyTop: chimneyTop(cabins.cabinStart) };
}

export function placeBoat(narrowboat, position, heading) {
    const { boat } = narrowboat;
    boat.position.copy(position);
    boat.rotation.set(0, heading, 0);
    return narrowboat;
}
