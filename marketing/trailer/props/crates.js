import { BoxGeometry, Group, Mesh, MeshStandardMaterial } from 'three';
import { Typefaces } from '../stage/fonts.js';
import { paintTexture } from '../textures/canvasTexture.js';
import { createRandom, between } from '../world/random.js';

const Crate = { length: 1.3, width: 0.55, height: 0.45 };

function crateFace() {
    return paintTexture(256, 128, (context, width, height) => {
        context.fillStyle = 'rgb(122, 92, 58)';
        context.fillRect(0, 0, width, height);
        context.strokeStyle = 'rgba(40, 26, 14, 0.8)';
        [0.33, 0.66].forEach((line) => context.strokeRect(0, height * line, width, 1));
        context.fillStyle = 'rgba(20, 16, 12, 0.85)';
        context.font = `44px "${Typefaces.poster}"`;
        context.textAlign = 'center';
        context.fillText('W ↑ D', width / 2, height * 0.66);
    });
}

export function buildCrateStack(count, seed) {
    const random = createRandom(seed);
    const material = new MeshStandardMaterial({ map: crateFace(), roughness: 0.85 });
    const stack = new Group();
    for (let index = 0; index < count; index += 1) {
        const crate = new Mesh(new BoxGeometry(Crate.length, Crate.height, Crate.width), material);
        const layer = Math.floor(index / 3);
        crate.position.set(between(random, -0.05, 0.05), Crate.height * (layer + 0.5), (index % 3) * (Crate.width + 0.04));
        crate.rotateY(between(random, -0.06, 0.06));
        crate.castShadow = true;
        crate.receiveShadow = true;
        stack.add(crate);
    }
    return stack;
}
