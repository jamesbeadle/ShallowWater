import { BufferGeometry, Float32BufferAttribute, Mesh } from 'three';

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

export function buildGridTerrain({ xFrom, xTo, zFrom, zTo, step, heightAt, colourAt, material }) {
    const columns = Math.floor((xTo - xFrom) / step) + 1;
    const rows = Math.floor((zTo - zFrom) / step) + 1;
    const positions = [];
    const colours = [];
    for (let column = 0; column < columns; column += 1) {
        for (let row = 0; row < rows; row += 1) {
            const x = xFrom + column * step;
            const z = zFrom + row * step;
            positions.push(x, heightAt(x, z), z);
            const colour = colourAt(x, z);
            colours.push(colour.r, colour.g, colour.b);
        }
    }
    const geometry = new BufferGeometry();
    geometry.setAttribute('position', new Float32BufferAttribute(positions, 3));
    geometry.setAttribute('color', new Float32BufferAttribute(colours, 3));
    geometry.setIndex(gridIndices(columns, rows));
    geometry.computeVertexNormals();
    const mesh = new Mesh(geometry, material);
    mesh.receiveShadow = true;
    return mesh;
}
