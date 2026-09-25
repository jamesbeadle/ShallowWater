"""Ensembles: many slightly detuned saws breathing together, the body of synthetic strings and brass."""
from dataclasses import dataclass

import numpy as np
from numpy.random import Generator

from . import noise, oscillators
from .pitches import detuned
from .timebase import samples_in, time_axis

QUARTER_TURN = np.pi / 4.0
RATE_SCATTER = 0.08
DRIFT_RATE = 0.7


@dataclass(frozen=True)
class Ensemble:
    """How a section of players spreads: how many, how far apart in tune and in the stereo field."""

    voices: int = 6
    detune_cents: float = 10.0
    vibrato_rate: float = 5.2
    vibrato_cents: float = 6.0
    drift_cents: float = 3.0
    width: float = 0.8


def vibrato(length: int, style: Ensemble, randomness: Generator) -> np.ndarray:
    """Each player's own vibrato and slow drift of tune, in cents."""
    rate = style.vibrato_rate * randomness.normal(1.0, RATE_SCATTER)
    moments = time_axis(length)
    shake = style.vibrato_cents * np.sin(oscillators.TAU * rate * moments + randomness.uniform(0.0, oscillators.TAU))
    return shake + style.drift_cents * noise.wander(length, DRIFT_RATE, randomness)


def ensemble(pitch, seconds: float, style: Ensemble, randomness: Generator) -> np.ndarray:
    """A stereo section of detuned saws playing one pitch (fixed or moving) for seconds."""
    length = samples_in(seconds)
    sound = np.zeros((2, length))
    for spread in np.linspace(-1.0, 1.0, style.voices):
        cents = spread * style.detune_cents + vibrato(length, style, randomness)
        voice = oscillators.saw(detuned(pitch, cents), length, randomness.uniform())
        angle = (spread * style.width + 1.0) * QUARTER_TURN
        sound += np.stack([np.cos(angle) * voice, np.sin(angle) * voice])
    return sound / np.sqrt(style.voices)
