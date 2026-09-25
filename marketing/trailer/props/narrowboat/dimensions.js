export const Boat = {
    length: 21.3,
    beam: 2.13,
    depth: 1.05,
    draft: 0.78,
    bowLength: 4.6,
    sternLength: 2.8,
    bowRise: 0.62,
    sternRise: 0.16,
};

export const Cabin = { length: 3.1, height: 0.95, inset: 0.06, camber: 0.07, sternDeck: 0.55 };
export const EngineRoom = { length: 2.2, height: 0.82 };
export const Hold = { plankHeight: 0.92, foreDeck: 1.4, stringSpacing: 0.62 };

export function freeboard() {
    return Boat.depth - Boat.draft;
}

export function sternX() {
    return -Boat.length / 2;
}
