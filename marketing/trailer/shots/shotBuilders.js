import { buildBlackTake } from './blackTake.js';
import { buildMapHeOwns } from './mapHeOwns.js';
import { buildRunningTheRide } from './runningTheRide.js';
import { buildNightRunning } from './nightRunning.js';
import { buildPicketAtTheBridge } from './picketAtTheBridge.js';
import { buildRedFlags } from './redFlags.js';
import { buildBowThroughMist } from './bowThroughMist.js';
import { buildClientOrme } from './clientOrme.js';
import { buildClientHolland } from './clientHolland.js';
import { buildMillRoofByDay } from './millRoofByDay.js';
import { buildTitleCard } from './titleCard.js';
import { buildUnderTheBridge } from './underTheBridge.js';
import { buildChainFalls } from './chainFalls.js';
import { buildTheWholeTown } from './theWholeTown.js';
import { buildScopeNight } from './scopeNight.js';
import { buildNightRoof } from './nightRoof.js';
import { buildLorriesOnTheBridge } from './lorriesOnTheBridge.js';
import { buildVokesLantern } from './vokesLantern.js';
import { buildPeelsWharf } from './peelsWharf.js';
import { buildEngineHole } from './engineHole.js';
import { buildClientFenn } from './clientFenn.js';
import { buildAskewProfile } from './askewProfile.js';

export const shotBuilders = {
    blackOpen: buildBlackTake,
    runningTheRide: buildRunningTheRide,
    mapHeOwns: buildMapHeOwns,
    engineHole: buildEngineHole,
    bowThroughMist: buildBowThroughMist,
    redFlags: buildRedFlags,
    picketAtTheBridge: buildPicketAtTheBridge,
    clientFenn: buildClientFenn,
    clientOrme: buildClientOrme,
    clientHolland: buildClientHolland,
    askewProfile: buildAskewProfile,
    millRoofByDay: buildMillRoofByDay,
    cardFindIt: buildBlackTake,
    peelsWharf: buildPeelsWharf,
    cardUseIt: buildBlackTake,
    vokesLantern: buildVokesLantern,
    cardLeaveByWater: buildBlackTake,
    nightRunning: buildNightRunning,
    lorriesOnTheBridge: buildLorriesOnTheBridge,
    nightRoof: buildNightRoof,
    scopeNight: buildScopeNight,
    theWholeTown: buildTheWholeTown,
    chainFalls: buildChainFalls,
    underTheBridge: buildUnderTheBridge,
    titleCard: buildTitleCard,
    button: buildBlackTake,
};
