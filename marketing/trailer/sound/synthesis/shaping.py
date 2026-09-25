"""Level and colour: decibels, saturation and peak normalising."""
import numpy as np

SILENCE_FLOOR = 1e-12
DECIBELS_PER_DECADE = 20.0


def decibels(level):
    """The gain a level in decibels stands for."""
    return 10.0 ** (level / DECIBELS_PER_DECADE)


def level_of(gain):
    """The level in decibels of a gain: the inverse of decibels."""
    return DECIBELS_PER_DECADE * np.log10(np.maximum(gain, SILENCE_FLOOR))


def saturate(sound: np.ndarray, drive: float) -> np.ndarray:
    return np.tanh(drive * sound) / np.tanh(drive)


def normalised(sound: np.ndarray, peak: float = 1.0) -> np.ndarray:
    return sound * (peak / max(float(np.max(np.abs(sound))), SILENCE_FLOOR))


def reversed_in_time(sound: np.ndarray) -> np.ndarray:
    return np.ascontiguousarray(sound[..., ::-1])
