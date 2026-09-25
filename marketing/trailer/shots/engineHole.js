import { PerspectiveCamera, Vector3 } from 'three';
import { blowlampAt, bulbGlowAt, flywheelAngleAt, shudderAt, startingSequenceOf } from '../props/engine/startingSequence.js';
import { swingCabinLamp } from '../sets/cabinLampSwing.js';
import { buildEngineHoleSet } from '../sets/engineHoleSet.js';
import { lookOf } from '../stage/looks.js';
import { srgb } from '../world/colours.js';
import { disposeScene } from '../world/disposal.js';
import { valueNoise } from '../world/noise.js';
import { glide, handheld, progressOf } from './cameraMoves.js';

const Lens = { fieldOfView: 30, near: 0.05, far: 30 };
const PushIn = {
    from: new Vector3(-1.3, 0.42, 0.95), to: new Vector3(-1.0, 0.5, 0.68), lookFrom: new Vector3(0.0, 0.9, 0.0), lookTo: new Vector3(0.02, 1.0, 0.02),
};
const Shake = { handheld: 0.0025, roll: 0.004, lift: 0.006, engineLift: 0.003, engineSway: 0.002, engineRock: 0.005 };
const Flicker = { speed: 11, depth: 0.3, lightIntensity: 2.6 };
const Glow = { colour: srgb(0.5, 0.2, 0.05), centre: [0.56, 0.62], radius: 0.35 };
const Lensing = { aperture: 0.01, maximumBlur: 0.007 };
const Heat = { lightIntensity: 1.6 };

function runEngine({ bolinder, bulbLight }, sequence, shotTime) {
    const shudder = shudderAt(sequence, shotTime);
    const { flywheel, engine, heat } = bolinder;
    flywheel.rotation.set(-flywheelAngleAt(sequence, shotTime), 0, 0);
    engine.position.set(0, Math.abs(shudder) * Shake.engineLift, shudder * Shake.engineSway);
    engine.rotation.set(shudder * Shake.engineRock, 0, shudder * Shake.engineRock);
    const { glow } = heat;
    glow.value = bulbGlowAt(sequence, shotTime);
    bulbLight.intensity = Heat.lightIntensity * glow.value ** 2;
    return shudder;
}

function burnBlowlamp(blowlamp, sequence, shotTime) {
    const { burning, withdrawn } = blowlampAt(sequence, shotTime);
    const flicker = 1 - Flicker.depth * valueNoise(shotTime * Flicker.speed, 3.1);
    const { lamp, light, flame, restingPlace, direction } = blowlamp;
    lamp.position.copy(restingPlace).addScaledVector(direction, -withdrawn).add(new Vector3(0, -withdrawn * 0.7, 0));
    const { strength } = flame;
    strength.value = burning * flicker;
    light.intensity = Flicker.lightIntensity * burning * flicker;
    return burning;
}

function frameCamera(camera, setting, shotTime, shudder) {
    glide(camera, PushIn, progressOf(setting, shotTime));
    handheld(camera, shotTime, Shake.handheld);
    camera.rotateZ(shudder * Shake.roll);
    camera.translateY(shudder * Shake.lift);
}

function depthOf(camera, point) {
    const forward = camera.getWorldDirection(new Vector3());
    return new Vector3().subVectors(point, camera.position).dot(forward);
}

function engineLook(moment) {
    const glowColour = Glow.colour.toArray().map((channel) => channel * moment.burning);
    return lookOf('firelight', {
        exposure: 1.05,
        bloom: { strength: 0.5, radius: 0.45, threshold: 0.9 },
        focus: { distance: moment.focus, aperture: Lensing.aperture, maximumBlur: Lensing.maximumBlur },
        finish: { glowColour, glowCentre: Glow.centre, glowRadius: Glow.radius, vignette: 0.55 },
    });
}

export function buildEngineHole(setting) {
    const { aspect, edit, shot, renderer } = setting;
    const camera = new PerspectiveCamera(Lens.fieldOfView, aspect, Lens.near, Lens.far);
    const sequence = startingSequenceOf(edit, shot);
    const time = { value: 0 };
    const set = buildEngineHoleSet({ renderer, sequence, time });
    const { bolinder, blowlamp, cabinLamp, smoke, smokeLight } = set;
    const moment = { focus: 1, burning: 1 };
    return {
        scene: set.scene,
        camera,
        update: (shotTime) => {
            time.value = shotTime;
            const shudder = runEngine(set, sequence, shotTime);
            moment.burning = burnBlowlamp(blowlamp, sequence, shotTime);
            swingCabinLamp(cabinLamp, shotTime, sequence.beats);
            frameCamera(camera, setting, shotTime, shudder);
            smoke.update(shotTime, camera, smokeLight);
            moment.focus = depthOf(camera, bolinder.bulbCentre);
        },
        look: () => engineLook(moment),
        dispose: () => {
            disposeScene(set.scene);
            set.reflections.dispose();
        },
    };
}
