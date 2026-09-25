import { BoxGeometry, CylinderGeometry, Group, Mesh, MeshStandardMaterial } from 'three';
import { srgb } from '../world/colours.js';
import { buildHeadlampBeam } from './headlampBeam.js';

const Wheel = { radius: 0.46, width: 0.26 };
const Glow = srgb(1.0, 0.82, 0.55);

function part(geometry, material, x, y, z) {
    const mesh = new Mesh(geometry, material);
    mesh.position.set(x, y, z);
    mesh.castShadow = true;
    mesh.receiveShadow = true;
    return mesh;
}

function lorryMaterials(paintColour) {
    return {
        paint: new MeshStandardMaterial({ color: paintColour, roughness: 0.45, metalness: 0.2 }),
        dark: new MeshStandardMaterial({ color: srgb(0.04, 0.04, 0.04), roughness: 0.8 }),
        chrome: new MeshStandardMaterial({ color: srgb(0.7, 0.7, 0.68), roughness: 0.2, metalness: 1 }),
        tarpaulin: new MeshStandardMaterial({ color: srgb(0.16, 0.17, 0.12), roughness: 0.95 }),
        lamp: new MeshStandardMaterial({ color: srgb(0.9, 0.85, 0.7), emissive: Glow, emissiveIntensity: 0, roughness: 0.2 }),
    };
}

function wheels(dark) {
    const tyre = new CylinderGeometry(Wheel.radius, Wheel.radius, Wheel.width, 16);
    tyre.rotateX(Math.PI / 2);
    return [[2.1, 0.95], [2.1, -0.95], [-1.9, 0.95], [-1.9, -0.95], [-1.9, 0.68], [-1.9, -0.68]].map(([x, z]) => part(tyre, dark, x, Wheel.radius, z));
}

function body(materials) {
    return [
        part(new BoxGeometry(6.6, 0.22, 1.5), materials.dark, -0.2, 0.78, 0),
        part(new BoxGeometry(1.3, 0.85, 1.25), materials.paint, 2.55, 1.35, 0),
        part(new BoxGeometry(0.08, 0.8, 0.9), materials.chrome, 3.22, 1.33, 0),
        part(new BoxGeometry(1.45, 1.55, 1.9), materials.paint, 1.25, 1.75, 0),
        part(new BoxGeometry(1.2, 0.42, 1.95), materials.dark, 1.25, 2.3, 0),
        part(new BoxGeometry(4.1, 0.25, 2.1), materials.paint, -1.6, 1.05, 0),
        part(new BoxGeometry(3.8, 1.1, 1.9), materials.tarpaulin, -1.6, 1.75, 0),
    ];
}

export function buildLorry({ paintColour = srgb(0.1, 0.18, 0.12), hasLights = false, beamStrength = 0.06 } = {}) {
    const materials = lorryMaterials(paintColour);
    const lorry = new Group();
    lorry.add(...body(materials), ...wheels(materials.dark));
    const { lamp } = materials;
    lamp.emissiveIntensity = hasLights ? 8 : 0;
    [0.72, -0.72].forEach((z) => {
        const headlamp = part(new CylinderGeometry(0.15, 0.15, 0.14, 14), materials.lamp, 3.05, 1.55, z);
        headlamp.rotateZ(Math.PI / 2);
        lorry.add(headlamp);
        if (hasLights) {
            const beam = buildHeadlampBeam(Glow, beamStrength);
            beam.position.set(3.12, 1.55, z);
            lorry.add(beam);
        }
    });
    return lorry;
}
