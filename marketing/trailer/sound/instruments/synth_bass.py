"""The hybrid bass: a gritty saw on the roots in quavers, and a pure sub underneath."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, oscillators
from ..synthesis.envelopes import Attacks
from ..synthesis.pitches import detuned
from ..synthesis.shaping import saturate
from ..synthesis.sweeps import swept_lowpass
from ..synthesis.timebase import samples_in, time_axis

DETUNE_CENTS = 8.0
PLUCK_DECAY = 0.12
FILTER_DECAY = 0.07
DRIVE = 1.8
SUB_ATTACK = 0.01
SUB_RELEASE = 0.08
PUMP_DEPTH = 0.85
PUMP_RECOVERY = 0.14
CUTOFF_FLOOR = 2.0
LETTING_GO = 0.01


def pulse(pitch: float, seconds: float, brightness: float, randomness: Generator) -> np.ndarray:
    """One quaver of saw bass: a bright bite that closes quickly."""
    length = samples_in(seconds)
    saws = sum(oscillators.saw(detuned(pitch, cents), length, randomness.uniform()) for cents in (-DETUNE_CENTS, DETUNE_CENTS))
    moments = time_axis(length)
    cutoffs = pitch * CUTOFF_FLOOR + brightness * np.exp(-moments / FILTER_DECAY)
    shaped = swept_lowpass(saws, cutoffs) * envelopes.strike(length, Attacks.quick, PLUCK_DECAY)
    return envelopes.fade_edges(saturate(shaped, DRIVE), 0.0, LETTING_GO)


def sub(pitch: float, seconds: float) -> np.ndarray:
    length = samples_in(seconds)
    return oscillators.sine(pitch, length) * envelopes.fade_edges(np.ones(length), SUB_ATTACK, SUB_RELEASE)



def pumping(length: int, kicks) -> np.ndarray:
    """Gain that ducks under each kick, given in seconds from the start, and swells back before the next."""
    moments = time_axis(length)
    gain = np.ones(length)
    for kick in kicks:
        since = moments - kick
        gain *= 1.0 - PUMP_DEPTH * np.exp(-np.maximum(since, 0.0) / PUMP_RECOVERY) * (since >= 0.0)
    return gain
