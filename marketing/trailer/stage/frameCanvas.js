import { drawCards } from '../titles/drawCards.js';

export class FrameCanvas {
    constructor(shape) {
        this.shape = shape;
        this.canvas = document.createElement('canvas');
        this.canvas.width = shape.width;
        this.canvas.height = shape.height;
        this.context = this.canvas.getContext('2d');
        document.body.appendChild(this.canvas);
    }

    compose(picture, cards, time) {
        const { context, shape } = this;
        context.fillStyle = 'black';
        context.fillRect(0, 0, shape.width, shape.height);
        context.drawImage(picture, 0, shape.barHeight);
        drawCards(context, shape, cards, time);
        return this.canvas;
    }
}
