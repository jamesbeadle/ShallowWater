import { createRandom, spread } from '../world/random.js';
import { paintTexture, rgb } from './canvasTexture.js';

const Slates = { rows: 16, perRow: 10 };

export function createSlateTexture({ seed = 9, tone = [58, 60, 66] } = {}) {
    const random = createRandom(seed);
    return paintTexture(256, 256, (context, size) => {
        const rowHeight = size / Slates.rows;
        const slateWidth = size / Slates.perRow;
        for (let row = 0; row < Slates.rows; row += 1) {
            const offset = row % 2 === 0 ? 0 : slateWidth / 2;
            for (let slate = -1; slate <= Slates.perRow; slate += 1) {
                const shade = spread(random, 12);
                context.fillStyle = rgb(tone[0] + shade, tone[1] + shade, tone[2] + shade);
                context.fillRect(slate * slateWidth + offset, row * rowHeight, slateWidth - 1.5, rowHeight - 1.5);
            }
        }
    });
}
