"""Laying one sound into a longer one at a given moment, keeping only what fits."""
import numpy as np

from .timebase import samples_in


def mix_into(target: np.ndarray, sound: np.ndarray, offset: int) -> None:
    """Adds sound into target starting offset samples in; a mono sound lands on every channel."""
    first = max(offset, 0)
    last = min(offset + sound.shape[-1], target.shape[-1])
    if last <= first:
        return
    target[..., first:last] += sound[..., first - offset:last - offset]


def laid_at(target: np.ndarray, sound: np.ndarray, seconds: float) -> None:
    mix_into(target, sound, samples_in(seconds))


def padded(sound: np.ndarray, length: int) -> np.ndarray:
    """A sound cut or extended with silence to exactly length samples."""
    extension = [(0, 0)] * (sound.ndim - 1) + [(0, max(length - sound.shape[-1], 0))]
    return np.pad(sound, extension)[..., :length]


def summed(sounds) -> np.ndarray:
    """Sounds of different lengths added together from their first sample, the shorter ones padded with silence."""
    sounds = list(sounds)
    length = max(sound.shape[-1] for sound in sounds)
    return sum(padded(sound, length) for sound in sounds)
