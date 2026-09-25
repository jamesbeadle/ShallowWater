"""The crackle of a valve wireless: sparse clicks and the odd pop over a faint hiss."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise
from ..synthesis.shaping import normalised
from ..synthesis.timebase import SAMPLE_RATE, samples_in

CLICK_RATE = 6.0
CLICK_DECAY = 0.0004
CLICK_FLOOR = 1500.0
POP_RATE = 0.4
POP_DECAY = 0.0025
POP_CEILING = 1800.0
POP_LEVEL = 0.6
HISS_LEVEL = 0.025
HISS_FLOOR = 2200.0
LOUDNESS_SPREAD = 0.9
KERNEL_SECONDS = 0.012


def scattered_impulses(length: int, rate: float, randomness: Generator) -> np.ndarray:
    """Impulses falling at random, rate a second on average, with a few much louder than the rest."""
    count = randomness.poisson(rate * length / SAMPLE_RATE)
    train = np.zeros(length)
    strengths = randomness.lognormal(0.0, LOUDNESS_SPREAD, count) * randomness.choice((-1.0, 1.0), count)
    np.add.at(train, randomness.integers(0, length, count), strengths)
    return train


def speck(decay: float, randomness: Generator) -> np.ndarray:
    length = samples_in(KERNEL_SECONDS)
    return noise.white(length, randomness) * envelopes.exponential_decay(length, decay)


def crackle(seconds: float, name: str) -> np.ndarray:
    randomness = noise.generator(name)
    length = samples_in(seconds)
    clicks = filters.highpass(np.convolve(scattered_impulses(length, CLICK_RATE, randomness), speck(CLICK_DECAY, randomness))[:length], CLICK_FLOOR)
    pops = filters.lowpass(np.convolve(scattered_impulses(length, POP_RATE, randomness), speck(POP_DECAY, randomness))[:length], POP_CEILING)
    hiss = HISS_LEVEL * filters.highpass(noise.white(length, randomness), HISS_FLOOR)
    return normalised(clicks) + POP_LEVEL * normalised(pops) + hiss
