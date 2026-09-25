import { Vector3 } from 'three';
import { srgb } from './colours.js';

export const TreeKinds = {
    oak: {
        trunk: { height: 5.5, bottomRadius: 0.5, topRadius: 0.3 },
        canopy: { centre: new Vector3(0, 8.6, 0), extent: new Vector3(3.4, 2.2, 3.4), blobCount: { near: 6, far: 3 }, blobRadius: [2.3, 3.5] },
        colours: [srgb(0.6, 0.38, 0.13), srgb(0.5, 0.33, 0.12), srgb(0.66, 0.5, 0.2), srgb(0.44, 0.4, 0.17), srgb(0.55, 0.28, 0.11)],
        bark: srgb(0.16, 0.13, 0.11),
    },
    birch: {
        trunk: { height: 8, bottomRadius: 0.22, topRadius: 0.12 },
        canopy: { centre: new Vector3(0, 9.2, 0), extent: new Vector3(1.3, 3.2, 1.3), blobCount: { near: 5, far: 3 }, blobRadius: [1.3, 2.0] },
        colours: [srgb(0.8, 0.62, 0.18), srgb(0.74, 0.66, 0.24), srgb(0.66, 0.5, 0.14)],
        bark: srgb(0.62, 0.6, 0.55),
    },
    pine: {
        trunk: { height: 12, bottomRadius: 0.38, topRadius: 0.2 },
        canopy: { centre: new Vector3(0, 13.4, 0), extent: new Vector3(2.4, 1.1, 2.4), blobCount: { near: 5, far: 3 }, blobRadius: [1.6, 2.4] },
        colours: [srgb(0.1, 0.17, 0.1), srgb(0.13, 0.2, 0.11), srgb(0.09, 0.14, 0.1)],
        bark: srgb(0.3, 0.17, 0.1),
    },
    bush: {
        canopy: { centre: new Vector3(0, 1.1, 0), extent: new Vector3(1.4, 0.45, 1.4), blobCount: { near: 3, far: 2 }, blobRadius: [1.0, 1.5] },
        colours: [srgb(0.3, 0.3, 0.13), srgb(0.4, 0.3, 0.13), srgb(0.26, 0.24, 0.12), srgb(0.46, 0.26, 0.12)],
    },
};

export const Detail = { near: 1, far: 0 };
export const farBlobGrowth = 1.25;
