import { Canal } from '../world/canal.js';
import { fractalNoise, smoothstep } from '../world/noise.js';
import { blend, srgb } from '../world/colours.js';
import { isOpenGround } from './hopwasPlaces.js';

const Hill = { height: 70, foot: 30, crest: 420, steepening: 1.4, roughness: 4 };
const Fields = { rise: 0.9, roughness: 2.2, scale: 140 };
const Soil = {
    bed: srgb(0.09, 0.075, 0.055),
    mud: srgb(0.2, 0.15, 0.1),
    towpath: srgb(0.45, 0.39, 0.3),
    grass: srgb(0.33, 0.36, 0.16),
    stubble: srgb(0.52, 0.45, 0.25),
    leafLitter: srgb(0.3, 0.17, 0.08),
    bracken: srgb(0.5, 0.27, 0.11),
    heath: srgb(0.36, 0.37, 0.18),
};

function bankHeight(distanceFromCentre) {
    const bankProgress = smoothstep(Canal.halfWidth, Canal.towpathFrom, distanceFromCentre);
    return Canal.bed + (Canal.bankTop - Canal.bed) * bankProgress;
}

function hillHeight(x, across) {
    const climb = Math.min(Math.max((-across - Hill.foot) / (Hill.crest - Hill.foot), 0), 1);
    const rise = Hill.height * climb ** Hill.steepening;
    const lumps = Hill.roughness * fractalNoise(x / 90, across / 90) * smoothstep(12, 70, -across);
    return Canal.bankTop + rise + lumps;
}

function fieldHeight(x, across) {
    const verge = Fields.rise * smoothstep(Canal.towpathTo, Canal.towpathTo + 4, across);
    const swell = Fields.roughness * fractalNoise(x / Fields.scale, across / Fields.scale) * smoothstep(14, 60, across);
    return Canal.bankTop + verge + swell;
}

export function hopwasHeight(x, across) {
    const distanceFromCentre = Math.abs(across);
    if (distanceFromCentre < Canal.towpathFrom) {
        return bankHeight(distanceFromCentre);
    }
    return across > 0 ? fieldHeight(x, across) : hillHeight(x, across);
}

function heathColour(x, across) {
    const patch = fractalNoise(x / 23 + 11, across / 23, 3);
    return blend(Soil.heath, Soil.bracken, smoothstep(0.42, 0.62, patch));
}

function landColour(x, across) {
    if (isOpenGround(x, across)) {
        return heathColour(x, across);
    }
    const patch = fractalNoise(x / 160 + 7, across / 110, 2);
    const fieldColour = blend(Soil.grass, Soil.stubble, smoothstep(0.45, 0.6, patch));
    const woodColour = blend(Soil.grass, Soil.leafLitter, smoothstep(20, 45, -across));
    return across > 0 ? fieldColour : woodColour;
}

export function hopwasColour(x, across) {
    const distanceFromCentre = Math.abs(across);
    const isTowpath = across > Canal.towpathFrom && across < Canal.towpathTo;
    const land = isTowpath ? Soil.towpath : landColour(x, across);
    const nearWater = blend(Soil.bed, Soil.mud, smoothstep(Canal.halfWidth - 1, Canal.halfWidth + 0.8, distanceFromCentre));
    return blend(nearWater, land, smoothstep(Canal.halfWidth + 0.6, Canal.towpathFrom, distanceFromCentre));
}
