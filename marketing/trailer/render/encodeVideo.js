import { spawn } from 'node:child_process';
import { existsSync } from 'node:fs';
import { join } from 'node:path';
import { env } from 'node:process';
import { framesDirectory, outputDirectory } from './paths.js';

const ffmpeg = env.FFMPEG ?? 'ffmpeg';
const soundtrack = join(outputDirectory, 'soundtrack.wav');
const trailer = join(outputDirectory, 'shallow-water-the-pound-trailer.mp4');
const framesPerSecond = '30';

const pictureSettings = [
    '-c:v', 'libx264', '-preset', 'slow', '-crf', '20', '-maxrate', '16M', '-bufsize', '32M', '-tune', 'film', '-pix_fmt', 'yuv420p',
    '-colorspace', 'bt709', '-color_primaries', 'bt709', '-color_trc', 'bt709', '-movflags', '+faststart',
];
const soundSettings = ['-c:a', 'aac', '-b:a', '320k', '-ar', '48000'];

function inputsFor(hasSoundtrack) {
    const pictures = ['-framerate', framesPerSecond, '-i', join(framesDirectory, 'frame-%05d.png')];
    const sound = hasSoundtrack ? ['-i', soundtrack] : [];
    return [...pictures, ...sound];
}

function encode() {
    const hasSoundtrack = existsSync(soundtrack);
    const soundOutput = hasSoundtrack ? soundSettings : ['-an'];
    const command = ['-y', ...inputsFor(hasSoundtrack), ...pictureSettings, ...soundOutput, trailer];
    const encoder = spawn(ffmpeg, command, { stdio: 'inherit' });
    return new Promise((resolve) => encoder.on('close', resolve));
}

const exitCode = await encode();
console.log(`wrote ${trailer} (ffmpeg exited ${exitCode})`);
