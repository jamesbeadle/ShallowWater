import { Vector3 } from 'three';
import { createSmokeColumn, driftSmoke } from '../effects/smokeColumn.js';
import { buildWindowSilhouette } from '../props/windowSilhouette.js';
import { Mill, MillChimney } from '../sets/fazeleyPlaces.js';
import { FazeleyTimes } from '../sets/fazeleyTimes.js';
import { lookOf } from '../stage/looks.js';
import { srgb } from '../world/colours.js';
import { glide, progressOf } from './cameraMoves.js';
import { fazeleyTake } from './fazeleyTake.js';

const Lens = { fieldOfView: 20 };
const Office = { bay: 16, storey: 2 };
const LitPattern = { perBay: 7, perStorey: 3, every: 11 };
const bays = 22;
const bayWidth = (Mill.xTo - Mill.xFrom) / bays;
const officeX = Mill.xFrom + (Office.bay + 0.5) * bayWidth;
const officeY = 0.62 + Office.storey * Mill.storeyHeight + 0.8 + 1.1;
const path = {
    from: new Vector3(officeX - 8, 1.6, Mill.zTo + 24),
    to: new Vector3(officeX - 6, 1.7, Mill.zTo + 19),
    lookFrom: new Vector3(officeX + 1, officeY + 2.5, Mill.zTo),
    lookTo: new Vector3(officeX + 0.3, officeY + 0.6, Mill.zTo),
};

function isOfficeWindow(placement) {
    const isOffice = placement.bay === Office.bay && placement.storey === Office.storey;
    const patternIndex = placement.bay * LitPattern.perBay + placement.storey * LitPattern.perStorey;
    return isOffice || patternIndex % LitPattern.every === 0;
}

export function buildClientHolland(setting) {
    const buildings = { millPlan: { isWindowLit: isOfficeWindow } };
    const take = fazeleyTake(setting, { time: FazeleyTimes.dusk, shadow: { focus: new Vector3(officeX, 0, Mill.zTo), span: 80 }, lens: Lens, buildings });
    const holland = buildWindowSilhouette({ width: 1.2, height: 1.2, hasHat: false });
    holland.position.set(officeX + 0.1, officeY - 0.45, Mill.zTo + 0.08);
    const smoke = createSmokeColumn(new Vector3(MillChimney.x, MillChimney.height + 1, MillChimney.z), srgb(0.25, 0.25, 0.3), 0.5);
    take.scene.add(holland, smoke.column);
    return {
        scene: take.scene,
        camera: take.camera,
        update: (shotTime, clock) => {
            driftSmoke(smoke, new Vector3(1.4, 0, 0.6), clock);
            take.flow(clock);
            glide(take.camera, path, progressOf(setting, shotTime));
        },
        look: () => lookOf('night', { exposure: 1.3, bloom: { strength: 0.8, radius: 0.6, threshold: 0.8 } }),
        dispose: take.dispose,
    };
}
