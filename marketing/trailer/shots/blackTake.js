import { PerspectiveCamera, Scene } from 'three';
import { lookOf } from '../stage/looks.js';

export function buildBlackTake() {
    return {
        scene: new Scene(),
        camera: new PerspectiveCamera(),
        update: () => {},
        look: () => lookOf('black'),
        dispose: () => {},
    };
}
