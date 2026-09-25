import { Group } from 'three';
import { buildGasLamp } from '../props/gasLamp.js';
import { Wharf } from './fazeleyPlaces.js';

const bankHeight = 0.62;
const TowpathLamps = [-230, -110, -40, 20, 90];
const StreetLamps = [[-110, 57], [-40, 57], [20, 57], [60, 57], [-60, 118], [30, 122]];

function lampAt(x, z, options) {
    const lamp = buildGasLamp(options);
    lamp.position.set(x, bankHeight, z);
    return lamp;
}

export function buildFazeleyLamps({ isLit, wharfLight = 0 }) {
    const lamps = new Group();
    TowpathLamps.forEach((x) => lamps.add(lampAt(x, 10.5, { isLit })));
    StreetLamps.forEach(([x, z]) => lamps.add(lampAt(x, z, { isLit })));
    lamps.add(lampAt(Wharf.lampX, Wharf.lampZ, { isLit, lightIntensity: wharfLight, lightDistance: 22 }));
    return lamps;
}
