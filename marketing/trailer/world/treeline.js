import { ClampToEdgeWrapping, DoubleSide, Mesh, MeshBasicMaterial, PlaneGeometry } from 'three';
import { paintTexture } from '../textures/canvasTexture.js';
import { between, createRandom } from './random.js';

const Canvas = { width: 2048, height: 512 };
const Leafage = { pixelsPerLeaf: 55, smallest: 0.014, largest: 0.036, trunkMetres: 0.45, spread: 0.7, clustering: 0.75, edgeRoom: 0.75 };

function crownsOf({ random, trees, tallest, aspect, width }) {
    return Array.from({ length: trees }, (unused, tree) => {
        const height = between(random, tallest * 0.6, tallest);
        const crownHeight = height * 0.66;
        const x = (tree + between(random, 0.2, 0.8)) / trees;
        const halfWidth = Math.min(crownHeight * aspect * between(random, 0.75, 1.1), x * Leafage.edgeRoom, (1 - x) * Leafage.edgeRoom);
        return { x, top: 1 - height, crownHeight, halfWidth, trunk: Leafage.trunkMetres / width };
    });
}

function paintCrown(context, random, crown) {
    const middleX = crown.x * Canvas.width;
    const middleY = (crown.top + crown.crownHeight / 2) * Canvas.height;
    context.fillRect(middleX - (crown.trunk * Canvas.width) / 2, middleY, crown.trunk * Canvas.width, Canvas.height - middleY);
    const leaves = Math.round((crown.halfWidth * Canvas.width * crown.crownHeight * Canvas.height) / 2 / Leafage.pixelsPerLeaf);
    for (let leaf = 0; leaf < leaves; leaf += 1) {
        const angle = random() * Math.PI * 2;
        const reach = random() ** Leafage.clustering;
        const radius = between(random, Leafage.smallest, Leafage.largest) * Canvas.height * (1.3 - reach * Leafage.spread);
        const x = middleX + Math.cos(angle) * reach * crown.halfWidth * Canvas.width;
        const y = middleY + Math.sin(angle) * reach * (crown.crownHeight / 2) * Canvas.height;
        context.beginPath();
        context.arc(x, y, radius, 0, Math.PI * 2);
        context.fill();
    }
}

function paintTreeline(context, { seed, trees, tallest, ground, aspect, width }) {
    const random = createRandom(seed);
    context.clearRect(0, 0, Canvas.width, Canvas.height);
    context.fillStyle = 'white';
    crownsOf({ random, trees, tallest, aspect, width }).forEach((crown) => paintCrown(context, random, crown));
    context.fillRect(0, Canvas.height * (1 - ground), Canvas.width, Canvas.height * ground);
}

export function createTreeline({ width, height, colour, seed, trees, place: [x, z], turn = 0, tallest = 0.95, ground = 0 }) {
    const plan = { seed, trees, tallest, ground, aspect: height / width, width };
    const painted = paintTexture(Canvas.width, Canvas.height, (context) => paintTreeline(context, plan));
    const map = Object.assign(painted, { wrapS: ClampToEdgeWrapping, wrapT: ClampToEdgeWrapping });
    const material = new MeshBasicMaterial({ color: colour, alphaMap: map, transparent: true, depthWrite: false, side: DoubleSide });
    const treeline = new Mesh(new PlaneGeometry(width, height), material);
    treeline.position.set(x, height / 2, z);
    treeline.rotation.set(0, turn, 0);
    return treeline;
}
