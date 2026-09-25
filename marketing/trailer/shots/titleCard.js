import { Vector3 } from 'three';
import { lookOf } from '../stage/looks.js';
import { buildHeroVoyage, Swing } from './underTheBridge.js';

const Darkening = { fade: 0.45, rampSeconds: 0.5 };
const Recede = {
    from: Swing.to,
    to: new Vector3(-30, 6.5, -2.0),
    lookFrom: Swing.lookTo,
    lookTo: new Vector3(40, 3.5, 1.0),
};

export function buildTitleCard(setting) {
    const look = (shotTime) => lookOf('dawn', {
        exposure: 0.6,
        bloom: { strength: 0.5, radius: 0.7, threshold: 1.5 },
        finish: { fade: Darkening.fade * Math.min(shotTime / Darkening.rampSeconds, 1) },
    });
    return buildHeroVoyage(setting, Recede, look);
}
