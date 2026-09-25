import { CanvasTexture, Group, Sprite, SpriteMaterial } from 'three';

const Column = { puffs: 26, lifetime: 9, rise: 2.2, spread: 5.5, startSize: 1.6 };

function softTexture() {
    const canvas = document.createElement('canvas');
    Object.assign(canvas, { width: 64, height: 64 });
    const context = canvas.getContext('2d');
    const soft = context.createRadialGradient(32, 32, 1, 32, 32, 32);
    soft.addColorStop(0, 'rgba(255,255,255,0.8)');
    soft.addColorStop(0.55, 'rgba(255,255,255,0.3)');
    soft.addColorStop(1, 'rgba(255,255,255,0)');
    context.fillStyle = soft;
    context.fillRect(0, 0, 64, 64);
    return new CanvasTexture(canvas);
}

export function createSmokeColumn(source, colour, opacity) {
    const material = new SpriteMaterial({ map: softTexture(), color: colour, transparent: true, depthWrite: false, opacity });
    const column = new Group();
    for (let puff = 0; puff < Column.puffs; puff += 1) {
        column.add(new Sprite(material.clone()));
    }
    return { column, source, opacity };
}

export function driftSmoke(smoke, wind, time) {
    const { column, source } = smoke;
    column.children.forEach((puff, index) => {
        const age = (time + (index * Column.lifetime) / Column.puffs) % Column.lifetime;
        const life = age / Column.lifetime;
        puff.position.copy(source).addScaledVector(wind, age).setY(source.y + Column.rise * age);
        puff.scale.setScalar(Column.startSize + Column.spread * life);
        const { material } = puff;
        material.opacity = smoke.opacity * Math.sin(Math.PI * Math.min(life * 1.4, 1)) * (1 - life);
    });
}
