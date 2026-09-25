import { Group } from 'three';
import { buildButts } from '../props/range/butts.js';
import { buildRangeFlag } from '../props/range/flag.js';
import { centreZ, headingAt } from '../world/canal.js';
import { Butts } from './hopwasPlaces.js';

const FlagSpots = [{ x: 36, across: -366 }, { x: 128, across: -372 }, { x: 214, across: -364 }, { x: 30, across: -160 }, { x: 222, across: -150 }];

export function buildHopwasRange(groundAt, time) {
    const range = new Group();
    const butts = buildButts();
    butts.position.set(Butts.x, groundAt(Butts.x, Butts.across) - 1.5, centreZ(Butts.x) + Butts.across);
    butts.rotation.set(0, headingAt(Butts.x), 0);
    range.add(butts);
    const flags = FlagSpots.map(({ x, across }) => {
        const flag = buildRangeFlag(time);
        flag.position.set(x, groundAt(x, across) - 0.2, centreZ(x) + across);
        flag.rotation.set(0, -0.6, 0);
        return flag;
    });
    range.add(...flags);
    return range;
}
