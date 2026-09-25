"""Heavy rain sweeping over Peel's Wharf: a wide hiss of water and a dense patter of drops."""
import numpy as np
from numpy.random import Generator
from scipy import signal

from ..synthesis import envelopes, filters, noise, oscillators
from ..synthesis.shaping import normalised
from ..synthesis.timebase import SAMPLE_RATE, samples_in, time_axis

HISS_BAND = (450.0, 9000.0)
DROPS_PER_SECOND = 700.0
DROP_PITCHES = (1800.0, 5200.0)
DROP_DECAY = 0.004
DROP_SECONDS = 0.02
DROP_VARIETY = 12
DROP_LEVEL = 0.8
SWELL_POWER = 1.4
EDGE_SECONDS = 0.05


def drop_kernel(randomness: Generator) -> np.ndarray:
    length = samples_in(DROP_SECONDS)
    moments = time_axis(length)
    return np.sin(oscillators.TAU * randomness.uniform(*DROP_PITCHES) * moments) * np.exp(-moments / DROP_DECAY)


def patter(length: int, randomness: Generator) -> np.ndarray:
    """Drops of a dozen sizes landing independently on water, stone and boat roofs."""
    rate = DROPS_PER_SECOND / DROP_VARIETY / SAMPLE_RATE
    sizes = []
    for _ in range(DROP_VARIETY):
        landings = (randomness.random(length) < rate) * randomness.exponential(1.0, length)
        sizes.append(signal.fftconvolve(landings, drop_kernel(randomness))[:length])
    return normalised(sum(sizes))


def side_of_rain(length: int, randomness: Generator) -> np.ndarray:
    hiss = normalised(filters.bandpass(noise.pink(length, randomness), *HISS_BAND))
    return hiss + DROP_LEVEL * patter(length, randomness)


def rain(seconds: float) -> np.ndarray:
    """A downpour that swells in and passes over within seconds."""
    length = samples_in(seconds)
    randomness = noise.generator("heavy rain")
    swell = np.sin(np.pi * np.linspace(0.0, 1.0, length)) ** SWELL_POWER
    return envelopes.fade_edges(np.stack([side_of_rain(length, randomness) for _ in range(2)]) * swell, EDGE_SECONDS, EDGE_SECONDS)
