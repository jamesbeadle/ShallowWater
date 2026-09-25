import { createRandom, between } from '../../world/random.js';
import { paintTexture } from '../../textures/canvasTexture.js';

function paintSkyAndRiver(context, width, height) {
    const sky = context.createLinearGradient(0, 0, 0, height);
    sky.addColorStop(0, 'rgb(236, 196, 90)');
    sky.addColorStop(0.55, 'rgb(120, 170, 190)');
    sky.addColorStop(1, 'rgb(40, 90, 120)');
    context.fillStyle = sky;
    context.fillRect(0, 0, width, height);
}

function paintCastle(context, width, height) {
    context.fillStyle = 'rgb(240, 236, 220)';
    context.fillRect(width * 0.3, height * 0.36, width * 0.4, height * 0.3);
    [0.28, 0.46, 0.64].forEach((left) => context.fillRect(width * left, height * 0.24, width * 0.08, height * 0.42));
    context.fillStyle = 'rgb(170, 36, 30)';
    [0.28, 0.46, 0.64].forEach((left) => {
        context.beginPath();
        context.moveTo(width * (left - 0.01), height * 0.24);
        context.lineTo(width * (left + 0.04), height * 0.12);
        context.lineTo(width * (left + 0.09), height * 0.24);
        context.fill();
    });
}

function paintRoses(context, width, height) {
    const random = createRandom(23);
    for (let rose = 0; rose < 14; rose += 1) {
        const x = rose % 2 === 0 ? between(random, 0, width * 0.2) : between(random, width * 0.8, width);
        const y = between(random, 0, height);
        const radius = between(random, width * 0.05, width * 0.09);
        context.fillStyle = random() < 0.5 ? 'rgb(190, 30, 36)' : 'rgb(236, 150, 150)';
        context.beginPath();
        context.arc(x, y, radius, 0, Math.PI * 2);
        context.fill();
        context.strokeStyle = 'rgba(60, 10, 10, 0.8)';
        context.lineWidth = 2;
        context.beginPath();
        context.arc(x, y, radius * 0.55, 0.3, Math.PI * 1.6);
        context.stroke();
    }
}

export function paintRosesAndCastles() {
    return paintTexture(256, 384, (context, width, height) => {
        paintSkyAndRiver(context, width, height);
        paintCastle(context, width, height);
        paintRoses(context, width, height);
        context.fillStyle = 'rgba(14, 12, 10, 0.35)';
        context.fillRect(0, 0, width, height);
    });
}
