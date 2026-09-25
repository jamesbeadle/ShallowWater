import { PerspectiveCamera } from 'three';
import { buildHopwasSet } from '../sets/hopwasSet.js';
import { flowWater } from '../world/canalWater.js';
import { driftSky } from '../world/dawnSky.js';
import { disposeScene } from '../world/disposal.js';
import { seeDistantThings } from '../world/layers.js';
import { viewpointsOf } from './cameraMoves.js';

export function hopwasTake(setting, { daylight, path, lens, shadow, village = {}, nearRadius = 200, mist }) {
    const camera = seeDistantThings(new PerspectiveCamera(lens.fieldOfView, setting.aspect, lens.near ?? 0.2, lens.far ?? 6000));
    const view = { lens, aspect: setting.aspect, viewpoints: viewpointsOf(path), nearRadius };
    const hopwas = buildHopwasSet({ daylight, view, shadow, village, ...(mist && { mist }) });
    const { scene, water, sky, time } = hopwas;
    return {
        hopwas,
        scene,
        camera,
        flow: (clock, wake = {}) => {
            flowWater(water, clock, wake);
            driftSky(sky, clock);
            time.value = clock;
        },
        dispose: () => disposeScene(scene),
    };
}
