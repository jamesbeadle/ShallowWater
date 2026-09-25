"""A look-ahead peak limiter that watches the true peak between samples, so nothing crosses the ceiling."""
import numpy as np
from scipy import signal
from scipy.ndimage import minimum_filter1d, uniform_filter1d

from ..synthesis.shaping import decibels, level_of
from ..synthesis.timebase import SAMPLE_RATE, samples_in

OVERSAMPLING = 4
LOOK_AHEAD = 0.003
HOLD = 0.012
RELEASE_DECIBELS_PER_SECOND = 30.0
SMALLEST_PEAK = 1e-12


def true_peaks(stereo: np.ndarray) -> np.ndarray:
    """The highest level each sample reaches on either side, including the peaks between samples."""
    between = signal.resample_poly(stereo, OVERSAMPLING, 1, axis=-1)
    loudest = np.max(np.abs(between), axis=0)
    return loudest.reshape(-1, OVERSAMPLING).max(axis=1)[:stereo.shape[-1]]


def attack(needed: np.ndarray) -> np.ndarray:
    """Gain that reaches each peak's reduction before the peak arrives, and holds it a moment after."""
    look, hold = samples_in(LOOK_AHEAD), samples_in(HOLD)
    lowest = minimum_filter1d(needed, size=look + hold + 1, origin=(hold - look) // 2)
    return uniform_filter1d(lowest, size=look)


def release(gain: np.ndarray) -> np.ndarray:
    """Recovery at a steady rate in decibels: never faster, so the programme does not pump."""
    reduction = -level_of(gain)
    rate = RELEASE_DECIBELS_PER_SECOND / SAMPLE_RATE
    slope = rate * np.arange(len(reduction))
    held = np.maximum.accumulate(reduction + slope) - slope
    return decibels(-np.maximum(held, reduction))


def limited(stereo: np.ndarray, ceiling: float) -> np.ndarray:
    needed = np.minimum(1.0, ceiling / np.maximum(true_peaks(stereo), SMALLEST_PEAK))
    return np.clip(stereo * release(attack(needed)), -ceiling, ceiling)
