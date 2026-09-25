"""Breath: a runner's hard, even breathing, and the long, controlled exhale before a shot."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise, oscillators
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in

OUT_BAND = (350.0, 2600.0)
IN_BAND = (900.0, 4500.0)
MOUTH_RESONANCES = ((820.0, 3.0), (1650.0, 4.0))
RESONANCE_LEVEL = 0.8
CYCLE_SECONDS = 1.33
IN_SHARE = 0.4
IN_LEVEL = 0.6
CYCLE_SCATTER = 0.05
SETTLING_SECONDS = 0.4
EXHALE_RISE = 0.12
EXHALE_TREMBLE_RATE = 7.0
EXHALE_TREMBLE_DEPTH = 0.08
EMPTYING = 0.7
PUFF_ROUNDNESS = 1.5


def airflow(length: int, band: tuple, randomness: Generator) -> np.ndarray:
    """Air through an open mouth: broad noise with the mouth's own resonances."""
    rushing = filters.bandpass(noise.white(length, randomness), *band)
    shaped = rushing + RESONANCE_LEVEL * sum(filters.resonance(rushing, frequency, quality) for frequency, quality in MOUTH_RESONANCES)
    return normalised(shaped)


def puff(seconds: float, band: tuple, randomness: Generator) -> np.ndarray:
    length = samples_in(seconds)
    return airflow(length, band, randomness) * np.sin(np.pi * np.arange(length) / length) ** PUFF_ROUNDNESS


def running_breath(seconds: float) -> np.ndarray:
    """In through the mouth, out through the mouth, in time with the stride."""
    randomness = noise.generator("running breath")
    breaths = []
    length = samples_in(seconds)
    breathed = 0
    while breathed < length:
        cycle = CYCLE_SECONDS * randomness.normal(1.0, CYCLE_SCATTER)
        breaths.append(IN_LEVEL * puff(cycle * IN_SHARE, IN_BAND, randomness))
        breaths.append(puff(cycle * (1.0 - IN_SHARE), OUT_BAND, randomness))
        breathed += samples_in(cycle * IN_SHARE) + samples_in(cycle * (1.0 - IN_SHARE))
    return envelopes.fade_edges(np.concatenate(breaths)[:length], 0.0, SETTLING_SECONDS)


def exhale(seconds: float) -> np.ndarray:
    """The marksman's breath let out slowly and evenly, trailing away to nothing before the trigger."""
    randomness = noise.generator("exhale")
    length = samples_in(seconds)
    moments = np.linspace(0.0, seconds, length)
    rising = np.minimum(moments / EXHALE_RISE, 1.0)
    emptying = (1.0 - moments / seconds) ** EMPTYING
    tremble = 1.0 + EXHALE_TREMBLE_DEPTH * np.sin(oscillators.TAU * EXHALE_TREMBLE_RATE * moments)
    return airflow(length, OUT_BAND, randomness) * rising * emptying * tremble
