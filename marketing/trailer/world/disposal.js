function disposeMaterial(material) {
    Object.values(material).filter((value) => value?.isTexture).forEach((texture) => texture.dispose());
    material.dispose();
}

export function disposeScene(scene) {
    scene.traverse((object) => {
        object.geometry?.dispose();
        const materials = [object.material].flat().filter(Boolean);
        materials.forEach(disposeMaterial);
        object.renderTarget?.dispose();
        object.dispose?.();
    });
}
