"""Making room: a sound pulled down while another, more important one plays over it."""
import numpy as np

from ..synthesis import envelopes
from ..synthesis.shaping import decibels
from ..synthesis.timebase import samples_in

DUCK_DEPTH = -6.0
DUCK_RAMP = 0.3


def dip(length: int, start: float, windows) -> np.ndarray:
    """Gain for a sound starting at start: full, except pulled down through each window of (start, end) seconds."""
    gain = np.ones(length)
    ramp = envelopes.rising(samples_in(DUCK_RAMP))
    for window_start, window_end in windows:
        first = samples_in(window_start - start)
        last = samples_in(window_end - start)
        shape = np.concatenate([ramp[::-1], np.zeros(max(last - first - 2 * len(ramp), 0)), ramp])
        region = gain[max(first, 0):max(first, 0) + len(shape)]
        region *= decibels(DUCK_DEPTH) + (1.0 - decibels(DUCK_DEPTH)) * shape[:len(region)]
    return gain


def ducked(sound: np.ndarray, start: float, windows) -> np.ndarray:
    return sound * dip(sound.shape[-1], start, windows)
