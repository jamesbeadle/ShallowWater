import { PerspectiveCamera } from 'three';
import { buildFazeleySet } from '../sets/fazeleySet.js';
import { flowWater } from '../world/canalWater.js';
import { disposeScene } from '../world/disposal.js';

export function fazeleyTake(setting, { time, shadow, lens, buildings, wharfLight }) {
    const fazeley = buildFazeleySet({ time, shadow, buildings, wharfLight });
    const camera = new PerspectiveCamera(lens.fieldOfView, setting.aspect, lens.near ?? 0.2, lens.far ?? 6000);
    const { scene, water } = fazeley;
    return {
        fazeley,
        scene,
        camera,
        flow: (clock, wake = {}) => flowWater(water, clock, wake),
        dispose: () => disposeScene(scene),
    };
}
