import { NoColorSpace } from 'three';
import { between, createRandom } from '../world/random.js';
import { paintTexture, rgb } from './canvasTexture.js';

const Plating = { size: 512, platesAcross: 2, rivetSpacing: 22, rivetRadius: 4.5, inset: 11, blotches: 420, streaks: 60 };

function seamsOf(size) {
    const plate = size / Plating.platesAcross;
    return Array.from({ length: Plating.platesAcross + 1 }, (unused, index) => index * plate);
}

function eachRivet(size, visit) {
    seamsOf(size).forEach((seam) => {
        for (let along = Plating.rivetSpacing / 2; along < size; along += Plating.rivetSpacing) {
            [seam - Plating.inset, seam + Plating.inset].forEach((across) => {
                visit(along, across);
                visit(across, along);
            });
        }
    });
}

function blotchShade(tone, random) {
    const shade = between(random, -40, 25);
    const [red, green, blue] = [tone[0] + shade, tone[1] + shade * 0.8, tone[2] + shade * 0.6].map(Math.round);
    return `rgba(${red}, ${green}, ${blue}, ${between(random, 0.05, 0.25)})`;
}

function paintWeathering(context, size, tone, random) {
    for (let blotch = 0; blotch < Plating.blotches; blotch += 1) {
        context.fillStyle = blotchShade(tone, random);
        context.beginPath();
        context.ellipse(random() * size, random() * size, between(random, 6, 60), between(random, 4, 40), random() * Math.PI, 0, Math.PI * 2);
        context.fill();
    }
    for (let streak = 0; streak < Plating.streaks; streak += 1) {
        context.fillStyle = `rgba(20, 12, 6, ${between(random, 0.1, 0.35)})`;
        context.fillRect(random() * size, random() * size, between(random, 1, 4), between(random, 30, 160));
    }
}

function paintRivets(context, size, light, shadow) {
    context.fillStyle = shadow;
    seamsOf(size).forEach((seam) => {
        context.fillRect(seam - 1.5, 0, 3, size);
        context.fillRect(0, seam - 1.5, size, 3);
    });
    eachRivet(size, (x, y) => {
        context.fillStyle = shadow;
        context.beginPath();
        context.arc(x + 1.5, y + 1.5, Plating.rivetRadius, 0, Math.PI * 2);
        context.fill();
        context.fillStyle = light;
        context.beginPath();
        context.arc(x - 0.8, y - 0.8, Plating.rivetRadius * 0.8, 0, Math.PI * 2);
        context.fill();
    });
}

export function createRivetedSteel({ tone = [50, 44, 38], seed = 12 } = {}) {
    const random = createRandom(seed);
    const map = paintTexture(Plating.size, Plating.size, (context, size) => {
        context.fillStyle = rgb(...tone);
        context.fillRect(0, 0, size, size);
        paintWeathering(context, size, tone, random);
        paintRivets(context, size, rgb(tone[0] * 1.6, tone[1] * 1.5, tone[2] * 1.4), 'rgba(8, 5, 3, 0.85)');
    });
    const bumpMap = paintTexture(Plating.size, Plating.size, (context, size) => {
        context.fillStyle = 'rgb(128, 128, 128)';
        context.fillRect(0, 0, size, size);
        paintRivets(context, size, 'rgb(250, 250, 250)', 'rgb(40, 40, 40)');
    });
    return { map, bumpMap: Object.assign(bumpMap, { colorSpace: NoColorSpace }) };
}
