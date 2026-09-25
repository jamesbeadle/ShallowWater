import { BoxGeometry, CylinderGeometry, ExtrudeGeometry, Mesh, PlaneGeometry, Shape } from 'three';
import { mergeGeometries } from 'three/addons/utils/BufferGeometryUtils.js';
import { applyWorldUvs } from '../../world/worldUvs.js';

const Roof = { overhang: 0.3, thickness: 0.12 };
const Brickwork = { metresPerRepeat: 2.2 };
const Chimney = { width: 0.9, depth: 0.55, rise: 1.3, potRadius: 0.13, potHeight: 0.45 };

function shaded(mesh) {
    mesh.castShadow = true;
    mesh.receiveShadow = true;
    return mesh;
}

export function wallsWithGables({ width, depth, wallHeight, roofRise }, material) {
    const outline = new Shape();
    outline.moveTo(-depth / 2, 0);
    outline.lineTo(depth / 2, 0);
    outline.lineTo(depth / 2, wallHeight);
    outline.lineTo(0, wallHeight + roofRise);
    outline.lineTo(-depth / 2, wallHeight);
    const geometry = new ExtrudeGeometry(outline, { depth: width, bevelEnabled: false });
    geometry.rotateY(Math.PI / 2);
    geometry.translate(-width / 2, 0, 0);
    return shaded(new Mesh(applyWorldUvs(geometry, Brickwork.metresPerRepeat), material));
}

function roofSlope(width, depth, roofRise, side) {
    const slopeLength = Math.hypot(depth / 2, roofRise) + Roof.overhang;
    const geometry = new BoxGeometry(width + Roof.overhang * 2, Roof.thickness, slopeLength);
    geometry.translate(0, 0, (side * slopeLength) / 2);
    geometry.rotateX(side * Math.atan2(roofRise, depth / 2));
    return geometry;
}

export function roofOf({ width, depth, wallHeight, roofRise }, material) {
    const slopes = [-1, 1].map((side) => roofSlope(width, depth, roofRise, side));
    const geometry = mergeGeometries(slopes);
    geometry.translate(0, wallHeight + roofRise + Roof.thickness / 2, 0);
    return shaded(new Mesh(applyWorldUvs(geometry, 1.8), material));
}

export function chimneyStack({ wallHeight, roofRise }, x, materials) {
    const height = roofRise + Chimney.rise;
    const stackGeometry = applyWorldUvs(new BoxGeometry(Chimney.width, height, Chimney.depth), Brickwork.metresPerRepeat);
    const stack = shaded(new Mesh(stackGeometry, materials.brick));
    stack.position.set(x, wallHeight + height / 2, 0);
    [-0.2, 0.2].forEach((offset) => {
        const pot = shaded(new Mesh(new CylinderGeometry(Chimney.potRadius * 0.8, Chimney.potRadius, Chimney.potHeight, 8), materials.pot));
        pot.position.set(offset, height / 2 + Chimney.potHeight / 2, 0);
        stack.add(pot);
    });
    return stack;
}

export function windowPanes(placements, size) {
    const panes = placements.map(({ x, y, z, turn }) => {
        const pane = new PlaneGeometry(size.width, size.height);
        pane.rotateY(turn);
        pane.translate(x, y, z);
        return pane;
    });
    return mergeGeometries(panes);
}
