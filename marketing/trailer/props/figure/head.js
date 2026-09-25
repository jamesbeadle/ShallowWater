import { ConeGeometry, CylinderGeometry, Group, LatheGeometry, Mesh, SphereGeometry, Vector2 } from 'three';
import { Proportions } from './proportions.js';

const radius = Proportions.headRadius;

function skull(skin) {
    const geometry = new SphereGeometry(radius, 20, 16);
    geometry.scale(0.88, 1.1, 1.0);
    const mesh = new Mesh(geometry, skin);
    const nose = new Mesh(new ConeGeometry(0.018, 0.05, 8), skin);
    nose.rotateX(Math.PI / 2);
    nose.position.set(0, -0.01, radius * 0.98);
    mesh.add(nose);
    return mesh;
}

function flatCap(material) {
    const crownGeometry = new SphereGeometry(radius * 1.1, 20, 10, 0, Math.PI * 2, 0, Math.PI / 2);
    crownGeometry.scale(0.92, 0.36, 1.22);
    crownGeometry.translate(0, 0, radius * 0.22);
    const crown = new Mesh(crownGeometry, material);
    const brimGeometry = new CylinderGeometry(radius * 0.8, radius * 0.8, 0.014, 16, 1, false, -Math.PI / 2, Math.PI);
    brimGeometry.scale(1.0, 1.0, 0.62);
    const brim = new Mesh(brimGeometry, material);
    brim.position.set(0, 0.004, radius * 0.72);
    brim.rotateX(0.22);
    crown.add(brim);
    crown.position.set(0, radius * 0.62, 0);
    crown.rotateX(0.2);
    return crown;
}

function homburg(material) {
    const crown = new Mesh(new CylinderGeometry(radius * 0.92, radius * 1.02, radius * 1.05, 18), material);
    const brim = new Mesh(new CylinderGeometry(radius * 1.6, radius * 1.6, 0.012, 20), material);
    brim.position.set(0, -radius * 0.48, 0);
    crown.add(brim);
    crown.position.set(0, radius * 0.95, 0);
    return crown;
}

function custodianHelmet(material) {
    const outline = [[0.001, 0.2], [0.05, 0.19], [0.085, 0.14], [0.105, 0.05], [0.112, -0.02], [0.14, -0.05], [0.001, -0.05]];
    const geometry = new LatheGeometry(outline.map(([x, y]) => new Vector2(x, y)), 20);
    geometry.scale(1, 1, 1.15);
    const helmet = new Mesh(geometry, material);
    helmet.position.set(0, radius * 0.65, 0);
    return helmet;
}

const headwearOf = { cap: flatCap, hat: homburg, helmet: custodianHelmet };

export function buildHead(clothing, wardrobe) {
    const head = new Group();
    head.add(skull(clothing.skin));
    const headwearKind = wardrobe.headwear ?? (wardrobe.hasHat ? 'hat' : 'cap');
    const headwear = headwearOf[headwearKind](clothing.cap);
    const isCovered = wardrobe.hasCap || wardrobe.hasHat || Boolean(wardrobe.headwear);
    if (isCovered) {
        head.add(headwear);
    }
    head.traverse((part) => Object.assign(part, { castShadow: true }));
    return head;
}
