import { srgb } from './colours.js';
import { sunDirectionAt } from './sunlight.js';

export const Daylights = {
    dawn: {
        sunDirection: sunDirectionAt(6, 128),
        sunColour: srgb(1.0, 0.72, 0.46),
        sunIntensity: 4.2,
        skyFill: srgb(0.5, 0.58, 0.78),
        groundFill: srgb(0.36, 0.26, 0.18),
        fillIntensity: 1.3,
        fogColour: srgb(0.6, 0.47, 0.38),
        fogDensity: 0.0011,
        waterBody: srgb(0.03, 0.036, 0.028),
        weather: { turbidity: 5, rayleigh: 2.0, mieCoefficient: 0.004, mieDirectionalG: 0.8, cloudCoverage: 0.55, cloudDensity: 0.55, cloudElevation: 0.35 },
    },
    morning: {
        sunDirection: sunDirectionAt(14, 118),
        sunColour: srgb(1.0, 0.86, 0.68),
        sunIntensity: 3.0,
        skyFill: srgb(0.58, 0.66, 0.8),
        groundFill: srgb(0.38, 0.3, 0.2),
        fillIntensity: 1.0,
        fogColour: srgb(0.7, 0.68, 0.64),
        fogDensity: 0.0018,
        waterBody: srgb(0.035, 0.042, 0.032),
        weather: { turbidity: 5, rayleigh: 1.6, mieCoefficient: 0.005, mieDirectionalG: 0.82, cloudCoverage: 0.45, cloudDensity: 0.4 },
    },
};
