import { Vector3 } from 'three';
import { createRain, fallRain } from '../effects/rain.js';
import { createClothing, Wardrobe } from '../props/figure/clothing.js';
import { buildFigure } from '../props/figure/figureRig.js';
import { layProne } from '../props/figure/prone.js';
import { createRim } from '../props/figure/rimLight.js';
import { Sniper } from '../sets/millRoof.js';
import { FazeleyTimes, underMoon } from '../sets/fazeleyTimes.js';
import { lookOf } from '../stage/looks.js';
import { srgb } from '../world/colours.js';
import { glide, progressOf } from './cameraMoves.js';
import { fazeleyTake } from './fazeleyTake.js';

const Lens = { fieldOfView: 30, near: 0.05 };
const rifleAt = Sniper.position.clone().add(new Vector3(1.28, 0.37, -0.12));
const path = {
    from: Sniper.position.clone().add(new Vector3(-3.6, 2.6, 1.0)),
    to: Sniper.position.clone().add(new Vector3(-2.9, 2.15, 0.75)),
    lookFrom: Sniper.target.clone().setY(-6),
    lookTo: Sniper.target.clone().setY(-4),
};

export function buildNightRoof(setting) {
    const time = { ...underMoon(FazeleyTimes.night, 34, 300), fogDensity: 0.006 };
    const take = fazeleyTake(setting, { time, shadow: { focus: Sniper.position, span: 30 }, lens: Lens, wharfLight: 42 });
    const askew = buildFigure({ clothing: createClothing({}, createRim(srgb(0.6, 0.7, 1.0), 0.7, time.sunDirection)), wardrobe: Wardrobe.workman });
    const prone = layProne(askew, Sniper.position, Sniper.heading);
    const shower = createRain({ count: 3000, centre: rifleAt, size: new Vector3(10, 7, 10), colour: srgb(0.66, 0.7, 0.8), opacity: 0.2 });
    take.scene.add(prone.holder, shower.rain);
    return {
        scene: take.scene,
        camera: take.camera,
        update: (shotTime, clock) => {
            fallRain(shower, clock);
            take.flow(clock);
            glide(take.camera, path, progressOf(setting, shotTime));
        },
        look: () => lookOf('night', { exposure: 1.35, bloom: { strength: 0.9, radius: 0.6, threshold: 0.7 } }),
        dispose: take.dispose,
    };
}
