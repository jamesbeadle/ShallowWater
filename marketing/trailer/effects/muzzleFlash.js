import { AdditiveBlending, CanvasTexture, PointLight, Sprite, SpriteMaterial } from 'three';
import { srgb } from '../world/colours.js';

const Flash = { lasts: 0.07, size: 0.9, light: 60 };
const flame = srgb(1.0, 0.78, 0.45);

function flashTexture() {
    const canvas = document.createElement('canvas');
    Object.assign(canvas, { width: 64, height: 64 });
    const context = canvas.getContext('2d');
    const burst = context.createRadialGradient(32, 32, 0, 32, 32, 32);
    burst.addColorStop(0, 'rgba(255,255,240,1)');
    burst.addColorStop(0.3, 'rgba(255,200,120,0.8)');
    burst.addColorStop(1, 'rgba(255,120,40,0)');
    context.fillStyle = burst;
    context.fillRect(0, 0, 64, 64);
    return new CanvasTexture(canvas);
}

export function createMuzzleFlash(hasLight) {
    const glare = new SpriteMaterial({ map: flashTexture(), color: flame, transparent: true, blending: AdditiveBlending, depthWrite: false, fog: false });
    const sprite = new Sprite(glare);
    const light = hasLight ? new PointLight(flame, 0, 30, 2) : null;
    return { sprite, light };
}

function strengthAt(shotTimes, time) {
    const recent = shotTimes.filter((shotTime) => time >= shotTime && time - shotTime < Flash.lasts);
    return recent.length ? 1 - (time - recent[0]) / Flash.lasts : 0;
}

export function fireAt(flash, position, shotTimes, time) {
    const strength = strengthAt(shotTimes, time);
    const { sprite, light } = flash;
    const { material } = sprite;
    material.opacity = strength;
    sprite.position.copy(position);
    sprite.scale.setScalar(Flash.size * (0.6 + strength));
    if (light) {
        light.position.copy(position);
        light.intensity = Flash.light * strength;
    }
}
