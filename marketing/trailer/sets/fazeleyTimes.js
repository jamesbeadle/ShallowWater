import { Vector3 } from 'three';
import { srgb } from '../world/colours.js';
import { Daylights } from '../world/daylights.js';
import { Mists } from '../world/heightFog.js';
import { sunDirectionAt } from '../world/sunlight.js';

export function underMoon(time, elevation, azimuth) {
    const direction = sunDirectionAt(elevation, azimuth);
    return { ...time, sunDirection: direction, glowDirection: direction };
}

export const FazeleyTimes = {
    afternoon: {
        skyKind: 'shader', sunDirection: sunDirectionAt(19, 228), sunColour: srgb(1.0, 0.86, 0.68), sunIntensity: 3.4,
        skyFill: srgb(0.55, 0.62, 0.76), groundFill: srgb(0.34, 0.3, 0.24), fillIntensity: 1.1,
        fogColour: srgb(0.6, 0.6, 0.62), fogDensity: 0.0017, waterBody: srgb(0.04, 0.045, 0.036), mist: Mists.none, litShare: 0, glintStrength: 5,
        weather: { turbidity: 6, rayleigh: 1.4, mieCoefficient: 0.005, mieDirectionalG: 0.8, cloudCoverage: 0.55, cloudDensity: 0.5, cloudElevation: 0.4 },
    },
    dusk: {
        skyKind: 'gradient', zenith: srgb(0.035, 0.06, 0.13), horizon: srgb(0.36, 0.25, 0.25), glow: srgb(0.5, 0.2, 0.05),
        glowDirection: new Vector3(-0.8, 0.1, 0.2).normalize(),
        sunDirection: sunDirectionAt(3, 250), sunColour: srgb(0.9, 0.55, 0.35), sunIntensity: 0.5,
        skyFill: srgb(0.28, 0.34, 0.55), groundFill: srgb(0.12, 0.1, 0.1), fillIntensity: 0.55,
        fogColour: srgb(0.16, 0.15, 0.2), fogDensity: 0.004, waterBody: srgb(0.02, 0.022, 0.03), mist: Mists.thin, litShare: 0.4, glintStrength: 2,
    },
    night: {
        skyKind: 'gradient', zenith: srgb(0.012, 0.02, 0.05), horizon: srgb(0.09, 0.11, 0.17), glow: srgb(0.08, 0.1, 0.16),
        glowDirection: new Vector3(0.3, 0.5, -0.8).normalize(),
        starCount: 1400, hasMoon: true, sunDirection: sunDirectionAt(34, 200), sunColour: srgb(0.62, 0.72, 1.0), sunIntensity: 3.6,
        skyFill: srgb(0.34, 0.42, 0.7), groundFill: srgb(0.06, 0.06, 0.08), fillIntensity: 1.25,
        fogColour: srgb(0.07, 0.085, 0.13), fogDensity: 0.0045, waterBody: srgb(0.006, 0.008, 0.012), mist: Mists.thin, litShare: 0.22, glintStrength: 1.4,
    },
    dawn: { ...Daylights.dawn, skyKind: 'shader', mist: Mists.valley, litShare: 0.08, glintStrength: 6 },
};
