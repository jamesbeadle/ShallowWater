"""Taiko-style drums: big skins struck hard, the pitch dropping as the head settles."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise, oscillators
from ..synthesis.envelopes import Attacks
from ..synthesis.shaping import normalised, saturate
from ..synthesis.timebase import samples_in, time_axis

DRUM_SECONDS = 1.0
PITCH_DROP = 0.9
DROP_SECONDS = 0.035
BODY_DECAY = 0.28
OVERTONE_RATIO = 1.52
OVERTONE_LEVEL = 0.35
OVERTONE_DECAY = 0.12
SKIN_BAND = (300.0, 2500.0)
SKIN_LEVEL = 0.45
SKIN_DECAY = 0.015
AIR_CUTOFF = 160.0
AIR_LEVEL = 0.6
AIR_DECAY = 0.06
DRIVE = 1.6


def drum(pitch: float, velocity: float, randomness: Generator) -> np.ndarray:
    length = samples_in(DRUM_SECONDS)
    moments = time_axis(length)
    falling = pitch * (1.0 + PITCH_DROP * np.exp(-moments / DROP_SECONDS))
    body = oscillators.sine(falling, length) * envelopes.strike(length, Attacks.sharp, BODY_DECAY)
    overtone = OVERTONE_LEVEL * oscillators.sine(falling * OVERTONE_RATIO, length) * envelopes.strike(length, Attacks.sharp, OVERTONE_DECAY)
    skin = normalised(filters.bandpass(noise.white(length, randomness), *SKIN_BAND)) * envelopes.strike(length, Attacks.snap, SKIN_DECAY)
    air = normalised(filters.lowpass(noise.white(length, randomness), AIR_CUTOFF)) * envelopes.strike(length, Attacks.quick, AIR_DECAY)
    return velocity * saturate(body + overtone + SKIN_LEVEL * skin + AIR_LEVEL * air, DRIVE)
