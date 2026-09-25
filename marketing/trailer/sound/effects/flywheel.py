"""Starting the engine by hand: the heavy kick that sets the flywheel turning, and the firing that fails."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, modes, noise, oscillators
from ..synthesis.envelopes import Attacks
from ..synthesis.layering import padded, summed
from ..synthesis.shaping import decibels, normalised
from ..synthesis.timebase import samples_in, time_axis

CLUNK_MODES = modes.partials_of(
    (182.0, 331.0, 517.0, 743.0, 1012.0, 1420.0, 2210.0),
    (1.0, 0.8, 0.6, 0.45, 0.35, 0.22, 0.12),
    (0.45, 0.35, 0.28, 0.2, 0.15, 0.1, 0.06),
)
CLUNK_SECONDS = 1.4
THUMP_PITCHES = (75.0, 44.0)
THUMP_SECONDS = 0.6
THUMP_DECAY = 0.14
THUMP_SWEEP = 0.03
IMPACT_BAND = (500.0, 4500.0)
IMPACT_SECONDS = 0.05
IMPACT_DECAY = 0.008
TURNING_RATES = (1.4, 2.2)
TURNING_CUTOFF = 240.0
TURNING_SWELL = (0.2, 1.0)
REVOLUTION_DEPTH = 0.35
WHEEZE_BAND = (300.0, 900.0)
WHEEZE_LEVEL = 0.3
COUGH_BAND = (140.0, 650.0)
COUGH_SECONDS = 0.35
COUGH_DECAY = 0.045
COUGH_THUMP = (85.0, 62.0, 0.06)
COUGH_THUMP_LEVEL = -10.0
TURNING_SECONDS = 1.5
TURNING_LEVEL = 0.6


class ClunkBalance:
    """The kick's layers against each other."""

    metal = 0.7
    impact = 0.5


def thump(pitch_from: float, pitch_to: float, seconds: float, decay: float) -> np.ndarray:
    moments = time_axis(samples_in(seconds))
    pitch = pitch_to + (pitch_from - pitch_to) * np.exp(-moments / THUMP_SWEEP)
    return envelopes.strike(len(moments), Attacks.quick, decay) * oscillators.sine(pitch, len(moments))


def burst(seconds: float, band: tuple, decay: float, randomness: Generator) -> np.ndarray:
    length = samples_in(seconds)
    shaped = noise.white(length, randomness) * envelopes.strike(length, Attacks.sharp, decay)
    return normalised(filters.bandpass(shaped, *band))


def clunk() -> np.ndarray:
    """A boot on the flywheel spoke: iron ringing over a heavy thud."""
    randomness = noise.generator("flywheel clunk")
    length = samples_in(CLUNK_SECONDS)
    metal = normalised(modes.ring(CLUNK_MODES, CLUNK_SECONDS, randomness))
    body = padded(thump(*THUMP_PITCHES, THUMP_SECONDS, THUMP_DECAY), length)
    impact = padded(burst(IMPACT_SECONDS, IMPACT_BAND, IMPACT_DECAY, randomness), length)
    return ClunkBalance.metal * metal + body + ClunkBalance.impact * impact


def turning(seconds: float) -> np.ndarray:
    """The flywheel gathering speed: a low rumble swelling once a revolution, rising towards the catch."""
    length = samples_in(seconds)
    randomness = noise.generator("flywheel turning")
    revolutions = oscillators.sine(envelopes.curve(length, *TURNING_RATES), length)
    rumble = filters.lowpass(noise.brown(length, randomness), TURNING_CUTOFF)
    wheeze = WHEEZE_LEVEL * normalised(filters.bandpass(noise.white(length, randomness), *WHEEZE_BAND))
    swelling = envelopes.curve(length, *TURNING_SWELL) * (1.0 - REVOLUTION_DEPTH + REVOLUTION_DEPTH * revolutions)
    return normalised(normalised(rumble) + wheeze * np.maximum(revolutions, 0.0)) * swelling


def cough() -> np.ndarray:
    """A failed firing: a soft pop and a chuff of unburnt vapour, no thud behind it."""
    randomness = noise.generator("engine cough")
    pop = burst(COUGH_SECONDS, COUGH_BAND, COUGH_DECAY, randomness)
    pitch_from, pitch_to, decay = COUGH_THUMP
    return pop + decibels(COUGH_THUMP_LEVEL) * thump(pitch_from, pitch_to, COUGH_SECONDS, decay)


def kick_and_turn() -> np.ndarray:
    """The kick, and the flywheel it sets turning, gathering towards the catch."""
    return summed((clunk(), TURNING_LEVEL * turning(TURNING_SECONDS)))
