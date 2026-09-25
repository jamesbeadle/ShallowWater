import { Vector3 } from 'three';

const Progressions = [0, 0.5, 1];
const Shake = { first: 1.7, second: 2.9, third: 4.3 };

export function easeInOut(progress) {
    const clamped = Math.min(Math.max(progress, 0), 1);
    return clamped * clamped * (3 - 2 * clamped);
}

export function easeOut(progress) {
    const clamped = Math.min(Math.max(progress, 0), 1);
    return 1 - (1 - clamped) ** 3;
}

function framingAt(path, progress) {
    const eased = path.easing ? path.easing(progress) : easeInOut(progress);
    const position = new Vector3().lerpVectors(path.from, path.to, eased);
    const target = new Vector3().lerpVectors(path.lookFrom, path.lookTo, eased);
    return { position, target };
}

export function glide(camera, path, progress) {
    const { position, target } = framingAt(path, progress);
    camera.position.copy(position);
    camera.lookAt(target);
}

export function viewpointsOf(path) {
    return Progressions.map((progress) => framingAt(path, progress));
}

export function progressOf(setting, shotTime) {
    const { shot } = setting;
    return shotTime / (shot.end - shot.start);
}

export function handheld(camera, time, amount) {
    const sway = Math.sin(time * Shake.first) + 0.5 * Math.sin(time * Shake.second + 1.3);
    const nod = Math.sin(time * Shake.second * 0.8 + 0.4) + 0.4 * Math.sin(time * Shake.third);
    camera.rotateY(sway * amount);
    camera.rotateX(nod * amount * 0.7);
}
