import { ShaderChunk } from 'three';

const fogVertexDeclarations = [
    '#ifdef USE_FOG',
    'varying float vFogDepth;',
    'varying float vFogHeight;',
    '#endif',
].join('\n');

const fogVertex = [
    '#ifdef USE_FOG',
    'vFogDepth = - mvPosition.z;',
    'vFogHeight = ( transpose( mat3( viewMatrix ) ) * ( mvPosition.xyz - viewMatrix[ 3 ].xyz ) ).y;',
    '#endif',
].join('\n');

const fogFragmentDeclarations = [
    '#ifdef USE_FOG',
    'uniform vec3 fogColor;',
    'uniform float fogDensity;',
    'varying float vFogDepth;',
    'varying float vFogHeight;',
    '#endif',
].join('\n');

function fogFragment({ density, base, falloff }) {
    return [
        '#ifdef USE_FOG',
        `float fragmentMist = exp( - max( vFogHeight - ${base.toFixed(3)}, 0.0 ) / ${falloff.toFixed(3)} );`,
        `float eyeMist = exp( - max( cameraPosition.y - ${base.toFixed(3)}, 0.0 ) / ${falloff.toFixed(3)} );`,
        'float rise = cameraPosition.y - vFogHeight;',
        `float meanMist = abs( rise ) > 0.05 ? ${falloff.toFixed(3)} * ( fragmentMist - eyeMist ) / rise : fragmentMist;`,
        `float depthOfMist = ${density.toFixed(5)} * meanMist * vFogDepth;`,
        'float fogFactor = 1.0 - exp( - fogDensity * fogDensity * vFogDepth * vFogDepth - depthOfMist );',
        'gl_FragColor.rgb = mix( gl_FragColor.rgb, fogColor, fogFactor );',
        '#endif',
    ].join('\n');
}

export function useHeightFog(mist) {
    Object.assign(ShaderChunk, {
        fog_pars_vertex: fogVertexDeclarations,
        fog_vertex: fogVertex,
        fog_pars_fragment: fogFragmentDeclarations,
        fog_fragment: fogFragment(mist),
    });
}

export const Mists = {
    valley: { density: 0.02, base: 0.6, falloff: 7 },
    thin: { density: 0.004, base: 0.2, falloff: 3 },
    waterside: { density: 0.008, base: 0.25, falloff: 2.6 },
    none: { density: 0, base: 0, falloff: 1 },
};
