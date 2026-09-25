import { PerspectiveCamera, Vector3 } from 'three';
import { createLightShafts } from '../effects/lightShafts.js';
import { createClothing, Wardrobe } from '../props/figure/clothing.js';
import { buildFigure } from '../props/figure/figureRig.js';
import { placeFigure } from '../props/figure/placement.js';
import { runPose } from '../props/figure/poses.js';
import { createRim } from '../props/figure/rimLight.js';
import { Ride } from '../sets/hopwasPlaces.js';
import { buildHopwasSet } from '../sets/hopwasSet.js';
import { lookOf } from '../stage/looks.js';
import { centreZ } from '../world/canal.js';
import { srgb } from '../world/colours.js';
import { Daylights } from '../world/daylights.js';
import { disposeScene } from '../world/disposal.js';
import { seeDistantThings } from '../world/layers.js';
import { handheld } from './cameraMoves.js';
import { scatterGroundCover } from '../world/groundCover.js';
import { between } from '../world/random.js';

const Lens = { fieldOfView: 24, near: 0.3, far: 3000 };
const Run = { speed: 3.9, startAcross: -196, secondsPerStep: 1 / 3, heading: Math.PI };
const Tracking = { offset: -11.5, lead: -1.2, height: 1.35, aimHeight: 1.05 };
const flat = () => 0;

function spot(x, across, height) {
    return new Vector3(x, height, centreZ(x) + across);
}

function framingAt(groundAt, shotTime) {
    const runnerAcross = Run.startAcross - Run.speed * shotTime;
    const cameraX = Ride.x + Tracking.offset;
    const cameraAcross = runnerAcross + Tracking.lead;
    const position = spot(cameraX, cameraAcross, groundAt(cameraX, cameraAcross) + Tracking.height);
    const target = spot(Ride.x, runnerAcross - 1.5, groundAt(Ride.x, runnerAcross) + Tracking.aimHeight);
    return { position, target, runnerAcross };
}

function sunlitShafts(sun, groundAt) {
    const centre = spot(Ride.x + 6, Run.startAcross - 10, groundAt(Ride.x, Run.startAcross) + 5);
    const reach = new Vector3(14, 4, 22);
    const beams = { count: 18, length: 60, width: 1.3, opacity: 0.085, seed: 7 };
    return createLightShafts({ ...beams, centre, spread: reach, direction: sun.direction, colour: sun.colour });
}

const Bracken = {
    count: 2600, nearSide: -17, farSide: 14, rideClearance: 3.2,
    colours: [srgb(0.55, 0.3, 0.12), srgb(0.46, 0.25, 0.1), srgb(0.62, 0.38, 0.16), srgb(0.36, 0.33, 0.15)],
};

function brackenAround(groundAt) {
    const pointAt = (random) => {
        const offset = between(random, Bracken.nearSide, Bracken.farSide);
        const x = Ride.x + offset + Math.sign(offset) * Bracken.rideClearance;
        const across = between(random, Run.startAcross - 34, Run.startAcross + 12);
        return spot(x, across, groundAt(x, across) - 0.05);
    };
    return scatterGroundCover({ count: Bracken.count, pointAt, colours: Bracken.colours, seed: 11 });
}

function buildRunner(sunDirection) {
    const clothing = createClothing({}, createRim(srgb(1.0, 0.72, 0.45), 1.1, sunDirection));
    return buildFigure({ clothing, wardrobe: Wardrobe.runner });
}

export function buildRunningTheRide(setting) {
    const camera = seeDistantThings(new PerspectiveCamera(Lens.fieldOfView, setting.aspect, Lens.near, Lens.far));
    const viewpoints = [0, 2.5, 5].map((shotTime) => framingAt(flat, shotTime));
    const view = { lens: Lens, aspect: setting.aspect, viewpoints, nearRadius: 170 };
    const shadow = { focus: spot(Ride.x, Run.startAcross - 10, 30), span: 70 };
    const daylight = { ...Daylights.dawn, fillIntensity: 0.55 };
    const hopwas = buildHopwasSet({ daylight, view, shadow, fogDensity: 0.012 });
    const runner = buildRunner(daylight.sunDirection);
    hopwas.scene.add(runner.root, sunlitShafts(hopwas.sun, hopwas.groundAt), brackenAround(hopwas.groundAt));
    return {
        scene: hopwas.scene,
        camera,
        update: (shotTime) => {
            const { position, target, runnerAcross } = framingAt(hopwas.groundAt, shotTime);
            placeFigure(runner, spot(Ride.x, runnerAcross, hopwas.groundAt(Ride.x, runnerAcross)), Run.heading);
            runPose(runner, (Math.PI * shotTime) / Run.secondsPerStep);
            camera.position.copy(position);
            camera.lookAt(target);
            handheld(camera, shotTime, 0.0035);
        },
        look: () => lookOf('dawn', { exposure: 0.78, bloom: { strength: 0.5, radius: 0.65, threshold: 1.0 } }),
        dispose: () => disposeScene(hopwas.scene),
    };
}
