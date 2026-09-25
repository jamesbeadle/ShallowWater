"""Envelopes: how a sound rises, holds and dies away, as arrays of gain."""
import numpy as np

from .timebase import samples_in, time_axis

SIXTY_DECIBELS = np.log(1000.0)


class Attacks:
    """How quickly a struck sound reaches its peak, from a spark to a soft mallet, in seconds."""

    spark = 0.0001
    snap = 0.0005
    sharp = 0.001
    quick = 0.002
    firm = 0.008


def exponential_decay(length: int, time_constant: float) -> np.ndarray:
    return np.exp(-time_axis(length) / time_constant)


def strike(length: int, attack: float, time_constant: float) -> np.ndarray:
    """A struck sound: a quick linear rise, then an exponential fall."""
    moments = time_axis(length)
    rise = np.minimum(moments / attack, 1.0)
    return rise * np.exp(-np.maximum(moments - attack, 0.0) / time_constant)


def adsr(hold: float, attack: float, decay: float, sustain: float, release: float) -> np.ndarray:
    """A played note held for hold seconds, released over release seconds to sixty decibels down."""
    moments = time_axis(samples_in(hold + release))
    rise = np.minimum(moments / attack, 1.0)
    settle = sustain + (1.0 - sustain) * np.exp(-np.maximum(moments - attack, 0.0) / decay)
    held = rise * settle
    release_level = held[min(samples_in(hold), len(held) - 1)]
    fall = release_level * np.exp(-SIXTY_DECIBELS * np.maximum(moments - hold, 0.0) / release)
    return np.where(moments < hold, held, fall)


def rising(length: int) -> np.ndarray:
    """A raised-cosine ramp from nothing to full."""
    return 0.5 - 0.5 * np.cos(np.linspace(0.0, np.pi, length))


def fade_edges(sound: np.ndarray, fade_in: float, fade_out: float) -> np.ndarray:
    gain = np.ones(sound.shape[-1])
    opening = min(samples_in(fade_in), len(gain))
    closing = min(samples_in(fade_out), len(gain))
    gain[:opening] *= rising(opening)
    gain[len(gain) - closing:] *= rising(closing)[::-1]
    return sound * gain


def curve(length: int, start: float, end: float, bend: float = 1.0) -> np.ndarray:
    """A line from start to end; bend above one lingers near the start, below one rushes away from it."""
    progress = np.linspace(0.0, 1.0, length) ** bend
    return start + (end - start) * progress


def glide(length: int, start: float, end: float) -> np.ndarray:
    """An exponential path between two positive values, even in pitch or in cutoff."""
    return start * (end / start) ** np.linspace(0.0, 1.0, length)
