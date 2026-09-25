"""Seeded noise: every random sound draws from a generator named for what it is, so every render is identical."""
import zlib

import numpy as np
from numpy.random import Generator
from scipy import signal
from scipy.ndimage import uniform_filter1d

from .timebase import SAMPLE_RATE

BROWN_LEAK = 0.997
SMOOTHED_SPREAD = 0.72


def generator(name: str, index: int = 0) -> Generator:
    seed = zlib.crc32(name.encode()) + index
    return np.random.default_rng(seed)


def white(length: int, randomness: Generator) -> np.ndarray:
    return randomness.standard_normal(length)


def pink(length: int, randomness: Generator) -> np.ndarray:
    spectrum = np.fft.rfft(white(length, randomness))
    bins = np.arange(len(spectrum))
    spectrum /= np.sqrt(np.maximum(bins, 1))
    spectrum[0] = 0.0
    shaped = np.fft.irfft(spectrum, n=length)
    return shaped / np.std(shaped)


def brown(length: int, randomness: Generator) -> np.ndarray:
    walked = signal.lfilter([1.0], [1.0, -BROWN_LEAK], white(length, randomness))
    return walked / np.std(walked)


def wander(length: int, rate: float, randomness: Generator) -> np.ndarray:
    """A smooth random curve of unit spread that changes about rate times a second, however short the sound."""
    step = max(int(SAMPLE_RATE / rate), 1)
    points = randomness.standard_normal(length // step + 2)
    curve = np.interp(np.arange(length), np.arange(len(points)) * step, points)
    return uniform_filter1d(curve, step) / SMOOTHED_SPREAD
