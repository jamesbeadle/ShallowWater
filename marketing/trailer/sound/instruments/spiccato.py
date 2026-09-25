"""Spiccato strings: short bouncing bow strokes that drive the night along."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise
from ..synthesis.envelopes import Attacks
from ..synthesis.ensembles import Ensemble, ensemble
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in

STROKE = Ensemble(voices=3, detune_cents=7.0, vibrato_rate=0.0, vibrato_cents=0.0, drift_cents=2.0, width=0.7)
STROKE_SECONDS = 0.22
ATTACK = 0.004
DECAY = 0.05
BOW_BAND = (2500.0, 7000.0)
BOW_LEVEL = 0.1
BOW_DECAY = 0.008
DULLEST = 0.45


def stroke(pitch: float, velocity: float, brightness: float, randomness: Generator) -> np.ndarray:
    """One bounced note: harder strokes are louder and brighter."""
    length = samples_in(STROKE_SECONDS)
    tone = ensemble(pitch, STROKE_SECONDS, STROKE, randomness)
    shaped = filters.lowpass(tone, brightness * (DULLEST + (1.0 - DULLEST) * velocity)) * envelopes.strike(length, ATTACK, DECAY)
    scrape = normalised(filters.bandpass(noise.white(length, randomness), *BOW_BAND)) * envelopes.strike(length, Attacks.sharp, BOW_DECAY)
    return velocity * (shaped + BOW_LEVEL * scrape)
