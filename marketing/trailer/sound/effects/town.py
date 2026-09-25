"""The town waking to the shot, far below: dogs setting each other off, doors, a sash thrown up, a murmur."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, modes, noise, oscillators
from ..synthesis.envelopes import Attacks
from ..synthesis.layering import laid_at
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in
from ..space.stereo import pan

BARK_SECONDS = 0.2
BARK_FORMANTS = ((620.0, 3.0), (1450.0, 4.0), (2600.0, 5.0))
BARK_FALL = (1.18, 0.82)
BARK_NOISE = 0.35
DOGS = ((310.0, -0.6, (0.05, 0.3, 0.95, 1.2)), (400.0, 0.5, (0.55, 0.78, 1.45)), (260.0, 0.15, (1.1, 1.4)))
DOOR_MODES = modes.partials_of((95.0, 180.0, 420.0, 760.0), (1.0, 0.8, 0.5, 0.3), (0.08, 0.07, 0.05, 0.04))
DOORS = ((0.3, -0.35), (1.25, 0.7))
SASH_BAND = (800.0, 3000.0)
SASH_SECONDS = 0.35
SASH = (0.7, -0.8)
MURMUR_BAND = (250.0, 1400.0)
MURMUR_SYLLABLES = 4.5
MURMUR_LEVEL = 0.18
BARK_DECAY = 0.045
BARK_SCATTER = 0.03
DOOR_SECONDS = 0.4
DOOR_LEVEL = 0.8
SASH_LEVEL = 0.5
SASH_ROUGHNESS = 40.0
SASH_OPENING = (0.3, 1.0)


def bark(pitch: float, randomness: Generator) -> np.ndarray:
    length = samples_in(BARK_SECONDS)
    falling = envelopes.glide(length, pitch * BARK_FALL[0], pitch * BARK_FALL[1])
    source = oscillators.saw(falling, length) + BARK_NOISE * noise.white(length, randomness)
    voiced = sum(filters.resonance(source, frequency, quality) for frequency, quality in BARK_FORMANTS)
    return normalised(voiced) * envelopes.strike(length, Attacks.firm, BARK_DECAY)


def door(randomness: Generator) -> np.ndarray:
    return normalised(modes.ring(DOOR_MODES, DOOR_SECONDS, randomness))


def sash(randomness: Generator) -> np.ndarray:
    length = samples_in(SASH_SECONDS)
    scrape = filters.bandpass(noise.white(length, randomness), *SASH_BAND) * np.abs(noise.wander(length, SASH_ROUGHNESS, randomness))
    return normalised(scrape) * np.linspace(*SASH_OPENING, length)


def murmur(seconds: float, randomness: Generator) -> np.ndarray:
    """Many voices far off, too far for a single word: a breathing band of sound rising and falling."""
    length = samples_in(seconds)
    syllables = np.clip(noise.wander(length, MURMUR_SYLLABLES, randomness), 0.0, None)
    return normalised(filters.bandpass(noise.pink(length, randomness), *MURMUR_BAND) * syllables) * envelopes.curve(length, 0.0, 1.0)


def town_wakes(seconds: float) -> np.ndarray:
    randomness = noise.generator("town wakes")
    town = np.zeros((2, samples_in(seconds)))
    for pitch, position, moments in DOGS:
        for moment in moments:
            laid_at(town, pan(bark(pitch * randomness.normal(1.0, BARK_SCATTER), randomness), position), moment)
    for moment, position in DOORS:
        laid_at(town, pan(DOOR_LEVEL * door(randomness), position), moment)
    laid_at(town, pan(SASH_LEVEL * sash(randomness), SASH[1]), SASH[0])
    laid_at(town, MURMUR_LEVEL * np.stack([murmur(seconds, randomness), murmur(seconds, randomness)]), 0.0)
    return town
