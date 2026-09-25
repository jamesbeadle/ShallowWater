"""Static filters: the band limits and resonances that shape each sound."""
from functools import lru_cache

import numpy as np
from scipy import signal

from .timebase import NYQUIST, SAMPLE_RATE

HIGHEST_CUTOFF = NYQUIST * 0.95
LOWEST_CUTOFF = 5.0
STEEP = 4
DESIGNS_REMEMBERED = 8192


class Kind:
    """The shapes a Butterworth filter can take."""

    low = "lowpass"
    high = "highpass"
    band = "bandpass"


def within_range(cutoff):
    return tuple(within_range(edge) for edge in cutoff) if isinstance(cutoff, tuple) else min(max(cutoff, LOWEST_CUTOFF), HIGHEST_CUTOFF)


@lru_cache(maxsize=DESIGNS_REMEMBERED)
def design(kind: str, cutoff, order: int) -> np.ndarray:
    return signal.butter(order, within_range(cutoff), kind, fs=SAMPLE_RATE, output="sos")


def lowpass(sound: np.ndarray, cutoff: float, order: int = 2) -> np.ndarray:
    return signal.sosfilt(design(Kind.low, float(cutoff), order), sound, axis=-1)


def highpass(sound: np.ndarray, cutoff: float, order: int = 2) -> np.ndarray:
    return signal.sosfilt(design(Kind.high, float(cutoff), order), sound, axis=-1)


def bandpass(sound: np.ndarray, low: float, high: float, order: int = 2) -> np.ndarray:
    return signal.sosfilt(design(Kind.band, (float(low), float(high)), order), sound, axis=-1)


def resonance(sound: np.ndarray, frequency: float, quality: float) -> np.ndarray:
    """The part of a sound that rings at one frequency, as a tuned body would pick it out."""
    numerator, denominator = signal.iirpeak(frequency, quality, fs=SAMPLE_RATE)
    return signal.lfilter(numerator, denominator, sound, axis=-1)
