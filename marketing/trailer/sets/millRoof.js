import { Vector3 } from 'three';
import { Mill, Wharf } from './fazeleyPlaces.js';

const bankHeight = 0.62;
const roofTop = Mill.storeys * Mill.storeyHeight + 0.3 + bankHeight;

export const Sniper = {
    position: new Vector3(Mill.xTo - 2.6, roofTop, Mill.zFrom + 3.2),
    heading: Math.PI / 2,
    eye: new Vector3(Mill.xTo - 1.35, roofTop + 0.47, Mill.zFrom + 3.32),
    target: new Vector3(Wharf.lampX + 0.4, bankHeight + 1.55, Wharf.lampZ - 1.9),
};

export const RoofTop = roofTop;
