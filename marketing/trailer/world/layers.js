export const Layers = { everything: 0, distant: 1 };

export function seeDistantThings(camera) {
    camera.layers.enable(Layers.distant);
    return camera;
}

export function keepFromReflections(object) {
    object.layers.set(Layers.distant);
    return object;
}
