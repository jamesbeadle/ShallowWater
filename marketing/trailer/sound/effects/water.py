"""Water on the cut: a soft lap against the hull, and the bubbles that any disturbance sends up."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise, oscillators
from ..synthesis.envelopes import Attacks
from ..synthesis.layering import laid_at
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in, time_axis

LAP_BAND = (160.0, 1300.0)
LAP_RATE = 0.9
LAP_DEPTH = 0.45
SLOP_BAND = (150.0, 900.0)
SLOP_SECONDS = 0.6
SLOP_INTERVALS = (0.9, 2.6)
BUBBLE_RISE = 0.6
BUBBLE_LENGTH = 6.0
SLOP_SHAPE = (0.07, 0.15)
PLINK_PITCHES = (450.0, 900.0)
PLINK_DECAYS = (0.008, 0.02)
PLINK_MOMENTS = (0.05, 0.25)
PLINK_LEVEL = 0.3
SLOP_LEVEL = 0.8


def bubble(pitch: float, decay: float) -> np.ndarray:
    """A bubble forming and ringing: a pure tone that climbs as it dies."""
    length = samples_in(decay * BUBBLE_LENGTH)
    moments = time_axis(length)
    climbing = pitch * (1.0 + BUBBLE_RISE * moments / decay)
    return oscillators.sine(climbing, length) * envelopes.strike(length, Attacks.sharp, decay)


def slop(randomness: Generator) -> np.ndarray:
    length = samples_in(SLOP_SECONDS)
    body = filters.bandpass(noise.white(length, randomness), *SLOP_BAND) * envelopes.strike(length, *SLOP_SHAPE)
    plink = np.zeros(length)
    rising_bubble = bubble(randomness.uniform(*PLINK_PITCHES), randomness.uniform(*PLINK_DECAYS))
    laid_at(plink, PLINK_LEVEL * rising_bubble, randomness.uniform(*PLINK_MOMENTS))
    return normalised(body) + plink


def side_of_hull(seconds: float, randomness: Generator) -> np.ndarray:
    length = samples_in(seconds)
    lapping = normalised(filters.bandpass(noise.pink(length, randomness), *LAP_BAND))
    lapping *= np.clip(1.0 + LAP_DEPTH * noise.wander(length, LAP_RATE, randomness), 0.0, None)
    moment = randomness.uniform(*SLOP_INTERVALS)
    while moment < seconds:
        laid_at(lapping, SLOP_LEVEL * slop(randomness), moment)
        moment += randomness.uniform(*SLOP_INTERVALS)
    return lapping


def lapping(seconds: float) -> np.ndarray:
    randomness = noise.generator("canal lapping")
    return np.stack([side_of_hull(seconds, randomness) for _ in range(2)])
