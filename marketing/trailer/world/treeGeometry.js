import { CylinderGeometry, Float32BufferAttribute } from 'three';
import { buildCanopy } from './canopyGeometry.js';
import { createRandom } from './random.js';
import { smoothstep } from './noise.js';
import { Detail, TreeKinds, farBlobGrowth } from './treeKinds.js';

const Shading = { darkest: 0.45, trunkSides: 7 };

function paintShade(geometry, bottom, top) {
    const positions = geometry.getAttribute('position');
    const shades = [];
    for (let index = 0; index < positions.count; index += 1) {
        const lift = smoothstep(bottom, top, positions.getY(index));
        const shade = Shading.darkest + (1 - Shading.darkest) * lift;
        shades.push(shade, shade, shade);
    }
    geometry.setAttribute('color', new Float32BufferAttribute(shades, 3));
    return geometry;
}

function paintSolid(geometry, colour) {
    const count = geometry.getAttribute('position').count;
    const colours = Array.from({ length: count }, () => [colour.r, colour.g, colour.b]).flat();
    geometry.setAttribute('color', new Float32BufferAttribute(colours, 3));
    return geometry;
}

export function canopyGeometryOf(kindName, detailName, seed) {
    const kind = TreeKinds[kindName];
    const { canopy } = kind;
    const isFar = detailName === 'far';
    const growth = isFar ? farBlobGrowth : 1;
    const blobRadius = canopy.blobRadius.map((radius) => radius * growth);
    const blobCount = canopy.blobCount[detailName];
    const geometry = buildCanopy(createRandom(seed), { ...canopy, blobRadius, blobCount, detail: Detail[detailName] });
    const { centre, extent } = canopy;
    return paintShade(geometry, centre.y - extent.y * 2, centre.y + extent.y * 2);
}

export function trunkGeometryOf(kindName) {
    const { trunk, bark } = TreeKinds[kindName];
    const geometry = new CylinderGeometry(trunk.topRadius, trunk.bottomRadius, trunk.height, Shading.trunkSides);
    geometry.translate(0, trunk.height / 2, 0);
    return paintSolid(geometry, bark);
}
