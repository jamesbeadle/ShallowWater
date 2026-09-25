import { Shape, Vector2 } from 'three';

export const Saloon = {
    frontAxle: 1.35,
    rearAxle: -1.65,
    wheelRadius: 0.37,
    track: 0.71,
    cabinDepth: 1.3,
    cabinBevel: 0.08,
    bonnetDepth: 0.8,
    bonnetBevel: 0.06,
    wingDepth: 0.22,
    wingBevel: 0.035,
    wingInset: 0.74,
};

export const Profiles = {
    cabin: [[0.55, 0.52], [0.62, 1.03], [0.47, 1.58], [0.2, 1.65], [-1.1, 1.65], [-1.55, 1.57], [-1.95, 1.3], [-2.3, 1.02], [-2.42, 0.76],
        [-2.38, 0.5], [-1.3, 0.47]],
    bonnet: [[0.6, 0.55], [0.6, 1.03], [1.2, 1.02], [1.95, 0.99], [1.99, 0.62], [1.9, 0.55]],
    sideWindows: [[0.4, 1.12], [0.31, 1.5], [-1.08, 1.53], [-1.48, 1.45], [-1.72, 1.2]],
};

export function closedShape(points) {
    const [first, ...rest] = points.map(([x, y]) => new Vector2(x, y));
    const shape = new Shape();
    shape.moveTo(first.x, first.y);
    rest.forEach((point) => shape.lineTo(point.x, point.y));
    return shape;
}

function arcPoints(centreX, radius, from, to, steps) {
    return Array.from({ length: steps + 1 }, (unused, step) => {
        const angle = from + ((to - from) * step) / steps;
        return [centreX + Math.cos(angle) * radius[0], Saloon.wheelRadius + Math.sin(angle) * radius[1]];
    });
}

export const Wings = {
    front: { axle: Saloon.frontAxle, end: Math.PI, tail: [[-0.9, 0.43], [-0.97, 0.38]] },
    rear: { axle: Saloon.rearAxle, end: Math.PI + 0.25, tail: [] },
};

export function wingShape({ axle, end, tail }) {
    const outer = arcPoints(axle, [0.6, 0.55], -0.08, end, 22);
    const inner = arcPoints(axle, [0.47, 0.46], end, -0.08, 22);
    return closedShape([...outer, ...tail.map(([along, height]) => [axle + along, height]), ...inner]);
}
