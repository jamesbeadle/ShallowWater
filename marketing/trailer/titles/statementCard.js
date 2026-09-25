import { Palette, cssColour } from '../stage/palette.js';
import { Typefaces } from '../stage/fonts.js';
import { Fades, presenceOf, progressThrough, secondsInto } from './cardTiming.js';
import { fillLines, paint, useFont, withShadow } from './lettering.js';

const Lettering = { family: Typefaces.poster, size: 150, weight: '700', spacing: 10 };
const Slam = { seconds: 0.14, overshoot: 0.35, creep: 0.05, lineHeight: 160 };

function scaleAt(card, time) {
    const slamProgress = Math.min(secondsInto(card, time) / Slam.seconds, 1);
    const slam = Slam.overshoot * (1 - slamProgress) ** 2;
    return 1 + slam + Slam.creep * progressThrough(card, time);
}

export function drawStatement(context, shape, card, time) {
    const opacity = presenceOf(card, time, Fades.quick);
    const scale = scaleAt(card, time);
    context.save();
    context.translate(shape.width / 2, shape.height / 2);
    context.scale(scale, scale);
    useFont(context, Lettering);
    withShadow(context, cssColour(Palette.cream, 0.35 * opacity), 30, () => {
        paint(context, Palette.parchment, opacity);
        fillLines(context, card.lines, { x: 0, y: 0, lineHeight: Slam.lineHeight });
    });
    context.restore();
}
