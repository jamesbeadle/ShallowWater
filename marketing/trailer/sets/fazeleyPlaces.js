export const Channel = { halfWidth: 7, bankTop: 0.7, bed: -1.6, coping: 1.2 };
export const Branch = { xFrom: 119, xTo: 131 };
export const A5Bridge = { x: -270 };
export const Mill = { xFrom: -205, xTo: -125, zFrom: 12, zTo: 30, storeys: 4, storeyHeight: 3.6 };
export const MillChimney = { x: -216, z: 22, height: 38 };
export const Wharf = { xFrom: 60, xTo: 110, zFrom: -33, zTo: -13, yardFrom: 34, lampX: 52, lampZ: -9.5 };
export const TollHouse = { x: 108, z: 15 };
export const Chain = { x: 117 };
export const RovingBridge = { x: 152 };
export const Church = { x: -30, z: 125 };
export const Terraces = [
    { x: -85, z: 42, bays: 7, turn: 0 }, { x: -20, z: 42, bays: 6, turn: 0 }, { x: 40, z: 42, bays: 6, turn: 0 },
    { x: -85, z: 72, bays: 7, turn: Math.PI }, { x: -20, z: 72, bays: 6, turn: Math.PI }, { x: 40, z: 76, bays: 5, turn: Math.PI },
    { x: -60, z: 100, bays: 6, turn: 0 }, { x: 20, z: 104, bays: 7, turn: 0 }, { x: -100, z: 150, bays: 7, turn: Math.PI },
    { x: 60, z: 140, bays: 6, turn: Math.PI }, { x: -150, z: 60, bays: 6, turn: 0 },
];

export function distanceToWater(x, z) {
    const fromMain = Math.abs(z) - Channel.halfWidth;
    const branchCentre = (Branch.xFrom + Branch.xTo) / 2;
    const branchHalf = (Branch.xTo - Branch.xFrom) / 2;
    const fromBranch = z > 0 ? Math.abs(x - branchCentre) - branchHalf : Infinity;
    return Math.min(fromMain, fromBranch);
}
