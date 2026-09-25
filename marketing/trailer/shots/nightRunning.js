import { PerspectiveCamera, Vector3 } from 'three';
import { createRim } from '../props/figure/rimLight.js';
import { A5Bridge } from '../sets/fazeleyPlaces.js';
import { buildFazeleySet } from '../sets/fazeleySet.js';
import { FazeleyTimes, underMoon } from '../sets/fazeleyTimes.js';
import { lookOf } from '../stage/looks.js';
import { flowWater } from '../world/canalWater.js';
import { srgb } from '../world/colours.js';
import { disposeScene } from '../world/disposal.js';
import { buildCrewedBoat, sailBoat } from './crewedBoat.js';
import { glide, progressOf } from './cameraMoves.js';

const Lens = { fieldOfView: 19, near: 0.2, far: 6000 };
const Voyage = { startX: A5Bridge.x + 12, speed: 1.5, lane: -1.0 };
const path = {
    from: new Vector3(A5Bridge.x + 52, 1.25, 5.5),
    to: new Vector3(A5Bridge.x + 49, 1.15, 5.0),
    lookFrom: new Vector3(A5Bridge.x + 10, 1.6, -1.0),
    lookTo: new Vector3(A5Bridge.x + 13, 1.6, -1.2),
};

export function buildNightRunning(setting) {
    const time = underMoon(FazeleyTimes.night, 5.5, 268);
    const fazeley = buildFazeleySet({ time, shadow: { focus: new Vector3(A5Bridge.x, 0, 0), span: 80 } });
    const crewed = buildCrewedBoat({ rim: createRim(srgb(0.55, 0.65, 1.0), 0.9, time.sunDirection), smokeColour: srgb(0.4, 0.45, 0.55), smokeOpacity: 0.35 });
    fazeley.scene.add(...crewed.parts);
    const camera = new PerspectiveCamera(Lens.fieldOfView, setting.aspect, Lens.near, Lens.far);
    const beats = setting.edit.timesOf('engineBeat');
    return {
        scene: fazeley.scene,
        camera,
        update: (shotTime, clock) => {
            const position = new Vector3(Voyage.startX + Voyage.speed * shotTime, 0, Voyage.lane);
            const wake = sailBoat(crewed, { position, heading: 0, time: clock, beats });
            flowWater(fazeley.water, clock, wake);
            glide(camera, path, progressOf(setting, shotTime));
        },
        look: () => lookOf('night', { exposure: 1.7, bloom: { strength: 0.7, radius: 0.55, threshold: 0.75 } }),
        dispose: () => disposeScene(fazeley.scene),
    };
}
