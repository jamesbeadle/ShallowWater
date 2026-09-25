"""Filters that move: a cutoff or a band travelling over the length of a sound, block by block."""
import numpy as np
from scipy import signal

from .filters import Kind, design

BLOCK = 128
STEPS_PER_OCTAVE = 48


def quantised(frequency: float) -> float:
    """Rounds a cutoff to a fine grid so that designs can be reused from the cache."""
    return float(2.0 ** (round(np.log2(frequency) * STEPS_PER_OCTAVE) / STEPS_PER_OCTAVE))


def swept(sound: np.ndarray, kind: str, cutoff_at, order: int) -> np.ndarray:
    """Filters a mono sound block by block, asking cutoff_at for the cutoff or band edges at each block."""
    output = np.empty_like(sound)
    state = None
    for start in range(0, len(sound), BLOCK):
        sections = design(kind, cutoff_at(start), order)
        state = np.zeros((len(sections), 2)) if state is None else state
        output[start:start + BLOCK], state = signal.sosfilt(sections, sound[start:start + BLOCK], zi=state)
    return output


def per_channel(sound: np.ndarray, filtering) -> np.ndarray:
    is_mono = sound.ndim == 1
    return filtering(sound) if is_mono else np.stack([filtering(channel) for channel in sound])


def swept_lowpass(sound: np.ndarray, cutoffs: np.ndarray, order: int = 2) -> np.ndarray:
    def cutoff_at(position):
        return quantised(cutoffs[position])
    return per_channel(sound, lambda channel: swept(channel, Kind.low, cutoff_at, order))


def swept_bandpass(sound: np.ndarray, centres: np.ndarray, width: float, order: int = 2) -> np.ndarray:
    """A band width octaves wide whose centre follows centres."""
    half = 2.0 ** (width / 2.0)

    def band_at(position):
        centre = quantised(centres[position])
        return (centre / half, centre * half)
    return per_channel(sound, lambda channel: swept(channel, Kind.band, band_at, order))
