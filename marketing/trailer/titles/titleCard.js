import { Palette, cssColour } from '../stage/palette.js';
import { Typefaces } from '../stage/fonts.js';
import { Fades, presenceOf, secondsInto } from './cardTiming.js';
import { useFont, withShadow } from './lettering.js';
import { drawOrnamentedRule, drawShadedLetters } from './signwriting.js';

const Layout = { titleY: 455, subtitleY: 598, ruleAboveY: 342, ruleBelowY: 548, ruleMargin: 30 };
const Title = { family: Typefaces.signwriting, size: 150, spacing: 8 };
const Subtitle = { family: Typefaces.serif, size: 58, weight: '700', spacing: 26 };
const Arrival = { glowSeconds: 1.4, glowBlur: 70 };

function arrivalGlow(card, time) {
    const settling = Math.min(secondsInto(card, time) / Arrival.glowSeconds, 1);
    return (1 - settling) ** 2;
}

function drawTitleWords(context, shape, words, glow) {
    useFont(context, Title);
    withShadow(context, cssColour(Palette.cream, glow), Arrival.glowBlur * glow, () => {
        drawShadedLetters(context, words, { x: shape.width / 2, y: Layout.titleY, size: Title.size });
    });
}

export function drawTitle(context, shape, card, time) {
    const opacity = presenceOf(card, time, Fades.quick);
    const [words, subtitle] = card.lines;
    const centreX = shape.width / 2;
    context.save();
    context.globalAlpha = opacity;
    drawTitleWords(context, shape, words, arrivalGlow(card, time));
    const brass = cssColour(Palette.brass);
    const halfWidth = context.measureText(words).width / 2 + Layout.ruleMargin;
    drawOrnamentedRule(context, { x: centreX, y: Layout.ruleAboveY, halfWidth, colour: brass });
    drawOrnamentedRule(context, { x: centreX, y: Layout.ruleBelowY, halfWidth, colour: brass });
    useFont(context, Subtitle);
    context.fillStyle = brass;
    context.fillText(subtitle, centreX, Layout.subtitleY);
    context.restore();
}
