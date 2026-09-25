import { DirectionalLight, HemisphereLight, Vector3 } from 'three';

const Shadow = { mapSize: 2048, distance: 600, near: 50, far: 1400, bias: -0.0004, normalBias: 0.06, radius: 3 };

export function sunDirectionAt(elevationDegrees, azimuthDegrees) {
    const elevation = (elevationDegrees * Math.PI) / 180;
    const azimuth = (azimuthDegrees * Math.PI) / 180;
    const level = Math.cos(elevation);
    return new Vector3(level * Math.sin(azimuth), Math.sin(elevation), -level * Math.cos(azimuth));
}

export function createSunlight({ colour, intensity, direction, focus, span }) {
    const sun = new DirectionalLight(colour, intensity);
    sun.position.copy(direction).multiplyScalar(Shadow.distance).add(focus);
    const aim = sun.target;
    aim.position.copy(focus);
    sun.castShadow = true;
    const { shadow } = sun;
    shadow.mapSize.set(Shadow.mapSize, Shadow.mapSize);
    Object.assign(shadow, { bias: Shadow.bias, normalBias: Shadow.normalBias, radius: Shadow.radius });
    const shadowCamera = shadow.camera;
    Object.assign(shadowCamera, { left: -span, right: span, top: span, bottom: -span, near: Shadow.near, far: Shadow.far });
    shadowCamera.updateProjectionMatrix();
    return [sun, aim];
}

export function createSkyFill({ sky, ground, intensity }) {
    return new HemisphereLight(sky, ground, intensity);
}
