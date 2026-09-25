import { ACESFilmicToneMapping, PCFShadowMap, WebGLRenderer } from 'three';

export function createRenderer(shape) {
    const renderer = new WebGLRenderer({ antialias: false, preserveDrawingBuffer: true, powerPreference: 'high-performance' });
    renderer.setPixelRatio(1);
    renderer.setSize(shape.width, shape.pictureHeight, false);
    renderer.toneMapping = ACESFilmicToneMapping;
    const { shadowMap } = renderer;
    shadowMap.enabled = true;
    shadowMap.type = PCFShadowMap;
    shadowMap.autoUpdate = false;
    return renderer;
}
