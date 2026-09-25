import { IcosahedronGeometry, Vector3 } from 'three';
import { mergeGeometries, mergeVertices } from 'three/addons/utils/BufferGeometryUtils.js';
import { between, spread } from './random.js';
import { valueNoise } from './noise.js';

const Lumpiness = { amount: 0.28, frequency: 1.3, foliageNormalWeight: 0.75 };

function lumpyBlob(random, radius, detail) {
    const faceted = new IcosahedronGeometry(radius, detail);
    faceted.deleteAttribute('normal');
    faceted.deleteAttribute('uv');
    const blob = mergeVertices(faceted);
    const positions = blob.getAttribute('position');
    const seed = random() * 100;
    for (let index = 0; index < positions.count; index += 1) {
        const point = new Vector3().fromBufferAttribute(positions, index);
        const lump = valueNoise(point.x * Lumpiness.frequency + seed, point.y * Lumpiness.frequency + point.z);
        point.multiplyScalar(1 + (lump - 0.5) * 2 * Lumpiness.amount);
        positions.setXYZ(index, point.x, point.y, point.z);
    }
    return blob;
}

function roundNormals(geometry, centre) {
    geometry.computeVertexNormals();
    const positions = geometry.getAttribute('position');
    const normals = geometry.getAttribute('normal');
    for (let index = 0; index < positions.count; index += 1) {
        const outward = new Vector3().fromBufferAttribute(positions, index).sub(centre).normalize();
        const surface = new Vector3().fromBufferAttribute(normals, index);
        surface.lerp(outward, Lumpiness.foliageNormalWeight).normalize();
        normals.setXYZ(index, surface.x, surface.y, surface.z);
    }
    return geometry;
}

export function buildCanopy(random, { centre, extent, blobCount, blobRadius, detail }) {
    const blobs = Array.from({ length: blobCount }, () => {
        const radius = between(random, blobRadius[0], blobRadius[1]);
        const blob = lumpyBlob(random, radius, detail);
        blob.translate(centre.x + spread(random, extent.x), centre.y + spread(random, extent.y), centre.z + spread(random, extent.z));
        return blob;
    });
    const canopy = mergeGeometries(blobs);
    return roundNormals(canopy, centre);
}
