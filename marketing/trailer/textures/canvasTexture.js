import { CanvasTexture, RepeatWrapping, SRGBColorSpace } from 'three';

export function paintTexture(width, height, paint) {
    const canvas = document.createElement('canvas');
    Object.assign(canvas, { width, height });
    const context = canvas.getContext('2d');
    paint(context, width, height);
    const texture = new CanvasTexture(canvas);
    Object.assign(texture, { wrapS: RepeatWrapping, wrapT: RepeatWrapping, colorSpace: SRGBColorSpace, anisotropy: 4 });
    return texture;
}

export function rgb(red, green, blue) {
    return `rgb(${Math.round(red)}, ${Math.round(green)}, ${Math.round(blue)})`;
}
