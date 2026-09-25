import { Palette } from '../stage/palette.js';
import { Typefaces } from '../stage/fonts.js';
import { Fades, presenceOf } from './cardTiming.js';
import { fillLines, paint, useFont } from './lettering.js';

const Lettering = { family: Typefaces.serif, size: 36, style: 'italic', spacing: 0.5 };
const lineHeight = 44;

export function drawSubtitle(context, shape, card, time) {
    const opacity = presenceOf(card, time, Fades.quick);
    const y = shape.height - shape.barHeight / 2;
    useFont(context, Lettering);
    paint(context, Palette.parchment, opacity);
    fillLines(context, card.lines, { x: shape.width / 2, y, lineHeight });
}
