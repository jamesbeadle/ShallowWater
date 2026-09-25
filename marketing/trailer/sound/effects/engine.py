"""The Bolinder's beat: one slow, heavy firing of a single-cylinder hot-bulb engine."""
from dataclasses import dataclass

import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, modes, noise, oscillators
from ..synthesis.envelopes import Attacks
from ..synthesis.layering import padded
from ..synthesis.shaping import decibels, normalised, saturate
from ..synthesis.timebase import samples_in, time_axis
from ..space.stereo import delayed

THUD_ATTACK = 0.002
THUD_OVERTONES = (1.0, 0.35, 0.12)
KNOCK_BAND = (1000.0, 2000.0)
KNOCK_SECONDS = 0.02
CHUFF_BAND = (200.0, 800.0)
CHUFF_DELAY = 0.012
RING_MODES = modes.partials_of((312.0, 437.0, 561.0, 689.0), (1.0, 0.7, 0.55, 0.4), (0.06, 0.05, 0.04, 0.035))
TUNING_SCATTER = 0.015
KNOCK_DECAY = KNOCK_SECONDS / 4.0
CHUFF_RING_OUT = 4.0
LEVEL_SCATTER = 0.08


@dataclass(frozen=True)
class EngineVoice:
    """How the engine sounds from where the camera is standing."""

    strike_pitch: float = 110.0
    settle_pitch: float = 50.0
    sweep: float = 0.03
    body: float = 0.16
    knock: float = -12.0
    chuff: float = -8.0
    ring: float = -16.0
    chuff_length: float = 0.03
    brightness: float = 9000.0
    drive: float = 1.6
    seconds: float = 0.9


def thud(voice: EngineVoice, randomness: Generator) -> np.ndarray:
    moments = time_axis(samples_in(voice.seconds))
    settle = voice.settle_pitch * randomness.normal(1.0, TUNING_SCATTER)
    pitch = settle + (voice.strike_pitch - settle) * np.exp(-moments / voice.sweep)
    envelope = envelopes.strike(len(moments), THUD_ATTACK, voice.body)
    return envelope * oscillators.harmonics(pitch, len(moments), THUD_OVERTONES)


def knock(randomness: Generator) -> np.ndarray:
    length = samples_in(KNOCK_SECONDS)
    burst = noise.white(length, randomness) * envelopes.strike(length, Attacks.snap, KNOCK_DECAY)
    return normalised(filters.bandpass(burst, *KNOCK_BAND))


def chuff(voice: EngineVoice, randomness: Generator) -> np.ndarray:
    length = samples_in(voice.chuff_length * CHUFF_RING_OUT)
    puff = noise.white(length, randomness) * envelopes.strike(length, Attacks.firm, voice.chuff_length)
    return delayed(normalised(filters.bandpass(puff, *CHUFF_BAND)), CHUFF_DELAY)


def firing(voice: EngineVoice, randomness: Generator) -> np.ndarray:
    """One beat of the engine, with a little of the variation a real engine has from stroke to stroke."""
    body = thud(voice, randomness)
    length = len(body)
    ringing = modes.ring(modes.scattered(RING_MODES, TUNING_SCATTER, randomness), voice.seconds, randomness)
    layers = (
        body,
        decibels(voice.knock) * padded(knock(randomness), length),
        decibels(voice.chuff) * padded(chuff(voice, randomness), length),
        decibels(voice.ring) * padded(normalised(ringing), length),
    )
    beat = saturate(sum(layers), voice.drive) * randomness.normal(1.0, LEVEL_SCATTER)
    return filters.lowpass(beat, voice.brightness)
