import { chromium } from 'playwright';

const softwareGraphics = ['--use-angle=swiftshader', '--enable-unsafe-swiftshader', '--ignore-gpu-blocklist'];
const viewport = { width: 1920, height: 1080 };
const Timeouts = { stageReady: 120000, frame: 600000 };

export async function openStage(stageUrl) {
    const browser = await chromium.launch({ args: softwareGraphics });
    const page = await browser.newPage({ viewport });
    page.setDefaultTimeout(Timeouts.frame);
    page.on('console', (message) => console.log(`stage: ${message.text()}`));
    page.on('pageerror', (failure) => console.error(`stage error: ${failure.message}`));
    await page.goto(stageUrl);
    await page.waitForFunction(() => {
        const { trailer } = window;
        return trailer?.isReady;
    }, null, { timeout: Timeouts.stageReady });
    return { browser, page };
}

export async function deliverFrames(page, frames, onDelivered) {
    for (const frameNumber of frames) {
        await page.evaluate((number) => window.trailer.deliverFrame(number), frameNumber);
        onDelivered(frameNumber);
    }
}
