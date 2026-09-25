import { cssColour } from '../stage/palette.js';

export function useFont(context, { family, size, weight = '400', style = 'normal', spacing = 0 }) {
    context.font = `${style} ${weight} ${size}px "${family}"`;
    context.letterSpacing = `${spacing}px`;
    context.textAlign = 'center';
    context.textBaseline = 'middle';
}

export function fillLines(context, lines, { x, y, lineHeight }) {
    const top = y - ((lines.length - 1) * lineHeight) / 2;
    lines.forEach((line, index) => context.fillText(line, x, top + index * lineHeight));
}

export function withShadow(context, colour, blur, draw) {
    context.save();
    context.shadowColor = colour;
    context.shadowBlur = blur;
    draw();
    context.restore();
}

export function paint(context, colour, opacity) {
    context.fillStyle = cssColour(colour, opacity);
}
