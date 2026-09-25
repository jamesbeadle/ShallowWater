"""Convolution reverb: a send bus played through a room's synthetic impulse response."""
from functools import lru_cache

import numpy as np
from scipy import signal

from .impulses import impulse_response


@lru_cache(maxsize=None)
def response_of(room) -> np.ndarray:
    return impulse_response(room)


def reverberate(send: np.ndarray, room) -> np.ndarray:
    """The room's answer to a stereo send, the same length as the send."""
    response = response_of(room)
    length = send.shape[-1]
    return np.stack([signal.oaconvolve(channel, side)[:length] for channel, side in zip(send, response)])
