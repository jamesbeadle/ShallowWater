import { Palette, cssColour } from '../stage/palette.js';
import { Typefaces } from '../stage/fonts.js';
import { Fades, presenceOf, secondsInto } from './cardTiming.js';
import { useFont, withShadow } from './lettering.js';

const Placement = { left: 150, fromBottom: 150, slide: 46, slideSeconds: 0.5, ruleLength: 330 };
const Name = { family: Typefaces.poster, size: 62, weight: '700', spacing: 6 };
const Role = { family: Typefaces.serif, size: 34, style: 'italic', spacing: 1 };

function slideOffset(card, time) {
    const settling = Math.min(secondsInto(card, time) / Placement.slideSeconds, 1);
    return Placement.slide * (1 - settling) ** 3;
}

export function drawName(context, shape, card, time) {
    const opacity = presenceOf(card, time, Fades.gentle);
    const [name, role] = card.lines;
    const x = Placement.left - slideOffset(card, time);
    const baseline = shape.barHeight + shape.pictureHeight - Placement.fromBottom;
    context.save();
    context.globalAlpha = opacity;
    context.textAlign = 'left';
    withShadow(context, cssColour(Palette.ink, 0.9), 16, () => {
        useFont(context, Name);
        context.textAlign = 'left';
        context.fillStyle = cssColour(Palette.parchment);
        context.fillText(name, x, baseline);
        context.fillStyle = cssColour(Palette.brass);
        context.fillRect(x, baseline + 40, Placement.ruleLength, 2);
        useFont(context, Role);
        context.textAlign = 'left';
        context.fillStyle = cssColour(Palette.cream);
        context.fillText(role, x, baseline + 74);
    });
    context.restore();
}
