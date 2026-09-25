import { BoxGeometry, ExtrudeGeometry, Mesh, Path, Shape } from 'three';
import { mergeGeometries } from 'three/addons/utils/BufferGeometryUtils.js';
import { Facade, Georgian, storeyFloor, windowOpenings } from './georgianPlan.js';

const corniceProfile = [[0, 0], [0.12, 0.06], [0.16, 0.18], [0.34, 0.3], [0.52, 0.4], [0.55, 0.55], [0, 0.55]];
const Band = { height: 0.2, projection: 0.12 };
const Coping = { height: 0.12, overhang: 0.08 };

function rectangularHole({ x, bottom, width, height }) {
    const hole = new Path();
    hole.moveTo(x - width / 2, bottom);
    [[x + width / 2, bottom], [x + width / 2, bottom + height], [x - width / 2, bottom + height]].forEach(([pointX, pointY]) => hole.lineTo(pointX, pointY));
    return hole;
}

function doorwayHole({ x, bottom }) {
    const { width, height } = Georgian.door;
    const hole = new Path();
    hole.moveTo(x - width / 2, bottom);
    hole.lineTo(x + width / 2, bottom);
    hole.lineTo(x + width / 2, bottom + height);
    hole.absarc(x, bottom + height, width / 2, 0, Math.PI, false);
    return hole;
}

function frontWall(material) {
    const outline = new Shape();
    outline.moveTo(-Facade.width / 2, 0);
    [[Facade.width / 2, 0], [Facade.width / 2, Facade.height], [-Facade.width / 2, Facade.height]].forEach(([x, y]) => outline.lineTo(x, y));
    const doorway = { x: 0, bottom: Georgian.plinth };
    outline.holes.push(...windowOpenings().filter((opening) => !opening.isDoorway).map(rectangularHole), doorwayHole(doorway));
    const geometry = new ExtrudeGeometry(outline, { depth: Georgian.wallThickness, bevelEnabled: false });
    geometry.translate(0, 0, -Georgian.wallThickness);
    return new Mesh(geometry, material);
}

function corniceGeometry(length) {
    const profile = new Shape();
    profile.moveTo(corniceProfile[0][0], corniceProfile[0][1]);
    corniceProfile.slice(1).forEach(([depth, height]) => profile.lineTo(depth, height));
    const geometry = new ExtrudeGeometry(profile, { depth: length, bevelEnabled: false });
    geometry.rotateY(-Math.PI / 2);
    geometry.translate(length / 2, Facade.height, 0);
    return geometry;
}

function bandsAndParapet() {
    const bands = [1, 2].map((storeyIndex) => {
        const band = new BoxGeometry(Facade.width + 0.3, Band.height, Band.projection * 2);
        band.translate(0, storeyFloor(storeyIndex), 0);
        return band.toNonIndexed();
    });
    const parapetTop = Facade.height + Georgian.cornice + Georgian.parapet;
    const parapet = new BoxGeometry(Facade.width + 0.6, Georgian.parapet, 0.4);
    parapet.translate(0, parapetTop - Georgian.parapet / 2, 0.1);
    const coping = new BoxGeometry(Facade.width + 0.6 + Coping.overhang * 2, Coping.height, 0.4 + Coping.overhang * 2);
    coping.translate(0, parapetTop, 0.1);
    return [...bands, parapet.toNonIndexed(), coping.toNonIndexed()];
}

export function buildGeorgianFacade(stucco) {
    const trim = new Mesh(mergeGeometries([corniceGeometry(Facade.width + 0.6), ...bandsAndParapet()]), stucco);
    return [frontWall(stucco), trim];
}
