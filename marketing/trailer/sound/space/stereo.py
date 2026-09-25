"""Placing sounds between the speakers: constant-power panning, balance and width."""
import numpy as np

from ..synthesis.timebase import samples_in

QUARTER_TURN = np.pi / 4.0
CENTRE_COMPENSATION = np.sqrt(2.0)


def pan(mono: np.ndarray, position: float) -> np.ndarray:
    """A mono sound placed from hard left at -1 to hard right at +1."""
    angle = (position + 1.0) * QUARTER_TURN
    return np.stack([np.cos(angle) * mono, np.sin(angle) * mono])


def balance(sound: np.ndarray, position: float) -> np.ndarray:
    """A stereo sound leaned towards one side, leaving the centre untouched."""
    angle = (position + 1.0) * QUARTER_TURN
    gains = np.minimum(np.array([np.cos(angle), np.sin(angle)]) * CENTRE_COMPENSATION, 1.0)
    return sound * gains[:, np.newaxis]


def as_stereo(sound: np.ndarray, position: float) -> np.ndarray:
    is_mono = sound.ndim == 1
    return pan(sound, position) if is_mono else balance(sound, position)


def delayed(sound: np.ndarray, seconds: float) -> np.ndarray:
    offset = samples_in(seconds)
    return np.concatenate([np.zeros(sound.shape[:-1] + (offset,)), sound[..., :sound.shape[-1] - offset]], axis=-1)


def paired(left: np.ndarray, right: np.ndarray) -> np.ndarray:
    """Two independent takes of a sound, one for each side, trimmed to the shorter."""
    length = min(len(left), len(right))
    return np.stack([left[:length], right[:length]]) / CENTRE_COMPENSATION
