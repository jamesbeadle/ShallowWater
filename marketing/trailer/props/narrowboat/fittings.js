import { CatmullRomCurve3, CylinderGeometry, Mesh, MeshStandardMaterial, TorusGeometry, TubeGeometry, Vector3 } from 'three';
import { srgb } from '../../world/colours.js';
import { Boat, Cabin, freeboard, sternX } from './dimensions.js';

const Brass = new MeshStandardMaterial({ color: srgb(0.78, 0.6, 0.3), roughness: 0.3, metalness: 0.9 });
const Iron = new MeshStandardMaterial({ color: srgb(0.04, 0.04, 0.04), roughness: 0.6, metalness: 0.3 });
const Timber = new MeshStandardMaterial({ color: srgb(0.36, 0.22, 0.12), roughness: 0.6 });

function shaded(mesh) {
    mesh.castShadow = true;
    return mesh;
}

export function chimney(cabinStart) {
    const pipe = shaded(new Mesh(new CylinderGeometry(0.075, 0.075, 0.7, 12), Iron));
    [0.1, 0.22, 0.34].forEach((height) => {
        const band = new Mesh(new TorusGeometry(0.078, 0.012, 6, 16), Brass);
        band.rotateX(Math.PI / 2);
        band.position.setY(height);
        pipe.add(band);
    });
    pipe.position.set(cabinStart + Cabin.length - 0.55, freeboard() + Cabin.height + 0.4, -0.28);
    return pipe;
}

export function chimneyTop(cabinStart) {
    return new Vector3(cabinStart + Cabin.length - 0.55, freeboard() + Cabin.height + 0.78, -0.28);
}

export function exhaust(holdStart) {
    const pipe = shaded(new Mesh(new CylinderGeometry(0.06, 0.07, 0.5, 10), Iron));
    pipe.position.set(holdStart - 0.5, freeboard() + Cabin.height + 0.12, 0.35);
    return pipe;
}

export function exhaustMouth(holdStart) {
    return new Vector3(holdStart - 0.5, freeboard() + Cabin.height + 0.4, 0.35);
}

export function tiller() {
    const post = new Vector3(sternX() - 0.18, freeboard() + 0.3, 0);
    const bend = new Vector3(sternX() - 0.1, freeboard() + 0.95, 0);
    const grip = new Vector3(sternX() + 0.85, freeboard() + 1.2, 0.05);
    const curve = new CatmullRomCurve3([post, bend, grip]);
    const bar = shaded(new Mesh(new TubeGeometry(curve, 20, 0.03, 8), Iron));
    const handle = shaded(new Mesh(new CylinderGeometry(0.035, 0.035, 0.34, 10), Timber));
    handle.position.copy(grip);
    handle.rotateZ(Math.PI / 2 - 0.3);
    bar.add(handle);
    return bar;
}

export function steeringSpot() {
    return new Vector3(sternX() + 0.28, freeboard() - 0.1, 0.1);
}

export function bowFender() {
    const fender = shaded(new Mesh(new CylinderGeometry(0.13, 0.13, 0.5, 10), new MeshStandardMaterial({ color: srgb(0.5, 0.45, 0.36), roughness: 1 })));
    fender.position.set(Boat.length / 2 + 0.05, 0.15, 0);
    return fender;
}
