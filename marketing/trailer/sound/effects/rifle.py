"""The rifle: a .303 heard close enough to hurt, and the range up the hill heard flat and late from the cut."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise, oscillators
from ..synthesis.envelopes import Attacks
from ..synthesis.layering import laid_at, padded
from ..synthesis.shaping import normalised, saturate
from ..synthesis.timebase import samples_in, time_axis
from ..space.stereo import paired

CRACK_SECONDS = 0.004
SHOCK_SECONDS = 0.0012
BOOM_BAND = (80.0, 400.0)
BOOM_DECAY = 0.03
BOOM_PITCHES = (130.0, 62.0)
BOOM_LEVEL = 0.9
SHOT_SECONDS = 0.35
SHOT_DRIVE = 2.5
CRACK_CEILING = 12000.0
CRACK_DECAY = CRACK_SECONDS / 5.0
BOOM_SWEEP = 0.02
BOOM_TONE_DECAY = 1.8 * BOOM_DECAY
BOOM_TONE_SHARE = 0.8
REPORT_DECAY = 0.012
REPORT_THUMP = (110.0, 0.035, 0.6)
SMEAR_DECAY_SHARE = 0.25
TAIL_BUILD = 0.12
DISTANT_BAND = (200.0, 2000.0)
DISTANT_SECONDS = 0.25
WOOD_DELAY = 0.35
WOOD_LEVEL = 0.45
WOOD_SMEAR = 0.07
WOOD_CUTOFF = 1300.0
TAIL_SECONDS = 2.8
TAIL_DECAY = 0.55
TAIL_CUTOFF = 900.0
TAIL_LEVEL = 0.16


def shock_wave() -> np.ndarray:
    """The supersonic crack: a pressure jump that ramps through to its opposite and snaps back."""
    length = samples_in(SHOCK_SECONDS)
    return np.linspace(1.0, -1.0, length)


def crack(randomness: Generator) -> np.ndarray:
    length = samples_in(CRACK_SECONDS)
    hiss = noise.white(length, randomness) * envelopes.strike(length, Attacks.spark, CRACK_DECAY)
    return normalised(filters.lowpass(hiss + padded(shock_wave(), length), CRACK_CEILING))


def boom(randomness: Generator) -> np.ndarray:
    moments = time_axis(samples_in(SHOT_SECONDS))
    pitch = BOOM_PITCHES[1] + (BOOM_PITCHES[0] - BOOM_PITCHES[1]) * np.exp(-moments / BOOM_SWEEP)
    tone = oscillators.sine(pitch, len(moments)) * envelopes.strike(len(moments), Attacks.sharp, BOOM_TONE_DECAY)
    blast = filters.bandpass(noise.white(len(moments), randomness) * envelopes.strike(len(moments), Attacks.snap, BOOM_DECAY), *BOOM_BAND)
    return normalised(normalised(blast) + BOOM_TONE_SHARE * tone)


def close_shot() -> np.ndarray:
    """A .303 fired beside the camera: a razor crack on top of a short, heavy boom."""
    randomness = noise.generator("rifle shot")
    body = BOOM_LEVEL * boom(randomness)
    body[:samples_in(CRACK_SECONDS)] += crack(randomness)
    return filters.lowpass(saturate(body, SHOT_DRIVE), CRACK_CEILING)


def report(randomness: Generator) -> np.ndarray:
    length = samples_in(DISTANT_SECONDS)
    burst = noise.white(length, randomness) * envelopes.strike(length, Attacks.snap, REPORT_DECAY)
    pitch, decay, share = REPORT_THUMP
    thump = share * oscillators.sine(pitch, length) * envelopes.strike(length, Attacks.sharp, decay)
    return normalised(filters.bandpass(burst + thump, *DISTANT_BAND))


def smeared(sound: np.ndarray, seconds: float, randomness: Generator) -> np.ndarray:
    """A sound as a stand of trees gives it back: spread out over a few dozen milliseconds."""
    length = samples_in(seconds)
    diffuser = noise.white(length, randomness) * envelopes.exponential_decay(length, seconds * SMEAR_DECAY_SHARE)
    return normalised(np.convolve(sound, diffuser))


def distant_tail(randomness: Generator) -> np.ndarray:
    length = samples_in(TAIL_SECONDS)
    building = envelopes.strike(length, TAIL_BUILD, TAIL_DECAY)
    sides = [filters.lowpass(noise.white(length, randomness), TAIL_CUTOFF) * building for _ in range(2)]
    return normalised(paired(*sides))


def distant_shot(index: int) -> np.ndarray:
    """The range up the hill: a dull crack, its roll off the wood a third of a second later, and the valley's tail."""
    randomness = noise.generator("distant rifle", index)
    shot = np.zeros((2, samples_in(TAIL_SECONDS + WOOD_DELAY)))
    direct = report(randomness)
    laid_at(shot, direct, 0.0)
    laid_at(shot, WOOD_LEVEL * filters.lowpass(smeared(direct, WOOD_SMEAR, randomness), WOOD_CUTOFF), WOOD_DELAY)
    laid_at(shot, TAIL_LEVEL * distant_tail(randomness), WOOD_DELAY / 2.0)
    return shot
