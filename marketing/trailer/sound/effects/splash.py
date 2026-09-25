"""A heavy chain plunging into the cut: the slap, the plunge, the gulp of air and the bubbles coming up."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise, oscillators
from ..synthesis.envelopes import Attacks
from ..synthesis.layering import laid_at
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in, time_axis
from ..space.stereo import pan
from .water import bubble

SPLASH_SECONDS = 2.0
SLAP_SECONDS = 0.5
SLAP_BAND = (200.0, 5000.0)
THUMP_PITCHES = (75.0, 40.0)
THUMP_SWEEP = 0.03
THUMP_DECAY = 0.08
CRACK_DECAY = 0.04
PLUNGE_MOMENT = 0.01
PLUNGE_SECONDS = 1.2
PLUNGE_BAND = (250.0, 2500.0)
PLUNGE_SHAPE = (0.02, 0.25)
PLUNGE_LEVEL = 0.8
GULP_PITCHES = (120.0, 320.0)
GULP_SECONDS = 0.15
GULP_MOMENT = 0.08
GULP_LEVEL = 0.4
BUBBLES = (34, (300.0, 1500.0), (0.1, 1.3), 0.35)
DROPLETS = (14, (1200.0, 3000.0), (0.35, 1.2), 0.2)
BUBBLE_DECAYS = (0.01, 0.05)
BUBBLE_LOUDNESS = (0.2, 1.0)
BUBBLE_SPREAD = 0.7


def slap(randomness: Generator) -> np.ndarray:
    length = samples_in(SLAP_SECONDS)
    moments = time_axis(length)
    falling = THUMP_PITCHES[1] + (THUMP_PITCHES[0] - THUMP_PITCHES[1]) * np.exp(-moments / THUMP_SWEEP)
    thump = oscillators.sine(falling, length) * envelopes.strike(length, Attacks.quick, THUMP_DECAY)
    crack = filters.bandpass(noise.white(length, randomness) * envelopes.strike(length, Attacks.quick, CRACK_DECAY), *SLAP_BAND)
    return thump + normalised(crack)


def plunge(randomness: Generator) -> np.ndarray:
    length = samples_in(PLUNGE_SECONDS)
    body = normalised(filters.bandpass(noise.white(length, randomness), *PLUNGE_BAND)) * envelopes.strike(length, *PLUNGE_SHAPE)
    gulp_length = samples_in(GULP_SECONDS)
    gulp = oscillators.sine(envelopes.glide(gulp_length, *GULP_PITCHES), gulp_length) * np.hanning(gulp_length)
    laid_at(body, GULP_LEVEL * gulp, GULP_MOMENT)
    return body


def bubbling(kind: tuple, randomness: Generator) -> np.ndarray:
    """Bubbles or droplets scattered through a span of seconds and across the stereo field."""
    count, pitches, span, level = kind
    rising = np.zeros((2, samples_in(SPLASH_SECONDS)))
    for _ in range(count):
        single = bubble(randomness.uniform(*pitches), randomness.uniform(*BUBBLE_DECAYS)) * randomness.uniform(*BUBBLE_LOUDNESS)
        laid_at(rising, pan(single, randomness.uniform(-BUBBLE_SPREAD, BUBBLE_SPREAD)), randomness.uniform(*span))
    return level * rising


def splash() -> np.ndarray:
    randomness = noise.generator("chain splash")
    water = bubbling(BUBBLES, randomness) + bubbling(DROPLETS, randomness)
    laid_at(water, slap(randomness), 0.0)
    laid_at(water, PLUNGE_LEVEL * plunge(randomness), PLUNGE_MOMENT)
    return water
