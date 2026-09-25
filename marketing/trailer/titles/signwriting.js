import { Palette, cssColour } from '../stage/palette.js';

const Shade = { depth: 12, lining: 3 };

function faceGradient(context, y, size) {
    const gradient = context.createLinearGradient(0, y - size / 2, 0, y + size / 2);
    gradient.addColorStop(0, cssColour(Palette.parchment));
    gradient.addColorStop(1, cssColour(Palette.cream));
    return gradient;
}

function drawShade(context, text, x, y) {
    context.fillStyle = cssColour(Palette.shadowRed);
    for (let step = Shade.depth; step > 0; step -= 1) {
        context.fillText(text, x + step, y + step);
    }
    context.fillStyle = cssColour(Palette.signwritingRed);
    context.fillText(text, x + Shade.lining, y + Shade.lining);
}

export function drawShadedLetters(context, text, { x, y, size }) {
    drawShade(context, text, x, y);
    context.lineWidth = Shade.lining;
    context.strokeStyle = cssColour(Palette.ink);
    context.strokeText(text, x, y);
    context.fillStyle = faceGradient(context, y, size);
    context.fillText(text, x, y);
}

export function drawOrnamentedRule(context, { x, y, halfWidth, colour }) {
    context.fillStyle = colour;
    context.fillRect(x - halfWidth, y - 1, halfWidth * 2, 2);
    const diamondSize = 9;
    [x - halfWidth, x, x + halfWidth].forEach((diamondX) => {
        context.beginPath();
        context.moveTo(diamondX, y - diamondSize);
        context.lineTo(diamondX + diamondSize, y);
        context.lineTo(diamondX, y + diamondSize);
        context.lineTo(diamondX - diamondSize, y);
        context.fill();
    });
}
