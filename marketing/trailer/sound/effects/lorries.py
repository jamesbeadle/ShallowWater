"""Leyland and Albion lorries on Watling Street at night: petrol engines idling and grinding past the bridge."""
from dataclasses import dataclass

import numpy as np
from numpy.random import Generator
from scipy import signal

from ..synthesis import envelopes, filters, noise
from ..synthesis.shaping import normalised, saturate
from ..synthesis.timebase import SAMPLE_RATE, samples_in
from ..space.stereo import pan

CYLINDER_STRENGTHS = (1.0, 0.78, 0.9, 0.72)
FIRING_JITTER = 0.04
PUFF_SECONDS = 0.05
PUFF_DECAY = 0.009
EXHAUST_RESONANCES = ((85.0, 2.0), (230.0, 3.0), (520.0, 5.0))
EXHAUST_CEILING = 1100.0
CLATTER_BAND = (1400.0, 3800.0)
CLATTER_LEVEL = 0.35
ROUGHNESS = 3.0
PASSING_CURVE = 0.7
FAR_SHARE = 0.5


@dataclass(frozen=True)
class Lorry:
    """One lorry: how fast its engine fires, where it drives across the frame and how near it comes."""

    name: str
    firing_rates: tuple
    positions: tuple
    nearest: float


LORRIES = (
    Lorry("idling leyland", (21.0, 22.5), (-0.6, -0.5), 0.35),
    Lorry("passing albion", (31.0, 27.0), (0.8, -0.7), 0.6),
)


def firing_train(rates: np.ndarray, randomness: Generator) -> np.ndarray:
    """An impulse at every cylinder firing, each cylinder a little stronger or weaker than the next."""
    cycle = np.cumsum(rates / SAMPLE_RATE)
    firings = np.flatnonzero(np.diff(np.floor(cycle)) > 0)
    jitter = (randomness.normal(0.0, FIRING_JITTER, len(firings)) * SAMPLE_RATE / rates[firings]).astype(int)
    positions = np.clip(firings + jitter, 0, len(rates) - 1)
    train = np.zeros(len(rates))
    train[positions] = np.resize(CYLINDER_STRENGTHS, len(positions))
    return train


def engine_note(lorry: Lorry, length: int, randomness: Generator) -> np.ndarray:
    train = firing_train(envelopes.curve(length, *lorry.firing_rates), randomness)
    puff = noise.white(samples_in(PUFF_SECONDS), randomness) * envelopes.exponential_decay(samples_in(PUFF_SECONDS), PUFF_DECAY)
    exhaust = signal.fftconvolve(train, puff)[:length]
    body = sum(filters.resonance(exhaust, frequency, quality) for frequency, quality in EXHAUST_RESONANCES)
    clatter = CLATTER_LEVEL * normalised(filters.bandpass(exhaust, *CLATTER_BAND))
    return saturate(filters.lowpass(normalised(body), EXHAUST_CEILING) + clatter, ROUGHNESS)


def drive_by(lorry: Lorry, seconds: float) -> np.ndarray:
    """A lorry crossing the frame, loudest where it passes nearest the camera."""
    length = samples_in(seconds)
    randomness = noise.generator(lorry.name)
    passing = lorry.nearest * np.sin(np.pi * np.linspace(0.0, 1.0, length)) ** PASSING_CURVE + (1.0 - lorry.nearest) * FAR_SHARE
    note = engine_note(lorry, length, randomness) * passing
    halfway = length // 2
    return np.concatenate([pan(note[:halfway], lorry.positions[0]), pan(note[halfway:], lorry.positions[1])], axis=1)


def lorries(seconds: float) -> np.ndarray:
    return sum(drive_by(lorry, seconds) for lorry in LORRIES)
