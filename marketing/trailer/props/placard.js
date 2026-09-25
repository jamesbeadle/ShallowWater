import { BoxGeometry, CylinderGeometry, Group, Mesh, MeshStandardMaterial } from 'three';
import { Typefaces } from '../stage/fonts.js';
import { paintTexture } from '../textures/canvasTexture.js';
import { srgb } from '../world/colours.js';

function paintedBoard(lines) {
    return paintTexture(512, 320, (context, width, height) => {
        context.fillStyle = 'rgb(226, 220, 200)';
        context.fillRect(0, 0, width, height);
        context.fillStyle = 'rgb(24, 20, 18)';
        context.font = `${Math.round(height / (lines.length + 1.2))}px "${Typefaces.poster}"`;
        context.textAlign = 'center';
        context.textBaseline = 'middle';
        lines.forEach((line, index) => context.fillText(line, width / 2, ((index + 0.8) * height) / (lines.length + 0.6)));
    });
}

export function buildPlacard(lines) {
    const placard = new Group();
    const face = new MeshStandardMaterial({ map: paintedBoard(lines), roughness: 0.9 });
    const board = new Mesh(new BoxGeometry(0.9, 0.56, 0.02), face);
    board.position.setY(1.9);
    const stick = new Mesh(new CylinderGeometry(0.02, 0.02, 1.8, 6), new MeshStandardMaterial({ color: srgb(0.35, 0.25, 0.15) }));
    stick.position.setY(0.9);
    placard.add(board, stick);
    placard.traverse((part) => Object.assign(part, { castShadow: true }));
    return placard;
}
