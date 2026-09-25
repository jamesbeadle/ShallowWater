import { Vector3 } from 'three';

export const Canal = { width: 14, halfWidth: 7, bankTop: 0.55, bed: -1.6, towpathFrom: 8.5, towpathTo: 11.5 };

const Meander = { amplitude: 16, wavelength: 260, secondAmplitude: 4, secondWavelength: 83 };

export function centreZ(x) {
    const broad = Meander.amplitude * Math.sin(x / Meander.wavelength);
    const fine = Meander.secondAmplitude * Math.sin(x / Meander.secondWavelength);
    return broad + fine;
}

export function headingAt(x) {
    const slope = (centreZ(x + 1) - centreZ(x - 1)) / 2;
    return -Math.atan(slope);
}

export function pointBeside(x, across, height = 0) {
    return new Vector3(x, height, centreZ(x) + across);
}
