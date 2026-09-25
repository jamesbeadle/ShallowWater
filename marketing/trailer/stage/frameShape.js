const Letterbox = { aspect: 2.39 };

export function frameShapeOf(edit) {
    const halfPictureHeight = Math.round(edit.width / Letterbox.aspect / 2);
    const pictureHeight = halfPictureHeight * 2;
    return {
        width: edit.width,
        height: edit.height,
        pictureHeight,
        barHeight: (edit.height - pictureHeight) / 2,
        aspect: edit.width / pictureHeight,
    };
}
