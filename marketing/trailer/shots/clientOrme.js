import { BoxGeometry, Mesh, MeshStandardMaterial, Vector3 } from 'three';
import { buildBrazier, flicker } from '../effects/brazier.js';
import { createClothing, Wardrobe } from '../props/figure/clothing.js';
import { buildFigure } from '../props/figure/figureRig.js';
import { placeFigure } from '../props/figure/placement.js';
import { carryForward, standPose } from '../props/figure/poses.js';
import { createRim } from '../props/figure/rimLight.js';
import { buildPlacard } from '../props/placard.js';
import { FazeleyTimes } from '../sets/fazeleyTimes.js';
import { lookOf } from '../stage/looks.js';
import { srgb } from '../world/colours.js';
import { glide, progressOf } from './cameraMoves.js';
import { fazeleyTake } from './fazeleyTake.js';

const Lens = { fieldOfView: 34 };
const bank = 0.62;
const Fires = [{ x: 96, z: 9.3, intensity: 22 }, { x: 102.5, z: 9.0, intensity: 18 }];
const Pickets = [
    { x: 94.5, z: 10.4, turn: 2.4, placard: ['KEEPERS', 'ON STRIKE'] },
    { x: 101.2, z: 10.9, turn: -2.2, placard: ['NOT A PENNY', 'OFF THE LOCKS'] },
    { x: 96.4, z: 8.1, turn: 0.4 },
];
const path = {
    from: new Vector3(110.5, 1.55, 11.4),
    to: new Vector3(108.6, 1.5, 10.9),
    lookFrom: new Vector3(101.5, 1.6, 9.0),
    lookTo: new Vector3(102.5, 1.55, 9.1),
};

function picketLine(scene) {
    Pickets.forEach(({ x, z, turn, placard }) => {
        const striker = buildFigure({ clothing: createClothing({}, createRim(srgb(1.0, 0.5, 0.2), 0.8, new Vector3(0, 0, 1))), wardrobe: Wardrobe.workman });
        standPose(striker, { turnHead: 0.2 });
        scene.add(placeFigure(striker, new Vector3(x, bank, z), turn).root);
        if (placard) {
            const sign = buildPlacard(placard);
            sign.position.set(x + 0.3, bank, z - 0.3);
            scene.add(sign);
        }
    });
}

function doctorOrme() {
    const blackCoat = new MeshStandardMaterial({ color: srgb(0.06, 0.06, 0.07), roughness: 0.8 });
    const orme = buildFigure({ clothing: createClothing({ coat: blackCoat }), wardrobe: Wardrobe.gentleman });
    standPose(orme, { turnHead: -0.3 });
    carryForward(orme, { shoulder: 0.15, elbow: 0.1 });
    const bag = new Mesh(new BoxGeometry(0.38, 0.26, 0.18), new MeshStandardMaterial({ color: srgb(0.12, 0.06, 0.04), roughness: 0.5 }));
    bag.position.set(0, -0.62, 0.05);
    orme.rightWrist.add(bag);
    return placeFigure(orme, new Vector3(104.2, bank, 9.3), -Math.PI / 2 + 0.4);
}

export function buildClientOrme(setting) {
    const take = fazeleyTake(setting, { time: FazeleyTimes.dusk, shadow: { focus: new Vector3(99, 0, 9), span: 30 }, lens: Lens });
    const { scene } = take.fazeley;
    const fires = Fires.map(({ x, z, intensity }) => {
        const fire = buildBrazier(intensity);
        const { brazier } = fire;
        brazier.position.set(x, bank, z);
        scene.add(brazier);
        return { fire, intensity };
    });
    picketLine(scene);
    scene.add(doctorOrme().root);
    return {
        scene,
        camera: take.camera,
        update: (shotTime, clock) => {
            fires.forEach(({ fire, intensity }, index) => flicker(fire, intensity, clock + index));
            take.flow(clock);
            glide(take.camera, path, progressOf(setting, shotTime));
        },
        look: () => lookOf('firelight', { exposure: 1.25, bloom: { strength: 0.8, radius: 0.6, threshold: 0.8 } }),
        dispose: take.dispose,
    };
}
