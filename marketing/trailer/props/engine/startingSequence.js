import { easeInOut, easeOut } from '../../shots/cameraMoves.js';

const SoundNames = { blowlamp: 'blowlamp', kick: 'flywheelKick', cough: 'engineCough', beat: 'engineBeat' };
const Push = { angle: 1.1 };
const Rolling = { startSpeed: 2.6, acceleration: 3.4 };
const Running = { turnsPerBeat: 1, surge: 0.22 };
const Shudder = { cough: 0.45, beat: 1, decay: 7, frequency: 55 };
const Blowlamp = { dying: 0.22, withdrawing: 0.9, reach: 0.55 };
const Glow = { cold: 0.6, hot: 0.82, fireFlare: 0.22, flareDecay: 3.5 };

export function startingSequenceOf(edit, shot) {
    const isWithinShot = (time) => time >= shot.start && time < shot.end;
    const cueTimes = (name) => edit.timesOf(name).filter(isWithinShot).map((time) => time - shot.start);
    const { cues } = edit.sound;
    const blowlamp = cues.find((cue) => cue.sound === SoundNames.blowlamp && isWithinShot(cue.start));
    const [kick] = cueTimes(SoundNames.kick);
    const [cough] = cueTimes(SoundNames.cough);
    const beats = cueTimes(SoundNames.beat);
    return { kick, cough, beats, firstBeat: beats[0], lampOut: blowlamp.end - shot.start };
}

export function flywheelAngleAt(sequence, time) {
    if (time < sequence.kick) {
        return 0;
    }
    const pushed = Push.angle * easeOut((time - sequence.kick) / (sequence.cough - sequence.kick));
    if (time < sequence.cough) {
        return pushed;
    }
    const rolling = Math.min(time, sequence.firstBeat) - sequence.cough;
    const rolled = pushed + Rolling.startSpeed * rolling + (Rolling.acceleration * rolling * rolling) / 2;
    if (time < sequence.firstBeat) {
        return rolled;
    }
    const running = (time - sequence.firstBeat) * Math.PI * 2 * Running.turnsPerBeat;
    return rolled + running + Running.surge * Math.sin(running);
}

function jolt(since, strength) {
    return since < 0 ? 0 : strength * Math.exp(-since * Shudder.decay) * Math.sin(since * Shudder.frequency);
}

export function shudderAt(sequence, time) {
    const beats = sequence.beats.reduce((total, beat) => total + jolt(time - beat, Shudder.beat), 0);
    return beats + jolt(time - sequence.cough, Shudder.cough);
}

export function blowlampAt(sequence, time) {
    const sinceOut = time - sequence.lampOut;
    const burning = 1 - Math.min(Math.max(sinceOut / Blowlamp.dying, 0), 1);
    const withdrawn = easeInOut(sinceOut / Blowlamp.withdrawing) * Blowlamp.reach;
    return { burning, withdrawn };
}

export function bulbGlowAt(sequence, time) {
    const heating = Glow.cold + (Glow.hot - Glow.cold) * Math.min(time / sequence.lampOut, 1);
    const sinceFiring = sequence.beats.filter((beat) => beat <= time).map((beat) => time - beat);
    return heating + sinceFiring.reduce((total, since) => total + Glow.fireFlare * Math.exp(-since * Glow.flareDecay), 0);
}
