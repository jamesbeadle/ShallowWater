import { CylinderGeometry, Group, LatheGeometry, Mesh, TorusGeometry, Vector2 } from 'three';
import { Saloon } from './saloonShapes.js';

const Tyre = { tube: 0.085, sides: 12, around: 36 };
const hubcapProfile = [[0, 0.075], [0.06, 0.07], [0.1, 0.05], [0.12, 0.03], [0.12, 0.02]];

export function buildWheel({ rubber, paint, chrome }) {
    const wheel = new Group();
    const tyre = new Mesh(new TorusGeometry(Saloon.wheelRadius - Tyre.tube, Tyre.tube, Tyre.sides, Tyre.around), rubber);
    const disc = new Mesh(new CylinderGeometry(Saloon.wheelRadius - Tyre.tube * 1.2, Saloon.wheelRadius - Tyre.tube * 1.2, 0.1, 28), paint);
    disc.rotation.set(Math.PI / 2, 0, 0);
    const hubcap = new Mesh(new LatheGeometry(hubcapProfile.map(([radius, height]) => new Vector2(radius, height)), 24), chrome);
    hubcap.rotation.set(Math.PI / 2, 0, 0);
    wheel.add(tyre, disc, hubcap);
    wheel.traverse((part) => Object.assign(part, { castShadow: true }));
    return wheel;
}

export function mountWheels(wheel) {
    return [Saloon.frontAxle, Saloon.rearAxle].flatMap((axle) => [-1, 1].map((side) => {
        const mounted = wheel.clone();
        mounted.position.set(axle, Saloon.wheelRadius, side * Saloon.track);
        mounted.rotation.set(0, side > 0 ? 0 : Math.PI, 0);
        return mounted;
    }));
}
