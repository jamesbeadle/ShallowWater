import { AdditiveBlending, ConeGeometry, DoubleSide, Mesh, ShaderMaterial } from 'three';

const Beam = { length: 26, spread: 4.2 };

const beamShader = {
    vertexShader: /* glsl */ `
        varying float vAlong;
        varying vec3 vNormalView;
        varying vec3 vToCamera;
        void main() {
            vAlong = clamp(position.x / ${Beam.length.toFixed(1)}, 0.0, 1.0);
            vec4 viewPosition = modelViewMatrix * vec4(position, 1.0);
            vNormalView = normalize(normalMatrix * normal);
            vToCamera = normalize(-viewPosition.xyz);
            gl_Position = projectionMatrix * viewPosition;
        }`,
    fragmentShader: /* glsl */ `
        uniform vec3 colour;
        uniform float strength;
        varying float vAlong;
        varying vec3 vNormalView;
        varying vec3 vToCamera;
        void main() {
            float edgeSoftness = pow(abs(dot(vNormalView, vToCamera)), 1.5);
            float fade = pow(1.0 - vAlong, 2.2);
            gl_FragColor = vec4(colour * strength * fade * edgeSoftness, 1.0);
        }`,
};

export function buildHeadlampBeam(colour, strength) {
    const geometry = new ConeGeometry(Beam.spread, Beam.length, 20, 1, true);
    geometry.translate(0, -Beam.length / 2, 0);
    geometry.rotateZ(Math.PI / 2);
    const uniforms = { colour: { value: colour }, strength: { value: strength } };
    const material = new ShaderMaterial({ ...beamShader, uniforms, transparent: true, depthWrite: false, blending: AdditiveBlending, side: DoubleSide });
    return new Mesh(geometry, material);
}
