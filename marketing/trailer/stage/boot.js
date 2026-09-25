import { shotBuilders } from '../shots/shotBuilders.js';
import { deliverFrame } from './delivery.js';
import { Edit } from './edit.js';
import { loadFonts } from './fonts.js';
import { Stage } from './stage.js';

const editResponse = await fetch("../edit.json");
const edit = new Edit(await editResponse.json());
await loadFonts();
const stage = new Stage(edit, shotBuilders);

window.trailer = {
    frameCount: edit.frameCount,
    framesPerSecond: edit.framesPerSecond,
    deliverFrame: (frameNumber) => deliverFrame(stage, frameNumber),
    isReady: true,
};
