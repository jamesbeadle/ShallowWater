import { Color, SRGBColorSpace } from 'three';

export function srgb(red, green, blue) {
    return new Color().setRGB(red, green, blue, SRGBColorSpace);
}

export function blend(from, to, amount) {
    return from.clone().lerp(to, Math.min(Math.max(amount, 0), 1));
}
