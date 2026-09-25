import { Palette } from '../stage/palette.js';
import { Typefaces } from '../stage/fonts.js';
import { Fades, presenceOf } from './cardTiming.js';
import { fillLines, paint, useFont } from './lettering.js';

const Lettering = { family: Typefaces.serif, size: 52, style: 'italic', spacing: 1 };

export function drawButton(context, shape, card, time) {
    const opacity = presenceOf(card, time, Fades.gentle);
    useFont(context, Lettering);
    paint(context, Palette.parchment, opacity);
    fillLines(context, card.lines, { x: shape.width / 2, y: shape.height / 2, lineHeight: 64 });
}
