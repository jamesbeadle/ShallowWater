import { Palette } from '../stage/palette.js';
import { Typefaces } from '../stage/fonts.js';
import { Fades, presenceOf, secondsInto } from './cardTiming.js';
import { paint, useFont } from './lettering.js';

const Typing = { charactersPerSecond: 22, pauseBetweenLines: 0.25, lineGap: 58 };
const Sizes = [{ size: 38, spacing: 9 }, { size: 30, spacing: 12 }];

function delayBefore(lines, index) {
    const earlierCharacters = lines.slice(0, index).join('').length;
    return earlierCharacters / Typing.charactersPerSecond + index * Typing.pauseBetweenLines;
}

function typedPart(line, secondsTyping) {
    const characterCount = Math.max(0, Math.floor(secondsTyping * Typing.charactersPerSecond));
    return line.slice(0, characterCount);
}

export function drawSuper(context, shape, card, time) {
    const opacity = presenceOf(card, time, Fades.slow);
    const secondsTyping = secondsInto(card, time);
    const centreY = shape.height / 2;
    card.lines.forEach((line, index) => {
        useFont(context, { family: Typefaces.typewriter, ...Sizes[index] });
        paint(context, Palette.cream, opacity);
        const y = centreY + (index - 0.5) * Typing.lineGap;
        const typed = typedPart(line, secondsTyping - delayBefore(card.lines, index));
        context.fillText(typed, shape.width / 2, y);
    });
}
