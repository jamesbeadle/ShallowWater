import { Vector3 } from 'three';
import { createSmokeColumn, driftSmoke } from '../effects/smokeColumn.js';
import { createClothing, Wardrobe } from '../props/figure/clothing.js';
import { buildFigure } from '../props/figure/figureRig.js';
import { placeFigure } from '../props/figure/placement.js';
import { raiseHand, standPose } from '../props/figure/poses.js';
import { createRim } from '../props/figure/rimLight.js';
import { MillChimney } from '../sets/fazeleyPlaces.js';
import { FazeleyTimes } from '../sets/fazeleyTimes.js';
import { RoofTop, Sniper } from '../sets/millRoof.js';
import { lookOf } from '../stage/looks.js';
import { srgb } from '../world/colours.js';
import { sunDirectionAt } from '../world/sunlight.js';
import { glide, progressOf } from './cameraMoves.js';
import { fazeleyTake } from './fazeleyTake.js';

const Lens = { fieldOfView: 30, near: 0.1 };
const hazyAfternoon = { ...FazeleyTimes.afternoon, sunDirection: sunDirectionAt(13, 236), fogColour: srgb(0.74, 0.7, 0.64), fogDensity: 0.003 };
const wind = new Vector3(1.6, 0, -0.5);
const crouchAt = Sniper.position.clone().add(new Vector3(0.6, 0, -1.2));
const path = {
    from: crouchAt.clone().add(new Vector3(-6.5, 1.7, 3.6)),
    to: crouchAt.clone().add(new Vector3(-5.6, 1.55, 3.0)),
    lookFrom: new Vector3(40, 2, -2),
    lookTo: new Vector3(55, 0, -4),
};

function crouchingAskew() {
    const askew = buildFigure({ clothing: createClothing({}, createRim(srgb(1.0, 0.85, 0.65), 0.9, hazyAfternoon.sunDirection)), wardrobe: Wardrobe.workman });
    standPose(askew, { turnHead: -0.25, lookUp: -0.1 });
    raiseHand(askew, 0.32);
    return placeFigure(askew, crouchAt.clone().setY(RoofTop), Math.PI / 2 - 0.15);
}

export function buildMillRoofByDay(setting) {
    const take = fazeleyTake(setting, { time: hazyAfternoon, shadow: { focus: crouchAt, span: 120 }, lens: Lens });
    const chimneySmoke = createSmokeColumn(new Vector3(MillChimney.x, MillChimney.height + 1, MillChimney.z), srgb(0.55, 0.53, 0.5), 0.55);
    take.scene.add(crouchingAskew().root, chimneySmoke.column);
    return {
        scene: take.scene,
        camera: take.camera,
        update: (shotTime, clock) => {
            driftSmoke(chimneySmoke, wind, clock);
            take.flow(clock);
            glide(take.camera, path, progressOf(setting, shotTime));
        },
        look: () => lookOf('morning', { exposure: 0.85, bloom: { strength: 0.3, radius: 0.5, threshold: 2.5 } }),
        dispose: take.dispose,
    };
}
