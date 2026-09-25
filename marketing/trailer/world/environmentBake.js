import { PMREMGenerator } from 'three';
import { disposeScene } from './disposal.js';

const Bake = { blur: 0.04, near: 0.1, far: 200 };

export function bakeEnvironment(renderer, surroundings) {
    const generator = new PMREMGenerator(renderer);
    const reflections = generator.fromScene(surroundings, Bake.blur, Bake.near, Bake.far);
    generator.dispose();
    disposeScene(surroundings);
    return reflections;
}
