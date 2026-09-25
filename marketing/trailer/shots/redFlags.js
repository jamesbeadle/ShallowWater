import { Vector3 } from 'three';
import { createMuzzleFlash, fireAt } from '../effects/muzzleFlash.js';
import { buildRangeFlag } from '../props/range/flag.js';
import { cadetClothing, Wardrobe } from '../props/figure/clothing.js';
import { buildFigure } from '../props/figure/figureRig.js';
import { placeFigure } from '../props/figure/placement.js';
import { handsOnHips, standPose } from '../props/figure/poses.js';
import { layProne } from '../props/figure/prone.js';
import { createRim } from '../props/figure/rimLight.js';
import { lookOf } from '../stage/looks.js';
import { centreZ, pointBeside } from '../world/canal.js';
import { srgb } from '../world/colours.js';
import { Daylights } from '../world/daylights.js';
import { glide, progressOf } from './cameraMoves.js';
import { hopwasTake } from './hopwasTake.js';
import { Mists } from '../world/heightFog.js';

const Lens = { fieldOfView: 32 };
const FiringPoint = { across: -300, lanes: [106, 111, 116, 121, 126] };
const firingHeight = 43;

function onGround(groundAt, point, height) {
    return point.clone().setY(groundAt(point.x, point.z - centreZ(point.x)) + height);
}

function pathOver(groundAt) {
    return {
        from: onGround(groundAt, pointBeside(135, -313), 2.1),
        to: onGround(groundAt, pointBeside(133.5, -312), 1.9),
        lookFrom: onGround(groundAt, pointBeside(104, -283), 0.5),
        lookTo: onGround(groundAt, pointBeside(103, -284), 0.8),
    };
}

function squad(scene, groundAt, rim) {
    const muzzles = FiringPoint.lanes.map((x) => {
        const cadet = buildFigure({ clothing: cadetClothing(rim), wardrobe: Wardrobe.workman });
        const spot = onGround(groundAt, pointBeside(x, FiringPoint.across), 0);
        scene.add(layProne(cadet, spot, 0, { hasScope: false }).holder);
        return spot.clone().add(new Vector3(0.1, 0.4, 2.55));
    });
    const garner = buildFigure({ clothing: cadetClothing(rim), wardrobe: Wardrobe.workman });
    standPose(garner, { turnHead: 0.3 });
    handsOnHips(garner);
    scene.add(placeFigure(garner, onGround(groundAt, pointBeside(114, FiringPoint.across - 5), 0), 0).root);
    return muzzles;
}

export function buildRedFlags(setting) {
    const daylight = Daylights.morning;
    const shadow = { focus: pointBeside(112, -290), span: 50 };
    const take = hopwasTake(setting, { daylight, path: pathOver(() => firingHeight), lens: Lens, shadow, mist: Mists.thin });
    const { groundAt, scene, time } = take.hopwas;
    const path = pathOver(groundAt);
    const muzzles = squad(scene, groundAt, createRim(srgb(1.0, 0.75, 0.5), 0.9, daylight.sunDirection));
    const flag = buildRangeFlag(time);
    flag.position.copy(onGround(groundAt, pointBeside(96, -296), -0.3));
    const flash = createMuzzleFlash(false);
    scene.add(flag, flash.sprite);
    const shots = setting.edit.timesOf('distantRifle');
    return {
        scene,
        camera: take.camera,
        update: (shotTime, clock) => {
            const firing = shots.findLastIndex((firedAt) => firedAt <= clock);
            fireAt(flash, muzzles[Math.max(firing, 0) * 2 % muzzles.length], shots, clock);
            take.flow(clock);
            glide(take.camera, path, progressOf(setting, shotTime));
        },
        look: () => lookOf('morning', { exposure: 0.8, bloom: { strength: 0.35, radius: 0.5, threshold: 1.8 } }),
        dispose: take.dispose,
    };
}
