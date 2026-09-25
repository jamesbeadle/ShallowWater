import { Typefaces } from '../../stage/fonts.js';
import { createRandom, between } from '../../world/random.js';
import { paintTexture, rgb } from '../../textures/canvasTexture.js';

export const Liveries = {
    halcyon: { name: 'HALCYON', field: [30, 62, 42], border: [140, 30, 22], lining: [226, 208, 160], grime: 0.45 },
    patience: { name: 'PATIENCE', field: [120, 32, 28], border: [26, 48, 92], lining: [236, 214, 150], grime: 0.25 },
    company: { name: 'SIRIUS', field: [22, 42, 96], border: [214, 170, 40], lining: [240, 232, 210], grime: 0.05 },
};

function paintGrime(context, width, height, amount) {
    const random = createRandom(17);
    for (let smudge = 0; smudge < 260; smudge += 1) {
        context.fillStyle = `rgba(12, 10, 8, ${between(random, 0.02, 0.1) * amount * 2})`;
        context.beginPath();
        context.ellipse(random() * width, random() * height, between(random, 8, 70), between(random, 4, 26), 0, 0, Math.PI * 2);
        context.fill();
    }
}

function paintLettering(context, name, width, height, livery) {
    context.font = `${Math.round(height * 0.44)}px "${Typefaces.signwriting}"`;
    context.textAlign = 'center';
    context.textBaseline = 'middle';
    context.letterSpacing = `${Math.round(height * 0.04)}px`;
    context.fillStyle = rgb(...livery.border);
    context.fillText(name, width / 2 + 6, height / 2 + 6);
    context.fillStyle = rgb(...livery.lining);
    context.fillText(name, width / 2, height / 2);
}

export function paintCabinSide(livery) {
    return paintTexture(1024, 320, (context, width, height) => {
        context.fillStyle = rgb(...livery.border);
        context.fillRect(0, 0, width, height);
        const margin = height * 0.1;
        context.fillStyle = rgb(...livery.lining);
        context.fillRect(margin - 4, margin - 4, width - margin * 2 + 8, height - margin * 2 + 8);
        context.fillStyle = rgb(...livery.field);
        context.fillRect(margin, margin, width - margin * 2, height - margin * 2);
        paintLettering(context, livery.name, width, height, livery);
        paintGrime(context, width, height, livery.grime);
    });
}
