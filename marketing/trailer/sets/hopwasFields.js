import { createRandom, spread } from '../world/random.js';
import { placementAt } from './hopwasTrees.js';
import { HopwasBridge, Village, isInside } from './hopwasPlaces.js';

const Boundaries = { columns: [-840, -650, -480, -310, -165, 120, 275, 445, 615, 790], rows: [13.6, 128, 268, 432], farthest: 600 };
const Hedge = { spacing: 3.1, treeChance: 0.03, treeGrowth: { least: 1.0, most: 1.45 } };
const HedgeBush = { least: 1.1, most: 1.5 };

function isGap(x, across) {
    const isVillage = isInside(Village, x, across) && across > Boundaries.rows[0] + 1;
    const isBridge = Math.abs(x - HopwasBridge.x) < HopwasBridge.halfWidth;
    return isVillage || isBridge;
}

function hedgePlant(random, x, across, focus) {
    const isTree = random() < Hedge.treeChance;
    const kind = isTree ? 'oak' : 'bush';
    const growth = isTree ? Hedge.treeGrowth : HedgeBush;
    return placementAt(random, { kind, x, across, focus, growth });
}

function hedgeAlongRow(random, across, extent, focus) {
    const plants = [];
    for (let x = extent.from; x < extent.to; x += Hedge.spacing) {
        const plantAcross = across + spread(random, 0.6);
        if (!isGap(x, plantAcross)) {
            plants.push(hedgePlant(random, x + spread(random, 0.8), plantAcross, focus));
        }
    }
    return plants;
}

function hedgeAcrossColumn(random, x, focus) {
    const plants = [];
    for (let across = Boundaries.rows[0] + 2; across < Boundaries.farthest; across += Hedge.spacing) {
        const plantX = x + spread(random, 0.6);
        if (!isGap(plantX, across)) {
            plants.push(hedgePlant(random, plantX, across + spread(random, 0.8), focus));
        }
    }
    return plants;
}

export function hopwasFieldPlacements(extent, focus) {
    const random = createRandom(1066);
    const rows = Boundaries.rows.flatMap((across) => hedgeAlongRow(random, across, extent, focus));
    const columns = Boundaries.columns.flatMap((x) => hedgeAcrossColumn(random, x, focus));
    return [...rows, ...columns];
}
