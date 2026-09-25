"""A muted clock: tick and tock on the quavers, felt under the talk more than heard."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, modes, noise
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in

TICK_PITCHES = (2650.0, 2150.0)
TICK_DECAY = 0.006
TICK_SECONDS = 0.05
MUFFLE = 4500.0
CLICK_LEVEL = 0.5
OVERTONE_RATIO = 2.3
OVERTONE_LEVEL = 0.4
CLICK_DECAY = 0.0005


def tick(is_tock: bool, randomness: Generator) -> np.ndarray:
    pitch = TICK_PITCHES[int(is_tock)]
    body = modes.partials_of((pitch, pitch * OVERTONE_RATIO), (1.0, OVERTONE_LEVEL), (TICK_DECAY, TICK_DECAY / 2.0))
    ringing = modes.ring(body, TICK_SECONDS, randomness)
    length = len(ringing)
    click = noise.white(length, randomness) * envelopes.exponential_decay(length, CLICK_DECAY)
    return normalised(filters.lowpass(normalised(ringing) + CLICK_LEVEL * click, MUFFLE))
