import { DataTexture, LinearMipmapLinearFilter, RepeatWrapping, RGBAFormat } from 'three';
import { createRandom, between } from './random.js';

const Tile = { size: 256, waveCount: 36, seed: 41 };
const byteMiddle = 127.5;

function randomWaves() {
    const random = createRandom(Tile.seed);
    return Array.from({ length: Tile.waveCount }, () => {
        const frequencyX = Math.round(between(random, -9, 9));
        const frequencyY = Math.round(between(random, -9, 9));
        const wavelength = Math.hypot(frequencyX, frequencyY) || 1;
        return { frequencyX, frequencyY, amplitude: 1 / wavelength, phase: between(random, 0, Math.PI * 2) };
    });
}

function slopeAt(waves, u, v) {
    let slopeU = 0;
    let slopeV = 0;
    waves.forEach((wave) => {
        const angle = Math.PI * 2 * (wave.frequencyX * u + wave.frequencyY * v) + wave.phase;
        const rate = Math.cos(angle) * wave.amplitude * Math.PI * 2;
        slopeU += rate * wave.frequencyX;
        slopeV += rate * wave.frequencyY;
    });
    return [slopeU * 0.02, slopeV * 0.02];
}

export function createWaterNormals() {
    const waves = randomWaves();
    const pixels = new Uint8Array(Tile.size * Tile.size * 4);
    for (let row = 0; row < Tile.size; row += 1) {
        for (let column = 0; column < Tile.size; column += 1) {
            const [slopeU, slopeV] = slopeAt(waves, column / Tile.size, row / Tile.size);
            const length = Math.hypot(slopeU, slopeV, 1);
            const offset = (row * Tile.size + column) * 4;
            pixels.set([(-slopeU / length + 1) * byteMiddle, (-slopeV / length + 1) * byteMiddle, (1 / length + 1) * byteMiddle, 255], offset);
        }
    }
    const texture = new DataTexture(pixels, Tile.size, Tile.size, RGBAFormat);
    Object.assign(texture, { wrapS: RepeatWrapping, wrapT: RepeatWrapping, minFilter: LinearMipmapLinearFilter, generateMipmaps: true });
    texture.needsUpdate = true;
    return texture;
}
