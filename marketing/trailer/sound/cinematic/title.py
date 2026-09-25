"""The title hit: a cymbal drawn backwards into the downbeat, then the crash, the slam and the sub all at once."""
import numpy as np

from ..synthesis.layering import laid_at, padded
from ..synthesis.timebase import samples_in
from .cymbals import CRASH_SECONDS, crash, reverse_cymbal
from .impacts import hit

REVERSE_SECONDS = 1.0
CRASH_LEVEL = 0.6
REVERSE_LEVEL = 0.5


def title_impact() -> np.ndarray:
    """Everything the title card lands with, starting REVERSE_SECONDS before it lands."""
    impact = np.zeros((2, samples_in(REVERSE_SECONDS + CRASH_SECONDS)))
    laid_at(impact, REVERSE_LEVEL * reverse_cymbal(REVERSE_SECONDS), 0.0)
    laid_at(impact, CRASH_LEVEL * crash("title crash"), REVERSE_SECONDS)
    laid_at(impact, padded(hit("title hit"), samples_in(CRASH_SECONDS)), REVERSE_SECONDS)
    return impact
