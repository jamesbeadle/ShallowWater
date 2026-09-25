import { Vector3 } from 'three';
import { chainPoints, hangLinks } from '../props/chain.js';
import { Chain } from '../sets/fazeleyPlaces.js';
import { FazeleyTimes } from '../sets/fazeleyTimes.js';
import { lookOf } from '../stage/looks.js';
import { glide, progressOf } from './cameraMoves.js';
import { fazeleyTake } from './fazeleyTake.js';

const Lens = { fieldOfView: 42 };
const Fall = { snapAt: 0.1, swing: 1.3, sinkBelow: -0.4 };
const path = {
    from: new Vector3(Chain.x - 2.4, 1.3, 9.8),
    to: new Vector3(Chain.x - 2.1, 1.2, 9.2),
    lookFrom: new Vector3(Chain.x, 1.0, 1.5),
    lookTo: new Vector3(Chain.x, 0.4, 1.0),
};

function swungHalf(anchor, points, since) {
    const angle = Math.min(since * since * Fall.swing, Math.PI / 2);
    return points.map((point) => {
        const reach = point.distanceTo(anchor);
        const outward = Math.sign(point.z - anchor.z);
        const hanging = Math.max(anchor.y - Math.sin(angle) * reach, Fall.sinkBelow);
        return new Vector3(point.x, hanging, anchor.z + outward * Math.cos(angle) * reach);
    });
}

function fallingChain(chain, shotTime) {
    const points = chainPoints(chain.start, chain.end, 0.4, chain.count);
    const since = shotTime - Fall.snapAt;
    if (since <= 0) {
        return points;
    }
    const middle = Math.floor(points.length / 2);
    const nearHalf = swungHalf(chain.start, points.slice(0, middle), since);
    const farHalf = swungHalf(chain.end, points.slice(middle), since);
    return [...nearHalf, ...farHalf];
}

export function buildChainFalls(setting) {
    const take = fazeleyTake(setting, { time: FazeleyTimes.dawn, shadow: { focus: new Vector3(Chain.x, 0, 0), span: 40 }, lens: Lens });
    const { chain } = take.fazeley;
    return {
        scene: take.scene,
        camera: take.camera,
        update: (shotTime, clock) => {
            hangLinks(chain, fallingChain(chain, shotTime));
            take.flow(clock);
            glide(take.camera, path, progressOf(setting, shotTime));
        },
        look: () => lookOf('dawn', { exposure: 0.75, bloom: { strength: 0.5, radius: 0.6, threshold: 1.2 } }),
        dispose: take.dispose,
    };
}
