import { CylinderGeometry, Group, Mesh, MeshStandardMaterial, PointLight, TorusGeometry } from 'three';
import { srgb } from '../world/colours.js';

const Lantern = { radius: 0.075, height: 0.2, glow: srgb(1.0, 0.7, 0.36) };

export function buildHandLantern(lightIntensity) {
    const lantern = new Group();
    const frame = new MeshStandardMaterial({ color: srgb(0.08, 0.07, 0.06), roughness: 0.5, metalness: 0.6 });
    const flame = new MeshStandardMaterial({ emissive: Lantern.glow, emissiveIntensity: 9, color: srgb(0.3, 0.25, 0.2) });
    const glass = new Mesh(new CylinderGeometry(Lantern.radius, Lantern.radius, Lantern.height, 12), flame);
    const cap = new Mesh(new CylinderGeometry(0.02, Lantern.radius * 1.15, 0.08, 12), frame);
    cap.position.setY(Lantern.height / 2 + 0.04);
    const handle = new Mesh(new TorusGeometry(0.06, 0.008, 6, 16, Math.PI), frame);
    handle.position.setY(Lantern.height / 2 + 0.09);
    const light = new PointLight(Lantern.glow, lightIntensity, 12, 1.7);
    lantern.add(glass, cap, handle, light);
    return lantern;
}
