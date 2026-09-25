import { CylinderGeometry, Group, Mesh, MeshStandardMaterial, PointLight } from 'three';
import { srgb } from '../world/colours.js';

const Brazier = { radius: 0.32, height: 0.55, legHeight: 0.45 };
const Embers = srgb(1.0, 0.42, 0.12);

export function buildBrazier(lightIntensity) {
    const brazier = new Group();
    const ironwork = new MeshStandardMaterial({ color: srgb(0.08, 0.06, 0.05), roughness: 0.7, metalness: 0.5, wireframe: true });
    const basket = new Mesh(new CylinderGeometry(Brazier.radius, Brazier.radius * 0.8, Brazier.height, 12, 1, true), ironwork);
    basket.position.setY(Brazier.legHeight + Brazier.height / 2);
    const burning = new MeshStandardMaterial({ color: srgb(0.2, 0.05, 0.02), emissive: Embers, emissiveIntensity: 4 });
    const coals = new Mesh(new CylinderGeometry(Brazier.radius * 0.95, Brazier.radius * 0.7, Brazier.height * 0.8, 12), burning);
    coals.position.setY(Brazier.legHeight + Brazier.height * 0.45);
    const light = new PointLight(Embers, lightIntensity, 14, 1.8);
    light.position.setY(Brazier.legHeight + Brazier.height + 0.3);
    brazier.add(basket, coals, light);
    return { brazier, light, coals };
}

export function flicker(fire, baseIntensity, time) {
    const wobble = 0.8 + 0.12 * Math.sin(time * 17.3) + 0.08 * Math.sin(time * 29.1 + 1.7);
    const { light } = fire;
    light.intensity = baseIntensity * wobble;
}
