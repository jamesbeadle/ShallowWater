"""A wireless with a bad valve: the band squeezed through a small speaker, wobbling, humming and dropping out."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise, oscillators
from ..synthesis.shaping import decibels, normalised, saturate
from ..synthesis.timebase import SAMPLE_RATE, samples_in, time_axis
from .crackle import crackle

LOW_EDGE = 350.0
HIGH_EDGE = 3200.0
DRIVE = 2.4
WOBBLE_RATE = 0.23
WOBBLE_DEPTH = 0.11
DRIFT_RATE = 0.6
DRIFT_DEPTH = 0.06
DROPOUT_SECONDS = 0.08
DROPOUT_FLOOR = 0.05
DROPOUT_EDGE = 0.006
DROPOUT_WINDOWS = ((0.32, 0.42), (0.63, 0.72))
HUM_PITCH = 50.0
HUM_HARMONICS = (1.0, 0.6, 0.35)
HUM_LEVEL = -44.0
CRACKLE_LEVEL = -27.0


def speaker(programme: np.ndarray) -> np.ndarray:
    """A small cone driven a little too hard: saturated, then held between its narrow limits."""
    driven = saturate(normalised(programme), DRIVE)
    return filters.lowpass(filters.highpass(driven, LOW_EDGE), HIGH_EDGE, order=filters.STEEP)


def wobble(length: int, randomness: Generator) -> np.ndarray:
    moments = time_axis(length)
    swing = WOBBLE_DEPTH * np.sin(oscillators.TAU * WOBBLE_RATE * moments)
    return 1.0 + swing + DRIFT_DEPTH * noise.wander(length, DRIFT_RATE, randomness)


def dropouts(length: int, randomness: Generator) -> np.ndarray:
    """The valve losing its grip for a moment, once or twice in the broadcast."""
    gain = np.ones(length)
    edge = samples_in(DROPOUT_EDGE)
    gap = np.concatenate([envelopes.rising(edge)[::-1], np.zeros(samples_in(DROPOUT_SECONDS) - 2 * edge), envelopes.rising(edge)])
    for earliest, latest in DROPOUT_WINDOWS:
        start = int(randomness.uniform(earliest, latest) * length)
        gain[start:start + len(gap)] = DROPOUT_FLOOR + (1.0 - DROPOUT_FLOOR) * gap
    return gain


def hum(length: int) -> np.ndarray:
    return oscillators.harmonics(HUM_PITCH, length, HUM_HARMONICS) / sum(HUM_HARMONICS)


def broadcast(programme: np.ndarray) -> np.ndarray:
    """The band as the cabin hears it: one small, tired speaker."""
    mono = programme.mean(axis=0)
    length = len(mono)
    randomness = noise.generator("wireless valve")
    heard = speaker(mono) * wobble(length, randomness) * dropouts(length, randomness)
    interference = decibels(CRACKLE_LEVEL) * crackle(length / SAMPLE_RATE, "wireless crackle")
    return heard + interference + decibels(HUM_LEVEL) * hum(length)
