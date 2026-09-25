"""Timpani: a tuned skin struck once, or rolled into the bar line with a crescendo."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, modes, noise
from ..synthesis.layering import laid_at
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in

MODE_RATIOS = (0.62, 1.0, 1.5, 1.98, 2.44, 2.9)
MODE_LEVELS = (0.5, 1.0, 0.55, 0.35, 0.2, 0.1)
MODE_DECAYS = (0.12, 1.6, 1.0, 0.8, 0.5, 0.35)
MALLET_CUTOFF = 1200.0
MALLET_DECAY = 0.006
MALLET_LEVEL = 0.35
STROKE_SECONDS = 2.5
ROLL_STROKE_SECONDS = 1.2
ROLL_SPACING = 0.055
ROLL_UNEVENNESS = 0.12
QUIETEST_ROLL = 0.08
HEAD_SCATTER = 0.003
STICK_EVENNESS = (0.9, 1.0)


def stroke(pitch: float, velocity: float, randomness: Generator, seconds: float = STROKE_SECONDS) -> np.ndarray:
    skin = modes.partials_of([pitch * ratio for ratio in MODE_RATIOS], MODE_LEVELS, MODE_DECAYS)
    tone = normalised(modes.ring(modes.scattered(skin, HEAD_SCATTER, randomness), seconds, randomness))
    length = len(tone)
    mallet = filters.lowpass(noise.white(length, randomness), MALLET_CUTOFF) * envelopes.exponential_decay(length, MALLET_DECAY)
    return velocity * (tone + MALLET_LEVEL * normalised(mallet))


def roll(pitch: float, seconds: float, randomness: Generator) -> np.ndarray:
    """Single strokes from alternate sticks, growing from almost nothing to full over seconds."""
    rolled = np.zeros(samples_in(seconds + ROLL_STROKE_SECONDS))
    count = int(seconds / ROLL_SPACING)
    for number in range(count):
        growing = QUIETEST_ROLL + (1.0 - QUIETEST_ROLL) * (number / count) ** 2
        velocity = growing * (1.0 - ROLL_UNEVENNESS * (number % 2)) * randomness.uniform(*STICK_EVENNESS)
        laid_at(rolled, stroke(pitch, velocity, randomness, ROLL_STROKE_SECONDS), number * ROLL_SPACING)
    return rolled
