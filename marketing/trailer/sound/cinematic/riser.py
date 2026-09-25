"""The riser into the cut: a noise sweep, an endlessly climbing tone and a ticking that will not slow down."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise, oscillators
from ..synthesis.shaping import normalised
from ..synthesis.sweeps import swept_bandpass
from ..synthesis.timebase import SAMPLE_RATE, samples_in, time_axis

SWEEP_CENTRES = (1500.0, 12000.0)
SWEEP_WIDTH = 1.2
SWEEP_LEVELS = (0.06, 1.0)
CLIMB_LEVELS = (0.2, 1.0)
TICK_LEVELS = (0.3, 1.0)
CLIMB_SHARE = 0.3
TICK_SHARE = 0.3
LOWEST_STRAND = 55.0
STRANDS = 8
OCTAVES_CLIMBED = 1.5
CENTRE_OCTAVE = 3.6
SPECTRAL_WIDTH = 1.4
TICK_RATES = (4.0, 30.0)
TICK_SECONDS = 0.01
TICK_BAND = (2000.0, 6000.0)
TICK_DECAY = 0.0025


def noise_sweep(seconds: float, randomness: Generator) -> np.ndarray:
    length = samples_in(seconds)
    centres = envelopes.glide(length, *SWEEP_CENTRES)
    swelling = envelopes.glide(length, *SWEEP_LEVELS)
    sides = np.stack([noise.white(length, randomness) for _ in range(2)])
    return normalised(swept_bandpass(sides, centres, SWEEP_WIDTH)) * swelling


def endless_climb(seconds: float) -> np.ndarray:
    """Octave-spaced strands all gliding upward, fading in at the bottom and out at the top, so the climb never arrives."""
    length = samples_in(seconds)
    progress = np.linspace(0.0, OCTAVES_CLIMBED, length)
    climb = np.zeros(length)
    for strand in range(STRANDS):
        octave = np.mod(strand + progress, STRANDS)
        weight = np.exp(-0.5 * ((octave - CENTRE_OCTAVE) / SPECTRAL_WIDTH) ** 2)
        climb += weight * oscillators.sine(LOWEST_STRAND * 2.0 ** octave, length)
    return normalised(climb)


def ticks(seconds: float, randomness: Generator) -> np.ndarray:
    """A clock tick accelerating from four a second to a blur."""
    length = samples_in(seconds)
    rates = envelopes.glide(length, *TICK_RATES)
    count = np.floor(np.cumsum(rates) / SAMPLE_RATE)
    train = np.concatenate([[0.0], np.diff(count)])
    tick_length = samples_in(TICK_SECONDS)
    click = filters.bandpass(noise.white(tick_length, randomness), *TICK_BAND) * np.exp(-time_axis(tick_length) / TICK_DECAY)
    return normalised(np.convolve(train, click)[:length]) * envelopes.glide(length, *TICK_LEVELS)


def riser(seconds: float) -> np.ndarray:
    randomness = noise.generator("riser")
    climb = endless_climb(seconds) * envelopes.glide(samples_in(seconds), *CLIMB_LEVELS)
    return noise_sweep(seconds, randomness) + CLIMB_SHARE * np.stack([climb, climb]) + TICK_SHARE * ticks(seconds, randomness)
