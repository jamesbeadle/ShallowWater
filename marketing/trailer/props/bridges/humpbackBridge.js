import { ExtrudeGeometry, Group, Mesh, Path, Shape } from 'three';
import { applyWorldUvs } from '../../world/worldUvs.js';

const Arch = { segments: 24 };
const Parapet = { height: 1.05, thickness: 0.36, coping: 0.12 };

function deckHeightAt(plan, x) {
    const distance = Math.min(Math.abs(x) / plan.halfLength, 1);
    return plan.approachHeight + (plan.crownHeight - plan.approachHeight) * (1 - distance * distance);
}

function deckOutline(plan, lift) {
    const outline = new Shape();
    outline.moveTo(-plan.halfLength, plan.footing);
    for (let step = 0; step <= Arch.segments; step += 1) {
        const x = -plan.halfLength + (2 * plan.halfLength * step) / Arch.segments;
        outline.lineTo(x, deckHeightAt(plan, x) + lift);
    }
    outline.lineTo(plan.halfLength, plan.footing);
    return outline;
}

function archOpening(plan) {
    const opening = new Path();
    opening.moveTo(plan.archCentre - plan.archHalfSpan, plan.footing);
    opening.absellipse(plan.archCentre, plan.springing, plan.archHalfSpan, plan.archRise, Math.PI, 0, true);
    opening.lineTo(plan.archCentre + plan.archHalfSpan, plan.footing);
    return opening;
}

function extruded(shape, depth, material, offset) {
    const geometry = new ExtrudeGeometry(shape, { depth, bevelEnabled: false, curveSegments: Arch.segments });
    geometry.translate(0, 0, offset);
    const mesh = new Mesh(applyWorldUvs(geometry, 2.2), material);
    mesh.castShadow = true;
    mesh.receiveShadow = true;
    return mesh;
}

function parapetShape(plan) {
    const band = deckOutline({ ...plan, footing: 0 }, Parapet.height);
    const shape = new Shape(band.getPoints().filter((point) => point.y > 0.01));
    const lower = deckOutline({ ...plan, footing: 0 }, 0).getPoints().filter((point) => point.y > 0.01).reverse();
    lower.forEach((point) => shape.lineTo(point.x, point.y));
    return shape;
}

export function buildHumpbackBridge(plan, materials) {
    const bridge = new Group();
    const body = deckOutline(plan, 0);
    body.holes.push(archOpening(plan));
    bridge.add(extruded(body, plan.width, materials.brick, -plan.width / 2));
    const parapet = parapetShape(plan);
    [-1, 1].forEach((side) => {
        const offset = side * (plan.width / 2) - (side > 0 ? Parapet.thickness : 0);
        bridge.add(extruded(parapet, Parapet.thickness, materials.brick, offset));
    });
    return bridge;
}
