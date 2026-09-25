import { paintTexture, rgb } from './canvasTexture.js';

const Sash = { panesAcross: 3, panesDown: 4, frame: 10, bar: 4 };

function paintPanes(context, width, height, glass) {
    const inner = { width: width - Sash.frame * 2, height: height - Sash.frame * 2 };
    const paneWidth = inner.width / Sash.panesAcross;
    const paneHeight = inner.height / Sash.panesDown;
    context.fillStyle = glass;
    for (let across = 0; across < Sash.panesAcross; across += 1) {
        for (let down = 0; down < Sash.panesDown; down += 1) {
            const x = Sash.frame + across * paneWidth + Sash.bar / 2;
            const y = Sash.frame + down * paneHeight + Sash.bar / 2;
            context.fillRect(x, y, paneWidth - Sash.bar, paneHeight - Sash.bar);
        }
    }
}

export function createWindowTexture({ frame = [214, 206, 188], glass = [22, 26, 30] } = {}) {
    return paintTexture(96, 128, (context, width, height) => {
        context.fillStyle = rgb(...frame);
        context.fillRect(0, 0, width, height);
        paintPanes(context, width, height, rgb(...glass));
    });
}

export function createWindowGlow() {
    return paintTexture(96, 128, (context, width, height) => {
        context.fillStyle = 'black';
        context.fillRect(0, 0, width, height);
        paintPanes(context, width, height, rgb(255, 214, 150));
    });
}
