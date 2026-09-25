import { Palette } from '../stage/palette.js';
import { Typefaces } from '../stage/fonts.js';
import { Fades, presenceOf } from './cardTiming.js';
import { paint, useFont } from './lettering.js';

const Rows = [
    { y: 720, lettering: { family: Typefaces.poster, size: 30, weight: '400', spacing: 9 }, opacity: 0.8 },
    { y: 782, lettering: { family: Typefaces.poster, size: 40, weight: '700', spacing: 14 }, opacity: 1 },
];

export function drawPlatforms(context, shape, card, time) {
    const presence = presenceOf(card, time, Fades.slow);
    card.lines.forEach((line, index) => {
        const row = Rows[index];
        useFont(context, row.lettering);
        paint(context, Palette.cream, presence * row.opacity);
        context.fillText(line, shape.width / 2, row.y);
    });
}
