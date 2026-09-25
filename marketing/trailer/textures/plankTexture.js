import { between, createRandom } from '../world/random.js';
import { paintTexture, rgb } from './canvasTexture.js';

const Boards = { size: 512, count: 5, grainLines: 26, stains: 70 };

function paintBoard(context, random, board, size, tone) {
    const width = size / Boards.count;
    const shade = between(random, -18, 14);
    context.fillStyle = rgb(tone[0] + shade, tone[1] + shade * 0.8, tone[2] + shade * 0.6);
    context.fillRect(board * width, 0, width - 3, size);
    context.strokeStyle = 'rgba(10, 6, 3, 0.35)';
    for (let line = 0; line < Boards.grainLines; line += 1) {
        const x = board * width + random() * width;
        context.lineWidth = between(random, 0.5, 2);
        context.beginPath();
        context.moveTo(x, 0);
        context.bezierCurveTo(x + between(random, -8, 8), size * 0.3, x + between(random, -8, 8), size * 0.7, x, size);
        context.stroke();
    }
}

export function createPlankTexture({ tone = [70, 46, 28], seed = 31 } = {}) {
    const random = createRandom(seed);
    return paintTexture(Boards.size, Boards.size, (context, size) => {
        context.fillStyle = 'rgb(8, 6, 4)';
        context.fillRect(0, 0, size, size);
        for (let board = 0; board < Boards.count; board += 1) {
            paintBoard(context, random, board, size, tone);
        }
        for (let stain = 0; stain < Boards.stains; stain += 1) {
            context.fillStyle = `rgba(6, 5, 4, ${between(random, 0.1, 0.5)})`;
            context.beginPath();
            context.ellipse(random() * size, random() * size, between(random, 5, 50), between(random, 5, 30), random() * Math.PI, 0, Math.PI * 2);
            context.fill();
        }
    });
}
