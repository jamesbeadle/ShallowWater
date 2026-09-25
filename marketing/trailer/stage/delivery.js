function encodePicture(canvas) {
    return new Promise((resolve) => canvas.toBlob(resolve, 'image/png'));
}

export async function deliverFrame(stage, frameNumber) {
    const frame = stage.renderFrame(frameNumber);
    const picture = await encodePicture(frame);
    await fetch(`/frames/${frameNumber}`, { method: 'POST', body: picture });
}
