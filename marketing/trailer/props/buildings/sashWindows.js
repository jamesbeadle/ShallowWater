import { BoxGeometry, Mesh, MeshStandardMaterial, PlaneGeometry } from 'three';
import { mergeGeometries } from 'three/addons/utils/BufferGeometryUtils.js';
import { createWindowGlow, createWindowTexture } from '../../textures/windowTexture.js';
import { srgb } from '../../world/colours.js';

const Surround = { width: 0.16, proud: 0.07, sillHeight: 0.09, sillDepth: 0.2, hoodHeight: 0.12, hoodDepth: 0.22 };
const glassSetBack = 0.3;
const Lamplight = { bright: srgb(1.0, 0.74, 0.44), curtained: srgb(0.95, 0.46, 0.2) };

export function createSashMaterials() {
    const panes = createWindowTexture({ frame: [206, 200, 186], glass: [16, 20, 28] });
    const glow = createWindowGlow();
    return {
        dark: new MeshStandardMaterial({ map: panes, roughness: 0.12, metalness: 0.4 }),
        bright: new MeshStandardMaterial({ map: panes, emissiveMap: glow, emissive: Lamplight.bright, emissiveIntensity: 2.6, roughness: 0.3 }),
        curtained: new MeshStandardMaterial({ map: panes, emissiveMap: glow, emissive: Lamplight.curtained, emissiveIntensity: 1.5, roughness: 0.3 }),
    };
}

function boxAt(width, height, depth, [x, y, z]) {
    const box = new BoxGeometry(width, height, depth);
    box.translate(x, y, z);
    return box;
}

function surroundOf({ x, bottom, width, height, storeyIndex }) {
    const half = width / 2 + Surround.width / 2;
    const top = bottom + height;
    const z = Surround.proud / 2;
    const pieces = [
        boxAt(Surround.width, height + Surround.width, Surround.proud, [x - half, bottom + height / 2, z]),
        boxAt(Surround.width, height + Surround.width, Surround.proud, [x + half, bottom + height / 2, z]),
        boxAt(width + Surround.width * 2, Surround.width, Surround.proud, [x, top + Surround.width / 2, z]),
        boxAt(width + Surround.width * 3, Surround.sillHeight, Surround.sillDepth, [x, bottom - Surround.sillHeight / 2, Surround.sillDepth / 2]),
    ];
    const hasHood = storeyIndex === 1;
    const hood = hasHood ? [boxAt(width + Surround.width * 4, Surround.hoodHeight, Surround.hoodDepth, [x, top + Surround.width * 1.6, Surround.hoodDepth / 2])] : [];
    return [...pieces, ...hood];
}

export function buildSurrounds(openings, material) {
    return new Mesh(mergeGeometries(openings.flatMap(surroundOf)), material);
}

export function glaze(opening, material) {
    const pane = new Mesh(new PlaneGeometry(opening.width, opening.height), material);
    pane.position.set(opening.x, opening.bottom + opening.height / 2, -glassSetBack);
    return pane;
}
