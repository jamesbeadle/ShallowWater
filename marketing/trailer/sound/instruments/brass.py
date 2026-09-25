"""Orchestral brass built from saws: horns, trombones and trumpets that bloom brighter the harder they are pushed."""
from dataclasses import dataclass

import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes
from ..synthesis.ensembles import Ensemble, ensemble
from ..synthesis.shaping import saturate
from ..synthesis.sweeps import swept_lowpass
from ..synthesis.timebase import SAMPLE_RATE, time_axis

KEY_TRACKING = 3.0
SETTLE_SECONDS = 0.5
SETTLED_SHARE = 0.65
SPEAKING = (0.3, 0.85)


@dataclass(frozen=True)
class BrassTimbre:
    """A brass section: its players, how dark it sits at rest, how far it blooms, and how it speaks."""

    section: Ensemble
    closed: float
    bloom: float
    bloom_seconds: float
    attack: float
    release: float
    drive: float


HORNS = BrassTimbre(Ensemble(4, 7.0, 4.8, 4.0, 2.0, 0.8), 240.0, 1300.0, 0.14, 0.07, 0.5, 1.3)
TROMBONES = BrassTimbre(Ensemble(4, 6.0, 5.0, 3.0, 2.0, 0.6), 300.0, 2200.0, 0.1, 0.04, 0.4, 1.6)
TRUMPETS = BrassTimbre(Ensemble(3, 5.0, 5.6, 9.0, 2.0, 0.4), 550.0, 4200.0, 0.06, 0.03, 0.3, 1.5)


def bloom(length: int, pitch: float, velocity: float, timbre: BrassTimbre) -> np.ndarray:
    """The cutoff through a note: opening fast as the lips buzz, then settling as the note is held."""
    moments = time_axis(length)
    opening = 1.0 - np.exp(-moments / timbre.bloom_seconds)
    settling = SETTLED_SHARE + (1.0 - SETTLED_SHARE) * np.exp(-moments / SETTLE_SECONDS)
    return np.maximum(timbre.closed + timbre.bloom * velocity * opening * settling, np.min(pitch) * KEY_TRACKING)


def note(pitch, seconds: float, velocity: float, timbre: BrassTimbre, randomness: Generator) -> np.ndarray:
    envelope = envelopes.adsr(seconds, timbre.attack, *SPEAKING, timbre.release)
    length = len(envelope)
    tone = ensemble(pitch, length / SAMPLE_RATE, timbre.section, randomness)
    voiced = swept_lowpass(tone, bloom(length, pitch, velocity, timbre)) * envelope
    return saturate(voiced * velocity, timbre.drive)
