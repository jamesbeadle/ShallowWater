"""Synthetic impulse responses: decaying, filtered, decorrelated noise that stands in for a real space."""
import numpy as np
from numpy.random import Generator

from ..synthesis import filters, noise
from ..synthesis.envelopes import SIXTY_DECIBELS
from ..synthesis.timebase import samples_in, time_axis
from .stereo import delayed

CROSSOVER = 900.0
TAIL_LENGTH = 1.15
EARLY_LEVEL = 3.0
RESONANCE_LEVEL = 0.6
SIDES = 2


def early_reflections(room, randomness: Generator, length: int) -> np.ndarray:
    """The first few distinct bounces off the nearest surfaces."""
    reflections = np.zeros(length)
    arrivals = np.sort(randomness.uniform(0.0, room.early_span, room.early_taps))
    for arrival in arrivals:
        loudness = EARLY_LEVEL * (1.0 - arrival / room.early_span) * randomness.choice((-1.0, 1.0))
        reflections[samples_in(arrival)] += loudness
    return filters.lowpass(reflections, room.brightness)


def diffuse_tail(room, randomness: Generator, length: int) -> np.ndarray:
    moments = time_axis(length)
    source = noise.white(length, randomness)
    lows = filters.lowpass(source, CROSSOVER) * np.exp(-SIXTY_DECIBELS * moments / room.low_decay)
    highs = filters.highpass(source, CROSSOVER) * np.exp(-SIXTY_DECIBELS * moments / room.high_decay)
    building = 1.0 - np.exp(-moments / room.build)
    return filters.lowpass(lows + highs, room.brightness) * building


def coloured(response: np.ndarray, room) -> np.ndarray:
    """A room's own ringing frequencies drawn out of its response, as bare steel walls ring."""
    return response + RESONANCE_LEVEL * sum(filters.resonance(response, frequency, quality) for frequency, quality in room.resonances)


def side_response(room, side: int) -> np.ndarray:
    randomness = noise.generator(room.name, side)
    length = samples_in(room.low_decay * TAIL_LENGTH)
    response = diffuse_tail(room, randomness, length) + early_reflections(room, randomness, length)
    return delayed(np.pad(coloured(response, room), (0, samples_in(room.predelay))), room.predelay)


def impulse_response(room) -> np.ndarray:
    """A stereo response with decorrelated sides, scaled to carry unit energy per side."""
    response = np.stack([side_response(room, side) for side in range(SIDES)])
    return response / np.sqrt(np.sum(response ** 2) / SIDES)
