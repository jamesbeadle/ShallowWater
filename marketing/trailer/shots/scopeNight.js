import { Vector3 } from 'three';
import { createRain, fallRain } from '../effects/rain.js';
import { createClothing, Wardrobe } from '../props/figure/clothing.js';
import { buildFigure } from '../props/figure/figureRig.js';
import { placeFigure } from '../props/figure/placement.js';
import { standPose } from '../props/figure/poses.js';
import { buildLorry } from '../props/lorry.js';
import { Sniper } from '../sets/millRoof.js';
import { FazeleyTimes, underMoon } from '../sets/fazeleyTimes.js';
import { lookOf } from '../stage/looks.js';
import { srgb } from '../world/colours.js';
import { fazeleyTake } from './fazeleyTake.js';

const Lens = { fieldOfView: 1.55, near: 1, far: 3000 };
const Breath = { sway: 0.0028, settle: 2.6, tremor: 0.00018 };

function foremanUnderLamp() {
    const foreman = buildFigure({ clothing: createClothing(), wardrobe: Wardrobe.gentleman });
    standPose(foreman, { turnHead: -0.4 });
    return placeFigure(foreman, Sniper.target.clone().setY(0.62), -Math.PI / 2);
}

function aim(camera, shotTime) {
    const settling = Math.max(1 - shotTime / Breath.settle, 0);
    const yaw = Breath.sway * settling * Math.sin(shotTime * 2.2) + Breath.tremor * Math.sin(shotTime * 23);
    const pitch = Breath.sway * 0.7 * settling * Math.sin(shotTime * 1.6 + 0.8) + Breath.tremor * Math.sin(shotTime * 31);
    camera.position.copy(Sniper.eye);
    camera.lookAt(Sniper.target);
    camera.rotateY(yaw);
    camera.rotateX(pitch);
}

export function buildScopeNight(setting) {
    const time = { ...underMoon(FazeleyTimes.night, 26, 60), fogDensity: 0.003 };
    const take = fazeleyTake(setting, { time, shadow: { focus: Sniper.target, span: 30 }, lens: Lens, wharfLight: 42 });
    const lorry = buildLorry({ paintColour: srgb(0.24, 0.06, 0.05), hasLights: true, beamStrength: 0.05 });
    lorry.position.copy(Sniper.target).add(new Vector3(4.8, -0.95, -11));
    lorry.rotation.set(0, -Math.PI / 2, 0);
    const rainAround = { centre: Sniper.target.clone().setY(0), size: new Vector3(10, 10, 10) };
    const shower = createRain({ count: 2600, ...rainAround, colour: srgb(0.7, 0.72, 0.8), opacity: 0.45 });
    take.scene.add(foremanUnderLamp().root, lorry, shower.rain);
    return {
        scene: take.scene,
        camera: take.camera,
        update: (shotTime, clock) => {
            fallRain(shower, clock);
            take.flow(clock);
            aim(take.camera, shotTime);
        },
        look: () => lookOf('night', { exposure: 1.7, bloom: { strength: 0.8, radius: 0.5, threshold: 0.7 }, finish: { scope: 1, vignette: 0.2 } }),
        dispose: take.dispose,
    };
}
