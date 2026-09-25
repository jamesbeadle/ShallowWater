import { CylinderGeometry, Group, InstancedMesh, Matrix4, Mesh, MeshStandardMaterial, Quaternion, TorusGeometry, Vector3 } from 'three';
import { srgb } from '../world/colours.js';

const Link = { radius: 0.09, thickness: 0.026, pitch: 0.15 };
const Post = { radius: 0.2, height: 2.2 };
const iron = new MeshStandardMaterial({ color: srgb(0.12, 0.1, 0.09), roughness: 0.55, metalness: 0.75 });

function sagAt(fraction, sag) {
    return -4 * sag * fraction * (1 - fraction);
}

export function chainPoints(start, end, sag, count) {
    return Array.from({ length: count }, (unused, index) => {
        const fraction = index / (count - 1);
        return new Vector3().lerpVectors(start, end, fraction).setY(start.y + sagAt(fraction, sag));
    });
}

export function buildChain(start, end, sag) {
    const count = Math.round(start.distanceTo(end) / Link.pitch);
    const links = new InstancedMesh(new TorusGeometry(Link.radius, Link.thickness, 6, 12), iron, count);
    links.castShadow = true;
    const posts = new Group();
    [start, end].forEach((point) => {
        const post = new Mesh(new CylinderGeometry(Post.radius * 0.8, Post.radius, Post.height, 12), iron);
        post.position.set(point.x, point.y - Post.height / 2 + 0.15, point.z);
        post.castShadow = true;
        posts.add(post);
    });
    return { links, posts, count };
}

export function hangLinks(chain, points) {
    const turn = new Quaternion();
    const along = new Vector3();
    points.forEach((point, index) => {
        const next = points[Math.min(index + 1, points.length - 1)];
        along.subVectors(next, point).normalize();
        turn.setFromUnitVectors(new Vector3(1, 0, 0), along.lengthSq() ? along : new Vector3(1, 0, 0));
        turn.multiply(new Quaternion().setFromAxisAngle(new Vector3(1, 0, 0), (index % 2) * (Math.PI / 2)));
        chain.links.setMatrixAt(index, new Matrix4().compose(point, turn, new Vector3(1, 1, 1)));
    });
    const { instanceMatrix } = chain.links;
    instanceMatrix.needsUpdate = true;
    chain.links.computeBoundingSphere();
}
