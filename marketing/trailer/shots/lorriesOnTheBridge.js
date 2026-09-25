import { Vector3 } from 'three';
import { buildLorry } from '../props/lorry.js';
import { A5Bridge } from '../sets/fazeleyPlaces.js';
import { RoadBridgePlan } from '../sets/fazeleyBridges.js';
import { FazeleyTimes, underMoon } from '../sets/fazeleyTimes.js';
import { lookOf } from '../stage/looks.js';
import { srgb } from '../world/colours.js';
import { glide, progressOf } from './cameraMoves.js';
import { fazeleyTake } from './fazeleyTake.js';

const Lens = { fieldOfView: 22 };
const Convoy = [{ lane: -2.3, start: -15, speed: 10, paint: srgb(0.1, 0.18, 0.12) }, { lane: 2.3, start: 13, speed: -9, paint: srgb(0.22, 0.2, 0.18) }];
const path = {
    from: new Vector3(A5Bridge.x + 44, 2.4, 9.5),
    to: new Vector3(A5Bridge.x + 40, 2.3, 9.0),
    lookFrom: new Vector3(A5Bridge.x, RoadBridgePlan.crownHeight + 0.8, 0),
    lookTo: new Vector3(A5Bridge.x, RoadBridgePlan.crownHeight + 0.6, 1.5),
};

function deckHeight(z) {
    const distance = Math.min(Math.abs(z) / RoadBridgePlan.halfLength, 1);
    return RoadBridgePlan.approachHeight + (RoadBridgePlan.crownHeight - RoadBridgePlan.approachHeight) * (1 - distance * distance);
}

export function buildLorriesOnTheBridge(setting) {
    const time = underMoon(FazeleyTimes.night, 28, 250);
    const take = fazeleyTake(setting, { time, shadow: { focus: new Vector3(A5Bridge.x, 0, 0), span: 60 }, lens: Lens });
    const lorries = Convoy.map((truck) => {
        const lorry = buildLorry({ paintColour: truck.paint, hasLights: true, beamStrength: 0.07 });
        take.scene.add(lorry);
        return { lorry, ...truck };
    });
    return {
        scene: take.scene,
        camera: take.camera,
        update: (shotTime, clock) => {
            lorries.forEach(({ lorry, lane, start, speed }) => {
                const z = start + speed * shotTime;
                const facing = speed > 0 ? -Math.PI / 2 : Math.PI / 2;
                lorry.position.set(A5Bridge.x + lane, deckHeight(z) - 0.05, z);
                lorry.rotation.set(0, facing, 0);
            });
            take.flow(clock);
            glide(take.camera, path, progressOf(setting, shotTime));
        },
        look: () => lookOf('night', { exposure: 1.5, bloom: { strength: 0.9, radius: 0.6, threshold: 0.7 } }),
        dispose: take.dispose,
    };
}
