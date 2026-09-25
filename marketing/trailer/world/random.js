const Mulberry = { increment: 0x6d2b79f5, shiftA: 15, shiftB: 7, shiftC: 14, range: 4294967296 };

export function createRandom(seed) {
    let state = seed >>> 0;
    return () => {
        state = (state + Mulberry.increment) >>> 0;
        let mixed = state;
        mixed = Math.imul(mixed ^ (mixed >>> Mulberry.shiftA), mixed | 1);
        mixed ^= mixed + Math.imul(mixed ^ (mixed >>> Mulberry.shiftB), mixed | 61);
        return ((mixed ^ (mixed >>> Mulberry.shiftC)) >>> 0) / Mulberry.range;
    };
}

export function between(random, minimum, maximum) {
    return minimum + (maximum - minimum) * random();
}

export function pick(random, choices) {
    return choices[Math.floor(random() * choices.length)];
}

export function spread(random, amount) {
    return (random() - 0.5) * 2 * amount;
}
