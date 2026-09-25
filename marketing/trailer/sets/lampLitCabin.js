import { AmbientLight, BoxGeometry, Mesh, MeshStandardMaterial, PlaneGeometry, PointLight, SpotLight, Vector3 } from 'three';
import { createLampHaze } from '../effects/lampHaze.js';
import { buildHurricaneLamp, LampHang } from '../props/hurricaneLamp.js';
import { createPlankTexture } from '../textures/plankTexture.js';
import { srgb } from '../world/colours.js';

const lampStand = new Vector3(-0.62, -0.36, -1.15);
export const LampFlame = lampStand.clone().add(LampHang.flame);
const Glow = { colour: srgb(1.0, 0.6, 0.27), intensity: 1.5, reach: 6, decay: 1.4 };
const Moon = { colour: srgb(0.45, 0.58, 0.9), intensity: 0.6, place: new Vector3(0.75, 0.6, -0.65), cone: 0.16 };
const Wall = { z: -1.75, width: 4, height: 2.4 };
const HazeLayers = [
    { size: [2.6, 1.6], depth: -0.55, reach: 0.55, grain: 3.6, seed: 1, strength: 0.3 },
    { size: [3.2, 2.0], depth: -1.05, reach: 0.5, grain: 2.6, seed: 7, strength: 0.42 },
];

function cabinWall() {
    const boards = createPlankTexture({ tone: [44, 28, 18], seed: 3 });
    boards.repeat.set(Wall.width / 0.8, Wall.height / 0.8);
    const wall = new Mesh(new PlaneGeometry(Wall.width, Wall.height), new MeshStandardMaterial({ map: boards, roughness: 0.55, metalness: 0.05 }));
    wall.position.set(-0.4, 0, Wall.z);
    const shelf = new Mesh(new BoxGeometry(1.6, 0.03, 0.34), new MeshStandardMaterial({ color: srgb(0.06, 0.035, 0.022), roughness: 0.6 }));
    shelf.position.set(lampStand.x, lampStand.y - 0.015, lampStand.z);
    return [wall, shelf];
}

function lampOnTheShelf() {
    const { lamp, flame } = buildHurricaneLamp({ brightness: 9 });
    lamp.position.copy(lampStand);
    const light = new PointLight(Glow.colour, Glow.intensity, Glow.reach, Glow.decay);
    light.position.copy(LampFlame);
    return { parts: [lamp, light], light, flame };
}

function moonlightKicker(target) {
    const kicker = new SpotLight(Moon.colour, Moon.intensity, 4, Moon.cone, 0.8, 1.2);
    kicker.position.copy(Moon.place);
    const aim = kicker.target;
    aim.position.copy(target);
    return [kicker, aim];
}

function hazeLayers(time) {
    return HazeLayers.map((layer) => {
        const { haze, strength } = createLampHaze({ ...layer, lamp: LampFlame, colour: Glow.colour, time });
        haze.position.set(LampFlame.x + 0.3, LampFlame.y + 0.35, layer.depth);
        strength.value = layer.strength;
        return haze;
    });
}

export function buildLampLitCabin({ time, headCentre }) {
    const lamp = lampOnTheShelf();
    const parts = [...cabinWall(), ...lamp.parts, ...moonlightKicker(headCentre), ...hazeLayers(time), new AmbientLight(srgb(0.4, 0.45, 0.6), 0.03)];
    return { parts, lampLight: lamp.light, baseIntensity: Glow.intensity };
}
