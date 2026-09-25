import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';

const renderDirectory = dirname(fileURLToPath(import.meta.url));

export const trailerDirectory = dirname(renderDirectory);
export const outputDirectory = join(trailerDirectory, 'output');
export const framesDirectory = join(outputDirectory, 'frames');
export const editFile = join(trailerDirectory, "edit.json");

export function frameFileName(frameNumber) {
    return `frame-${String(frameNumber).padStart(5, '0')}.png`;
}
