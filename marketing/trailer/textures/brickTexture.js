import { createRandom, spread } from '../world/random.js';
import { paintTexture, rgb } from './canvasTexture.js';

const Courses = { perTile: 24, bricksPerRow: 8, mortar: 2 };

function brickShade(random, tone) {
    const variation = spread(random, 22);
    const burnt = random() < 0.08 ? -45 : 0;
    return rgb(tone[0] + variation + burnt, tone[1] + variation * 0.7 + burnt, tone[2] + variation * 0.5 + burnt * 0.6);
}

function layCourse(context, random, row, size, tone) {
    const courseHeight = size / Courses.perTile;
    const brickLength = size / Courses.bricksPerRow;
    const offset = row % 2 === 0 ? 0 : brickLength / 2;
    for (let brick = -1; brick <= Courses.bricksPerRow; brick += 1) {
        context.fillStyle = brickShade(random, tone);
        const x = brick * brickLength + offset;
        context.fillRect(x, row * courseHeight, brickLength - Courses.mortar, courseHeight - Courses.mortar);
    }
}

export function createBrickTexture({ seed = 5, tone = [150, 72, 50], mortar = [120, 110, 96] } = {}) {
    const random = createRandom(seed);
    return paintTexture(512, 512, (context, size) => {
        context.fillStyle = rgb(...mortar);
        context.fillRect(0, 0, size, size);
        for (let row = 0; row < Courses.perTile; row += 1) {
            layCourse(context, random, row, size, tone);
        }
    });
}
