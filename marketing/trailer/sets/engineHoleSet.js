import { AmbientLight, FogExp2, PointLight, Quaternion, Scene, Vector3 } from 'three';
import { createDriftingSmoke, glowFromLights, puffFrom } from '../effects/driftingSmoke.js';
import { aimBlowlamp, buildBlowlamp } from '../props/engine/blowlamp.js';
import { buildBolinder } from '../props/engine/bolinder.js';
import { createEngineMaterials } from '../props/engine/engineMaterials.js';
import { srgb } from '../world/colours.js';
import { bakeEnvironment } from '../world/environmentBake.js';
import { Mists, useHeightFog } from '../world/heightFog.js';
import { createRandom } from '../world/random.js';
import { buildReachingArm } from '../props/reachingArm.js';
import { hangCabinLamp } from './cabinLampSwing.js';
import { buildEngineRoom, Room } from './engineRoom.js';
import { fitOutEngineRoom } from './engineRoomFittings.js';
import { engineRoomSurroundings, lightTheEngineRoom } from './engineRoomLight.js';

const Nozzle = new Vector3(0.25, 1.12, 0.24);
const Murk = { colour: srgb(0.02, 0.018, 0.016), density: 0.1, reflections: 0.45 };
const FlameLight = { colour: srgb(1.0, 0.52, 0.2), intensity: 2.6, reach: 4, decay: 1.8 };
const BulbLight = { colour: srgb(1.0, 0.25, 0.06), intensity: 0.5, reach: 2.2, decay: 1.8 };
const lampPivot = new Vector3(-0.42, Room.roof - 0.02, -0.62);
const upward = new Vector3(0, 1, 0);
const SmokeLight = { strength: 0.2, reach: 0.5 };
const Soot = { velocity: new Vector3(-0.18, 0.2, 0.4), size: [0.1, 0.7], life: 2.8, opacity: 0.85 };
const ArmRim = { rimColour: srgb(1.0, 0.55, 0.25), rimStrength: 1.4, rimDirection: new Vector3(0.8, 0.4, -0.45).normalize() };
const Haze = { velocity: new Vector3(-0.02, 0.28, 0.02), size: [0.1, 0.4], life: 3.2, opacity: 0.18 };

function mountBlowlamp(scene, materials, bolinder, time) {
    const blowlamp = buildBlowlamp(materials, { time, flameReach: 0.21, splashRadius: bolinder.bulbRadius * 1.22 });
    const direction = aimBlowlamp(blowlamp, Nozzle, bolinder.bulbCentre);
    const { splash } = blowlamp.flame;
    splash.position.copy(bolinder.bulbCentre);
    splash.quaternion.copy(new Quaternion().setFromUnitVectors(upward, direction.clone().negate()));
    const { hotSpot } = bolinder.heat;
    hotSpot.value.copy(bolinder.bulbCentre).addScaledVector(direction, -bolinder.bulbRadius);
    const light = new PointLight(FlameLight.colour, FlameLight.intensity, FlameLight.reach, FlameLight.decay);
    light.position.copy(Nozzle).lerp(bolinder.bulbCentre, 0.75);
    const { lamp } = blowlamp;
    scene.add(lamp, splash, light);
    return { ...blowlamp, light, restingPlace: lamp.position.clone(), direction };
}

function bulbHeatLight(bolinder) {
    const light = new PointLight(BulbLight.colour, BulbLight.intensity, BulbLight.reach, BulbLight.decay);
    light.position.copy(bolinder.bulbCentre);
    return light;
}

function exhaustSmoke(bolinder, sequence) {
    const random = createRandom(4417);
    const firings = [sequence.cough, ...sequence.beats];
    const puffs = firings.flatMap((bornAt) => Array.from({ length: 4 }, (unused, index) => puffFrom({
        bornAt: bornAt + index * 0.05, origin: bolinder.exhaustJoint, velocity: Soot.velocity, random, size: Soot.size, life: Soot.life, opacity: Soot.opacity,
    })));
    const heatHaze = Array.from({ length: 8 }, (unused, index) => puffFrom({
        bornAt: index * 0.45 - 1.5, origin: bolinder.bulbCentre, velocity: Haze.velocity, random, size: Haze.size, life: Haze.life, opacity: Haze.opacity,
    }));
    return createDriftingSmoke([...puffs, ...heatHaze], srgb(0.012, 0.011, 0.01));
}

export function buildEngineHoleSet({ renderer, sequence, time }) {
    useHeightFog(Mists.none);
    const scene = new Scene();
    scene.fog = new FogExp2(Murk.colour, Murk.density);
    const reflections = bakeEnvironment(renderer, engineRoomSurroundings());
    Object.assign(scene, { environment: reflections.texture, environmentIntensity: Murk.reflections });
    const materials = createEngineMaterials();
    const bolinder = buildBolinder(materials, time);
    const blowlamp = mountBlowlamp(scene, materials, bolinder, time);
    const cabinLamp = hangCabinLamp(lampPivot);
    const smoke = exhaustSmoke(bolinder, sequence);
    const roomLight = lightTheEngineRoom();
    const bulbLight = bulbHeatLight(bolinder);
    scene.add(buildEngineRoom(), ...fitOutEngineRoom(), ...roomLight.parts, bolinder.engine, bulbLight, cabinLamp.swing, smoke.smoke);
    scene.add(new AmbientLight(srgb(0.3, 0.32, 0.4), 0.05));
    const smokeLight = glowFromLights([blowlamp.light, cabinLamp.light, roomLight.dawn, bulbLight], SmokeLight);
    const askewsArm = buildReachingArm(ArmRim);
    scene.add(askewsArm.arm);
    return { scene, bolinder, blowlamp, cabinLamp, bulbLight, smoke, smokeLight, askewsArm, reflections };
}
