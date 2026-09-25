import { CylinderGeometry, Mesh, MeshBasicMaterial, MeshStandardMaterial, SphereGeometry, Vector3 } from 'three';
import { createClothing, Wardrobe } from '../props/figure/clothing.js';
import { buildFigure } from '../props/figure/figureRig.js';
import { placeFigure } from '../props/figure/placement.js';
import { standPose } from '../props/figure/poses.js';
import { Proportions } from '../props/figure/proportions.js';
import { srgb } from '../world/colours.js';

const Holder = { length: 0.26, radius: 0.0045, tilt: 0.5 };
const Ember = { radius: 0.009, glow: srgb(1.0, 0.35, 0.08).multiplyScalar(6) };
const Pose = { lookUp: -0.06, turnHead: 0.32, upperArm: [-0.22, 0, -0.32], forearm: [-2.35, 0, 0], otherUpperArm: [-0.3, 0, 0.42], otherForearm: [-1.35, 0, 0.9] };
const Stature = { scale: 0.96 };
const ladyAtHome = { ...Wardrobe.lady, hasHat: false };

function bobbedHair(neck, material) {
    const radius = Proportions.headRadius;
    const crown = new Mesh(new SphereGeometry(radius * 1.12, 16, 12), material);
    crown.scale.set(0.98, 0.9, 1.08);
    crown.position.set(0, Proportions.neck + radius * 0.98, -0.014);
    const bob = new Mesh(new SphereGeometry(radius * 0.95, 14, 10), material);
    bob.scale.set(1.15, 0.6, 0.9);
    bob.position.set(0, Proportions.neck + radius * 0.35, -0.03);
    neck.add(crown, bob);
}

function holderInHand(wrist) {
    const holder = new Mesh(new CylinderGeometry(Holder.radius, Holder.radius, Holder.length, 6), new MeshStandardMaterial({ color: srgb(0.02, 0.02, 0.02) }));
    holder.position.set(0, -0.06 - Holder.length / 2, 0.02);
    const ember = new Mesh(new SphereGeometry(Ember.radius, 8, 6), new MeshBasicMaterial({ color: Ember.glow }));
    ember.position.set(0, -Holder.length / 2, 0);
    holder.add(ember);
    holder.rotation.set(Holder.tilt, 0, 0);
    wrist.add(holder);
    return ember;
}

function strikePose(rig) {
    standPose(rig, { lookUp: Pose.lookUp, turnHead: Pose.turnHead });
    const { rightShoulder, rightElbow, leftShoulder, leftElbow } = rig;
    rightShoulder.rotation.set(...Pose.upperArm);
    rightElbow.rotation.set(...Pose.forearm);
    leftShoulder.rotation.set(...Pose.otherUpperArm);
    leftElbow.rotation.set(...Pose.otherForearm);
}

export function standMrsFenn(place, heading) {
    const gown = new MeshStandardMaterial({ color: srgb(0.08, 0.05, 0.07), roughness: 0.6 });
    const clothing = createClothing({ coat: gown, trousers: gown, skin: new MeshStandardMaterial({ color: srgb(0.5, 0.38, 0.33), roughness: 0.6 }) });
    const rig = buildFigure({ clothing, wardrobe: ladyAtHome });
    bobbedHair(rig.neck, new MeshStandardMaterial({ color: srgb(0.05, 0.04, 0.035), roughness: 0.7 }));
    strikePose(rig);
    const ember = holderInHand(rig.rightWrist);
    placeFigure(rig, place, heading);
    const { root } = rig;
    root.scale.setScalar(Stature.scale);
    root.updateMatrixWorld(true);
    return { figure: root, ember: ember.getWorldPosition(new Vector3()) };
}
