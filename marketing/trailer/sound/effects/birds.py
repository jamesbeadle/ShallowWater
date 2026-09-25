"""Birdsong at an October dawn on the cut: a robin close by, a wren in the hedge, another robin across the water."""
from dataclasses import dataclass

import numpy as np
from numpy.random import Generator

from ..synthesis import filters, noise, oscillators
from ..synthesis.envelopes import glide
from ..synthesis.layering import laid_at
from ..synthesis.timebase import samples_in, time_axis
from ..space.stereo import pan

OVERTONES = (1.0, 0.07)
WARBLE_RATES = (25.0, 80.0)
SYLLABLE_LOUDNESS = (0.5, 1.0)
BREATH_BETWEEN_PHRASES = 0.5


@dataclass(frozen=True)
class Singer:
    """One bird: its range, how it phrases, where it sits and how far away it is."""

    name: str
    pitch_range: tuple
    syllable_seconds: tuple
    syllables: tuple
    gap_seconds: tuple
    warble_depth: float
    phrases_per_second: float
    position: float
    distance_cutoff: float
    level: float


SINGERS = (
    Singer("robin", (2400.0, 6200.0), (0.05, 0.16), (4, 9), (0.02, 0.09), 350.0, 0.28, -0.35, 9000.0, 1.0),
    Singer("wren", (3500.0, 5800.0), (0.025, 0.05), (10, 22), (0.01, 0.025), 150.0, 0.12, 0.6, 7000.0, 0.55),
    Singer("far robin", (2200.0, 5600.0), (0.06, 0.15), (3, 7), (0.03, 0.1), 300.0, 0.2, 0.75, 3500.0, 0.3),
)


def syllable(singer: Singer, randomness: Generator) -> np.ndarray:
    length = samples_in(randomness.uniform(*singer.syllable_seconds))
    moments = time_axis(length)
    contour = glide(length, *randomness.uniform(*singer.pitch_range, 2))
    warble = randomness.uniform(0.0, singer.warble_depth) * np.sin(oscillators.TAU * randomness.uniform(*WARBLE_RATES) * moments)
    shape = np.sin(np.pi * np.arange(length) / length) ** 2
    return shape * oscillators.harmonics(contour + warble, length, OVERTONES)


def phrase(singer: Singer, randomness: Generator) -> np.ndarray:
    parts = []
    for _ in range(randomness.integers(*singer.syllables)):
        parts.append(syllable(singer, randomness) * randomness.uniform(*SYLLABLE_LOUDNESS))
        parts.append(np.zeros(samples_in(randomness.uniform(*singer.gap_seconds))))
    return np.concatenate(parts)


def song(singer: Singer, seconds: float) -> np.ndarray:
    """All of one bird's phrases over the dawn, at random moments and a distance away."""
    randomness = noise.generator(singer.name)
    singing = np.zeros(samples_in(seconds))
    moment = randomness.exponential(1.0 / singer.phrases_per_second)
    while moment < seconds:
        laid_at(singing, phrase(singer, randomness), moment)
        moment += randomness.exponential(1.0 / singer.phrases_per_second) + BREATH_BETWEEN_PHRASES
    return singer.level * filters.lowpass(singing, singer.distance_cutoff)


def dawn_chorus(seconds: float) -> np.ndarray:
    return sum(pan(song(singer, seconds), singer.position) for singer in SINGERS)
