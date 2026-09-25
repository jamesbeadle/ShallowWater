import { CapsuleGeometry, CylinderGeometry, Group, Matrix4, Mesh, MeshStandardMaterial, Quaternion, Vector3 } from 'three';
import { RoundedBoxGeometry } from 'three/addons/geometries/RoundedBoxGeometry.js';
import { mergeGeometries } from 'three/addons/utils/BufferGeometryUtils.js';
import { srgb } from '../world/colours.js';
import { createRim, withRim } from './figure/rimLight.js';

export const Reach = { upperArm: 0.31, forearm: 0.28, palmLength: 0.1 };
const Girth = { sleeve: 0.052, cuff: 0.058, elbow: 0.043, wrist: 0.029, finger: 0.0095, thumb: 0.011 };
const fingerRoots = [-0.03, -0.01, 0.01, 0.03];
const Curl = { first: new Vector3(0, -0.35, 0.94), second: new Vector3(0, -0.92, 0.38), firstLength: 0.042, secondLength: 0.036 };
const upward = new Vector3(0, 1, 0);

function segment(from, direction, length, radius) {
    const piece = new CapsuleGeometry(radius, length, 3, 8).toNonIndexed();
    const turn = new Quaternion().setFromUnitVectors(upward, direction.clone().normalize());
    const middle = from.clone().addScaledVector(direction.clone().normalize(), length / 2);
    return piece.applyMatrix4(new Matrix4().compose(middle, turn, new Vector3(1, 1, 1)));
}

function handGeometry() {
    const palm = new RoundedBoxGeometry(0.082, 0.03, Reach.palmLength, 2, 0.012);
    palm.translate(0, 0, Reach.palmLength / 2);
    const fingers = fingerRoots.flatMap((across) => {
        const knuckle = new Vector3(across, -0.004, Reach.palmLength);
        const bend = knuckle.clone().addScaledVector(Curl.first, Curl.firstLength);
        return [segment(knuckle, Curl.first, Curl.firstLength, Girth.finger), segment(bend, Curl.second, Curl.secondLength, Girth.finger)];
    });
    const thumb = segment(new Vector3(0.04, -0.01, 0.02), new Vector3(-0.3, -0.45, 0.85), 0.055, Girth.thumb);
    return mergeGeometries([palm, ...fingers, thumb]);
}

function stretchBetween(mesh, from, to) {
    const span = new Vector3().subVectors(to, from);
    mesh.position.copy(from).addScaledVector(span, 0.5);
    mesh.quaternion.copy(new Quaternion().setFromUnitVectors(upward, span.clone().normalize()));
    mesh.scale.set(1, span.length(), 1);
}

export function buildReachingArm({ rimColour, rimStrength, rimDirection }) {
    const rim = createRim(rimColour, rimStrength, rimDirection);
    const skin = withRim(new MeshStandardMaterial({ color: srgb(0.42, 0.28, 0.21), roughness: 0.62 }), rim);
    const wool = withRim(new MeshStandardMaterial({ color: srgb(0.09, 0.085, 0.075), roughness: 1 }), rim);
    const parts = {
        upper: new Mesh(new CapsuleGeometry(Girth.sleeve, 1 - Girth.sleeve * 2, 4, 10), wool),
        cuff: new Mesh(new CapsuleGeometry(Girth.cuff, 1 - Girth.cuff * 2, 4, 10), wool),
        lower: new Mesh(new CylinderGeometry(Girth.wrist, Girth.elbow, 1, 12), skin),
        hand: new Mesh(handGeometry(), skin),
    };
    const arm = new Group();
    arm.add(parts.upper, parts.cuff, parts.lower, parts.hand);
    return { arm, ...parts };
}

export function poseArm(reachingArm, { shoulder, elbow, wrist, palmFacing, fingersPointing }) {
    const { upper, cuff, lower, hand } = reachingArm;
    stretchBetween(upper, shoulder, elbow);
    stretchBetween(cuff, elbow, elbow.clone().lerp(wrist, 0.22));
    stretchBetween(lower, elbow, wrist);
    const across = new Vector3().crossVectors(palmFacing, fingersPointing).normalize();
    hand.quaternion.setFromRotationMatrix(new Matrix4().makeBasis(across, palmFacing, fingersPointing));
    hand.position.copy(wrist);
}
