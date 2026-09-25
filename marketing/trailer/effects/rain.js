import { BufferGeometry, Float32BufferAttribute, LineBasicMaterial, LineSegments, Vector3 } from 'three';
import { between, createRandom } from '../world/random.js';

const Fall = { speed: 11, streak: 0.55, slant: new Vector3(0.14, 0, 0.05) };

export function createRain({ count, centre, size, colour, opacity, seed = 5 }) {
    const random = createRandom(seed);
    const drops = Array.from({ length: count }, () => new Vector3(between(random, -1, 1) * size.x, random() * size.y, between(random, -1, 1) * size.z));
    const geometry = new BufferGeometry();
    geometry.setAttribute('position', new Float32BufferAttribute(new Float32Array(count * 6), 3));
    const material = new LineBasicMaterial({ color: colour, transparent: true, opacity, depthWrite: false });
    const rain = new LineSegments(geometry, material);
    rain.position.copy(centre);
    rain.frustumCulled = false;
    return { rain, drops, height: size.y };
}

export function fallRain(shower, time) {
    const { geometry } = shower.rain;
    const positions = geometry.getAttribute('position');
    shower.drops.forEach((drop, index) => {
        const fallen = (drop.y - time * Fall.speed) % shower.height;
        const y = fallen < 0 ? fallen + shower.height : fallen;
        const top = new Vector3(drop.x, y, drop.z);
        const bottom = top.clone().addScaledVector(Fall.slant, -Fall.streak).setY(y - Fall.streak);
        positions.setXYZ(index * 2, top.x, top.y, top.z);
        positions.setXYZ(index * 2 + 1, bottom.x, bottom.y, bottom.z);
    });
    positions.needsUpdate = true;
}
