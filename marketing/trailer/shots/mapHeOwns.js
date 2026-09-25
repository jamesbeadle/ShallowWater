import { PerspectiveCamera, Vector3 } from 'three';
import { createClothing, Wardrobe } from '../props/figure/clothing.js';
import { buildFigure } from '../props/figure/figureRig.js';
import { placeFigure } from '../props/figure/placement.js';
import { handsOnHips, standPose } from '../props/figure/poses.js';
import { createRim } from '../props/figure/rimLight.js';
import { buildHopwasSet } from '../sets/hopwasSet.js';
import { lookOf } from '../stage/looks.js';
import { centreZ } from '../world/canal.js';
import { flowWater } from '../world/canalWater.js';
import { srgb } from '../world/colours.js';
import { driftSky } from '../world/dawnSky.js';
import { Daylights } from '../world/daylights.js';
import { disposeScene } from '../world/disposal.js';
import { seeDistantThings } from '../world/layers.js';
import { easeInOut, glide, progressOf, viewpointsOf } from './cameraMoves.js';

const Lens = { fieldOfView: 28, near: 0.3, far: 6000 };
const Summit = { x: 100, across: -362 };
const Horizon = { x: 40, across: 16 };
const Crane = { from: 3.6, to: -7, heightFrom: 1.95, heightTo: 27, sideways: 0.7 };

function groundPoint(groundAt, x, across, height) {
    return new Vector3(x, groundAt(x, across) + height, centreZ(x) + across);
}

function facing() {
    const direction = new Vector3(Horizon.x - Summit.x, 0, centreZ(Horizon.x) + Horizon.across - centreZ(Summit.x) - Summit.across);
    return direction.normalize();
}

function behindSummit(groundAt, distance, height) {
    const direction = facing();
    const x = Summit.x - direction.x * distance - direction.z * Crane.sideways;
    const across = Summit.across - direction.z * distance + direction.x * Crane.sideways;
    return groundPoint(groundAt, x, across, height);
}

function revealPath(groundAt) {
    const horizon = groundPoint(() => 0, Horizon.x, Horizon.across, 0);
    return {
        from: behindSummit(groundAt, Crane.from, Crane.heightFrom),
        to: behindSummit(groundAt, Crane.to, Crane.heightTo),
        lookFrom: horizon.clone().setY(4),
        lookTo: groundPoint(() => 0, Horizon.x - 20, Horizon.across - 4, 0),
        easing: easeInOut,
    };
}

function standingAskew(groundAt, sunDirection) {
    const clothing = createClothing({}, createRim(srgb(1.0, 0.74, 0.48), 1.4, sunDirection));
    const askew = buildFigure({ clothing, wardrobe: Wardrobe.runner });
    standPose(askew, { lookUp: -0.1, turnHead: 0.2 });
    handsOnHips(askew);
    const direction = facing();
    return placeFigure(askew, groundPoint(groundAt, Summit.x, Summit.across, 0), Math.atan2(direction.x, direction.z));
}

export function buildMapHeOwns(setting) {
    const camera = seeDistantThings(new PerspectiveCamera(Lens.fieldOfView, setting.aspect, Lens.near, Lens.far));
    const view = { lens: Lens, aspect: setting.aspect, viewpoints: viewpointsOf(revealPath(() => 58)), nearRadius: 220 };
    const daylight = { ...Daylights.dawn, fillIntensity: 1.1, sunIntensity: 5.2 };
    const hopwas = buildHopwasSet({ daylight, view, shadow: { focus: new Vector3(200, 20, -150), span: 280 } });
    const path = revealPath(hopwas.groundAt);
    const askew = standingAskew(hopwas.groundAt, daylight.sunDirection);
    const { time } = hopwas;
    hopwas.scene.add(askew.root);
    return {
        scene: hopwas.scene,
        camera,
        update: (shotTime) => {
            glide(camera, path, progressOf(setting, shotTime));
            flowWater(hopwas.water, shotTime, {});
            driftSky(hopwas.sky, shotTime);
            time.value = shotTime;
        },
        look: () => lookOf('dawn', { exposure: 0.82, bloom: { strength: 0.4, radius: 0.6, threshold: 1.3 } }),
        dispose: () => disposeScene(hopwas.scene),
    };
}
