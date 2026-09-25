import { BufferGeometry, Float32BufferAttribute, PlaneGeometry } from 'three';
import { mergeGeometries } from 'three/addons/utils/BufferGeometryUtils.js';

function ribbonIndices(steps) {
    const indices = [];
    for (let step = 0; step < steps; step += 1) {
        const corner = step * 2;
        indices.push(corner, corner + 2, corner + 1, corner + 1, corner + 2, corner + 3);
    }
    return indices;
}

export function canalRibbon({ centreZ, from, to, halfWidth, step }) {
    const positions = [];
    const steps = Math.ceil((to - from) / step);
    for (let index = 0; index <= steps; index += 1) {
        const x = from + index * step;
        const z = centreZ(x);
        positions.push(x, -(z - halfWidth), 0, x, -(z + halfWidth), 0);
    }
    const geometry = new BufferGeometry();
    geometry.setAttribute('position', new Float32BufferAttribute(positions, 3));
    geometry.setIndex(ribbonIndices(steps));
    geometry.computeVertexNormals();
    return geometry;
}

export function rectangleStrip({ xFrom, xTo, zFrom, zTo }) {
    const strip = new PlaneGeometry(xTo - xFrom, zTo - zFrom);
    strip.translate((xFrom + xTo) / 2, -(zFrom + zTo) / 2, 0);
    strip.deleteAttribute('uv');
    return strip;
}

export function joinStrips(strips) {
    const plain = strips.map((strip) => strip.index ? strip.toNonIndexed() : strip);
    plain.forEach((strip) => ['uv', 'normal'].forEach((name) => strip.deleteAttribute(name)));
    return mergeGeometries(plain);
}
