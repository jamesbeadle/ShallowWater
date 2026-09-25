export const RangeClearing = { xFrom: 18, xTo: 232, acrossFrom: -372, acrossTo: -10 };
export const Ride = { x: 300, halfWidth: 4.5, acrossFrom: -392, acrossTo: -34 };
export const Village = { xFrom: -45, xTo: 115, acrossFrom: 14, acrossTo: 130 };
export const HopwasBridge = { x: 0, halfWidth: 9 };
export const Butts = { x: 125, across: -150 };
export const Moorings = { halcyon: -56, patience: 74, offset: 4.3 };
export const Pub = { x: 30, across: 25 };
export const FiringPoint = { x: 125, across: -300 };
export const Summit = { x: 214, across: -380 };

export function isInside(area, x, across) {
    const isAlong = x > area.xFrom && x < area.xTo;
    return isAlong && across > area.acrossFrom && across < area.acrossTo;
}

export function isOnRide(x, across) {
    return Math.abs(x - Ride.x) < Ride.halfWidth && across > Ride.acrossFrom && across < Ride.acrossTo;
}

export function isOpenGround(x, across) {
    return isInside(RangeClearing, x, across) || isOnRide(x, across);
}
