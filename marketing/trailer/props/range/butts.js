import { BoxGeometry, CapsuleGeometry, Group, Mesh, MeshStandardMaterial } from 'three';
import { paintTexture } from '../../textures/canvasTexture.js';
import { srgb } from '../../world/colours.js';

const Bank = { length: 150, height: 7, thickness: 12 };
const Target = { size: 1.8, spacing: 5, standHeight: 3.1 };

function targetFace() {
    return paintTexture(128, 128, (context, width) => {
        context.fillStyle = 'rgb(232, 228, 214)';
        context.fillRect(0, 0, width, width);
        context.fillStyle = 'rgb(20, 20, 20)';
        context.beginPath();
        context.arc(width / 2, width / 2, width * 0.22, 0, Math.PI * 2);
        context.fill();
    });
}

function earthBank() {
    const geometry = new CapsuleGeometry(Bank.height, Bank.length - Bank.height * 2, 6, 16);
    geometry.rotateZ(Math.PI / 2);
    geometry.scale(1, 1, Bank.thickness / (Bank.height * 2));
    const bank = new Mesh(geometry, new MeshStandardMaterial({ color: srgb(0.33, 0.3, 0.17), roughness: 1 }));
    bank.receiveShadow = true;
    return bank;
}

export function buildButts() {
    const butts = new Group();
    butts.add(earthBank());
    const face = new MeshStandardMaterial({ map: targetFace(), roughness: 0.9 });
    const count = Math.floor((Bank.length - 20) / Target.spacing);
    for (let index = 0; index < count; index += 1) {
        const target = new Mesh(new BoxGeometry(Target.size, Target.size, 0.08), face);
        target.position.set(-Bank.length / 2 + 10 + index * Target.spacing, Target.standHeight, Bank.thickness * 0.42);
        target.castShadow = true;
        butts.add(target);
    }
    return butts;
}
