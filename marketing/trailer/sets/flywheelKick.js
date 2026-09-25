import { Vector3 } from 'three';
import { Crank } from '../props/engine/bolinder.js';
import { flywheelAngleAt } from '../props/engine/startingSequence.js';
import { poseArm, Reach } from '../props/reachingArm.js';
import { easeInOut, easeOut } from '../shots/cameraMoves.js';

const Rim = { radius: 0.36, standOff: 0.028, grip: 1.0, letGo: 0.42 };
const Moves = { reachingIn: 0.5, leadIn: 0.1, withdrawing: 0.55, searchStep: 0.01 };
const Body = { shoulder: new Vector3(-0.8, 0.3, 0.5), offstage: new Vector3(-0.98, 0.02, 0.8), elbowBend: new Vector3(-0.3, -1, 0.5) };
const flywheelCentre = new Vector3(Crank.flywheelX, Crank.height, 0);

function onTheRim(angle) {
    const outward = new Vector3(0, Math.cos(angle), Math.sin(angle));
    const place = flywheelCentre.clone().addScaledVector(outward, Rim.radius + Rim.standOff);
    return { place, outward, along: new Vector3(0, Math.sin(angle), -Math.cos(angle)) };
}

function elbowFor(shoulder, wrist) {
    const toWrist = new Vector3().subVectors(wrist, shoulder);
    const distance = Math.min(toWrist.length(), Reach.upperArm + Reach.forearm - 0.01);
    const direction = toWrist.normalize();
    const along = (Reach.upperArm ** 2 - Reach.forearm ** 2 + distance ** 2) / (2 * distance);
    const bend = Body.elbowBend.clone().addScaledVector(direction, -Body.elbowBend.dot(direction)).normalize();
    return shoulder.clone().addScaledVector(direction, along).addScaledVector(bend, Math.sqrt(Math.max(Reach.upperArm ** 2 - along ** 2, 0)));
}

export function kickTimingOf(sequence) {
    const grip = sequence.kick - Moves.leadIn;
    let letGo = sequence.kick;
    while (flywheelAngleAt(sequence, letGo) < Rim.grip - Rim.letGo) {
        letGo += Moves.searchStep;
    }
    return { start: grip - Moves.reachingIn, grip, letGo, gone: letGo + Moves.withdrawing };
}

function handAt(sequence, timing, time) {
    const grip = onTheRim(Rim.grip);
    const released = onTheRim(Rim.letGo);
    const holding = onTheRim(Rim.grip - flywheelAngleAt(sequence, Math.min(time, timing.letGo)));
    const reaching = easeOut((time - timing.start) / (timing.grip - timing.start));
    const leaving = easeInOut((time - timing.letGo) / (timing.gone - timing.letGo));
    const approach = { ...grip, place: Body.offstage.clone().lerp(grip.place, reaching) };
    const retreat = { ...released, place: released.place.clone().lerp(Body.offstage, leaving) };
    return [[timing.grip, approach], [timing.letGo, holding]].find(([until]) => time < until)?.[1] ?? retreat;
}

export function kickTheFlywheel(reachingArm, sequence, timing, time) {
    const hand = handAt(sequence, timing, time);
    const wrist = hand.place.clone().addScaledVector(hand.along, -Reach.palmLength / 2);
    const { arm } = reachingArm;
    arm.visible = time > timing.start && time < timing.gone;
    poseArm(reachingArm, { shoulder: Body.shoulder, elbow: elbowFor(Body.shoulder, wrist), wrist, palmFacing: hand.outward, fingersPointing: hand.along });
}
