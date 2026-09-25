"""Orchestral dynamics as gain curves: a sforzando that falls back and swells again, and a long diminuendo."""
import numpy as np

from ..synthesis.timebase import time_axis

SFORZANDO_FALL = 0.35
SFORZANDO_FLOOR = 0.3
SFORZANDO_SWELL = 0.75
SWELL_CURVE = 3
RING_HOLD = 0.6
RING_DECAY = 1.9
MAKING_WAY_SHARE = 0.3
MAKING_WAY_FLOOR = 0.25


def sforzando(length: int) -> np.ndarray:
    """Struck hard, fallen back to piano within a moment, then growing again towards the next bar."""
    moments = time_axis(length)
    fallen = SFORZANDO_FLOOR + (1.0 - SFORZANDO_FLOOR) * np.exp(-moments / SFORZANDO_FALL)
    return fallen + (SFORZANDO_SWELL - SFORZANDO_FLOOR) * np.linspace(0.0, 1.0, length) ** SWELL_CURVE


def ringing_away(length: int) -> np.ndarray:
    """Held full for a moment, then dying away as a great chord does in a big hall."""
    moments = time_axis(length)
    return np.exp(-np.maximum(moments - RING_HOLD, 0.0) / RING_DECAY)


def making_way(length: int) -> np.ndarray:
    """Full, then drawing back over the last stretch, leaving a hollow for the downbeat that follows."""
    drawing_back = int(length * MAKING_WAY_SHARE)
    return np.concatenate([np.ones(length - drawing_back), np.linspace(1.0, MAKING_WAY_FLOOR, drawing_back)])
