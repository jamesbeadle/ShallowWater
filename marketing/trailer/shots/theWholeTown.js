import { Vector3 } from 'three';
import { createWakingWindows, wakeWindows } from '../props/buildings/wakingWindows.js';
import { FazeleyTimes, underMoon } from '../sets/fazeleyTimes.js';
import { lookOf } from '../stage/looks.js';
import { glide, progressOf } from './cameraMoves.js';
import { fazeleyTake } from './fazeleyTake.js';

const Lens = { fieldOfView: 34 };
const Report = { flashSeconds: 0.1 };
const path = {
    from: new Vector3(-262, 44, 96),
    to: new Vector3(-256, 43, 92),
    lookFrom: new Vector3(-40, 0, 20),
    lookTo: new Vector3(-36, 0, 18),
};

export function buildTheWholeTown(setting) {
    const time = { ...underMoon(FazeleyTimes.night, 22, 80), litShare: 0 };
    const wakingMaterials = createWakingWindows();
    const buildings = { windowPlan: { litShare: 0.62, wakingMaterials }, millPlan: { litShare: 0.35, wakingMaterials } };
    const take = fazeleyTake(setting, { time, shadow: { focus: new Vector3(-80, 0, 40), span: 220 }, lens: Lens, buildings, wharfLight: 42 });
    return {
        scene: take.scene,
        camera: take.camera,
        update: (shotTime, clock) => {
            wakeWindows(wakingMaterials, clock);
            take.flow(clock);
            glide(take.camera, path, progressOf(setting, shotTime));
        },
        look: (shotTime) => lookOf('night', {
            exposure: 1.6,
            bloom: { strength: 1.0, radius: 0.6, threshold: 0.6 },
            finish: { flash: Math.max(1 - shotTime / Report.flashSeconds, 0) },
        }),
        dispose: take.dispose,
    };
}
