"""Oscillators: phase that follows a frequency, fixed or moving, and the waves read from it."""
import numpy as np

from .timebase import NYQUIST, SAMPLE_RATE

TAU = 2.0 * np.pi
HIGHEST_PARTIAL = NYQUIST * 0.9


def increments_of(frequency, length: int) -> np.ndarray:
    return np.broadcast_to(np.asarray(frequency, dtype=float) / SAMPLE_RATE, (length,))


def cycles(frequency, length: int) -> np.ndarray:
    return np.cumsum(increments_of(frequency, length))


def sine(frequency, length: int, phase: float = 0.0) -> np.ndarray:
    return np.sin(TAU * (cycles(frequency, length) + phase))


def edge_correction(position: np.ndarray, increments: np.ndarray) -> np.ndarray:
    """The polynomial step that rounds off each saw reset, so the wave carries no aliasing."""
    correction = np.zeros_like(position)
    just_wrapped = position < increments
    early = position[just_wrapped] / increments[just_wrapped]
    correction[just_wrapped] = 2.0 * early - early * early - 1.0
    about_to_wrap = position > 1.0 - increments
    late = (position[about_to_wrap] - 1.0) / increments[about_to_wrap]
    correction[about_to_wrap] = late * late + 2.0 * late + 1.0
    return correction


def saw(frequency, length: int, phase: float = 0.0) -> np.ndarray:
    increments = increments_of(frequency, length)
    position = np.mod(np.cumsum(increments) + phase, 1.0)
    return 2.0 * position - 1.0 - edge_correction(position, increments)


def harmonics(frequency, length: int, amplitudes, phase: float = 0.0) -> np.ndarray:
    """A tone built from whole-number partials, leaving out any the sample rate cannot hold."""
    base = cycles(frequency, length) + phase
    highest = float(np.max(frequency))
    audible = [(number, amplitude) for number, amplitude in enumerate(amplitudes, start=1) if number * highest < HIGHEST_PARTIAL]
    tone = np.zeros(length)
    for number, amplitude in audible:
        tone += amplitude * np.sin(TAU * number * base)
    return tone
