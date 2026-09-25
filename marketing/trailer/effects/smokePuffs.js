import { CanvasTexture, Sprite, SpriteMaterial } from 'three';

const Puff = { lasts: 2.6, rise: 0.55, drift: 0.9, growth: 1.8, startSize: 0.2 };

function puffTexture() {
    const canvas = document.createElement('canvas');
    Object.assign(canvas, { width: 64, height: 64 });
    const context = canvas.getContext('2d');
    const soft = context.createRadialGradient(32, 32, 2, 32, 32, 32);
    soft.addColorStop(0, 'rgba(255,255,255,0.9)');
    soft.addColorStop(0.6, 'rgba(255,255,255,0.35)');
    soft.addColorStop(1, 'rgba(255,255,255,0)');
    context.fillStyle = soft;
    context.fillRect(0, 0, 64, 64);
    return new CanvasTexture(canvas);
}

export function createSmokePuffs(count, colour, opacity) {
    const map = puffTexture();
    const puffs = Array.from({ length: count }, () => new Sprite(new SpriteMaterial({ map, color: colour, transparent: true, depthWrite: false, fog: false })));
    return { puffs, opacity };
}

export function blowPuffs(smoke, source, wind, beats, time) {
    smoke.puffs.forEach((puff, index) => {
        const beat = beats.filter((beatTime) => beatTime <= time).at(-(index + 1));
        const age = beat === undefined ? Puff.lasts : time - beat;
        const life = Math.min(age / Puff.lasts, 1);
        const { material } = puff;
        material.opacity = smoke.opacity * (1 - life) ** 1.5;
        puff.position.copy(source).addScaledVector(wind, Puff.drift * life * Puff.lasts).setY(source.y + Puff.rise * life * Puff.lasts);
        puff.scale.setScalar(Puff.startSize + Puff.growth * life);
    });
}
