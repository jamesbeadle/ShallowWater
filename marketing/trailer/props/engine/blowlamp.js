import { CylinderGeometry, Group, Quaternion, SphereGeometry, TorusGeometry, Vector3 } from 'three';
import { createBlowlampFlame } from '../../effects/blowlampFlame.js';
import { castingOf, placed, turned } from './turnedParts.js';

const tankProfile = [[0, 0], [0.056, 0], [0.063, 0.014], [0.063, 0.082], [0.05, 0.102], [0.016, 0.108], [0, 0.108]];
const Burner = { height: 0.2, length: 0.17, nozzle: 0.175, bore: 0.009 };
const alongJet = new Vector3(1, 0, 0);

function pumpAndCap(materials) {
    const rod = placed(castingOf(new CylinderGeometry(0.005, 0.005, 0.07, 8), materials.iron), [-0.03, 0.14, 0]);
    const knob = placed(castingOf(new SphereGeometry(0.013, 12, 8), materials.timber), [-0.03, 0.18, 0]);
    const cap = placed(turned([[0, 0], [0.014, 0], [0.014, 0.014], [0, 0.016]], materials.brass, 12), [0.025, 0.105, 0.02]);
    return [rod, knob, cap];
}

function burner(materials) {
    const riser = placed(castingOf(new CylinderGeometry(0.008, 0.008, 0.1, 10), materials.brass), [0, 0.15, 0]);
    const tubeShape = new CylinderGeometry(Burner.bore, Burner.bore * 1.2, Burner.length, 12);
    const tube = placed(castingOf(tubeShape, materials.iron), [Burner.length / 2, Burner.height, 0], [0, 0, Math.PI / 2]);
    const coil = placed(castingOf(new TorusGeometry(0.022, 0.006, 8, 20), materials.iron), [Burner.length * 0.72, Burner.height, 0], [0, Math.PI / 2, 0]);
    const handle = placed(castingOf(new TorusGeometry(0.05, 0.007, 8, 20, Math.PI), materials.iron), [-0.06, 0.06, 0], [0, 0, Math.PI / 2]);
    return [riser, tube, coil, handle];
}

export function buildBlowlamp(materials, { time, flameReach, splashRadius }) {
    const lamp = new Group();
    lamp.add(turned(tankProfile, materials.brass), ...pumpAndCap(materials), ...burner(materials));
    const flame = createBlowlampFlame({ reach: flameReach, radius: 0.042, splashRadius, time });
    placed(flame.jet, [Burner.nozzle, Burner.height, 0], [0, 0, -Math.PI / 2]);
    lamp.add(flame.jet);
    return { lamp, flame };
}

export function aimBlowlamp(blowlamp, nozzle, target) {
    const direction = new Vector3().subVectors(target, nozzle).normalize();
    const { lamp } = blowlamp;
    lamp.quaternion.copy(new Quaternion().setFromUnitVectors(alongJet, direction));
    const nozzleOffset = new Vector3(Burner.nozzle, Burner.height, 0).applyQuaternion(lamp.quaternion);
    lamp.position.copy(nozzle).sub(nozzleOffset);
    return direction;
}
