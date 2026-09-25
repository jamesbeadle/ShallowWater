import { mkdir } from 'node:fs/promises';
import { parseArgs } from 'node:util';
import { deliverFrames, openStage } from './chromium.js';
import { selectFrames } from './frameSelection.js';
import { framesDirectory } from './paths.js';
import { serveStage } from './stageServer.js';

const defaultPort = '8765';
const optionTypes = { shots: { type: 'string' }, times: { type: 'string' }, step: { type: 'string' }, workers: { type: 'string' }, port: { type: 'string' } };

function shareOut(frames, workerCount) {
    const shareSize = Math.ceil(frames.length / workerCount);
    return Array.from({ length: workerCount }, (unused, index) => frames.slice(index * shareSize, (index + 1) * shareSize));
}

function progressReporter(total) {
    const startedAt = Date.now();
    let delivered = 0;
    return (frameNumber) => {
        delivered += 1;
        const secondsEach = (Date.now() - startedAt) / 1000 / delivered;
        console.log(`frame ${frameNumber} (${delivered}/${total}, ${secondsEach.toFixed(2)} s each)`);
    };
}

async function renderShare(frames, port, report) {
    const { browser, page } = await openStage(`http://localhost:${port}/stage/index.html`);
    await deliverFrames(page, frames, report);
    await browser.close();
}

async function main() {
    const { values: options } = parseArgs({ options: optionTypes });
    const frames = await selectFrames(options);
    await mkdir(framesDirectory, { recursive: true });
    const port = Number(options.port ?? defaultPort);
    const server = await serveStage(port);
    const report = progressReporter(frames.length);
    const shares = shareOut(frames, Number(options.workers ?? 1));
    await Promise.all(shares.map((share) => renderShare(share, port, report)));
    server.close();
}

await main();
