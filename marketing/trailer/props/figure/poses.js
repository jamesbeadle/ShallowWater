import { Proportions } from './proportions.js';

const Stride = { thigh: 0.62, arm: 0.55, elbow: 1.45, lean: 0.14, bounce: 0.05, sink: 0.03, fold: 1.25, stanceBend: 0.28 };

function turn(pivot, x, y = 0, z = 0) {
    pivot.rotation.set(x, y, z);
}

function forward(angle) {
    return -angle;
}

function kneeBend(phase) {
    return Stride.stanceBend + Stride.fold * Math.max(0, Math.cos(phase)) ** 1.5;
}

export function runPose(rig, phase) {
    const stride = Math.sin(phase);
    turn(rig.leftHip, forward(Stride.thigh * stride));
    turn(rig.rightHip, forward(-Stride.thigh * stride));
    turn(rig.leftKnee, kneeBend(phase));
    turn(rig.rightKnee, kneeBend(phase + Math.PI));
    turn(rig.leftShoulder, forward(-Stride.arm * stride), 0, -0.1);
    turn(rig.rightShoulder, forward(Stride.arm * stride), 0, 0.1);
    turn(rig.leftElbow, forward(Stride.elbow));
    turn(rig.rightElbow, forward(Stride.elbow));
    turn(rig.torso, forward(Stride.lean), 0.12 * stride);
    const { pelvis } = rig;
    pelvis.position.setY(Proportions.hipHeight - Stride.sink + Stride.bounce * Math.abs(stride));
}

export function standPose(rig, { lookUp = 0, turnHead = 0 } = {}) {
    [rig.leftHip, rig.rightHip, rig.leftKnee, rig.rightKnee, rig.torso].forEach((pivot) => turn(pivot, 0));
    turn(rig.leftShoulder, 0.05, 0, -0.08);
    turn(rig.rightShoulder, 0.05, 0, 0.08);
    turn(rig.leftElbow, forward(0.18));
    turn(rig.rightElbow, forward(0.18));
    turn(rig.neck, forward(-lookUp), turnHead);
    const { pelvis } = rig;
    pelvis.position.setY(Proportions.hipHeight);
}

export function handsOnHips(rig) {
    turn(rig.rightShoulder, 0.12, 0, 0.72);
    turn(rig.rightElbow, 0, 0, -2.0);
    turn(rig.leftShoulder, 0.12, 0, -0.72);
    turn(rig.leftElbow, 0, 0, 2.0);
    turn(rig.leftHip, 0.04, 0, -0.06);
    turn(rig.rightHip, -0.08, 0, 0.1);
}

export function raiseHand(rig, amount) {
    turn(rig.rightShoulder, forward(2.5 * amount), 0, 0.35 * amount);
    turn(rig.rightElbow, forward(0.5 * amount));
}

export function carryForward(rig, { shoulder = 0.45, elbow = 0.5 } = {}) {
    turn(rig.rightShoulder, forward(shoulder), 0, 0.05);
    turn(rig.rightElbow, forward(elbow));
}

export function pronePose(rig) {
    const { root } = rig;
    root.rotation.set(Math.PI / 2, 0, 0);
    turn(rig.leftHip, 0, 0, -0.16);
    turn(rig.rightHip, 0, 0, 0.16);
    turn(rig.leftShoulder, forward(2.75), 0, -0.2);
    turn(rig.rightShoulder, forward(2.55), 0, 0.28);
    turn(rig.leftElbow, forward(0.35));
    turn(rig.rightElbow, forward(0.9));
    turn(rig.neck, forward(-1.05));
}
