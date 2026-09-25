import { BufferGeometry, Float32BufferAttribute, Mesh } from 'three';
import { centreZ } from './canal.js';
import { createTerrainSampler } from './terrainSampler.js';

const Spacing = [{ within: 16, step: 0.6 }, { within: 60, step: 2 }, { within: 200, step: 6 }, { within: Infinity, step: 14 }];

function stepAt(across) {
    const distance = Math.abs(across);
    return Spacing.find((band) => distance < band.within).step;
}

function acrossSamples(from, to) {
    const samples = [];
    for (let across = from; across <= to; across += stepAt(across)) {
        samples.push(across);
    }
    return samples;
}

function alongSamples(from, to, step) {
    const count = Math.floor((to - from) / step) + 1;
    return Array.from({ length: count }, (unused, index) => from + index * step);
}

function gridIndices(columns, rows) {
    const indices = [];
    for (let column = 0; column < columns - 1; column += 1) {
        for (let row = 0; row < rows - 1; row += 1) {
            const corner = column * rows + row;
            indices.push(corner, corner + 1, corner + rows, corner + 1, corner + rows + 1, corner + rows);
        }
    }
    return indices;
}

export function buildCanalTerrain({ along, across, heightAt, colourAt, material }) {
    const xs = alongSamples(along.from, along.to, along.step);
    const acrosses = acrossSamples(across.from, across.to);
    const positions = [];
    const colours = [];
    xs.forEach((x) => acrosses.forEach((offset) => {
        positions.push(x, heightAt(x, offset), centreZ(x) + offset);
        const colour = colourAt(x, offset);
        colours.push(colour.r, colour.g, colour.b);
    }));
    const geometry = new BufferGeometry();
    geometry.setAttribute('position', new Float32BufferAttribute(positions, 3));
    geometry.setAttribute('color', new Float32BufferAttribute(colours, 3));
    geometry.setIndex(gridIndices(xs.length, acrosses.length));
    geometry.computeVertexNormals();
    const mesh = new Mesh(geometry, material);
    mesh.receiveShadow = true;
    return { mesh, groundAt: createTerrainSampler(heightAt, xs, acrosses) };
}
