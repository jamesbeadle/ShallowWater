import { ExtrudeGeometry, Group, Mesh, MeshStandardMaterial, Vector3 } from 'three';
import { srgb } from '../../world/colours.js';
import { createRim, withRim } from '../figure/rimLight.js';
import { Outlines, outlineShape } from './profileOutlines.js';

const Pieces = {
    head: { depth: 0.13, bevel: 0.014, segments: 7 },
    cap: { depth: 0.18, bevel: 0.02, segments: 5 },
    peak: { depth: 0.15, bevel: 0.004, segments: 3 },
    ear: { depth: 0.012, bevel: 0.008, segments: 3 },
    collar: { depth: 0.15, bevel: 0.022, segments: 5 },
    shoulders: { depth: 0.3, bevel: 0.05, segments: 6 },
};
const Cloth = {
    skin: { colour: srgb(0.42, 0.28, 0.22), roughness: 0.5, rimShare: 1 },
    tweed: { colour: srgb(0.15, 0.13, 0.11), roughness: 0.95, rimShare: 0.6 },
    wool: { colour: srgb(0.04, 0.038, 0.04), roughness: 0.9, rimShare: 0.12 },
};
const earPlace = new Vector3(0.012, -0.004, 0);
const Turn = { awayFromCamera: -0.15 };
export const Mouth = new Vector3(-0.114, -0.066, 0.03).applyAxisAngle(new Vector3(0, 1, 0), Turn.awayFromCamera);

function extruded(points, piece, material) {
    const bevel = { bevelEnabled: true, bevelThickness: piece.bevel, bevelSize: piece.bevel, bevelOffset: -piece.bevel * 0.5, bevelSegments: piece.segments };
    const geometry = new ExtrudeGeometry(outlineShape(points), { depth: piece.depth, curveSegments: 10, ...bevel });
    geometry.translate(0, 0, -piece.depth / 2);
    return new Mesh(geometry, material);
}

function fabric(cloth, rim) {
    const material = new MeshStandardMaterial({ color: cloth.colour, roughness: cloth.roughness, metalness: 0 });
    return withRim(material, createRim(rim.colour, rim.strength * cloth.rimShare, rim.direction));
}

export function buildProfileBust(rim) {
    const [skin, tweed, wool] = [Cloth.skin, Cloth.tweed, Cloth.wool].map((cloth) => fabric(cloth, rim));
    const ear = extruded(Outlines.ear, Pieces.ear, skin);
    const { head } = Pieces;
    ear.position.copy(earPlace).setZ(head.depth / 2);
    const bust = new Group();
    bust.add(extruded(Outlines.head, head, skin), ear, extruded(Outlines.cap, Pieces.cap, tweed), extruded(Outlines.peak, Pieces.peak, tweed));
    bust.add(extruded(Outlines.collar, Pieces.collar, wool), extruded(Outlines.shoulders, Pieces.shoulders, wool));
    bust.rotation.set(0, Turn.awayFromCamera, 0);
    return bust;
}
