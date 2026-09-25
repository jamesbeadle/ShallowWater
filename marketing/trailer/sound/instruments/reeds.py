"""The saxophone section: soft sustained harmony under the tune, reedy and a little wide."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters
from ..synthesis.ensembles import Ensemble, ensemble
from ..synthesis.timebase import SAMPLE_RATE

SECTION = Ensemble(voices=3, detune_cents=6.0, vibrato_rate=5.0, vibrato_cents=9.0, drift_cents=2.0, width=0.5)
CEILING = 1700.0
BODY_FLOOR = 160.0
ATTACK = 0.09
RELEASE = 0.15
SWELL = 0.35
BREATHING_IN = (0.2, 0.9)


def held_note(pitch: float, seconds: float, randomness: Generator) -> np.ndarray:
    """A note swelling gently through its length, as a section breathes together."""
    envelope = envelopes.adsr(seconds, ATTACK, *BREATHING_IN, RELEASE)
    swelling = envelope * (1.0 - SWELL + SWELL * np.linspace(0.0, 1.0, len(envelope)))
    tone = ensemble(pitch, len(envelope) / SAMPLE_RATE, SECTION, randomness)
    return filters.highpass(filters.lowpass(tone, CEILING), BODY_FLOOR) * swelling
