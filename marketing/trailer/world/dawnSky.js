import { Sky } from 'three/addons/objects/Sky.js';
import { setUniforms } from '../stage/uniforms.js';

const skyRadius = 4500;

export function createSky(sunDirection, weather) {
    const sky = new Sky();
    sky.scale.setScalar(skyRadius);
    const { uniforms } = sky.material;
    setUniforms(uniforms, weather);
    const { sunPosition } = uniforms;
    sunPosition.value.copy(sunDirection);
    return sky;
}

export function driftSky(sky, time) {
    const { uniforms } = sky.material;
    setUniforms(uniforms, { time });
}
