"""Cymbals: a crash that hangs over the title, and the same crash run backwards into it."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, modes, noise
from ..synthesis.envelopes import Attacks
from ..synthesis.shaping import normalised, reversed_in_time
from ..synthesis.timebase import samples_in
from ..space.stereo import paired

CRASH_SECONDS = 6.0
SHIMMER_PARTIALS = 60
SHIMMER_RANGE = (2500.0, 14000.0)
SHIMMER_LOUDNESS = (0.2, 1.0)
SHIMMER_DECAYS = (0.8, 2.6)
WASH_FLOOR = 3000.0
WASH_DECAY = 1.4
SHIMMER_LEVEL = 0.5


def side_of_crash(randomness: Generator) -> np.ndarray:
    length = samples_in(CRASH_SECONDS)
    wash = filters.highpass(noise.white(length, randomness), WASH_FLOOR) * envelopes.strike(length, Attacks.sharp, WASH_DECAY)
    shimmer = modes.partials_of(
        randomness.uniform(*SHIMMER_RANGE, SHIMMER_PARTIALS),
        randomness.uniform(*SHIMMER_LOUDNESS, SHIMMER_PARTIALS),
        randomness.uniform(*SHIMMER_DECAYS, SHIMMER_PARTIALS),
    )
    return normalised(wash) + SHIMMER_LEVEL * normalised(modes.ring(shimmer, CRASH_SECONDS, randomness))


def crash(name: str) -> np.ndarray:
    randomness = noise.generator(name)
    return paired(side_of_crash(randomness), side_of_crash(randomness))


def reverse_cymbal(seconds: float) -> np.ndarray:
    """A crash played backwards: it swells out of nothing and stops dead on the beat."""
    backwards = reversed_in_time(crash("reverse cymbal"))
    return backwards[:, -samples_in(seconds):]
