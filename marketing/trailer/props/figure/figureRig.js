import { Group } from 'three';
import { createClothing, Wardrobe } from './clothing.js';
import { buildHead } from './head.js';
import { boot, hand, hangingLimb, joint } from './limbs.js';
import { Girth, Proportions } from './proportions.js';
import { chestMesh, coatSkirt, hipsMesh, neckMesh } from './torso.js';

function buildArm(torso, side, clothing, top) {
    const shoulder = joint(side * Proportions.shoulderSpread, Proportions.torso - 0.07, 0);
    shoulder.add(hangingLimb(Proportions.upperArm, Girth.upperArm, top));
    const elbow = joint(0, -Proportions.upperArm, 0);
    elbow.add(hangingLimb(Proportions.forearm, Girth.forearm, top));
    const wrist = joint(0, -Proportions.forearm, 0);
    wrist.add(hand(clothing.skin));
    elbow.add(wrist);
    shoulder.add(elbow);
    torso.add(shoulder);
    return { shoulder, elbow, wrist };
}

function buildLeg(pelvis, side, clothing) {
    const hip = joint(side * Proportions.hipSpread, 0, 0);
    hip.add(hangingLimb(Proportions.thigh, Girth.thigh, clothing.trousers));
    const knee = joint(0, -Proportions.thigh, 0);
    knee.add(hangingLimb(Proportions.shin, Girth.shin, clothing.trousers));
    const ankle = joint(0, -Proportions.shin, 0);
    ankle.add(boot(Proportions.footLength, clothing.boots));
    knee.add(ankle);
    hip.add(knee);
    pelvis.add(hip);
    return { hip, knee };
}

function buildUpperBody(pelvis, clothing, wardrobe) {
    const top = clothing[wardrobe.top];
    const torso = joint(0, 0, 0);
    torso.add(chestMesh(top));
    const neck = joint(0, Proportions.torso - 0.02, 0);
    neck.add(neckMesh(clothing.skin));
    const head = buildHead(clothing, wardrobe);
    head.position.set(0, Proportions.neck + Proportions.headRadius * 0.85, 0.01);
    neck.add(head);
    torso.add(neck);
    pelvis.add(torso);
    return { torso, neck, top };
}

export function buildFigure({ clothing = createClothing(), wardrobe = Wardrobe.workman } = {}) {
    const root = new Group();
    const pelvis = joint(0, Proportions.hipHeight, 0);
    pelvis.add(hipsMesh(clothing.trousers));
    root.add(pelvis);
    const { torso, neck, top } = buildUpperBody(pelvis, clothing, wardrobe);
    if (wardrobe.hasCoat) {
        pelvis.add(coatSkirt(top, wardrobe.hasSkirt));
    }
    const leftArm = buildArm(torso, -1, clothing, top);
    const rightArm = buildArm(torso, 1, clothing, top);
    const leftLeg = buildLeg(pelvis, -1, clothing);
    const rightLeg = buildLeg(pelvis, 1, clothing);
    return {
        root, pelvis, torso, neck,
        leftShoulder: leftArm.shoulder, leftElbow: leftArm.elbow, leftWrist: leftArm.wrist,
        rightShoulder: rightArm.shoulder, rightElbow: rightArm.elbow, rightWrist: rightArm.wrist,
        leftHip: leftLeg.hip, leftKnee: leftLeg.knee, rightHip: rightLeg.hip, rightKnee: rightLeg.knee,
    };
}
