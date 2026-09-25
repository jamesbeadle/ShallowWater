import { Frustum, Matrix4, PerspectiveCamera, Sphere, Vector3 } from 'three';

const Reach = { middleHeight: 8, radius: 12 };
const fieldOfViewMargin = 1.35;

function frustumFrom(lens, aspect, viewpoint) {
    const camera = new PerspectiveCamera(lens.fieldOfView * fieldOfViewMargin, aspect, lens.near, lens.far);
    camera.position.copy(viewpoint.position);
    camera.lookAt(viewpoint.target);
    camera.updateMatrixWorld();
    const projection = new Matrix4().multiplyMatrices(camera.projectionMatrix, camera.matrixWorldInverse);
    return new Frustum().setFromProjectionMatrix(projection);
}

export function frustumsAlong(lens, aspect, viewpoints) {
    return viewpoints.map((viewpoint) => frustumFrom(lens, aspect, viewpoint));
}

export function isSeenByAny(frustums, placement) {
    const centre = new Vector3(placement.x, placement.y + Reach.middleHeight * placement.scale, placement.z);
    const bounds = new Sphere(centre, Reach.radius * placement.scale);
    return frustums.some((frustum) => frustum.intersectsSphere(bounds));
}
