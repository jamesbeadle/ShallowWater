"""The shot rolling away across the valley: discrete echoes, each further, darker and quieter, then a long wash."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise
from ..synthesis.layering import laid_at
from ..synthesis.oscillators import TAU
from ..synthesis.shaping import decibels, normalised
from ..synthesis.timebase import samples_in
from ..space.stereo import pan, paired
from .rifle import close_shot, smeared

ECHO_DELAYS = (0.4, 1.05, 1.6, 2.3)
ECHO_LEVELS = (-9.0, -13.0, -17.0, -22.0)
ECHO_CUTOFFS = (3200.0, 2000.0, 1300.0, 800.0)
ECHO_SMEARS = (0.02, 0.04, 0.07, 0.1)
ECHO_POSITIONS = (-0.55, 0.6, -0.7, 0.45)
WASH_LEVEL = -22.0
WASH_SILENCE = 0.85
WASH_BUILD = 0.5
WASH_DECAY = 0.7
WASH_CUTOFF = 1400.0
RINGING_PITCHES = (6000.0, 6031.0)
RINGING_LEVEL = -42.0
RINGING_START = 0.1
RINGING_END = 2.5


def wash(seconds: float, randomness: Generator) -> np.ndarray:
    """The valley's rumble rolling back: nothing for the first second, then a swell that dies away."""
    length = samples_in(seconds)
    silence = samples_in(WASH_SILENCE)
    swelling = np.concatenate([np.zeros(silence), envelopes.strike(length - silence, WASH_BUILD, WASH_DECAY)])
    sides = [filters.lowpass(noise.white(length, randomness), WASH_CUTOFF) * swelling for _ in range(2)]
    return decibels(WASH_LEVEL) * normalised(paired(*sides))


def echo(shot: np.ndarray, number: int, randomness: Generator) -> np.ndarray:
    darkened = filters.lowpass(smeared(shot, ECHO_SMEARS[number], randomness), ECHO_CUTOFFS[number], order=filters.STEEP)
    return pan(decibels(ECHO_LEVELS[number]) * normalised(darkened), ECHO_POSITIONS[number])


def valley_echo(seconds: float) -> np.ndarray:
    """Everything the valley sends back after the shot, and the ringing it leaves in the ears."""
    randomness = noise.generator("valley echo")
    length = samples_in(seconds)
    rolling = wash(seconds, randomness) + ringing(seconds)
    shot = close_shot()
    for number, delay in enumerate(ECHO_DELAYS):
        laid_at(rolling, echo(shot, number, randomness), delay)
    return rolling


def ringing(seconds: float) -> np.ndarray:
    """The ears after the shot: a faint high tone, beating slightly, fading away."""
    length = samples_in(seconds)
    moments = np.arange(length) / length * seconds
    tone = sum(np.sin(TAU * pitch * moments) for pitch in RINGING_PITCHES) / len(RINGING_PITCHES)
    fading = np.clip((RINGING_END - moments) / (RINGING_END - RINGING_START), 0.0, 1.0) ** 2
    return decibels(RINGING_LEVEL) * tone * fading * (moments > RINGING_START)
