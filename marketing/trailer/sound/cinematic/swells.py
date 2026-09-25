"""The suck-back: a boom's reverberation run backwards, rising out of silence into the moment of the drop."""
import numpy as np

from ..synthesis import envelopes, filters, noise
from ..synthesis.layering import padded
from ..synthesis.shaping import normalised, reversed_in_time
from ..synthesis.timebase import samples_in
from ..space.reverb import reverberate
from ..space.rooms import Rooms
from .impacts import low_boom

BURST_SECONDS = 0.15
BURST_BAND = (200.0, 6000.0)
TAIL_SECONDS = 4.0
BOOM_SHARE = 0.8
BURST_DECAY = 0.04
RISE_POWER = 2.0


def struck_room() -> np.ndarray:
    """A burst and a boom let loose in the cathedral, recorded for a few seconds."""
    randomness = noise.generator("reverse swell")
    length = samples_in(TAIL_SECONDS)
    burst_length = samples_in(BURST_SECONDS)
    burst = filters.bandpass(noise.white(burst_length, randomness), *BURST_BAND) * envelopes.exponential_decay(burst_length, BURST_DECAY)
    source = padded(normalised(burst) + BOOM_SHARE * padded(low_boom(), burst_length), length)
    return reverberate(np.stack([source, source]), Rooms.cathedral)


def reverse_swell(seconds: float) -> np.ndarray:
    """The last seconds of the room's answer played backwards, rising out of silence to the moment it was struck."""
    backwards = reversed_in_time(struck_room())[:, -samples_in(seconds):]
    rising = np.linspace(0.0, 1.0, backwards.shape[-1]) ** RISE_POWER
    return normalised(backwards * rising)
