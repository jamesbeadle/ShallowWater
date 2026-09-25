"""Wind: a faint dawn breeze in the trees, and the open wind over the hilltop above the range."""
import numpy as np
from numpy.random import Generator

from ..synthesis import filters, noise
from ..synthesis.shaping import normalised
from ..synthesis.sweeps import swept_bandpass
from ..synthesis.timebase import samples_in

BREEZE_BAND = (120.0, 1500.0)
BREEZE_RATE = 0.15
BREEZE_DEPTH = 0.5
GUST_RATE = 0.35
GUST_DEPTH = 0.55
BODY_BAND = (70.0, 2200.0)
WHISTLE_CENTRES = (420.0, 950.0)
WHISTLE_WIDTH = 0.35
WHISTLE_LEVEL = 0.35
HISS_BAND = (1500.0, 7000.0)
HISS_LEVEL = 0.6
QUIETEST_GUST = 0.15
GUST_CENTRE = 0.5
STILLEST_BREEZE = 0.1


def breeze(seconds: float) -> np.ndarray:
    randomness = noise.generator("dawn breeze")
    length = samples_in(seconds)
    sides = [normalised(filters.bandpass(noise.pink(length, randomness), *BREEZE_BAND)) for _ in range(2)]
    gusting = np.clip(1.0 + BREEZE_DEPTH * noise.wander(length, BREEZE_RATE, randomness), STILLEST_BREEZE, None)
    return np.stack(sides) * gusting


def side_of_hill(length: int, gusts: np.ndarray, randomness: Generator) -> np.ndarray:
    """The wind on one side: a rumble of air, a hiss through the grass and the branches, and a whistle that rises with it."""
    body = normalised(filters.bandpass(noise.pink(length, randomness), *BODY_BAND))
    hiss = HISS_LEVEL * normalised(filters.bandpass(noise.white(length, randomness), *HISS_BAND))
    lowest, highest = WHISTLE_CENTRES
    whistle = swept_bandpass(noise.white(length, randomness), lowest + (highest - lowest) * gusts, WHISTLE_WIDTH)
    return body + hiss + WHISTLE_LEVEL * normalised(whistle)


def hilltop_wind(seconds: float) -> np.ndarray:
    """Open wind in gusts, whistling higher as each gust strengthens."""
    randomness = noise.generator("hilltop wind")
    length = samples_in(seconds)
    gusts = np.clip(GUST_CENTRE + GUST_CENTRE * GUST_DEPTH * noise.wander(length, GUST_RATE, randomness), QUIETEST_GUST, 1.0)
    return np.stack([side_of_hill(length, gusts, randomness) for _ in range(2)]) * gusts
