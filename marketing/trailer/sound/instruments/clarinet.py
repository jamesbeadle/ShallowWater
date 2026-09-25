"""A dance-band clarinet: a hollow, woody tone with the wide, late vibrato of the 1930s."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise, oscillators
from ..synthesis.pitches import detuned
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in, time_axis

ODD_HARMONICS = 1.0
EVEN_HARMONICS = 0.28
HARMONIC_COUNT = 14
VIBRATO_RATE = 5.6
VIBRATO_CENTS = 17.0
VIBRATO_DELAY = 0.16
VIBRATO_ONSET = 0.2
ATTACK = 0.035
RELEASE = 0.08
SUSTAIN = 0.82
SETTLING = 0.1
BREATH_BAND = (1500.0, 5000.0)
BREATH_LEVEL = 0.035


def spectrum() -> tuple:
    return tuple((ODD_HARMONICS if number % 2 else EVEN_HARMONICS) / number for number in range(1, HARMONIC_COUNT + 1))


def vibrato(length: int) -> np.ndarray:
    moments = time_axis(length)
    arriving = np.clip((moments - VIBRATO_DELAY) / VIBRATO_ONSET, 0.0, 1.0)
    return VIBRATO_CENTS * arriving * np.sin(oscillators.TAU * VIBRATO_RATE * moments)


def note(pitch: float, seconds: float, velocity: float, randomness: Generator) -> np.ndarray:
    """A tongued note held for seconds, breath and all."""
    envelope = envelopes.adsr(seconds, ATTACK, SETTLING, SUSTAIN, RELEASE)
    length = len(envelope)
    tone = oscillators.harmonics(detuned(pitch, vibrato(length)), length, spectrum(), randomness.uniform())
    breath = BREATH_LEVEL * normalised(filters.bandpass(noise.white(length, randomness), *BREATH_BAND))
    return velocity * envelope * (normalised(tone) + breath)
