import { DoubleSide, Mesh, MeshBasicMaterial, PlaneGeometry } from 'three';
import { paintTexture } from '../textures/canvasTexture.js';
import { between, createRandom } from './random.js';

const Canvas = { width: 1024, height: 256 };
const Crowns = { perTree: 9, spread: 0.55 };

function paintCrown(context, random, centre, height) {
    const top = Canvas.height * (1 - height);
    for (let clump = 0; clump < Crowns.perTree; clump += 1) {
        const radius = between(random, 0.1, 0.2) * Canvas.height * height;
        const x = centre + between(random, -Crowns.spread, Crowns.spread) * radius * 2.2;
        const y = between(random, top + radius * 0.6, Canvas.height * 0.72);
        context.beginPath();
        context.arc(x, y, radius, 0, Math.PI * 2);
        context.fill();
    }
    context.fillRect(centre - 3, Canvas.height * 0.6, 6, Canvas.height * 0.4);
}

function paintTreeline(context, { seed, trees, tallest }) {
    const random = createRandom(seed);
    context.clearRect(0, 0, Canvas.width, Canvas.height);
    context.fillStyle = 'white';
    for (let tree = 0; tree < trees; tree += 1) {
        const centre = ((tree + between(random, 0.1, 0.9)) / trees) * Canvas.width;
        paintCrown(context, random, centre, between(random, tallest * 0.65, tallest));
    }
    context.fillRect(0, Canvas.height * 0.82, Canvas.width, Canvas.height * 0.18);
}

export function createTreeline({ width, height, colour, seed, trees, place: [x, z], turn = 0, tallest = 0.95 }) {
    const map = paintTexture(Canvas.width, Canvas.height, (context) => paintTreeline(context, { seed, trees, tallest }));
    const material = new MeshBasicMaterial({ color: colour, alphaMap: map, alphaTest: 0.5, side: DoubleSide });
    const treeline = new Mesh(new PlaneGeometry(width, height), material);
    treeline.position.set(x, height / 2, z);
    treeline.rotation.set(0, turn, 0);
    return treeline;
}
