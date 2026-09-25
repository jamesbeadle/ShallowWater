import { Vector3 } from 'three';
import { createRim } from '../props/figure/rimLight.js';
import { RovingBridge } from '../sets/fazeleyPlaces.js';
import { FazeleyTimes } from '../sets/fazeleyTimes.js';
import { lookOf } from '../stage/looks.js';
import { srgb } from '../world/colours.js';
import { buildCrewedBoat, sailBoat } from './crewedBoat.js';
import { easeInOut } from './cameraMoves.js';
import { fazeleyTake } from './fazeleyTake.js';

const Lens = { fieldOfView: 34 };
const aimLingersOnTheBoat = 3;
const Voyage = { centreAt68: RovingBridge.x - 2, speed: 1.8, lane: 0.6, departs: 68 };

export const Swing = {
    from: new Vector3(16, 1.2, -4.2),
    to: new Vector3(-16.5, 3.1, -2.6),
    lookFrom: new Vector3(7, 1.4, 0),
    lookTo: new Vector3(32, 1.6, 0.8),
};

function framing(boatPosition, move, progress) {
    const eased = easeInOut(progress);
    const offset = new Vector3().lerpVectors(move.from, move.to, eased);
    const aim = new Vector3().lerpVectors(move.lookFrom, move.lookTo, eased ** aimLingersOnTheBoat);
    return { position: offset.add(boatPosition), target: aim.add(boatPosition) };
}

export function buildHeroVoyage(setting, move, look) {
    const { dawn } = FazeleyTimes;
    const time = { ...dawn, weather: { ...dawn.weather, mieCoefficient: 0.003 } };
    const take = fazeleyTake(setting, { time, shadow: { focus: new Vector3(RovingBridge.x + 10, 0, 0), span: 70 }, lens: Lens });
    const crewed = buildCrewedBoat({ rim: createRim(srgb(1.0, 0.72, 0.45), 1.2, time.sunDirection), smokeColour: srgb(0.8, 0.7, 0.6), smokeOpacity: 0.4 });
    take.scene.add(...crewed.parts);
    const beats = setting.edit.timesOf('engineBeat');
    const { shot } = setting;
    const { camera } = take;
    return {
        scene: take.scene,
        camera,
        update: (shotTime, clock) => {
            const position = new Vector3(Voyage.centreAt68 + Voyage.speed * (clock - Voyage.departs), 0, Voyage.lane);
            take.flow(clock, sailBoat(crewed, { position, heading: 0, time: clock, beats }));
            const { position: cameraAt, target } = framing(position, move, shotTime / (shot.end - shot.start));
            camera.position.copy(cameraAt);
            camera.lookAt(target);
        },
        look,
        dispose: take.dispose,
    };
}

export function buildUnderTheBridge(setting) {
    const look = () => lookOf('dawn', { exposure: 0.7, bloom: { strength: 0.5, radius: 0.7, threshold: 1.3 } });
    return buildHeroVoyage(setting, Swing, look);
}
