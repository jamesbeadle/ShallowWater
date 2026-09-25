import { createServer } from 'node:http';
import { readFile, writeFile } from 'node:fs/promises';
import { extname, join, normalize } from 'node:path';
import { frameFileName, framesDirectory, trailerDirectory } from './paths.js';

const HttpStatus = { ok: 200, noContent: 204, notFound: 404 };
const contentTypes = {
    '.html': 'text/html',
    '.js': 'text/javascript',
    '.css': 'text/css',
    '.json': 'application/json',
    '.woff2': 'font/woff2',
};

async function readBody(request) {
    const chunks = [];
    for await (const chunk of request) {
        chunks.push(chunk);
    }
    return Buffer.concat(chunks);
}

async function saveFrame(request, response) {
    const frameNumber = Number(request.url.split('/').at(-1));
    const picture = await readBody(request);
    await writeFile(join(framesDirectory, frameFileName(frameNumber)), picture);
    response.writeHead(HttpStatus.noContent);
    response.end();
}

async function sendFile(request, response) {
    const { pathname } = new URL(request.url, 'http://stage');
    const path = join(trailerDirectory, normalize(decodeURIComponent(pathname)));
    const contents = await readFile(path).catch(() => null);
    const status = contents ? HttpStatus.ok : HttpStatus.notFound;
    response.writeHead(status, { 'Content-Type': contentTypes[extname(path)] ?? 'application/octet-stream' });
    response.end(contents ?? '');
}

function route(request, response) {
    const isFrame = request.method === 'POST';
    const handle = isFrame ? saveFrame : sendFile;
    handle(request, response).catch((failure) => {
        console.error(failure);
        response.writeHead(HttpStatus.notFound);
        response.end();
    });
}

export function serveStage(port) {
    const server = createServer(route);
    return new Promise((resolve) => server.listen(port, () => resolve(server)));
}
