"""A piano from its strings up: stiff, slightly inharmonic partials, each dying at its own rate."""
from dataclasses import dataclass

import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise
from ..synthesis.oscillators import HIGHEST_PARTIAL, TAU
from ..synthesis.pitches import detuned
from ..synthesis.timebase import samples_in, time_axis

PARTIALS = 20
INHARMONICITY = 0.0004
STRIKE_POINT = 0.12
REFERENCE_PITCH = 220.0
REFERENCE_DECAY = 2.6
DECAY_SLOPE = 0.6
PARTIAL_DAMPING = 0.3
PROMPT_SHARE = 0.65
PROMPT_SPEED = 4.0
DAMPER_DECAY = 0.07
HAMMER_SECONDS = 0.012
HAMMER_BAND = (1000.0, 4000.0)
HAMMER_LEVEL = 0.04
HAMMER_DECAY = 0.002
SOFTEST_ROLL_OFF = 1.5
HARDNESS = 0.7
DAMPER_RING_OUT = 8.0


@dataclass(frozen=True)
class PianoStyle:
    """The instrument: how far its unison strings have drifted apart, and how long it rings."""

    unison_cents: tuple = (-0.8, 0.8)
    sustain: float = 1.0


PUB_UPRIGHT = PianoStyle(unison_cents=(-5.0, 0.0, 6.0), sustain=0.7)
CONCERT_GRAND = PianoStyle(unison_cents=(-0.7, 0.9), sustain=1.4)


def partial(pitch: float, number: int, velocity: float, moments: np.ndarray, style: PianoStyle) -> np.ndarray:
    stretched = pitch * number * np.sqrt(1.0 + INHARMONICITY * number * number)
    amplitude = abs(np.sin(np.pi * number * STRIKE_POINT)) / number ** (SOFTEST_ROLL_OFF - HARDNESS * velocity)
    decay = style.sustain * REFERENCE_DECAY * (REFERENCE_PITCH / pitch) ** DECAY_SLOPE / (1.0 + PARTIAL_DAMPING * (number - 1))
    dying = PROMPT_SHARE * np.exp(-moments * PROMPT_SPEED / decay) + (1.0 - PROMPT_SHARE) * np.exp(-moments / decay)
    strings = sum(np.sin(TAU * (detuned(stretched, cents) * moments + number * cents / TAU)) for cents in style.unison_cents)
    return amplitude * dying * strings / len(style.unison_cents)


def hammer(length: int, randomness: Generator) -> np.ndarray:
    knock_length = samples_in(HAMMER_SECONDS)
    knock = filters.bandpass(noise.white(knock_length, randomness), *HAMMER_BAND) * envelopes.exponential_decay(knock_length, HAMMER_DECAY)
    return np.pad(knock, (0, length - knock_length))


def note(pitch: float, held: float, velocity: float, style: PianoStyle, randomness: Generator) -> np.ndarray:
    """One key struck at velocity (nought to one) and held for held seconds before the damper falls."""
    length = samples_in(held + DAMPER_DECAY * DAMPER_RING_OUT)
    moments = time_axis(length)
    audible = [number for number in range(1, PARTIALS + 1) if pitch * number < HIGHEST_PARTIAL]
    tone = sum(partial(pitch, number, velocity, moments, style) for number in audible)
    damper = np.exp(-np.maximum(moments - held, 0.0) / DAMPER_DECAY)
    return velocity * (tone * damper + HAMMER_LEVEL * velocity * hammer(length, randomness))
