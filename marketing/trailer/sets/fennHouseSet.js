import { BoxGeometry, DirectionalLight, FogExp2, HemisphereLight, Mesh, MeshBasicMaterial, PlaneGeometry, Scene, Vector3 } from 'three';
import { createBuildingMaterials } from '../props/buildings/buildingMaterials.js';
import { buildGeorgianHouse } from '../props/buildings/georgianHouse.js';
import { storeyFloor, windowOpenings } from '../props/buildings/georgianPlan.js';
import { buildSaloon } from '../props/car/saloon.js';
import { srgb } from '../world/colours.js';
import { createDuskSky, Dusk } from '../world/duskSky.js';
import { bakeEnvironment } from '../world/environmentBake.js';
import { plantForest } from '../world/forest.js';
import { Mists, useHeightFog } from '../world/heightFog.js';
import { buildDrawingRoom } from './fennDrawingRoom.js';
import { buildLawnAndDrive, poolsBeforeWindows, treePlacements } from './fennGrounds.js';
import { standMrsFenn } from './fennSilhouette.js';

const Evening = { skyFill: srgb(0.3, 0.4, 0.68), groundFill: srgb(0.04, 0.045, 0.05), fillIntensity: 0.7, haze: srgb(0.16, 0.18, 0.26), fog: 0.0045 };
const Afterglow = { colour: srgb(1.0, 0.58, 0.36), intensity: 0.45 };
const Moonrise = { colour: srgb(0.55, 0.65, 0.95), intensity: 0.3, direction: new Vector3(-0.45, 0.6, 0.65), focus: new Vector3(3, 0, 8), span: 22 };
const Parking = { place: new Vector3(6.4, 0, 13.2), heading: 2.75 };
const Reflections = { intensity: 0.7, skyScale: 0.04 };
const fennStands = { along: 0.12, behindGlass: -1.35, heading: 0.18 };

function lightTheEvening() {
    const fill = new HemisphereLight(Evening.skyFill, Evening.groundFill, Evening.fillIntensity);
    const moon = new DirectionalLight(Moonrise.colour, Moonrise.intensity);
    moon.position.copy(Moonrise.direction).multiplyScalar(60).add(Moonrise.focus);
    const aim = moon.target;
    aim.position.copy(Moonrise.focus);
    moon.castShadow = true;
    const { shadow } = moon;
    shadow.mapSize.set(2048, 2048);
    const { camera } = shadow;
    Object.assign(camera, { left: -Moonrise.span, right: Moonrise.span, top: Moonrise.span, bottom: -Moonrise.span, near: 10, far: 140 });
    camera.updateProjectionMatrix();
    const glow = new DirectionalLight(Afterglow.colour, Afterglow.intensity);
    glow.position.copy(Dusk.afterglowDirection).multiplyScalar(80);
    return [fill, moon, aim, glow];
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

function mrsFennAtHerWindow(watched) {
    const floor = storeyFloor(watched.storeyIndex);
    const drawingRoom = buildDrawingRoom(watched, floor);
    const fenn = standMrsFenn(new Vector3(watched.x + fennStands.along, floor, fennStands.behindGlass), fennStands.heading);
    return { parts: [drawingRoom.room, fenn.figure], roomLight: drawingRoom.light, ember: fenn.ember };
}

export function buildFennHouseSet({ renderer }) {
    useHeightFog(Mists.thin);
    const scene = new Scene();
    scene.fog = new FogExp2(Evening.haze, Evening.fog);
    const reflections = bakeEnvironment(renderer, eveningReflections());
    Object.assign(scene, { environment: reflections.texture, environmentIntensity: Reflections.intensity });
    const { house, watched } = buildGeorgianHouse(createBuildingMaterials());
    const atTheWindow = mrsFennAtHerWindow(watched);
    const saloon = buildSaloon();
    saloon.position.copy(Parking.place);
    saloon.rotation.set(0, Parking.heading, 0);
    const pools = poolsBeforeWindows(windowOpenings().filter((opening) => opening.storeyIndex === 0 && !opening.isDoorway));
    const trees = plantForest(treePlacements(), { direction: Dusk.afterglowDirection, colour: Afterglow.colour });
    scene.add(createDuskSky(), buildLawnAndDrive(pools), house, saloon, ...atTheWindow.parts, ...trees, ...lightTheEvening());
    return { scene, reflections, roomLight: atTheWindow.roomLight, ember: atTheWindow.ember };
}
