import { BoxGeometry, CircleGeometry, ExtrudeGeometry, Group, LatheGeometry, Mesh, MeshStandardMaterial, PointLight, Shape, Vector2 } from 'three';
import { mergeGeometries } from 'three/addons/utils/BufferGeometryUtils.js';
import { srgb } from '../../world/colours.js';
import { Georgian } from './georgianPlan.js';

const Porch = { width: 6.6, depth: 2.75, columnHeight: 4.0, entablature: 0.65, pedimentRise: 1.25, columnZ: 2.3 };
const columnXs = [-2.75, -0.95, 0.95, 2.75];
const columnProfile = [[0, 0], [0.34, 0], [0.34, 0.12], [0.3, 0.14], [0.31, 0.2], [0.26, 0.24], [0.25, 0.3], [0.245, 1.6], [0.23, 3.0],
    [0.215, 3.62], [0.225, 3.64], [0.212, 3.7], [0.28, 3.8], [0.3, 3.86], [0, 3.86]];
const Steps = [{ width: 7.0, forward: 3.0 }, { width: 7.6, forward: 3.35 }, { width: 8.2, forward: 3.7 }];
const Lantern = { colour: srgb(1.0, 0.7, 0.38), intensity: 5, reach: 12, height: 4.25, forward: 1.3 };

function columnGeometry(x) {
    const shaft = new LatheGeometry(columnProfile.map(([radius, height]) => new Vector2(radius, height)), 24).toNonIndexed();
    const abacus = new BoxGeometry(0.68, 0.14, 0.68).toNonIndexed();
    abacus.translate(0, Porch.columnHeight - 0.07, 0);
    const column = mergeGeometries([shaft, abacus]);
    column.translate(x, Georgian.plinth, Porch.columnZ);
    return column;
}

function pedimentGeometry(base) {
    const gable = new Shape();
    gable.moveTo(-Porch.width / 2 - 0.2, 0);
    gable.lineTo(Porch.width / 2 + 0.2, 0);
    gable.lineTo(0, Porch.pedimentRise);
    const geometry = new ExtrudeGeometry(gable, { depth: Porch.depth + 0.25, bevelEnabled: false });
    geometry.translate(0, base, -0.1);
    return geometry;
}

function roofAndSteps() {
    const entablatureBase = Georgian.plinth + Porch.columnHeight;
    const entablature = new BoxGeometry(Porch.width + 0.4, Porch.entablature, Porch.depth + 0.3).toNonIndexed();
    entablature.translate(0, entablatureBase + Porch.entablature / 2, Porch.depth / 2);
    const steps = Steps.map(({ width, forward }, index) => {
        const height = Georgian.plinth - (index * Georgian.plinth) / Steps.length;
        const step = new BoxGeometry(width, height, forward).toNonIndexed();
        step.translate(0, height / 2, forward / 2);
        return step;
    });
    return [entablature, pedimentGeometry(entablatureBase + Porch.entablature), ...steps];
}

function doorAndFanlight() {
    const { width, height } = Georgian.door;
    const door = new Mesh(new BoxGeometry(width, height, 0.08), new MeshStandardMaterial({ color: srgb(0.03, 0.05, 0.04), roughness: 0.25, metalness: 0.1 }));
    door.position.set(0, Georgian.plinth + height / 2, -0.32);
    const fanlightGlass = new MeshStandardMaterial({ color: srgb(0.1, 0.08, 0.05), emissive: Lantern.colour, emissiveIntensity: 2.2 });
    const fanlight = new Mesh(new CircleGeometry(width / 2, 24, 0, Math.PI), fanlightGlass);
    fanlight.position.set(0, Georgian.plinth + height, -0.3);
    return [door, fanlight];
}

function hangingLantern() {
    const glass = new Mesh(new BoxGeometry(0.34, 0.5, 0.34), new MeshStandardMaterial({ color: srgb(0.2, 0.15, 0.1), emissive: Lantern.colour, emissiveIntensity: 4 }));
    glass.position.set(0, Lantern.height, Lantern.forward);
    const light = new PointLight(Lantern.colour, Lantern.intensity, Lantern.reach, 1.6);
    light.position.set(0, Lantern.height - 0.15, Lantern.forward);
    return [glass, light];
}

export function buildPortico(stucco) {
    const portico = new Group();
    const masonry = mergeGeometries([...columnXs.map(columnGeometry), ...roofAndSteps()]);
    const stone = new Mesh(masonry, stucco);
    stone.castShadow = true;
    stone.receiveShadow = true;
    portico.add(stone, ...doorAndFanlight(), ...hangingLantern());
    return portico;
}
