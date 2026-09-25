import { Float32BufferAttribute, Vector3 } from 'three';

export function applyWorldUvs(geometry, metresPerRepeat) {
    const positions = geometry.getAttribute('position');
    const normals = geometry.getAttribute('normal');
    const uvs = [];
    for (let index = 0; index < positions.count; index += 1) {
        const point = new Vector3().fromBufferAttribute(positions, index);
        const facing = new Vector3().fromBufferAttribute(normals, index);
        const isFloor = Math.abs(facing.y) > Math.max(Math.abs(facing.x), Math.abs(facing.z));
        const isSideways = Math.abs(facing.x) > Math.abs(facing.z);
        const horizontal = isSideways ? point.z : point.x;
        uvs.push((isFloor ? point.x : horizontal) / metresPerRepeat, (isFloor ? point.z : point.y) / metresPerRepeat);
    }
    geometry.setAttribute('uv', new Float32BufferAttribute(uvs, 2));
    return geometry;
}
