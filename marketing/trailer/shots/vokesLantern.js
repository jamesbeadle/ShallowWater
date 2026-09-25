import { Vector3 } from 'three';
import { buildHandLantern } from '../props/handLantern.js';
import { createClothing, Wardrobe } from '../props/figure/clothing.js';
import { buildFigure } from '../props/figure/figureRig.js';
import { placeFigure } from '../props/figure/placement.js';
import { carryForward, standPose } from '../props/figure/poses.js';
import { createRim } from '../props/figure/rimLight.js';
import { A5Bridge } from '../sets/fazeleyPlaces.js';
import { RoadBridgePlan } from '../sets/fazeleyBridges.js';
import { FazeleyTimes, underMoon } from '../sets/fazeleyTimes.js';
import { lookOf } from '../stage/looks.js';
import { srgb } from '../world/colours.js';
import { glide, progressOf } from './cameraMoves.js';
import { fazeleyTake } from './fazeleyTake.js';

const Lens = { fieldOfView: 26 };
const Parapet = { x: A5Bridge.x + RoadBridgePlan.width / 2 - 0.2, z: 2.5, height: RoadBridgePlan.crownHeight - 0.2 };
const path = {
    from: new Vector3(A5Bridge.x + 10.5, 7.3, 5.0),
    to: new Vector3(A5Bridge.x + 9.0, 7.4, 4.2),
    lookFrom: new Vector3(Parapet.x - 0.5, Parapet.height + 1.2, Parapet.z),
    lookTo: new Vector3(Parapet.x - 0.5, Parapet.height + 1.35, Parapet.z),
};

function sergeantVokes(moonDirection) {
    const clothing = createClothing({ coat: createClothing().trousers }, createRim(srgb(0.6, 0.7, 1.0), 1.2, moonDirection));
    const vokes = buildFigure({ clothing, wardrobe: Wardrobe.constable });
    standPose(vokes, { lookUp: -0.35, turnHead: 0.15 });
    carryForward(vokes, { shoulder: 0.85, elbow: 0.3 });
    return placeFigure(vokes, new Vector3(Parapet.x - 0.5, Parapet.height, Parapet.z), Math.PI / 2);
}

export function buildVokesLantern(setting) {
    const time = underMoon(FazeleyTimes.night, 12, 280);
    const take = fazeleyTake(setting, { time, shadow: { focus: new Vector3(A5Bridge.x, 0, 0), span: 50 }, lens: Lens });
    const vokes = sergeantVokes(time.sunDirection);
    const lantern = buildHandLantern(5);
    take.scene.add(vokes.root, lantern);
    return {
        scene: take.scene,
        camera: take.camera,
        update: (shotTime, clock) => {
            const swing = Math.sin(clock * 2.1) * 0.12;
            lantern.position.set(Parapet.x + 0.1, Parapet.height + 1.05, Parapet.z - 0.3 + swing);
            take.flow(clock);
            glide(take.camera, path, progressOf(setting, shotTime));
        },
        look: () => lookOf('night', { exposure: 1.6, bloom: { strength: 0.9, radius: 0.6, threshold: 0.7 } }),
        dispose: take.dispose,
    };
}
