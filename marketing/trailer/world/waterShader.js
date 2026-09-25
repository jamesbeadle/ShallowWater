import { ShaderChunk, UniformsLib, UniformsUtils, Vector3, Color, Matrix4 } from 'three';

export const WaterShader = {
    name: 'CanalWater',
    uniforms: UniformsUtils.merge([UniformsLib.fog, {
        color: { value: new Color() },
        tDiffuse: { value: null },
        textureMatrix: { value: new Matrix4() },
        normalMap: { value: null },
        time: { value: 0 },
        bodyColour: { value: new Color() },
        sunDirection: { value: new Vector3(0, 1, 0) },
        sunColour: { value: new Color() },
        rippleScale: { value: 0.55 },
        rippleStrength: { value: 0.11 },
        glintStrength: { value: 6 },
        distortion: { value: 0.03 },
        baseReflectance: { value: 0.06 },
        wakeOrigin: { value: new Vector3(0, -1000, 0) },
        wakeHeading: { value: 0 },
    }]),
    vertexShader: /* glsl */ `
        uniform mat4 textureMatrix;
        varying vec4 mirrorCoordinates;
        varying vec3 vWorldPosition;
        ${ShaderChunk.fog_pars_vertex}
        void main() {
            mirrorCoordinates = textureMatrix * vec4(position, 1.0);
            vWorldPosition = (modelMatrix * vec4(position, 1.0)).xyz;
            vec4 mvPosition = modelViewMatrix * vec4(position, 1.0);
            gl_Position = projectionMatrix * mvPosition;
            ${ShaderChunk.fog_vertex}
        }`,
};
