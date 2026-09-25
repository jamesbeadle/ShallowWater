import { readFile } from 'node:fs/promises';
import { editFile } from './paths.js';

function framesOfShot(edit, shotName) {
    const shot = edit.shots.find((candidate) => candidate.name === shotName);
    const first = Math.round(shot.start * edit.framesPerSecond);
    const last = Math.round(shot.end * edit.framesPerSecond);
    return Array.from({ length: last - first }, (unused, index) => first + index);
}

function framesOfTimes(edit, times) {
    return times.split(',').map((time) => Math.round(Number(time) * edit.framesPerSecond));
}

function allFrames(edit) {
    const frameCount = Math.round(edit.duration * edit.framesPerSecond);
    return Array.from({ length: frameCount }, (unused, index) => index);
}

function chosenFrames(edit, options) {
    if (options.times) {
        return framesOfTimes(edit, options.times);
    }
    if (options.shots) {
        return options.shots.split(',').flatMap((shotName) => framesOfShot(edit, shotName));
    }
    return allFrames(edit);
}

export async function selectFrames(options) {
    const edit = JSON.parse(await readFile(editFile, 'utf8'));
    const step = Number(options.step ?? 1);
    return chosenFrames(edit, options).filter((frame, index) => index % step === 0);
}
