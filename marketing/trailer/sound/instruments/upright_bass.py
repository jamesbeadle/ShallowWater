"""A plucked upright bass: a round thump of a note that dies away inside half a second."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise, oscillators
from ..synthesis.oscillators import TAU
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in, time_axis

HARMONIC_LEVELS = (1.0, 0.55, 0.3, 0.15, 0.08)
FUNDAMENTAL_DECAY = 0.45
DECAY_FALL = 0.7
PLUCK_SECONDS = 0.02
PLUCK_CUTOFF = 900.0
PLUCK_LEVEL = 0.25
PITCH_BEND = 0.012
BEND_SECONDS = 0.03
FINGER_DECAY = PLUCK_SECONDS / 3.0
PLUCK_EDGES = (0.002, 0.03)


def pluck(pitch: float, seconds: float, velocity: float, randomness: Generator) -> np.ndarray:
    length = samples_in(seconds)
    moments = time_axis(length)
    bending = pitch * (1.0 + PITCH_BEND * np.exp(-moments / BEND_SECONDS))
    phase = oscillators.cycles(bending, length)
    tone = sum(
        level * np.exp(-moments * number ** DECAY_FALL / FUNDAMENTAL_DECAY) * np.sin(TAU * number * phase)
        for number, level in enumerate(HARMONIC_LEVELS, start=1)
    )
    finger = filters.lowpass(noise.white(length, randomness), PLUCK_CUTOFF) * envelopes.exponential_decay(length, FINGER_DECAY)
    return velocity * envelopes.fade_edges(normalised(tone) + PLUCK_LEVEL * normalised(finger), *PLUCK_EDGES)
