import { shaderNoise } from './shaderNoise.js';

const worldPositionAssignment = [
    'vec4 placedPosition = vec4(transformed, 1.0);',
    '#ifdef USE_INSTANCING',
    'placedPosition = instanceMatrix * placedPosition;',
    '#endif',
    'vWorldPosition = (modelMatrix * placedPosition).xyz;',
].join('\n');

function patchVertex(shader, vertex) {
    shader.vertexShader = shader.vertexShader
        .replace('#include <common>', '#include <common>\nuniform float time;\nvarying vec3 vWorldPosition;')
        .replace('#include <begin_vertex>', `#include <begin_vertex>\n${vertex}\n${worldPositionAssignment}`);
}

function patchFragment(shader, { declarations, colour, light }) {
    shader.fragmentShader = shader.fragmentShader
        .replace('#include <common>', `#include <common>\nuniform float time;\nvarying vec3 vWorldPosition;\n${shaderNoise}\n${declarations}`)
        .replace('#include <color_fragment>', `#include <color_fragment>\n${colour}`)
        .replace('#include <opaque_fragment>', `${light}\n#include <opaque_fragment>`);
}

export function patchMaterial(material, { time = { value: 0 }, declarations = '', colour = '', vertex = '', light = '', uniforms = {} }) {
    material.onBeforeCompile = (shader) => {
        Object.assign(shader.uniforms, uniforms, { time });
        patchVertex(shader, vertex);
        patchFragment(shader, { declarations, colour, light });
    };
    material.customProgramCacheKey = () => `${declarations}${colour}${vertex}${light}`;
    return material;
}
