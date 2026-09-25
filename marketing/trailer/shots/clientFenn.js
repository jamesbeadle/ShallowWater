import { PerspectiveCamera, Vector3 } from 'three';
import { createDriftingSmoke, glowFromLights, puffFrom } from '../effects/driftingSmoke.js';
import { buildFennHouseSet } from '../sets/fennHouseSet.js';
import { lookOf } from '../stage/looks.js';
import { srgb } from '../world/colours.js';
import { disposeScene } from '../world/disposal.js';
import { createRandom } from '../world/random.js';
import { easeInOut, glide, progressOf } from './cameraMoves.js';

const Lens = { widest: 30, tightest: 15, near: 0.5, far: 4000 };
const Approach = {
    from: new Vector3(-2.5, 1.75, 38), to: new Vector3(2.2, 2.4, 25), lookFrom: new Vector3(2.0, 5.9, 0), lookTo: new Vector3(5.7, 6.4, -0.6),
};
const Smoke = { every: 0.3, lead: 3, drift: new Vector3(0.02, 0.16, 0.01), size: [0.03, 0.2], life: 3.2, opacity: 0.35, colour: srgb(0.18, 0.18, 0.2) };
const SmokeLight = { strength: 0.08, reach: 2.5 };

function cigaretteSmoke(ember, shot) {
    const random = createRandom(28);
    const count = Math.ceil((shot.end - shot.start + Smoke.lead) / Smoke.every);
    const puffs = Array.from({ length: count }, (unused, index) => puffFrom({
        bornAt: index * Smoke.every - Smoke.lead, origin: ember, velocity: Smoke.drift, random, size: Smoke.size, life: Smoke.life, opacity: Smoke.opacity,
    }));
    return createDriftingSmoke(puffs, Smoke.colour);
}

function zoomIn(camera, progress) {
    camera.fov = Lens.widest + (Lens.tightest - Lens.widest) * easeInOut(progress);
    camera.updateProjectionMatrix();
}

function eveningLook() {
    return lookOf('night', {
        exposure: 1.05,
        bloom: { strength: 0.55, radius: 0.5, threshold: 0.85 },
        finish: { saturation: 0.9, vignette: 0.5, glowColour: [0.02, 0.012, 0.004], glowCentre: [0.55, 0.6], glowRadius: 0.4 },
    });
}

export function buildClientFenn(setting) {
    const { aspect, shot, renderer } = setting;
    const camera = new PerspectiveCamera(Lens.widest, aspect, Lens.near, Lens.far);
    const set = buildFennHouseSet({ renderer });
    const smoke = cigaretteSmoke(set.ember, shot);
    const smokeLight = glowFromLights([set.roomLight], SmokeLight);
    set.scene.add(smoke.smoke);
    return {
        scene: set.scene,
        camera,
        update: (shotTime) => {
            const progress = progressOf(setting, shotTime);
            glide(camera, { ...Approach, easing: easeInOut }, progress);
            zoomIn(camera, progress);
            smoke.update(shotTime, camera, smokeLight);
        },
        look: eveningLook,
        dispose: () => {
            disposeScene(set.scene);
            set.reflections.dispose();
        },
    };
}
