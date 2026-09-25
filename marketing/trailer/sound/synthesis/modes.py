"""Modal resonance: struck metal, skins and wood as sums of decaying partials."""
from dataclasses import dataclass

import numpy as np
from numpy.random import Generator

from .oscillators import TAU
from .timebase import samples_in, time_axis

QUIETEST_RESTRIKE = 0.7
STRIKE_ATTACK = 0.0008


@dataclass(frozen=True)
class Partial:
    """One mode of a struck body: where it rings, how loud, and how long it takes to die by a factor of e."""

    frequency: float
    amplitude: float
    decay: float


def ring(partials, seconds: float, randomness: Generator) -> np.ndarray:
    """The body ringing after a strike: every mode at once, rising over a fraction of a millisecond, then dying at its own rate."""
    moments = time_axis(samples_in(seconds))
    tone = np.zeros(len(moments))
    for partial in partials:
        phase = randomness.uniform(0.0, TAU)
        tone += partial.amplitude * np.exp(-moments / partial.decay) * np.sin(TAU * partial.frequency * moments + phase)
    return tone * np.minimum(moments / STRIKE_ATTACK, 1.0)


def scattered(partials, spread: float, randomness: Generator) -> tuple:
    """The same body struck again: every mode nudged a little, so no two strikes are identical."""
    return tuple(
        Partial(partial.frequency * randomness.normal(1.0, spread), partial.amplitude * randomness.uniform(QUIETEST_RESTRIKE, 1.0), partial.decay)
        for partial in partials
    )


def partials_of(frequencies, amplitudes, decays) -> tuple:
    return tuple(Partial(*mode) for mode in zip(frequencies, amplitudes, decays))
