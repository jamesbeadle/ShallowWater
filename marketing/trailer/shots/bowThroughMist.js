import { createRim } from '../props/figure/rimLight.js';
import { lookOf } from '../stage/looks.js';
import { headingAt, pointBeside } from '../world/canal.js';
import { srgb } from '../world/colours.js';
import { Daylights } from '../world/daylights.js';
import { glide, progressOf } from './cameraMoves.js';
import { buildCrewedBoat, sailBoat } from './crewedBoat.js';
import { hopwasTake } from './hopwasTake.js';
import { Mists } from '../world/heightFog.js';
import { sunDirectionAt } from '../world/sunlight.js';

const Lens = { fieldOfView: 30 };
const risenSun = sunDirectionAt(22, 150);
const Voyage = { startX: -170, speed: 1.6, lane: 1.2, clockStart: 18 };
const path = {
    from: pointBeside(-146, 9.4, 2.3),
    to: pointBeside(-148, 9.1, 2.2),
    lookFrom: pointBeside(-166, 1.2, 1.0),
    lookTo: pointBeside(-160, 1.2, 1.0),
};

export function buildBowThroughMist(setting) {
    const daylight = { ...Daylights.dawn, sunDirection: risenSun, fillIntensity: 1.7 };
    const shadow = { focus: pointBeside(-160, 0), span: 60 };
    const take = hopwasTake(setting, { daylight, path, lens: Lens, shadow, village: { hasHalcyonMoored: false }, mist: Mists.waterside });
    const rim = createRim(srgb(1.0, 0.74, 0.48), 1.3, daylight.sunDirection);
    const crewed = buildCrewedBoat({ rim, smokeColour: srgb(0.85, 0.78, 0.7), smokeOpacity: 0.45 });
    take.scene.add(...crewed.parts);
    const beats = setting.edit.timesOf('engineBeat');
    return {
        scene: take.scene,
        camera: take.camera,
        update: (shotTime, clock) => {
            const x = Voyage.startX + Voyage.speed * (clock - Voyage.clockStart);
            const wake = sailBoat(crewed, { position: pointBeside(x, Voyage.lane), heading: headingAt(x), time: clock, beats });
            take.flow(clock, wake);
            glide(take.camera, path, progressOf(setting, shotTime));
        },
        look: () => lookOf('dawn', { exposure: 0.8, bloom: { strength: 0.45, radius: 0.7, threshold: 1.5 }, finish: { glowColour: [0.05, 0.025, 0.006] } }),
        dispose: take.dispose,
    };
}
