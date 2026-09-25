import { CylinderGeometry, Group, Mesh, MeshBasicMaterial, MeshStandardMaterial, TorusGeometry, Vector3 } from 'three';
import { mergeGeometries } from 'three/addons/utils/BufferGeometryUtils.js';
import { srgb } from '../world/colours.js';
import { turned, turnedGeometry } from './engine/turnedParts.js';

const Profiles = {
    font: [[0, 0], [0.07, 0], [0.074, 0.02], [0.07, 0.055], [0.03, 0.068], [0, 0.07]],
    globe: [[0.028, 0.07], [0.05, 0.095], [0.058, 0.14], [0.05, 0.185], [0.03, 0.215]],
    cap: [[0.034, 0.212], [0.05, 0.225], [0.042, 0.245], [0.016, 0.262], [0, 0.265]],
    flame: [[0, 0.098], [0.008, 0.106], [0.009, 0.118], [0.005, 0.132], [0, 0.142]],
};
const Guard = { wires: 4, radius: 0.066, bottom: 0.07, top: 0.215, thickness: 0.0028 };
export const LampHang = { eye: new Vector3(0, 0.33, 0), flame: new Vector3(0, 0.118, 0) };

function guardWires(material) {
    const height = Guard.top - Guard.bottom;
    const wires = Array.from({ length: Guard.wires }, (unused, index) => {
        const angle = (index / Guard.wires) * Math.PI * 2 + Math.PI / 4;
        const wire = new CylinderGeometry(Guard.thickness, Guard.thickness, height, 5);
        wire.translate(Math.cos(angle) * Guard.radius, Guard.bottom + height / 2, Math.sin(angle) * Guard.radius);
        return wire;
    });
    return new Mesh(mergeGeometries(wires), material);
}

function bail(material) {
    const loop = new Mesh(new TorusGeometry(0.062, 0.003, 6, 24, Math.PI), material);
    loop.position.set(0, 0.25, 0);
    return loop;
}

export function buildHurricaneLamp({ glow = srgb(1.0, 0.62, 0.28), brightness = 5 } = {}) {
    const lamp = new Group();
    const brass = new MeshStandardMaterial({ color: srgb(0.7, 0.5, 0.22), roughness: 0.3, metalness: 1 });
    const tin = new MeshStandardMaterial({ color: srgb(0.08, 0.075, 0.07), roughness: 0.5, metalness: 0.6 });
    const glassFinish = { roughness: 0.08, transparent: true, opacity: 0.35 };
    const glass = new MeshStandardMaterial({ color: glow, emissive: glow, emissiveIntensity: brightness * 0.06, ...glassFinish });
    const flame = new Mesh(turnedGeometry(Profiles.flame, 12), new MeshBasicMaterial({ color: glow.clone().multiplyScalar(brightness) }));
    lamp.add(turned(Profiles.font, brass), turned(Profiles.cap, tin), guardWires(tin), bail(tin), flame);
    lamp.add(new Mesh(turnedGeometry(Profiles.globe, 24), glass));
    return { lamp, glass, flame };
}
