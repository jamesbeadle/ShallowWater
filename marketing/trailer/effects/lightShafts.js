import { AdditiveBlending, CanvasTexture, DoubleSide, Group, Mesh, MeshBasicMaterial, PlaneGeometry, Quaternion, Vector3 } from 'three';
import { between, createRandom, spread } from '../world/random.js';

const Texture = { width: 64, height: 256 };
const upward = new Vector3(0, 1, 0);

function shaftTexture() {
    const canvas = document.createElement('canvas');
    Object.assign(canvas, Texture);
    const context = canvas.getContext('2d');
    const across = context.createLinearGradient(0, 0, Texture.width, 0);
    [[0, 0], [0.5, 1], [1, 0]].forEach(([stop, strength]) => across.addColorStop(stop, `rgba(255,255,255,${strength})`));
    context.fillStyle = across;
    context.fillRect(0, 0, Texture.width, Texture.height);
    context.globalCompositeOperation = 'destination-in';
    const along = context.createLinearGradient(0, 0, 0, Texture.height);
    [[0, 0], [0.3, 1], [0.75, 0.8], [1, 0]].forEach(([stop, strength]) => along.addColorStop(stop, `rgba(255,255,255,${strength})`));
    context.fillStyle = along;
    context.fillRect(0, 0, Texture.width, Texture.height);
    return new CanvasTexture(canvas);
}

function crossedShaft(material, width, length) {
    const shaft = new Group();
    [0, Math.PI / 2].forEach((angle) => {
        const plane = new Mesh(new PlaneGeometry(width, length), material);
        plane.rotateY(angle);
        shaft.add(plane);
    });
    return shaft;
}

export function createLightShafts({ count, centre, spread: reach, direction, colour, length, width, opacity, seed }) {
    const random = createRandom(seed);
    const material = new MeshBasicMaterial({
        map: shaftTexture(), color: colour, transparent: true, opacity, blending: AdditiveBlending, depthWrite: false, side: DoubleSide, fog: false,
    });
    const alignment = new Quaternion().setFromUnitVectors(upward, direction);
    const shafts = new Group();
    for (let index = 0; index < count; index += 1) {
        const shaft = crossedShaft(material, width * between(random, 0.5, 1.5), length);
        shaft.quaternion.copy(alignment);
        shaft.position.set(centre.x + spread(random, reach.x), centre.y + spread(random, reach.y), centre.z + spread(random, reach.z));
        shafts.add(shaft);
    }
    return shafts;
}
