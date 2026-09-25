import { BoxGeometry, DirectionalLight, FogExp2, HemisphereLight, Mesh, MeshBasicMaterial, PlaneGeometry, Scene, Vector3 } from 'three';
import { createBuildingMaterials } from '../props/buildings/buildingMaterials.js';
import { buildGeorgianHouse } from '../props/buildings/georgianHouse.js';
import { storeyFloor, windowOpenings } from '../props/buildings/georgianPlan.js';
import { buildSaloon } from '../props/car/saloon.js';
import { srgb } from '../world/colours.js';
import { createDuskSky } from '../world/duskSky.js';
import { bakeEnvironment } from '../world/environmentBake.js';
import { Mists, useHeightFog } from '../world/heightFog.js';
import { buildDrawingRoom } from './fennDrawingRoom.js';
import { buildLawnAndDrive, plantTreelines, poolsBeforeWindows } from './fennGrounds.js';
import { standMrsFenn } from './fennSilhouette.js';

const Evening = { skyFill: srgb(0.3, 0.4, 0.68), groundFill: srgb(0.04, 0.045, 0.05), fillIntensity: 0.55, haze: srgb(0.3, 0.3, 0.38), fog: 0.004 };
const Moonrise = { colour: srgb(0.55, 0.65, 0.95), intensity: 0.35, direction: new Vector3(-0.45, 0.6, 0.65), focus: new Vector3(3, 0, 8), span: 22 };
const Parking = { place: new Vector3(7.2, 0, 12.6), heading: -2.35 };
const Reflections = { intensity: 0.5, skyScale: 0.04 };
const fennStands = { along: -0.12, behindGlass: -0.8, heading: 0.55 };

function moonrise() {
    const moon = new DirectionalLight(Moonrise.colour, Moonrise.intensity);
    moon.position.copy(Moonrise.direction).multiplyScalar(60).add(Moonrise.focus);
    const aim = moon.target;
    aim.position.copy(Moonrise.focus);
    moon.castShadow = true;
    const { shadow } = moon;
    shadow.mapSize.set(1024, 1024);
    const { camera } = shadow;
    Object.assign(camera, { left: -Moonrise.span, right: Moonrise.span, top: Moonrise.span, bottom: -Moonrise.span, near: 10, far: 140 });
    camera.updateProjectionMatrix();
    return [new HemisphereLight(Evening.skyFill, Evening.groundFill, Evening.fillIntensity), moon, aim];
}

function eveningReflections() {
    const surroundings = new Scene();
    const sky = createDuskSky();
    sky.scale.setScalar(Reflections.skyScale);
    const lawn = new Mesh(new PlaneGeometry(400, 400), new MeshBasicMaterial({ color: srgb(0.02, 0.025, 0.02) }));
    lawn.rotation.set(-Math.PI / 2, 0, 0);
    const windows = new Mesh(new BoxGeometry(22, 6, 0.5), new MeshBasicMaterial({ color: srgb(0.5, 0.32, 0.16) }));
    windows.position.set(-6, 4, -12);
    surroundings.add(sky, lawn, windows);
    return surroundings;
}

function parkTheSaloon(reflections) {
    const saloon = buildSaloon();
    saloon.position.copy(Parking.place);
    saloon.rotation.set(0, Parking.heading, 0);
    saloon.traverse((part) => Object.assign(part.material ?? {}, { envMap: reflections.texture, envMapIntensity: Reflections.intensity }));
    return saloon;
}

function mrsFennAtHerWindow(watched) {
    const floor = storeyFloor(watched.storeyIndex);
    const fenn = standMrsFenn(new Vector3(watched.x + fennStands.along, floor, fennStands.behindGlass), fennStands.heading);
    return { parts: [buildDrawingRoom(watched, floor), fenn.figure], ember: fenn.ember };
}

export function buildFennHouseSet({ renderer }) {
    useHeightFog(Mists.thin);
    const scene = new Scene();
    scene.fog = new FogExp2(Evening.haze, Evening.fog);
    const reflections = bakeEnvironment(renderer, eveningReflections());
    const { house, watched } = buildGeorgianHouse(createBuildingMaterials());
    const atTheWindow = mrsFennAtHerWindow(watched);
    const pools = poolsBeforeWindows(windowOpenings().filter((opening) => opening.storeyIndex === 0 && !opening.isDoorway));
    scene.add(createDuskSky(), buildLawnAndDrive(pools), house, parkTheSaloon(reflections), ...atTheWindow.parts, ...plantTreelines(), ...moonrise());
    return { scene, reflections, ember: atTheWindow.ember };
}
