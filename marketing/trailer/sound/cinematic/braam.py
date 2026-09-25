"""The braam: a wall of detuned saws on D, opening like a furnace door and burning down slowly."""
import numpy as np

from ..synthesis import envelopes, filters, noise
from ..synthesis.ensembles import Ensemble, ensemble
from ..synthesis.layering import padded
from ..synthesis.pitches import frequency
from ..synthesis.shaping import normalised, saturate
from ..synthesis.sweeps import swept_lowpass
from ..synthesis.timebase import samples_in
from .impacts import sub_drop

PITCHES = ("D1", "D2", "A2", "D3")
WEIGHTS = (1.0, 0.9, 0.7, 0.6)
SECONDS = 5.5
WALL = Ensemble(voices=7, detune_cents=16.0, vibrato_rate=0.0, vibrato_cents=0.0, drift_cents=4.0, width=0.9)
CLOSED = 170.0
OPEN = 2600.0
SETTLED = 380.0
OPENING_SECONDS = 0.32
ATTACK = 0.025
HOLD = 0.35
BURN_DOWN = 1.3
DRIVE = 2.6
BREATH_BAND = (250.0, 1400.0)
BREATH_LEVEL = 0.12
SUB_LEVEL = 0.5


def cutoff_path(length: int) -> np.ndarray:
    opening = samples_in(OPENING_SECONDS)
    rise = envelopes.glide(opening, CLOSED, OPEN)
    fall = envelopes.glide(length - opening, OPEN, SETTLED)
    return np.concatenate([rise, fall])


def loudness(length: int) -> np.ndarray:
    moments = np.arange(length) / length * SECONDS
    rising = np.minimum(moments / ATTACK, 1.0)
    return rising * np.exp(-np.maximum(moments - HOLD, 0.0) / BURN_DOWN)


def braam() -> np.ndarray:
    randomness = noise.generator("braam")
    length = samples_in(SECONDS)
    wall = sum(weight * ensemble(frequency(pitch), SECONDS, WALL, randomness) for pitch, weight in zip(PITCHES, WEIGHTS))
    breath = BREATH_LEVEL * normalised(filters.bandpass(noise.white(length, randomness), *BREATH_BAND))
    opened = swept_lowpass(normalised(wall) + breath, cutoff_path(length))
    burning = saturate(opened * loudness(length) * DRIVE, 1.0)
    return normalised(burning) + SUB_LEVEL * padded(sub_drop(), length)
