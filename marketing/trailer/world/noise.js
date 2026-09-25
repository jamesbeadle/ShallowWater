const Hashing = { first: 127.1, second: 311.7, scale: 43758.5453 };

function hash(x, y) {
    const value = Math.sin(x * Hashing.first + y * Hashing.second) * Hashing.scale;
    return value - Math.floor(value);
}

function smooth(fraction) {
    return fraction * fraction * (3 - 2 * fraction);
}

function mix(from, to, amount) {
    return from + (to - from) * amount;
}

export function valueNoise(x, y) {
    const cellX = Math.floor(x);
    const cellY = Math.floor(y);
    const blendX = smooth(x - cellX);
    const blendY = smooth(y - cellY);
    const lower = mix(hash(cellX, cellY), hash(cellX + 1, cellY), blendX);
    const upper = mix(hash(cellX, cellY + 1), hash(cellX + 1, cellY + 1), blendX);
    return mix(lower, upper, blendY);
}

export function fractalNoise(x, y, octaves = 4) {
    let total = 0;
    let amplitude = 0.5;
    let frequency = 1;
    for (let octave = 0; octave < octaves; octave += 1) {
        total += amplitude * valueNoise(x * frequency, y * frequency);
        amplitude *= 0.5;
        frequency *= 2.03;
    }
    return total;
}

export function smoothstep(edgeFrom, edgeTo, value) {
    const amount = Math.min(Math.max((value - edgeFrom) / (edgeTo - edgeFrom), 0), 1);
    return smooth(amount);
}
