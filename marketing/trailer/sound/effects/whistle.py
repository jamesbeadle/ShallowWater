"""A police whistle in the streets below: the pea rattling a shrill note, blast after blast."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise, oscillators
from ..synthesis.layering import laid_at
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in, time_axis

PITCH = 2800.0
TRILL_RATE = 29.0
TRILL_DEPTH = 0.06
TRILL_LOUDNESS = 0.45
SCOOP = 0.04
SCOOP_SECONDS = 0.05
BREATH_BAND = (2000.0, 6000.0)
BREATH_LEVEL = 0.15
BLASTS = ((0.0, 0.4), (0.62, 0.28), (1.05, 0.6))
OVERTONES = (1.0, 0.18, 0.05)
RATE_SCATTER = 0.04
BLOWING = (0.015, 0.05, 0.9)
HELD_SHARE = 0.85


def blast(seconds: float, randomness: Generator) -> np.ndarray:
    """One blow: the note scoops up, the pea spins in the chamber and chops it into a trill."""
    length = samples_in(seconds)
    moments = time_axis(length)
    rattle = np.sin(oscillators.TAU * TRILL_RATE * moments * randomness.normal(1.0, RATE_SCATTER))
    pitch = PITCH * (1.0 - SCOOP * np.exp(-moments / SCOOP_SECONDS)) * (1.0 + TRILL_DEPTH * rattle)
    loudness = 1.0 - TRILL_LOUDNESS * (1.0 + rattle) / 2.0
    breath = BREATH_LEVEL * normalised(filters.bandpass(noise.white(length, randomness), *BREATH_BAND))
    tone = oscillators.harmonics(pitch, length, OVERTONES) * loudness + breath
    return tone * envelopes.adsr(seconds * HELD_SHARE, *BLOWING, seconds * (1.0 - HELD_SHARE))[:length]


def police_whistle() -> np.ndarray:
    randomness = noise.generator("police whistle")
    whistle = np.zeros(samples_in(sum(BLASTS[-1])))
    for moment, seconds in BLASTS:
        laid_at(whistle, blast(seconds, randomness), moment)
    return whistle
