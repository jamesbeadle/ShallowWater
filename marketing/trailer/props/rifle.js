import { BoxGeometry, CylinderGeometry, ExtrudeGeometry, Group, Mesh, MeshStandardMaterial, Shape } from 'three';
import { srgb } from '../world/colours.js';

const walnut = new MeshStandardMaterial({ color: srgb(0.3, 0.17, 0.08), roughness: 0.45 });
const steel = new MeshStandardMaterial({ color: srgb(0.06, 0.06, 0.065), roughness: 0.35, metalness: 0.85 });

function stockShape() {
    const stock = new Shape();
    stock.moveTo(0, 0);
    stock.lineTo(0.02, -0.14);
    stock.lineTo(0.3, -0.07);
    stock.lineTo(0.42, -0.035);
    stock.lineTo(1.02, -0.03);
    stock.lineTo(1.02, 0.01);
    stock.lineTo(0.36, 0.01);
    stock.lineTo(0.26, -0.02);
    stock.lineTo(0.0, 0.02);
    return stock;
}

function barrelPart(length, radius, x) {
    const barrel = new Mesh(new CylinderGeometry(radius, radius, length, 10), steel);
    barrel.rotateZ(Math.PI / 2);
    barrel.position.set(x + length / 2, 0.02, 0);
    return barrel;
}

export function buildRifle({ hasScope = false } = {}) {
    const rifle = new Group();
    const carving = { depth: 0.045, bevelEnabled: true, bevelSize: 0.008, bevelThickness: 0.008, bevelSegments: 2 };
    const stock = new Mesh(new ExtrudeGeometry(stockShape(), carving), walnut);
    stock.position.setZ(-0.022);
    const action = new Mesh(new BoxGeometry(0.2, 0.05, 0.035), steel);
    action.position.set(0.5, 0.02, 0);
    rifle.add(stock, action, barrelPart(0.25, 0.012, 1.0), barrelPart(0.62, 0.011, 0.58));
    if (hasScope) {
        const scope = barrelPart(0.36, 0.019, 0.34);
        scope.position.setY(0.085);
        rifle.add(scope);
    }
    rifle.traverse((part) => Object.assign(part, { castShadow: true }));
    return rifle;
}
