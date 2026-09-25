import { CylinderGeometry, Group, Mesh, MeshStandardMaterial, PointLight } from 'three';
import { buildHurricaneLamp, LampHang } from '../props/hurricaneLamp.js';
import { srgb } from '../world/colours.js';

const Chain = { length: 0.2, thickness: 0.004 };
const Light = { colour: srgb(1.0, 0.64, 0.3), intensity: 1.8, reach: 5, decay: 1.6 };
const Swing = { amplitude: 0.17, period: 2.9, phase: 0.8, beatKick: 0.05, kickDecay: 1.4 };

export function hangCabinLamp(pivot) {
    const swing = new Group();
    swing.position.copy(pivot);
    const links = new MeshStandardMaterial({ color: srgb(0.1, 0.09, 0.08), metalness: 0.8, roughness: 0.4 });
    const chain = new Mesh(new CylinderGeometry(Chain.thickness, Chain.thickness, Chain.length, 5), links);
    chain.position.set(0, -Chain.length / 2, 0);
    const { lamp, flame } = buildHurricaneLamp({ brightness: 7 });
    const { eye } = LampHang;
    lamp.position.set(0, -Chain.length - eye.y, 0);
    const light = new PointLight(Light.colour, Light.intensity, Light.reach, Light.decay);
    light.position.copy(lamp.position).add(LampHang.flame);
    swing.add(chain, lamp, light);
    return { swing, light, flame };
}

export function swingCabinLamp(cabinLamp, time, beats) {
    const kicks = beats.filter((beat) => beat <= time).reduce((total, beat) => total + Math.exp(-(time - beat) * Swing.kickDecay), 0);
    const phase = (time / Swing.period) * Math.PI * 2 + Swing.phase;
    const angle = (Swing.amplitude + Swing.beatKick * kicks) * Math.sin(phase);
    const { swing, light } = cabinLamp;
    swing.rotation.set(angle, 0, angle * 0.3);
    light.intensity = Light.intensity * (0.92 + 0.08 * Math.sin(time * 23.0) * Math.sin(time * 7.1));
}
