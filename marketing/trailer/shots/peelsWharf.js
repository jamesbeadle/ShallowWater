import { Vector3 } from 'three';
import { createRain, fallRain } from '../effects/rain.js';
import { buildCrateStack } from '../props/crates.js';
import { createClothing, Wardrobe } from '../props/figure/clothing.js';
import { buildFigure } from '../props/figure/figureRig.js';
import { placeFigure } from '../props/figure/placement.js';
import { carryForward, standPose } from '../props/figure/poses.js';
import { buildLorry } from '../props/lorry.js';
import { Liveries } from '../props/narrowboat/livery.js';
import { buildNarrowboat, placeBoat } from '../props/narrowboat/narrowboat.js';
import { Wharf } from '../sets/fazeleyPlaces.js';
import { FazeleyTimes, underMoon } from '../sets/fazeleyTimes.js';
import { lookOf } from '../stage/looks.js';
import { srgb } from '../world/colours.js';
import { glide, progressOf } from './cameraMoves.js';
import { fazeleyTake } from './fazeleyTake.js';

const Lens = { fieldOfView: 30 };
const Arrival = { from: -31, to: -21, speed: 5.5 };
const path = {
    from: new Vector3(Wharf.lampX - 16, 1.1, 5.5),
    to: new Vector3(Wharf.lampX - 13, 1.0, 5.0),
    lookFrom: new Vector3(Wharf.lampX + 4, 2.2, -12),
    lookTo: new Vector3(Wharf.lampX + 3, 2.0, -12),
};

function wharfHands() {
    const workers = [0, 1, 2].map((index) => {
        const worker = buildFigure({ clothing: createClothing(), wardrobe: Wardrobe.workman });
        standPose(worker, { turnHead: 0.3 });
        carryForward(worker, { shoulder: 0.9, elbow: 0.8 });
        return worker;
    });
    placeFigure(workers[0], new Vector3(Wharf.lampX + 3, 0.62, -9), Math.PI);
    placeFigure(workers[1], new Vector3(Wharf.lampX + 8, 0.62, -11.5), -Math.PI / 2);
    placeFigure(workers[2], new Vector3(Wharf.lampX - 1.5, 0.62, -10.5), Math.PI / 2);
    return workers;
}

export function buildPeelsWharf(setting) {
    const time = { ...underMoon(FazeleyTimes.night, 30, 150), fogDensity: 0.012 };
    const take = fazeleyTake(setting, { time, shadow: { focus: new Vector3(Wharf.lampX, 0, -10), span: 40 }, lens: Lens, wharfLight: 42 });
    const lorry = buildLorry({ paintColour: srgb(0.24, 0.06, 0.05), hasLights: true, beamStrength: 0.05 });
    lorry.rotation.set(0, -Math.PI / 2, 0);
    const motor = placeBoat(buildNarrowboat({ livery: Liveries.company }), new Vector3(Wharf.lampX + 10, 0, -4.3), Math.PI);
    const crates = buildCrateStack(7, 3);
    crates.position.set(Wharf.lampX + 4.5, 0.62, -10.2);
    const rainAround = { centre: new Vector3(Wharf.lampX, 0, -6), size: new Vector3(16, 14, 14) };
    const shower = createRain({ count: 3500, ...rainAround, colour: srgb(0.7, 0.72, 0.8), opacity: 0.4 });
    const { scene } = take.fazeley;
    scene.add(lorry, motor.boat, crates, shower.rain, ...wharfHands().map((worker) => worker.root));
    return {
        scene,
        camera: take.camera,
        update: (shotTime, clock) => {
            const creeping = Math.min(Arrival.from + Arrival.speed * shotTime, Arrival.to);
            lorry.position.set(Wharf.lampX + 6, 0.62, creeping);
            fallRain(shower, clock);
            take.flow(clock);
            glide(take.camera, path, progressOf(setting, shotTime));
        },
        look: () => lookOf('night', { exposure: 1.6, bloom: { strength: 0.8, radius: 0.6, threshold: 0.7 } }),
        dispose: take.dispose,
    };
}
