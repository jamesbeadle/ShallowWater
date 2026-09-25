import { DoubleSide, Mesh, MeshBasicMaterial, PlaneGeometry } from 'three';
import { paintTexture } from '../textures/canvasTexture.js';

function paintBust(context, width, height, hasHat) {
    context.fillStyle = 'black';
    context.beginPath();
    context.ellipse(width / 2, height * 0.34, width * 0.13, height * 0.13, 0, 0, Math.PI * 2);
    context.fill();
    context.fillRect(width * 0.44, height * 0.42, width * 0.12, height * 0.1);
    context.beginPath();
    context.moveTo(width * 0.12, height);
    context.quadraticCurveTo(width * 0.16, height * 0.52, width / 2, height * 0.5);
    context.quadraticCurveTo(width * 0.84, height * 0.52, width * 0.88, height);
    context.fill();
    if (hasHat) {
        context.fillRect(width * 0.28, height * 0.2, width * 0.44, height * 0.035);
        context.fillRect(width * 0.36, height * 0.08, width * 0.28, height * 0.13);
    }
}

export function buildWindowSilhouette({ width, height, hasHat = false }) {
    const texture = paintTexture(256, 256, (context, pixels) => {
        context.clearRect(0, 0, pixels, pixels);
        paintBust(context, pixels, pixels, hasHat);
    });
    const material = new MeshBasicMaterial({ map: texture, transparent: true, side: DoubleSide, color: 'black', fog: false });
    return new Mesh(new PlaneGeometry(width, height), material);
}
