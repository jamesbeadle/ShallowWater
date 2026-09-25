import { BoxGeometry, CylinderGeometry, Group, Mesh, MeshStandardMaterial } from 'three';
import { Typefaces } from '../../stage/fonts.js';
import { paintTexture } from '../../textures/canvasTexture.js';
import { srgb } from '../../world/colours.js';

function paintSign(name) {
    return paintTexture(256, 320, (context, width, height) => {
        context.fillStyle = 'rgb(28, 40, 30)';
        context.fillRect(0, 0, width, height);
        context.strokeStyle = 'rgb(200, 170, 90)';
        context.lineWidth = 8;
        context.strokeRect(10, 10, width - 20, height - 20);
        context.fillStyle = 'rgb(226, 206, 150)';
        context.font = `36px "${Typefaces.signwriting}"`;
        context.textAlign = 'center';
        name.split(' ').forEach((word, index) => context.fillText(word, width / 2, 80 + index * 60));
        context.fillStyle = 'rgb(90, 58, 34)';
        context.beginPath();
        context.ellipse(width / 2, height - 70, 70, 22, -0.1, 0, Math.PI * 2);
        context.fill();
    });
}

export function buildPubSign(name) {
    const sign = new Group();
    const post = new Mesh(new CylinderGeometry(0.08, 0.1, 4.2, 8), new MeshStandardMaterial({ color: srgb(0.14, 0.12, 0.1) }));
    post.position.setY(2.1);
    const arm = new Mesh(new BoxGeometry(1.2, 0.08, 0.08), post.material);
    arm.position.set(0.55, 4.0, 0);
    const board = new Mesh(new BoxGeometry(0.9, 1.15, 0.05), new MeshStandardMaterial({ map: paintSign(name), roughness: 0.7 }));
    board.position.set(0.75, 3.3, 0);
    sign.add(post, arm, board);
    sign.traverse((part) => Object.assign(part, { castShadow: true }));
    return sign;
}
