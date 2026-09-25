import { BoxGeometry, CylinderGeometry, Group, LatheGeometry, Mesh, Vector2 } from 'three';
import { mergeGeometries } from 'three/addons/utils/BufferGeometryUtils.js';
import { Saloon } from './saloonShapes.js';

const Radiator = { front: 2.02, bottom: 0.56, height: 0.56, width: 0.6, depth: 0.1, flutes: 9 };
const Headlamp = { forward: 2.12, height: 0.98, apart: 0.46 };
const lampProfile = [[0, -0.02], [0.08, -0.018], [0.13, 0.02], [0.15, 0.08], [0.155, 0.12], [0.14, 0.13], [0, 0.13]];
const Bumper = { front: 2.25, rear: -2.55, height: 0.38, width: 1.62, bar: 0.07 };

function radiatorShell(chrome, grille) {
    const shell = new Mesh(new BoxGeometry(Radiator.depth, Radiator.height, Radiator.width), chrome);
    shell.position.set(Radiator.front, Radiator.bottom + Radiator.height / 2, 0);
    const core = new Mesh(new BoxGeometry(0.02, Radiator.height * 0.86, Radiator.width * 0.8), grille);
    core.position.set(Radiator.front + Radiator.depth / 2, Radiator.bottom + Radiator.height * 0.47, 0);
    const fluteSpacing = Radiator.width / Radiator.flutes;
    const flutes = Array.from({ length: Radiator.flutes }, (unused, index) => {
        const flute = new CylinderGeometry(fluteSpacing * 0.42, fluteSpacing * 0.42, 0.07, 8);
        flute.translate(Radiator.front, Radiator.bottom + Radiator.height + 0.03, (index - (Radiator.flutes - 1) / 2) * fluteSpacing);
        return flute;
    });
    const fluting = new Mesh(mergeGeometries(flutes), chrome);
    return [shell, core, fluting];
}

function headlamps(chrome, lens) {
    const bowl = new LatheGeometry(lampProfile.map(([radius, depth]) => new Vector2(radius, depth)), 24);
    bowl.rotateZ(-Math.PI / 2);
    return [-1, 1].flatMap((side) => {
        const lamp = new Mesh(bowl, chrome);
        lamp.position.set(Headlamp.forward - 0.13, Headlamp.height, side * Headlamp.apart);
        const glass = new Mesh(new CylinderGeometry(0.135, 0.135, 0.01, 24), lens);
        glass.rotation.set(0, 0, Math.PI / 2);
        glass.position.set(Headlamp.forward + 0.005, Headlamp.height, side * Headlamp.apart);
        return [lamp, glass];
    });
}

function bumpers(chrome) {
    return [Bumper.front, Bumper.rear].map((x) => {
        const bar = new Mesh(new BoxGeometry(Bumper.bar, Bumper.bar * 1.4, Bumper.width), chrome);
        bar.position.set(x, Bumper.height, 0);
        return bar;
    });
}

function spareWheel(wheel) {
    const spare = wheel.clone();
    spare.rotation.set(0, Math.PI / 2, 0);
    spare.position.set(-2.48, Saloon.wheelRadius + 0.3, 0);
    return spare;
}

export function buildSaloonTrim({ chrome, grille, lens }, wheel) {
    const trim = new Group();
    trim.add(...radiatorShell(chrome, grille), ...headlamps(chrome, lens), ...bumpers(chrome), spareWheel(wheel));
    return trim;
}
