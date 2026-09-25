"""The small steel room of the engine hole: a low, close air with faint ringing plates."""
import numpy as np
from numpy.random import Generator

from ..synthesis import filters, noise
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in

AIR_CUTOFF = 260.0
PLATE_RINGS = ((140.0, 8.0), (385.0, 14.0), (910.0, 20.0))
PLATE_LEVEL = 0.25
BREATHING_RATE = 0.4
BREATHING_DEPTH = 0.2


def side_of_room(length: int, randomness: Generator) -> np.ndarray:
    air = normalised(filters.lowpass(noise.pink(length, randomness), AIR_CUTOFF))
    source = noise.white(length, randomness)
    plates = normalised(sum(filters.resonance(source, frequency, quality) for frequency, quality in PLATE_RINGS))
    breathing = 1.0 + BREATHING_DEPTH * noise.wander(length, BREATHING_RATE, randomness)
    return (air + PLATE_LEVEL * plates) * breathing


def room_tone(seconds: float) -> np.ndarray:
    length = samples_in(seconds)
    randomness = noise.generator("engine hole")
    return np.stack([side_of_room(length, randomness) for _ in range(2)])
