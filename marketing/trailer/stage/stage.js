import { FrameCanvas } from './frameCanvas.js';
import { frameShapeOf } from './frameShape.js';
import { PostProcessing } from './postProcessing.js';
import { createRenderer } from './renderer.js';

export class Stage {
    constructor(edit, shotBuilders) {
        this.edit = edit;
        this.shotBuilders = shotBuilders;
        this.shape = frameShapeOf(edit);
        this.renderer = createRenderer(this.shape);
        this.post = new PostProcessing(this.renderer, this.shape);
        this.frame = new FrameCanvas(this.shape);
        this.take = null;
        this.shotName = '';
    }

    takeFor(shot) {
        if (shot.name === this.shotName) {
            return this.take;
        }
        this.take?.dispose();
        const setting = { edit: this.edit, shape: this.shape, aspect: this.shape.aspect, shot, renderer: this.renderer };
        this.take = this.shotBuilders[shot.name](setting);
        this.shotName = shot.name;
        return this.take;
    }

    renderFrame(frameNumber) {
        const time = this.edit.timeOf(frameNumber);
        const shot = this.edit.shotAt(time);
        const take = this.takeFor(shot);
        const shotTime = time - shot.start;
        take.update(shotTime, time);
        this.post.show(take, take.look(shotTime, time), time);
        this.post.render();
        const cards = this.edit.cardsAt(time);
        return this.frame.compose(this.renderer.domElement, cards, time);
    }
}
