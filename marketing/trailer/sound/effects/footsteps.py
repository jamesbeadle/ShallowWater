"""Running feet on a woodland ride in October: a heel into soft earth and a crunch of fallen leaves."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise
from ..synthesis.envelopes import Attacks
from ..synthesis.layering import laid_at
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in
from ..space.stereo import pan

STEP_SECONDS = 0.25
THUD_CUTOFF = 170.0
THUD_DECAY = 0.028
LEAF_BAND = (900.0, 7000.0)
LEAF_GRAINS = (6, 14)
LEAF_SPAN = 0.1
GRAIN_SECONDS = 0.012
GRAIN_DECAY = 0.0035
LEAF_LEVEL = 1.6
TWIG_CHANCE = 0.2
TWIG_BAND = (1800.0, 5200.0)
TWIG_LEVEL = 0.5
FOOT_POSITIONS = (-0.12, 0.12)
STRIDE_LOUDNESS = (0.75, 1.0)
GRAIN_LOUDNESS = (0.3, 1.0)
LIFTING_OFF = 0.05


def grain(randomness: Generator) -> np.ndarray:
    length = samples_in(GRAIN_SECONDS)
    return noise.white(length, randomness) * envelopes.exponential_decay(length, GRAIN_DECAY) * randomness.uniform(*GRAIN_LOUDNESS)


def leaf_crunch(randomness: Generator) -> np.ndarray:
    """Dry leaves breaking under the foot: a handful of tiny cracks over a tenth of a second."""
    crunch = np.zeros(samples_in(STEP_SECONDS))
    for _ in range(randomness.integers(*LEAF_GRAINS)):
        laid_at(crunch, grain(randomness), randomness.uniform(0.0, LEAF_SPAN))
    return normalised(filters.bandpass(crunch, *LEAF_BAND))


def heel(randomness: Generator) -> np.ndarray:
    length = samples_in(STEP_SECONDS)
    strike = noise.white(length, randomness) * envelopes.strike(length, Attacks.quick, THUD_DECAY)
    return normalised(filters.lowpass(strike, THUD_CUTOFF, order=filters.STEEP))


def footfall(index: int) -> np.ndarray:
    """One stride, left or right foot in turn, never quite the same twice."""
    randomness = noise.generator("footfall", index)
    step = heel(randomness) + LEAF_LEVEL * leaf_crunch(randomness)
    is_twig = randomness.random() < TWIG_CHANCE
    twig = normalised(filters.bandpass(grain(randomness), *TWIG_BAND)) * is_twig
    laid_at(step, TWIG_LEVEL * twig, randomness.uniform(0.0, LEAF_SPAN))
    loudness = randomness.uniform(*STRIDE_LOUDNESS)
    return pan(loudness * envelopes.fade_edges(step, 0.0, LIFTING_OFF), FOOT_POSITIONS[index % len(FOOT_POSITIONS)])
