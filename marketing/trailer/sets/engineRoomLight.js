import { BackSide, BoxGeometry, Mesh, MeshBasicMaterial, PlaneGeometry, PointLight, Scene, SphereGeometry, SpotLight, Vector3 } from 'three';
import { keepOutOfDepth } from '../effects/depthVeil.js';
import { createLightShafts } from '../effects/lightShafts.js';
import { paintTexture } from '../textures/canvasTexture.js';
import { srgb } from '../world/colours.js';
import { createRandom } from '../world/random.js';
import { Room, SideDoor } from './engineRoom.js';

const Dawn = { colour: srgb(0.5, 0.64, 0.95), intensity: 3.2, reach: 6, cone: 0.5, softness: 0.95, brightness: 0.3 };
const doorCentre = new Vector3((SideDoor.from + SideDoor.to) / 2, (SideDoor.sill + SideDoor.head) / 2, Room.far);
const gap = new Vector3(SideDoor.from + 0.06, doorCentre.y, Room.far);
const Stove = { colour: srgb(1.0, 0.45, 0.16), intensity: 0.8, reach: 3.5, decay: 1.5, place: new Vector3(Room.aft - 0.15, 0.95, -0.45) };

function paintHedge(context, width, height) {
    const random = createRandom(77);
    context.fillStyle = 'rgb(26, 30, 28)';
    context.beginPath();
    context.moveTo(0, height);
    for (let x = 0; x <= width; x += 4) {
        context.lineTo(x, height * (0.42 + 0.06 * random()));
    }
    context.lineTo(width, height);
    context.fill();
}

function paintMorning(context, width, height) {
    const sky = context.createLinearGradient(0, 0, 0, height);
    [[0, 'rgb(120, 140, 175)'], [0.4, 'rgb(185, 192, 205)'], [0.5, 'rgb(150, 156, 162)'], [1, 'rgb(40, 44, 44)']]
        .forEach(([stop, colour]) => sky.addColorStop(stop, colour));
    context.fillStyle = sky;
    context.fillRect(0, 0, width, height);
    paintHedge(context, width, height);
}

function outsideView() {
    const view = new Mesh(new PlaneGeometry(2.4, 1.6), new MeshBasicMaterial({ map: paintTexture(64, 128, paintMorning), fog: false }));
    const { color } = view.material;
    color.setScalar(Dawn.brightness);
    view.position.set(doorCentre.x, doorCentre.y + 0.1, Room.far - 0.9);
    return view;
}

function dawnThroughDoor() {
    const light = new SpotLight(Dawn.colour, Dawn.intensity, Dawn.reach, Dawn.cone, Dawn.softness, 1.4);
    light.position.copy(gap).add(new Vector3(0.25, 0.45, -0.9));
    const aim = light.target;
    aim.position.set(-0.55, 0.1, 0.7);
    return [light, aim];
}

function dustInTheLight() {
    const direction = new Vector3(-0.3, -0.45, 1).normalize();
    const centre = gap.clone().add(new Vector3(-0.2, -0.2, 0.7));
    const spread = new Vector3(0.06, 0.1, 0.12);
    const shafts = createLightShafts({ count: 6, centre, spread, direction, colour: Dawn.colour, length: 1.8, width: 0.12, opacity: 0.07, seed: 5 });
    return keepOutOfDepth(shafts);
}

function stoveThroughTheCabinDoor() {
    const light = new PointLight(Stove.colour, Stove.intensity, Stove.reach, Stove.decay);
    light.position.copy(Stove.place);
    return light;
}

export function lightTheEngineRoom() {
    const [dawn, aim] = dawnThroughDoor();
    return { parts: [outsideView(), dawn, aim, dustInTheLight(), stoveThroughTheCabinDoor()], dawn };
}

function glowPanel(colour, size, position) {
    const panel = new Mesh(new BoxGeometry(...size), new MeshBasicMaterial({ color: colour }));
    panel.position.copy(position);
    return panel;
}

export function engineRoomSurroundings() {
    const surroundings = new Scene();
    surroundings.add(new Mesh(new SphereGeometry(5, 16, 8), new MeshBasicMaterial({ color: srgb(0.035, 0.025, 0.02), side: BackSide })));
    surroundings.add(glowPanel(srgb(1.4, 0.9, 0.6), [0.9, 1, 0.1], new Vector3(0.6, 1.1, -2.5)));
    surroundings.add(glowPanel(srgb(2.6, 1.1, 0.35), [0.6, 0.6, 0.6], new Vector3(1.5, 0.4, 1.5)));
    surroundings.add(glowPanel(srgb(1.6, 0.8, 0.3), [0.4, 0.4, 0.4], new Vector3(-1.4, 1.5, -1.2)));
    return surroundings;
}
