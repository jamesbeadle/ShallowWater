import { Vector3 } from 'three';
import { blowPuffs, createSmokePuffs } from '../effects/smokePuffs.js';
import { createClothing, Wardrobe } from '../props/figure/clothing.js';
import { buildFigure } from '../props/figure/figureRig.js';
import { placeFigure } from '../props/figure/placement.js';
import { carryForward, standPose } from '../props/figure/poses.js';
import { createRim } from '../props/figure/rimLight.js';
import { Liveries } from '../props/narrowboat/livery.js';
import { buildNarrowboat, placeBoat } from '../props/narrowboat/narrowboat.js';
import { Boat } from '../props/narrowboat/dimensions.js';
import { srgb } from '../world/colours.js';

const upward = new Vector3(0, 1, 0);
const Steering = { turnHead: 0.1, shoulder: 0.5, elbow: 0.7 };

function worldPoint(boatPosition, heading, local) {
    return local.clone().applyAxisAngle(upward, heading).add(boatPosition);
}

export function buildCrewedBoat({ rim = createRim(), smokeColour = srgb(0.3, 0.3, 0.3), smokeOpacity = 0.5, livery = Liveries.halcyon } = {}) {
    const narrowboat = buildNarrowboat({ livery });
    const askew = buildFigure({ clothing: createClothing({}, rim), wardrobe: Wardrobe.workman });
    standPose(askew, { turnHead: Steering.turnHead });
    carryForward(askew, { shoulder: Steering.shoulder, elbow: Steering.elbow });
    const smoke = createSmokePuffs(4, smokeColour, smokeOpacity);
    return { narrowboat, askew, smoke, parts: [narrowboat.boat, askew.root, ...smoke.puffs] };
}

export function sailBoat(crewed, { position, heading, time, beats, wind = new Vector3(-0.4, 0, 0.2) }) {
    placeBoat(crewed.narrowboat, position, heading);
    const { steeringSpot, exhaustMouth } = crewed.narrowboat;
    placeFigure(crewed.askew, worldPoint(position, heading, steeringSpot), heading + Math.PI / 2);
    blowPuffs(crewed.smoke, worldPoint(position, heading, exhaustMouth), wind, beats, time);
    const stern = worldPoint(position, heading, new Vector3(-Boat.length / 2, 0, 0));
    return { wakeOrigin: stern, wakeHeading: heading };
}
