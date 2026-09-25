"""A synthetic string section: wide, warm, slowly bowed chords and pedals."""
from dataclasses import dataclass

import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters
from ..synthesis.ensembles import Ensemble, ensemble
from ..synthesis.timebase import SAMPLE_RATE

VIOLINS = Ensemble(voices=6, detune_cents=9.0, vibrato_rate=5.4, vibrato_cents=8.0, drift_cents=3.0, width=0.95)
LOW_STRINGS = Ensemble(voices=4, detune_cents=7.0, vibrato_rate=4.9, vibrato_cents=5.0, drift_cents=2.5, width=0.5)


@dataclass(frozen=True)
class Bowing:
    """How the section draws the bow: how slowly it arrives and leaves, and how bright it plays."""

    attack: float
    release: float
    brightness: float
    floor: float = 55.0


SLOW_BOW = Bowing(attack=1.4, release=1.6, brightness=2400.0)
FULL_BOW = Bowing(attack=0.12, release=1.2, brightness=5200.0)
PEDAL_BOW = Bowing(attack=2.0, release=1.5, brightness=380.0, floor=30.0)
SETTLING = (0.6, 0.95)


def bowed(pitch, seconds: float, bowing: Bowing, section: Ensemble, randomness: Generator) -> np.ndarray:
    """One pitch held by the section for seconds, then released."""
    envelope = envelopes.adsr(seconds, bowing.attack, *SETTLING, bowing.release)
    tone = ensemble(pitch, len(envelope) / SAMPLE_RATE, section, randomness)
    shaped = filters.lowpass(filters.highpass(tone, bowing.floor), bowing.brightness)
    return shaped * envelope


def chord(pitches, seconds: float, bowing: Bowing, section: Ensemble, randomness: Generator) -> np.ndarray:
    return sum(bowed(pitch, seconds, bowing, section, randomness) for pitch in pitches) / np.sqrt(len(pitches))
