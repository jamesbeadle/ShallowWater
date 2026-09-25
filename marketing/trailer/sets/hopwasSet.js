import { FogExp2, Scene } from 'three';
import { centreZ } from '../world/canal.js';
import { canalRibbon } from '../world/waterRibbons.js';
import { createCanalWater } from '../world/canalWater.js';
import { createSky } from '../world/dawnSky.js';
import { plantForest } from '../world/forest.js';
import { createSkyFill, createSunlight } from '../world/sunlight.js';
import { createTerrainMaterial } from '../world/terrainMaterial.js';
import { buildCanalTerrain } from '../world/terrainMesh.js';
import { Mists, useHeightFog } from '../world/heightFog.js';
import { frustumsAlong, isSeenByAny } from '../world/viewCulling.js';
import { hopwasFieldPlacements } from './hopwasFields.js';
import { hopwasColour, hopwasHeight } from './hopwasLand.js';
import { hopwasTreePlacements } from './hopwasTrees.js';
import { buildHopwasVillage } from './hopwasVillage.js';
import { buildHopwasRange } from './hopwasRange.js';
import { createBuildingMaterials } from '../props/buildings/buildingMaterials.js';

const Extent = { along: { from: -900, to: 900, step: 3 }, across: { from: -760, to: 620 } };
const WaterRibbon = { centreZ, from: -900, to: 900, halfWidth: 7.6, step: 3 };

function addLight(scene, daylight, shadow) {
    const sun = { direction: daylight.sunDirection, colour: daylight.sunColour };
    const [sunlight, aim] = createSunlight({ ...sun, intensity: daylight.sunIntensity, focus: shadow.focus, span: shadow.span });
    const fill = createSkyFill({ sky: daylight.skyFill, ground: daylight.groundFill, intensity: daylight.fillIntensity });
    scene.add(sunlight, aim, fill);
    return sun;
}

function visibleTrees(view) {
    const frustums = frustumsAlong(view.lens, view.aspect, view.viewpoints);
    const focus = view.viewpoints.map(({ position }) => ({ x: position.x, z: position.z, radius: view.nearRadius }));
    const everyPlant = [...hopwasTreePlacements(Extent.along, focus), ...hopwasFieldPlacements(Extent.along, focus)];
    return everyPlant.filter((placement) => isSeenByAny(frustums, placement));
}

export function buildHopwasSet({ daylight, view, shadow, fogDensity = daylight.fogDensity, mist = Mists.valley, village: villageOptions = {} }) {
    useHeightFog(mist);
    const scene = new Scene();
    scene.fog = new FogExp2(daylight.fogColour, fogDensity);
    const sky = createSky(daylight.sunDirection, daylight.weather);
    const material = createTerrainMaterial();
    const terrain = buildCanalTerrain({ ...Extent, heightAt: hopwasHeight, colourAt: hopwasColour, material });
    const sun = addLight(scene, daylight, shadow);
    const water = createCanalWater({ geometry: canalRibbon(WaterRibbon), bodyColour: daylight.waterBody, sun });
    scene.add(sky, terrain.mesh, water, ...plantForest(visibleTrees(view), sun));
    const time = { value: 0 };
    const village = buildHopwasVillage({ groundAt: terrain.groundAt, materials: createBuildingMaterials(), ...villageOptions });
    scene.add(village.village, buildHopwasRange(terrain.groundAt, time));
    return { scene, sky, water, sun, time, village, groundAt: terrain.groundAt };
}
