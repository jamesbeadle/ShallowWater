import { FogExp2, HemisphereLight, Scene, Vector3 } from 'three';
import { createBuildingMaterials } from '../props/buildings/buildingMaterials.js';
import { buildChain, chainPoints, hangLinks } from '../props/chain.js';
import { createCanalWater } from '../world/canalWater.js';
import { createSky } from '../world/dawnSky.js';
import { buildGridTerrain } from '../world/gridTerrain.js';
import { useHeightFog } from '../world/heightFog.js';
import { createGradientSky } from '../world/nightSky.js';
import { createSunlight } from '../world/sunlight.js';
import { createTerrainMaterial } from '../world/terrainMaterial.js';
import { joinStrips, rectangleStrip } from '../world/waterRibbons.js';
import { buildFazeleyBridges } from './fazeleyBridges.js';
import { buildFazeleyBuildings } from './fazeleyBuildings.js';
import { fazeleyColour, fazeleyHeight } from './fazeleyLand.js';
import { buildFazeleyLamps } from './fazeleyLamps.js';
import { Chain, Channel } from './fazeleyPlaces.js';

const Ground = { xFrom: -420, xTo: 420, zFrom: -220, zTo: 260, step: 2 };
const WaterStrips = { main: { xFrom: -420, xTo: 420, zFrom: -7.8, zTo: 7.8 }, branch: { xFrom: 118.2, xTo: 131.8, zFrom: 7, zTo: 260 } };
const ChainSpan = { height: 1.35, sag: 0.4 };

function skyFor(time) {
    const isShaderSky = time.skyKind === 'shader';
    return isShaderSky ? createSky(time.sunDirection, time.weather) : createGradientSky(time);
}

function lightFor(scene, time, shadow) {
    const sun = { direction: time.sunDirection, colour: time.sunColour };
    const [light, aim] = createSunlight({ ...sun, intensity: time.sunIntensity, focus: shadow.focus, span: shadow.span });
    scene.add(light, aim, new HemisphereLight(time.skyFill, time.groundFill, time.fillIntensity));
    return sun;
}

function junctionChain() {
    const start = new Vector3(Chain.x, ChainSpan.height, -Channel.halfWidth - 0.6);
    const end = new Vector3(Chain.x, ChainSpan.height, Channel.halfWidth + 0.6);
    const chain = buildChain(start, end, ChainSpan.sag);
    hangLinks(chain, chainPoints(start, end, ChainSpan.sag, chain.count));
    return { ...chain, start, end };
}

export function buildFazeleySet({ time, shadow, buildings = {}, wharfLight = 0 }) {
    useHeightFog(time.mist);
    const scene = new Scene();
    scene.fog = new FogExp2(time.fogColour, time.fogDensity);
    const sun = lightFor(scene, time, shadow);
    const terrain = buildGridTerrain({ ...Ground, heightAt: fazeleyHeight, colourAt: fazeleyColour, material: createTerrainMaterial() });
    const channels = joinStrips([rectangleStrip(WaterStrips.main), rectangleStrip(WaterStrips.branch)]);
    const water = createCanalWater({ geometry: channels, bodyColour: time.waterBody, sun, glintStrength: time.glintStrength });
    const materials = createBuildingMaterials();
    const town = buildFazeleyBuildings(materials, { litShare: time.litShare, ...buildings });
    const chain = junctionChain();
    const isNight = time.litShare > 0.1;
    const sky = skyFor(time);
    scene.add(sky, terrain, water, town.town, buildFazeleyBridges(materials), chain.links, chain.posts, buildFazeleyLamps({ isLit: isNight, wharfLight }));
    return { scene, sky, water, sun, chain, town, materials, groundAt: fazeleyHeight };
}
