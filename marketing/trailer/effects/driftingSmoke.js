import { Color, DoubleSide, Group, Mesh, NormalBlending, PlaneGeometry, ShaderMaterial, Vector3 } from 'three';
import { setUniforms } from '../stage/uniforms.js';
import { shaderNoise } from '../world/shaderNoise.js';
import { keepOutOfDepth } from './depthVeil.js';

const unlit = new Color();

const vertexShader = /* glsl */ `
    varying vec2 vUv;
    void main() {
        vUv = uv;
        gl_Position = projectionMatrix * modelViewMatrix * vec4(position, 1.0);
    }`;

const fragmentShader = /* glsl */ `
    uniform vec3 colour, glow;
    uniform float opacity, seed, age;
    varying vec2 vUv;
    ${shaderNoise}
    void main() {
        vec2 centred = vUv - 0.5;
        float radial = 1.0 - smoothstep(0.12, 0.5, length(centred));
        float wisps = fractalAt(vec3(centred * 3.4 + seed, seed * 1.7 + age * 0.45));
        float density = radial * smoothstep(0.28, 0.78, wisps + radial * 0.3);
        vec3 shade = colour + glow * (0.55 + 0.6 * wisps);
        gl_FragColor = vec4(shade, density * opacity);
    }`;

function puffMesh(colour, seed, blending) {
    const uniforms = { colour: { value: colour }, glow: { value: new Color() }, opacity: { value: 0 }, seed: { value: seed * 17.3 }, age: { value: 0 } };
    const material = new ShaderMaterial({ vertexShader, fragmentShader, uniforms, blending, transparent: true, depthWrite: false, side: DoubleSide });
    return keepOutOfDepth(new Mesh(new PlaneGeometry(1, 1), material));
}

function presenceOf(puff, age) {
    const fadeIn = Math.min(age / puff.swell, 1);
    const fadeOut = 1 - Math.min(Math.max(age / puff.life, 0), 1);
    return fadeIn * fadeOut * fadeOut;
}

function placePuff(mesh, puff, age, lighting) {
    const { uniforms } = mesh.material;
    const eased = age / (age + puff.slowing);
    mesh.position.copy(puff.origin).addScaledVector(puff.velocity, eased * puff.slowing);
    mesh.scale.setScalar(puff.startSize + (puff.endSize - puff.startSize) * Math.sqrt(age / puff.life));
    setUniforms(uniforms, { age, opacity: puff.opacity * presenceOf(puff, age) });
    const { glow } = uniforms;
    glow.value.copy(lighting(mesh.position));
}

export function createDriftingSmoke(puffs, colour, blending = NormalBlending) {
    const smoke = new Group();
    const meshes = puffs.map((puff, index) => puffMesh(colour, index + 1, blending));
    smoke.add(...meshes);
    const update = (time, camera, lighting = () => unlit) => {
        meshes.forEach((mesh, index) => {
            const puff = puffs[index];
            const age = time - puff.bornAt;
            mesh.visible = age > 0 && age < puff.life;
            mesh.quaternion.copy(camera.quaternion);
            placePuff(mesh, puff, Math.max(age, 0), lighting);
        });
    };
    return { smoke, update };
}

export function glowFromLights(lights, { strength, reach }) {
    const glow = new Color();
    const share = new Color();
    const place = new Vector3();
    return (position) => {
        glow.setRGB(0, 0, 0);
        lights.forEach((light) => {
            const distance = light.getWorldPosition(place).distanceTo(position);
            glow.add(share.copy(light.color).multiplyScalar((light.intensity * strength) / (1 + (distance / reach) ** 2)));
        });
        return glow;
    };
}

export function puffFrom({ bornAt, origin, velocity, random, size = [0.1, 0.5], life = 2.5, opacity = 0.5 }) {
    const drift = new Vector3(random() - 0.5, random() * 0.5, random() - 0.5).multiplyScalar(0.3);
    const heading = velocity.clone().add(drift);
    return { bornAt, origin: origin.clone(), velocity: heading, startSize: size[0], endSize: size[1], life, opacity, swell: 0.25, slowing: 1.2 };
}
