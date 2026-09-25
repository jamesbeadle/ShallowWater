export const Georgian = {
    bays: 7,
    bayWidth: 3,
    depth: 13,
    plinth: 0.6,
    wallThickness: 0.5,
    storeys: [
        { height: 4.2, window: { width: 1.25, height: 2.6, sill: 0.8 } },
        { height: 4.0, window: { width: 1.25, height: 2.75, sill: 0.45 } },
        { height: 2.8, window: { width: 1.25, height: 1.45, sill: 0.6 } },
    ],
    door: { width: 1.5, height: 2.7 },
    cornice: 0.55,
    parapet: 0.9,
    roofRise: 3.2,
};

export const Facade = {
    width: Georgian.bays * Georgian.bayWidth,
    height: Georgian.plinth + Georgian.storeys.reduce((total, storey) => total + storey.height, 0),
};

export function storeyFloor(storeyIndex) {
    return Georgian.plinth + Georgian.storeys.slice(0, storeyIndex).reduce((total, storey) => total + storey.height, 0);
}

export function bayCentre(bayIndex) {
    return -Facade.width / 2 + (bayIndex + 0.5) * Georgian.bayWidth;
}

export function windowOpenings() {
    const middleBay = Math.floor(Georgian.bays / 2);
    return Georgian.storeys.flatMap((storey, storeyIndex) => Array.from({ length: Georgian.bays }, (unused, bayIndex) => {
        const { width, height, sill } = storey.window;
        const bottom = storeyFloor(storeyIndex) + sill;
        return { storeyIndex, bayIndex, x: bayCentre(bayIndex), bottom, width, height, isDoorway: storeyIndex === 0 && bayIndex === middleBay };
    }));
}
