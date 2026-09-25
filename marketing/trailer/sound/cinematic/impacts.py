"""Trailer impacts: the sub drop, the metal slam and the boom that land with a card, a cut or a title."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, modes, noise, oscillators
from ..synthesis.envelopes import Attacks
from ..synthesis.layering import padded
from ..synthesis.shaping import normalised, saturate
from ..synthesis.timebase import samples_in, time_axis
from ..space.stereo import paired

SUB_PITCHES = (95.0, 28.0)
SUB_SWEEP = 0.28
SUB_DECAY = 0.9
SUB_SECONDS = 3.5
SUB_SHARE = 0.6
SUB_DRIVE = 1.4
SLAM_MODES = modes.partials_of(
    (173.0, 311.0, 467.0, 709.0, 1033.0, 1487.0, 2213.0, 3120.0),
    (1.0, 0.85, 0.7, 0.6, 0.45, 0.35, 0.25, 0.15),
    (1.2, 0.9, 0.7, 0.5, 0.4, 0.3, 0.2, 0.15),
)
SLAM_SECONDS = 2.5
SLAM_SCATTER = 0.02
SLAM_DRIVE = 1.8
CRUNCH_BAND = (300.0, 6000.0)
CRUNCH_DECAY = 0.03
THUD_CUTOFF = 220.0
THUD_DECAY = 0.08
BOOM_PITCHES = (58.0, 34.0)
BOOM_SWEEP = 0.15
BOOM_DECAY = 0.7
BOOM_SECONDS = 3.0
AIR_CUTOFF = 90.0
AIR_SHAPE = (0.01, 0.25)
AIR_SHARE = 0.5


class SlamBalance:
    """The slam's layers against each other."""

    metal = 0.8
    crunch = 0.9
    thud = 0.7


def falling_tone(pitches: tuple, sweep: float, decay: float, seconds: float) -> np.ndarray:
    moments = time_axis(samples_in(seconds))
    pitch = pitches[1] + (pitches[0] - pitches[1]) * np.exp(-moments / sweep)
    return oscillators.sine(pitch, len(moments)) * envelopes.strike(len(moments), Attacks.quick, decay)


def sub_drop() -> np.ndarray:
    return saturate(falling_tone(SUB_PITCHES, SUB_SWEEP, SUB_DECAY, SUB_SECONDS), SUB_DRIVE)


def slam(randomness: Generator) -> np.ndarray:
    """Steel on steel: a ring of inharmonic modes over a crunch of broadband noise and a dull thud."""
    metal = normalised(modes.ring(modes.scattered(SLAM_MODES, SLAM_SCATTER, randomness), SLAM_SECONDS, randomness))
    length = len(metal)
    crunch = filters.bandpass(noise.white(length, randomness) * envelopes.strike(length, Attacks.snap, CRUNCH_DECAY), *CRUNCH_BAND)
    thud = filters.lowpass(noise.white(length, randomness) * envelopes.strike(length, Attacks.sharp, THUD_DECAY), THUD_CUTOFF)
    layered = SlamBalance.metal * metal + SlamBalance.crunch * normalised(crunch) + SlamBalance.thud * normalised(thud)
    return saturate(layered, SLAM_DRIVE)


def hit(name: str) -> np.ndarray:
    """An enormous impact: sub drop in the middle, metal slammed wide across both sides."""
    randomness = noise.generator(name)
    length = samples_in(SUB_SECONDS)
    wide_slam = paired(padded(slam(randomness), length), padded(slam(randomness), length))
    return wide_slam + SUB_SHARE * sub_drop()[:length]


def low_boom() -> np.ndarray:
    """A deep, soft boom felt more than heard."""
    randomness = noise.generator("low boom")
    tone = falling_tone(BOOM_PITCHES, BOOM_SWEEP, BOOM_DECAY, BOOM_SECONDS)
    air = filters.lowpass(noise.white(len(tone), randomness), AIR_CUTOFF) * envelopes.strike(len(tone), *AIR_SHAPE)
    return normalised(tone + AIR_SHARE * normalised(air))
