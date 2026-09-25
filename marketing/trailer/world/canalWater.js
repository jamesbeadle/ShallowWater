import { Reflector } from 'three/addons/objects/Reflector.js';
import { createWaterNormals } from './waterNormals.js';
import { WaterShader } from './waterShader.js';
import { waterFragmentShader } from './waterSurface.js';
import { setUniforms } from '../stage/uniforms.js';

const Mirror = { textureWidth: 960, textureHeight: 402, clipBias: 0.002 };

export function createCanalWater({ geometry, bodyColour, sun, glintStrength = 6 }) {
    const shader = { ...WaterShader, fragmentShader: waterFragmentShader };
    const water = new Reflector(geometry, { ...Mirror, multisample: 0, shader });
    water.rotateX(-Math.PI / 2);
    const { material } = water;
    material.fog = true;
    setUniforms(material.uniforms, { normalMap: createWaterNormals(), bodyColour, sunDirection: sun.direction, sunColour: sun.colour, glintStrength });
    return water;
}

export function flowWater(water, time, wake) {
    const { uniforms } = water.material;
    setUniforms(uniforms, { time, ...wake });
}
