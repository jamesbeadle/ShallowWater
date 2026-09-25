export class Edit {
    constructor(document) {
        Object.assign(this, document);
        this.frameCount = Math.round(document.duration * document.framesPerSecond);
    }

    timeOf(frameNumber) {
        return frameNumber / this.framesPerSecond;
    }

    shotAt(time) {
        const lastShot = this.shots.at(-1);
        return this.shots.find((shot) => time >= shot.start && time < shot.end) ?? lastShot;
    }

    shotNamed(name) {
        return this.shots.find((shot) => shot.name === name);
    }

    cardsAt(time) {
        return this.cards.filter((card) => time >= card.start && time < card.end);
    }

    timesOf(soundName) {
        const { cues } = this.sound;
        return cues.filter((cue) => cue.sound === soundName).flatMap(expandCue);
    }
}

function expandCue(cue) {
    if (cue.at !== undefined) {
        return [cue.at];
    }
    const count = Math.round((cue.end - cue.start) / cue.every);
    return Array.from({ length: count }, (unused, index) => cue.start + index * cue.every);
}
