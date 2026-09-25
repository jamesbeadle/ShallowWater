import { BoxGeometry, CylinderGeometry, Group, Mesh, MeshStandardMaterial, PointLight } from 'three';
import { srgb } from '../world/colours.js';

const Lamp = { postHeight: 3.4, headSize: 0.42, glow: srgb(1.0, 0.72, 0.38) };
const paint = new MeshStandardMaterial({ color: srgb(0.07, 0.08, 0.07), roughness: 0.5, metalness: 0.4 });

export function buildGasLamp({ isLit = true, lightIntensity = 0, lightDistance = 18 } = {}) {
    const lamp = new Group();
    const post = new Mesh(new CylinderGeometry(0.06, 0.1, Lamp.postHeight, 8), paint);
    post.position.setY(Lamp.postHeight / 2);
    const glass = new MeshStandardMaterial({ color: srgb(0.2, 0.18, 0.14), emissive: Lamp.glow, emissiveIntensity: isLit ? 6 : 0, roughness: 0.2 });
    const head = new Mesh(new BoxGeometry(Lamp.headSize, Lamp.headSize * 1.2, Lamp.headSize), glass);
    head.position.setY(Lamp.postHeight + Lamp.headSize * 0.6);
    const cap = new Mesh(new CylinderGeometry(0.05, Lamp.headSize * 0.75, 0.2, 4), paint);
    cap.position.setY(Lamp.postHeight + Lamp.headSize * 1.3);
    lamp.add(post, head, cap);
    if (lightIntensity) {
        const light = new PointLight(Lamp.glow, lightIntensity, lightDistance, 1.6);
        light.position.setY(Lamp.postHeight + Lamp.headSize * 0.6);
        lamp.add(light);
    }
    return lamp;
}
