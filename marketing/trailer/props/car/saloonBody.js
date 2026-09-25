import { BoxGeometry, ExtrudeGeometry, Mesh, PlaneGeometry } from 'three';
import { Profiles, Saloon, Wings, closedShape, wingShape } from './saloonShapes.js';

const RunningBoard = { from: -1.05, to: 0.42, height: 0.36, thickness: 0.04, width: 0.2 };
const Windscreen = { width: 1.18, height: 0.54, lean: 0.27, base: [0.553, 1.307] };

function extrudedAcross(shape, depth, bevel, material) {
    const options = { depth, bevelEnabled: bevel > 0, bevelThickness: bevel, bevelSize: bevel, bevelOffset: -bevel, bevelSegments: 4, curveSegments: 12 };
    const geometry = new ExtrudeGeometry(shape, options);
    geometry.translate(0, 0, -depth / 2);
    const mesh = new Mesh(geometry, material);
    mesh.castShadow = true;
    mesh.receiveShadow = true;
    return mesh;
}

function wingPair(wing, material) {
    return [-1, 1].map((side) => {
        const mudguard = extrudedAcross(wingShape(wing), Saloon.wingDepth, Saloon.wingBevel, material);
        mudguard.position.setZ(side * Saloon.wingInset);
        return mudguard;
    });
}

function runningBoards(material) {
    return [-1, 1].map((side) => {
        const length = RunningBoard.to - RunningBoard.from;
        const board = new Mesh(new BoxGeometry(length, RunningBoard.thickness, RunningBoard.width), material);
        board.position.set((RunningBoard.from + RunningBoard.to) / 2, RunningBoard.height, side * (Saloon.wingInset + 0.02));
        return board;
    });
}

function windscreen(glass) {
    const pane = new Mesh(new PlaneGeometry(Windscreen.width, Windscreen.height), glass);
    pane.rotation.set(0, Math.PI / 2, Windscreen.lean, 'ZYX');
    pane.position.set(Windscreen.base[0], Windscreen.base[1], 0);
    return pane;
}

export function buildSaloonBody({ paint, glass, rubber }) {
    const cabin = extrudedAcross(closedShape(Profiles.cabin), Saloon.cabinDepth, Saloon.cabinBevel, paint);
    const bonnet = extrudedAcross(closedShape(Profiles.bonnet), Saloon.bonnetDepth, Saloon.bonnetBevel, paint);
    const windowDepth = Saloon.cabinDepth + Saloon.cabinBevel * 2 + 0.01;
    const windows = extrudedAcross(closedShape(Profiles.sideWindows), windowDepth, 0, glass);
    return [cabin, bonnet, windows, windscreen(glass), ...wingPair(Wings.front, paint), ...wingPair(Wings.rear, paint), ...runningBoards(rubber)];
}
