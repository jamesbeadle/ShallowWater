import { AdditiveBlending, BackSide, BufferGeometry, CanvasTexture, Color, Float32BufferAttribute, Group, Mesh, Points, PointsMaterial, ShaderMaterial, SphereGeometry, Sprite, SpriteMaterial } from 'three';
import { createRandom, between } from './random.js';

const Dome = { radius: 4200, starRadius: 4000, moonDistance: 3800, moonSize: 150 };

const skyShader = {
    vertexShader: /* glsl */ `
        varying vec3 vDirection;
        void main() {
            vDirection = normalize(position);
            gl_Position = projectionMatrix * modelViewMatrix * vec4(position, 1.0);
        }`,
    fragmentShader: /* glsl */ `
        uniform vec3 zenith, horizon, glow, glowDirection;
        varying vec3 vDirection;
        void main() {
            float height = clamp(vDirection.y, 0.0, 1.0);
            vec3 colour = mix(horizon, zenith, pow(height, 0.45));
            float nearGlow = pow(max(dot(normalize(vDirection), glowDirection), 0.0), 6.0) * (1.0 - height);
            gl_FragColor = vec4(colour + glow * nearGlow, 1.0);
        }`,
};

function starField(count, seed) {
    const random = createRandom(seed);
    const positions = [];
    for (let star = 0; star < count; star += 1) {
        const azimuth = between(random, 0, Math.PI * 2);
        const altitude = Math.asin(between(random, 0.08, 1));
        positions.push(Math.cos(azimuth) * Math.cos(altitude), Math.sin(altitude), Math.sin(azimuth) * Math.cos(altitude));
    }
    const geometry = new BufferGeometry();
    geometry.setAttribute('position', new Float32BufferAttribute(positions.map((value) => value * Dome.starRadius), 3));
    return new Points(geometry, new PointsMaterial({ color: new Color(0.9, 0.92, 1), size: 1.6, sizeAttenuation: false, fog: false }));
}

function moonTexture() {
    const canvas = document.createElement('canvas');
    Object.assign(canvas, { width: 128, height: 128 });
    const context = canvas.getContext('2d');
    const halo = context.createRadialGradient(64, 64, 8, 64, 64, 64);
    halo.addColorStop(0, 'rgba(255,255,255,1)');
    halo.addColorStop(0.16, 'rgba(250,250,255,1)');
    halo.addColorStop(0.2, 'rgba(200,215,255,0.35)');
    halo.addColorStop(1, 'rgba(120,140,220,0)');
    context.fillStyle = halo;
    context.fillRect(0, 0, 128, 128);
    return new CanvasTexture(canvas);
}

function moonAt(direction) {
    const material = new SpriteMaterial({ map: moonTexture(), color: new Color(2.2, 2.3, 2.6), blending: AdditiveBlending, depthWrite: false, fog: false });
    const moon = new Sprite(material);
    moon.position.copy(direction).multiplyScalar(Dome.moonDistance);
    moon.scale.setScalar(Dome.moonSize);
    return moon;
}

export function createGradientSky({ zenith, horizon, glow = new Color(0, 0, 0), glowDirection, starCount = 0, hasMoon = false }) {
    const uniforms = { zenith: { value: zenith }, horizon: { value: horizon }, glow: { value: glow }, glowDirection: { value: glowDirection } };
    const material = new ShaderMaterial({ ...skyShader, uniforms, side: BackSide, depthWrite: false, fog: false });
    const sky = new Group();
    sky.add(new Mesh(new SphereGeometry(Dome.radius, 32, 16), material));
    if (starCount) {
        sky.add(starField(starCount, 1938));
    }
    if (hasMoon) {
        sky.add(moonAt(glowDirection));
    }
    return sky;
}
