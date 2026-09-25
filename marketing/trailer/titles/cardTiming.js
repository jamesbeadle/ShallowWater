export const Fades = { quick: 0.12, gentle: 0.45, slow: 0.9 };

export function presenceOf(card, time, fadeSeconds) {
    const sinceStart = time - card.start;
    const untilEnd = card.end - time;
    const fadingIn = Math.min(sinceStart / fadeSeconds, 1);
    const fadingOut = Math.min(untilEnd / fadeSeconds, 1);
    return Math.max(0, Math.min(fadingIn, fadingOut));
}

export function progressThrough(card, time) {
    return (time - card.start) / (card.end - card.start);
}

export function secondsInto(card, time) {
    return time - card.start;
}
