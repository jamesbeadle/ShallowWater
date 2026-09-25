"""Wire brushes on a snare: a steady circular sweep, and a tap that lands on two and four."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise
from ..synthesis.envelopes import Attacks
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in

SWEEP_BAND = (1500.0, 7000.0)
TAP_BAND = (900.0, 8000.0)
TAP_SECONDS = 0.18
TAP_DECAY = 0.045
SLAP_BAND = (180.0, 700.0)
SLAP_LEVEL = 0.3
SLAP_DECAY = TAP_DECAY / 2.0


def sweep(seconds: float, randomness: Generator) -> np.ndarray:
    """One circle of the brush across the head: a hush that swells and fades."""
    length = samples_in(seconds)
    hush = normalised(filters.bandpass(noise.white(length, randomness), *SWEEP_BAND))
    return hush * np.sin(np.pi * np.arange(length) / length) ** 2


def tap(velocity: float, randomness: Generator) -> np.ndarray:
    length = samples_in(TAP_SECONDS)
    splash = normalised(filters.bandpass(noise.white(length, randomness), *TAP_BAND)) * envelopes.strike(length, Attacks.quick, TAP_DECAY)
    slap = normalised(filters.bandpass(noise.white(length, randomness), *SLAP_BAND)) * envelopes.strike(length, Attacks.sharp, SLAP_DECAY)
    return velocity * (splash + SLAP_LEVEL * slap)
