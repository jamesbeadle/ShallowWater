function veilMesh(mesh) {
    const { material } = mesh;
    material.allowOverride = false;
    mesh.onBeforeRender = (renderer, scene) => {
        material.colorWrite = scene.overrideMaterial === null;
    };
}

export function keepOutOfDepth(object) {
    object.traverse((part) => {
        if (part.isMesh) {
            veilMesh(part);
        }
    });
    return object;
}
