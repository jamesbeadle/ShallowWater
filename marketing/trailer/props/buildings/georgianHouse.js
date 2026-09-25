import { BoxGeometry, BufferGeometry, Float32BufferAttribute, Group, Mesh } from 'three';
import { mergeGeometries } from 'three/addons/utils/BufferGeometryUtils.js';
import { buildGeorgianFacade } from './georgianFacade.js';
import { Facade, Georgian, windowOpenings } from './georgianPlan.js';
import { buildPortico } from './portico.js';
import { buildSurrounds, createSashMaterials, glaze } from './sashWindows.js';
import { createStucco } from './stuccoMaterial.js';

const Lighting = {
    plan: [
        ['bright', 'curtained', 'bright', 'door', 'bright', 'bright', 'curtained'],
        ['dark', 'curtained', 'dark', 'bright', 'watched', 'dark', 'curtained'],
        ['dark', 'dark', 'curtained', 'dark', 'dark', 'dark', 'dark'],
    ],
    watched: 'watched',
};
const Chimney = { width: 1.1, depth: 0.8, height: 2.6, placements: [[-9.6, -2.8], [-9.6, -10.2], [9.6, -2.8], [9.6, -10.2]] };

function boxAt(size, [x, y, z]) {
    const box = new BoxGeometry(...size).toNonIndexed();
    box.translate(x, y, z);
    return box;
}

function hipRoof(base) {
    const halfWidth = Facade.width / 2 - 0.3;
    const front = -0.4;
    const back = -Georgian.depth + 0.3;
    const ridgeHalf = halfWidth - (front - back) / 2;
    const ridgeZ = (front + back) / 2;
    const top = base + Georgian.roofRise;
    const corners = { frontLeft: [-halfWidth, base, front], frontRight: [halfWidth, base, front], backRight: [halfWidth, base, back], backLeft: [-halfWidth, base, back] };
    const ridge = { left: [-ridgeHalf, top, ridgeZ], right: [ridgeHalf, top, ridgeZ] };
    const faces = [
        [corners.frontLeft, corners.frontRight, ridge.right], [corners.frontLeft, ridge.right, ridge.left],
        [corners.backRight, corners.backLeft, ridge.left], [corners.backRight, ridge.left, ridge.right],
        [corners.frontRight, corners.backRight, ridge.right], [corners.backLeft, corners.frontLeft, ridge.left],
    ];
    const geometry = new BufferGeometry();
    geometry.setAttribute('position', new Float32BufferAttribute(faces.flat(2), 3));
    geometry.computeVertexNormals();
    return geometry;
}

function bodyGeometry() {
    const wall = Georgian.wallThickness;
    const halfDepth = Georgian.depth / 2;
    const top = Facade.height + Georgian.cornice + Georgian.parapet;
    const sides = [-1, 1].map((side) => boxAt([wall, top, Georgian.depth - wall], [side * (Facade.width / 2 - wall / 2), top / 2, -halfDepth - wall / 2]));
    const back = boxAt([Facade.width, top, wall], [0, top / 2, -Georgian.depth + wall / 2]);
    const stacks = Chimney.placements.map(([x, z]) => boxAt([Chimney.width, Chimney.height + Georgian.roofRise, Chimney.depth], [x, top + Chimney.height / 2, z]));
    return mergeGeometries([...sides, back, ...stacks]);
}

function glazing(sashes) {
    const openings = windowOpenings().filter((opening) => !opening.isDoorway);
    const lightingOf = (opening) => Lighting.plan[opening.storeyIndex][opening.bayIndex];
    const watched = openings.find((opening) => lightingOf(opening) === Lighting.watched);
    const panes = openings.filter((opening) => opening !== watched).map((opening) => glaze(opening, sashes[lightingOf(opening)]));
    return { panes, openings, watched };
}

export function buildGeorgianHouse({ slate }) {
    const stucco = createStucco({ rusticationTop: Georgian.plinth + Georgian.storeys[0].height });
    const { panes, openings, watched } = glazing(createSashMaterials());
    const house = new Group();
    house.add(...buildGeorgianFacade(stucco), new Mesh(bodyGeometry(), stucco), buildSurrounds(openings, stucco), ...panes, buildPortico(stucco));
    house.add(new Mesh(hipRoof(Facade.height + Georgian.cornice + 0.2), slate));
    house.traverse((part) => Object.assign(part, { castShadow: true, receiveShadow: true }));
    return { house, watched, stucco };
}
