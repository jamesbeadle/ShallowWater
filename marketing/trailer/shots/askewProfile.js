import { AdditiveBlending, PerspectiveCamera, Scene, Vector3 } from 'three';
import { createDriftingSmoke, glowFromLights, puffFrom } from '../effects/driftingSmoke.js';
import { buildProfileBust, Mouth } from '../props/profile/profileBust.js';
import { buildLampLitCabin, LampFlame } from '../sets/lampLitCabin.js';
import { lookOf } from '../stage/looks.js';
import { srgb } from '../world/colours.js';
import { disposeScene } from '../world/disposal.js';
import { valueNoise } from '../world/noise.js';
import { createRandom } from '../world/random.js';
import { glide, progressOf } from './cameraMoves.js';

const Lens = { fieldOfView: 26, near: 0.05, far: 20 };
const Creep = {
    from: new Vector3(-0.2, 0.045, 1.08), to: new Vector3(-0.165, 0.05, 0.86), lookFrom: new Vector3(-0.2, 0.0, 0), lookTo: new Vector3(-0.165, 0.004, 0),
};
const Rim = { colour: srgb(1.0, 0.64, 0.34), strength: 2.6, direction: new Vector3(-0.9, 0.12, -0.4).normalize() };
const Breathing = { period: 4.6, rise: 0.0022 };
const Flicker = { speed: 7, depth: 0.12, dipAt: 3.35, dipDepth: 0.35, dipLength: 0.16 };
const Speech = { speaker: 'ASKEW', everyWord: 0.62, tail: 0.45, drift: new Vector3(-0.2, 0.05, 0.04), size: [0.03, 0.22], life: 2.4, opacity: 0.45 };
const VapourLight = { strength: 1.1, reach: 0.9 };
const Lensing = { aperture: 0.04, maximumBlur: 0.01, focusBehindCheek: 0.02 };

function spokenPuffs(edit, shot) {
    const random = createRandom(39);
    const lines = edit.cards.filter((card) => card.speaker === Speech.speaker && card.start < shot.end && card.end > shot.start);
    const words = lines.flatMap((card) => {
        const count = Math.max(1, Math.floor((card.end - card.start - Speech.tail) / Speech.everyWord));
        return Array.from({ length: count }, (unused, index) => card.start - shot.start + index * Speech.everyWord);
    });
    return words.flatMap((bornAt) => [0, 0.08, 0.16].map((lag) => puffFrom({
        bornAt: bornAt + lag, origin: Mouth, velocity: Speech.drift, random, size: Speech.size, life: Speech.life, opacity: Speech.opacity,
    })));
}

function lampFlickerAt(shotTime) {
    const sinceDip = Math.abs(shotTime - Flicker.dipAt) / Flicker.dipLength;
    const dip = Flicker.dipDepth * Math.exp(-sinceDip * sinceDip);
    return 1 - Flicker.depth * valueNoise(shotTime * Flicker.speed, 1.7) - dip;
}

function profileLook(focus) {
    return lookOf('night', {
        exposure: 1.2,
        bloom: { strength: 0.55, radius: 0.55, threshold: 0.85 },
        focus: { distance: focus.value, aperture: Lensing.aperture, maximumBlur: Lensing.maximumBlur },
        finish: { contrast: 1.2, saturation: 0.78, vignette: 0.65, glowColour: [0.06, 0.03, 0.008], glowCentre: [0.3, 0.42], glowRadius: 0.3 },
    });
}

export function buildAskewProfile(setting) {
    const { aspect, edit, shot } = setting;
    const camera = new PerspectiveCamera(Lens.fieldOfView, aspect, Lens.near, Lens.far);
    const time = { value: 0 };
    const scene = new Scene();
    const bust = buildProfileBust(Rim);
    const cabin = buildLampLitCabin({ time, headCentre: new Vector3() });
    const vapour = createDriftingSmoke(spokenPuffs(edit, shot), srgb(0.01, 0.01, 0.012), AdditiveBlending);
    const { lampLight, baseIntensity } = cabin;
    const vapourLight = glowFromLights([lampLight], VapourLight);
    scene.add(bust, ...cabin.parts, vapour.smoke);
    const focus = { value: 1, place: new Vector3() };
    return {
        scene,
        camera,
        update: (shotTime) => {
            time.value = shotTime;
            bust.position.setY(Breathing.rise * Math.sin((shotTime / Breathing.period) * Math.PI * 2));
            lampLight.intensity = baseIntensity * lampFlickerAt(shotTime);
            glide(camera, Creep, progressOf(setting, shotTime));
            vapour.update(shotTime, camera, vapourLight);
            focus.value = camera.getWorldPosition(focus.place).z - Lensing.focusBehindCheek;
        },
        look: () => profileLook(focus),
        dispose: () => disposeScene(scene),
    };
}
