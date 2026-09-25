import { BoxGeometry, Mesh, MeshStandardMaterial } from 'three';
import { Typefaces } from '../../stage/fonts.js';
import { paintTexture, rgb } from '../../textures/canvasTexture.js';

const Plate = { width: 0.2, height: 0.065, thickness: 0.004, pixelsWide: 512, pixelsHigh: 168 };
const Lettering = { maker: "J. & C.G. BOLINDERS", place: 'STOCKHOLM', model: "No 4417  ·  15 B.H.P." };
const Ink = { brass: [176, 132, 62], recess: [38, 28, 14], rivet: [120, 90, 40] };

function paintPlate(context, width, height) {
    context.fillStyle = rgb(...Ink.brass);
    context.fillRect(0, 0, width, height);
    context.strokeStyle = rgb(...Ink.recess);
    context.lineWidth = 6;
    context.strokeRect(12, 12, width - 24, height - 24);
    context.fillStyle = rgb(...Ink.recess);
    context.textAlign = 'center';
    context.font = `44px "${Typefaces.signwriting}"`;
    context.fillText(Lettering.maker, width / 2, 70);
    context.font = `30px "${Typefaces.poster}"`;
    context.fillText(Lettering.place, width / 2, 110);
    context.fillText(Lettering.model, width / 2, 146);
    context.fillStyle = rgb(...Ink.rivet);
    [[26, 26], [width - 26, 26], [26, height - 26], [width - 26, height - 26]].forEach(([x, y]) => context.fillRect(x - 5, y - 5, 10, 10));
}

export function buildNamePlate() {
    const texture = paintTexture(Plate.pixelsWide, Plate.pixelsHigh, paintPlate);
    const material = new MeshStandardMaterial({ map: texture, roughness: 0.35, metalness: 0.85 });
    return new Mesh(new BoxGeometry(Plate.width, Plate.height, Plate.thickness), material);
}
