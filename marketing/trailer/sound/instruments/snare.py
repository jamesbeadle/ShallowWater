"""The big gated snare and clap of a modern trailer: a crack, then a burst of room that is cut off short."""
import numpy as np
from numpy.random import Generator
from scipy import signal

from ..synthesis import envelopes, filters, noise, oscillators
from ..synthesis.envelopes import Attacks
from ..synthesis.layering import laid_at
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in, time_axis
from ..space.stereo import pan

SNARE_SECONDS = 0.45
BODY_PITCHES = (200.0, 172.0)
BODY_DECAY = 0.05
RATTLE_BAND = (1500.0, 9000.0)
RATTLE_DECAY = 0.11
CLAP_TIMES = (0.0, 0.011, 0.023)
CLAP_BAND = (900.0, 2600.0)
CLAP_DECAY = 0.009
GATE_OPEN = 0.21
GATE_CLOSE = 0.012
GATE_LEVEL = 0.55
BODY_SWEEP = 0.02
GATE_SAG = 0.7


class Balance:
    """The snare's layers against each other."""

    rattle = 0.8
    clap = 0.7


def dry_hit(randomness: Generator) -> np.ndarray:
    length = samples_in(SNARE_SECONDS)
    moments = time_axis(length)
    falling = BODY_PITCHES[1] + (BODY_PITCHES[0] - BODY_PITCHES[1]) * np.exp(-moments / BODY_SWEEP)
    body = oscillators.sine(falling, length) * envelopes.strike(length, Attacks.sharp, BODY_DECAY)
    rattle = normalised(filters.bandpass(noise.white(length, randomness), *RATTLE_BAND)) * envelopes.strike(length, Attacks.sharp, RATTLE_DECAY)
    clap = np.zeros(length)
    for moment in CLAP_TIMES:
        burst = filters.bandpass(noise.white(length, randomness), *CLAP_BAND) * envelopes.strike(length, Attacks.snap, CLAP_DECAY)
        laid_at(clap, normalised(burst), moment)
    return body + Balance.rattle * rattle + Balance.clap * clap


def gated_room(randomness: Generator) -> np.ndarray:
    open_length = samples_in(GATE_OPEN)
    closing = envelopes.rising(samples_in(GATE_CLOSE))[::-1]
    shape = np.concatenate([np.linspace(1.0, GATE_SAG, open_length), GATE_SAG * closing])
    return noise.white(len(shape), randomness) * shape


def gated_snare(name: str) -> np.ndarray:
    randomness = noise.generator(name)
    hit = dry_hit(randomness)
    sides = [signal.fftconvolve(hit, gated_room(randomness))[:len(hit)] for _ in range(2)]
    room = normalised(np.stack(sides))
    return pan(normalised(hit), 0.0) + GATE_LEVEL * room
