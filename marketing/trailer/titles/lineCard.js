import { Palette, cssColour } from '../stage/palette.js';
import { Typefaces } from '../stage/fonts.js';
import { Fades, presenceOf, progressThrough } from './cardTiming.js';
import { fillLines, paint, useFont, withShadow } from './lettering.js';

const Placement = { heightInPicture: 0.8, drift: 10, lineHeight: 60 };
const Lettering = { family: Typefaces.serif, size: 50, style: 'italic', spacing: 1 };

export function drawLine(context, shape, card, time) {
    const opacity = presenceOf(card, time, Fades.slow);
    const drift = (1 - progressThrough(card, time)) * Placement.drift;
    const y = shape.barHeight + shape.pictureHeight * Placement.heightInPicture + drift;
    useFont(context, Lettering);
    withShadow(context, cssColour(Palette.ink, 0.85 * opacity), 18, () => {
        paint(context, Palette.parchment, opacity);
        fillLines(context, card.lines, { x: shape.width / 2, y, lineHeight: Placement.lineHeight });
    });
}
