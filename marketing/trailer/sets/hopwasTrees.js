import { centreZ } from '../world/canal.js';
import { hopwasHeight } from './hopwasLand.js';
import { fractalNoise } from '../world/noise.js';
import { between, createRandom, spread } from '../world/random.js';
import { isOpenGround } from './hopwasPlaces.js';

const Wood = { from: -700, to: -27, spacing: 11, farSpacing: 21, farBeyond: -240, pinePatch: 0.6, pineFrom: -60, birchShare: 0.24 };
const WoodEdge = { from: -33, to: -22, spacing: 4.2 };
const Growth = { least: 0.95, most: 1.55, stretchLeast: 0.85, stretchMost: 1.2 };
const sinkIntoGround = 0.4;

function woodKind(random, x, across) {
    const plantation = fractalNoise(x / 190 + 3, across / 190);
    const isPlantation = plantation > Wood.pinePatch && across < Wood.pineFrom;
    if (isPlantation) {
        return 'pine';
    }
    return random() < Wood.birchShare ? 'birch' : 'oak';
}

function isNearAny(focuses, x, z) {
    return focuses.some((focus) => Math.hypot(x - focus.x, z - focus.z) < focus.radius);
}

export function placementAt(random, { kind, x, across, focus, growth = Growth }) {
    const z = centreZ(x) + across;
    const isNear = isNearAny(focus, x, z);
    return {
        kind, x, z,
        y: hopwasHeight(x, across) - sinkIntoGround,
        turn: between(random, 0, Math.PI * 2),
        scale: between(random, growth.least, growth.most),
        stretch: between(random, Growth.stretchLeast, Growth.stretchMost),
        detail: isNear ? 'near' : 'far',
    };
}

function scatterWood(random, x, focus, placements) {
    for (let across = Wood.to; across > Wood.from; across -= across > Wood.farBeyond ? Wood.spacing : Wood.farSpacing) {
        const treeX = x + spread(random, Wood.spacing * 0.5);
        const treeAcross = across + spread(random, Wood.spacing * 0.5);
        if (isOpenGround(treeX, treeAcross)) {
            continue;
        }
        placements.push(placementAt(random, { kind: woodKind(random, treeX, treeAcross), x: treeX, across: treeAcross, focus }));
    }
}

function scatterWoodEdge(random, x, focus, placements) {
    for (let across = WoodEdge.to; across > WoodEdge.from; across -= WoodEdge.spacing) {
        const shrubAcross = across + spread(random, 1.2);
        const shrubX = x + spread(random, 1.5);
        if (!isOpenGround(shrubX, shrubAcross)) {
            placements.push(placementAt(random, { kind: 'bush', x: shrubX, across: shrubAcross, focus }));
        }
    }
}

export function hopwasTreePlacements(extent, focus) {
    const random = createRandom(1936);
    const placements = [];
    for (let x = extent.from; x < extent.to; x += Wood.spacing) {
        scatterWood(random, x, focus, placements);
    }
    for (let x = extent.from; x < extent.to; x += WoodEdge.spacing) {
        scatterWoodEdge(random, x, focus, placements);
    }
    return placements;
}
