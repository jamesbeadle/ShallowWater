"""Dawn at Hopwas: birdsong in the wood and along the cut, and a faint breeze in the trees."""
import numpy as np

from ..synthesis import envelopes
from ..synthesis.shaping import decibels
from .birds import dawn_chorus
from .wind import breeze

BIRD_LEVEL = -6.0
BREEZE_LEVEL = -17.0
OPENING_SECONDS = 0.2
CLOSING_SECONDS = 1.2


def dawn(seconds: float) -> np.ndarray:
    """The morning outdoors, opening on the cut to it and closing as the scene leaves."""
    layered = decibels(BIRD_LEVEL) * dawn_chorus(seconds) + decibels(BREEZE_LEVEL) * breeze(seconds)
    return envelopes.fade_edges(layered, OPENING_SECONDS, CLOSING_SECONDS)
