import { createClothing, Wardrobe } from '../props/figure/clothing.js';
import { buildFigure } from '../props/figure/figureRig.js';
import { placeFigure } from '../props/figure/placement.js';
import { raiseHand, standPose } from '../props/figure/poses.js';
import { createRim } from '../props/figure/rimLight.js';
import { buildPlacard } from '../props/placard.js';
import { lookOf } from '../stage/looks.js';
import { headingAt, pointBeside } from '../world/canal.js';
import { srgb } from '../world/colours.js';
import { Daylights } from '../world/daylights.js';
import { glide, progressOf } from './cameraMoves.js';
import { buildCrewedBoat, sailBoat } from './crewedBoat.js';
import { hopwasTake } from './hopwasTake.js';
import { Mists } from '../world/heightFog.js';
import { sunDirectionAt } from '../world/sunlight.js';

const Lens = { fieldOfView: 26 };
const sunBehindTheCamera = sunDirectionAt(16, 292);
const towpath = 0.55;
const Voyage = { startX: -6.5, speed: 1.6, lane: 1.4, clockStart: 25 };
const Greeting = { picketAt: 0.7, askewAt: 1.35, raiseSeconds: 0.35 };
const path = {
    from: pointBeside(-27, -9.5, 1.8),
    to: pointBeside(-25.5, -9.2, 1.75),
    lookFrom: pointBeside(-11, 7, 1.4),
    lookTo: pointBeside(-9, 7, 1.4),
};

function raised(since) {
    return Math.min(Math.max(since / Greeting.raiseSeconds, 0), 1);
}

function picket(scene, rim) {
    const striker = buildFigure({ clothing: createClothing({}, rim), wardrobe: Wardrobe.workman });
    standPose(striker, { turnHead: -0.3 });
    scene.add(placeFigure(striker, pointBeside(-13, 10.2, towpath), Math.PI + 0.3).root);
    const mate = buildFigure({ clothing: createClothing({}, rim), wardrobe: Wardrobe.workman });
    standPose(mate, { turnHead: 0.2 });
    scene.add(placeFigure(mate, pointBeside(-15.5, 10.6, towpath), Math.PI - 0.2).root);
    const placard = buildPlacard(['KEEPERS', 'ON STRIKE']);
    placard.position.copy(pointBeside(-15, 9.9, towpath));
    placard.rotation.set(0, Math.PI, 0);
    scene.add(placard);
    return striker;
}

export function buildPicketAtTheBridge(setting) {
    const daylight = { ...Daylights.morning, sunDirection: sunBehindTheCamera };
    const shadow = { focus: pointBeside(-8, 4), span: 50 };
    const take = hopwasTake(setting, { daylight, path, lens: Lens, shadow, village: { hasHalcyonMoored: false }, mist: Mists.thin });
    const rim = createRim(srgb(1.0, 0.85, 0.6), 0.9, daylight.sunDirection);
    const striker = picket(take.scene, rim);
    const crewed = buildCrewedBoat({ rim, smokeColour: srgb(0.6, 0.6, 0.6), smokeOpacity: 0.4 });
    take.scene.add(...crewed.parts);
    const beats = setting.edit.timesOf('engineBeat');
    return {
        scene: take.scene,
        camera: take.camera,
        update: (shotTime, clock) => {
            const x = Voyage.startX + Voyage.speed * (clock - Voyage.clockStart);
            const wake = sailBoat(crewed, { position: pointBeside(x, Voyage.lane), heading: headingAt(x), time: clock, beats });
            raiseHand(striker, raised(shotTime - Greeting.picketAt));
            raiseHand(crewed.askew, raised(shotTime - Greeting.askewAt));
            take.flow(clock, wake);
            glide(take.camera, path, progressOf(setting, shotTime));
        },
        look: () => lookOf('morning', { exposure: 0.8, bloom: { strength: 0.35, radius: 0.5, threshold: 1.8 } }),
        dispose: take.dispose,
    };
}
