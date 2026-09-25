"""The chain across the cut giving way: a link snapping, the clank, and the rattle as it runs out."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, modes, noise
from ..synthesis.envelopes import Attacks
from ..synthesis.layering import laid_at
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in
from ..space.stereo import pan

PING_MODES = modes.partials_of((2210.0, 3470.0, 4630.0, 5980.0, 7350.0), (1.0, 0.7, 0.5, 0.35, 0.2), (0.35, 0.25, 0.18, 0.12, 0.08))
CLANK_MODES = modes.partials_of((410.0, 655.0, 930.0, 1290.0, 1720.0), (1.0, 0.8, 0.6, 0.45, 0.3), (0.12, 0.1, 0.08, 0.06, 0.05))
LINK_RATIOS = (1.0, 1.47, 2.09)
LINK_AMPLITUDES = (1.0, 0.6, 0.3)
CLINK_PITCHES = (1300.0, 4500.0)
CLINK_DECAYS = (0.02, 0.06)
CLINK_LOUDNESS = (0.4, 1.0)
CLINK_FADE = 0.7
CLINKS = 26
RATTLE_SECONDS = 0.8
RATTLE_BUNCHING = 1.7
RATTLE_TRAVEL = (-0.2, 0.3)
SLITHER_BAND = (1000.0, 5000.0)
SLITHER_LEVEL = 0.2
SNAP_SECONDS = 1.3
CLANK_DELAY = 0.015
RATTLE_DELAY = 2.0 * CLANK_DELAY
CLICK_BAND = (2000.0, 12000.0)
CLICK_SECONDS = 0.01
CLICK_DECAY = 0.001
CLANK_BAND = (500.0, 3000.0)
CLANK_BURST_SECONDS = 0.05
CLANK_BURST_DECAY = 0.015
PING_SECONDS = 0.6
CLANK_SECONDS = 0.5
LINK_RING = 5.0
RATTLE_ROOM = 1.3
SLITHER_DECAY_SHARE = 1.0 / 3.0


class Balance:
    """The snap's layers against each other."""

    ping = 1.0
    click = 0.8
    clank = 0.9
    clank_burst = 0.6
    rattle = 0.6


def burst(seconds: float, band: tuple, decay: float, randomness: Generator) -> np.ndarray:
    length = samples_in(seconds)
    return normalised(filters.bandpass(noise.white(length, randomness) * envelopes.strike(length, Attacks.snap, decay), *band))


def clink(randomness: Generator) -> np.ndarray:
    pitch = randomness.uniform(*CLINK_PITCHES)
    decay = randomness.uniform(*CLINK_DECAYS)
    link = modes.partials_of([pitch * ratio for ratio in LINK_RATIOS], LINK_AMPLITUDES, (decay,) * len(LINK_RATIOS))
    return normalised(modes.ring(link, decay * LINK_RING, randomness))


def rattle(randomness: Generator) -> np.ndarray:
    """The links running over each other, dense at first and thinning out, sliding across the frame."""
    running = np.zeros((2, samples_in(RATTLE_SECONDS * RATTLE_ROOM)))
    for number in range(CLINKS):
        progress = (number / CLINKS) ** RATTLE_BUNCHING
        loudness = (1.0 - CLINK_FADE * number / CLINKS) * randomness.uniform(*CLINK_LOUDNESS)
        position = RATTLE_TRAVEL[0] + (RATTLE_TRAVEL[1] - RATTLE_TRAVEL[0]) * progress
        laid_at(running, pan(loudness * clink(randomness), position), RATTLE_SECONDS * progress)
    laid_at(running, SLITHER_LEVEL * burst(RATTLE_SECONDS, SLITHER_BAND, RATTLE_SECONDS * SLITHER_DECAY_SHARE, randomness), 0.0)
    return running


def chain_snap() -> np.ndarray:
    randomness = noise.generator("chain snap")
    snap = np.zeros((2, samples_in(SNAP_SECONDS)))
    layers = (
        (Balance.ping * normalised(modes.ring(PING_MODES, PING_SECONDS, randomness)), 0.0),
        (Balance.click * burst(CLICK_SECONDS, CLICK_BAND, CLICK_DECAY, randomness), 0.0),
        (Balance.clank * normalised(modes.ring(CLANK_MODES, CLANK_SECONDS, randomness)), CLANK_DELAY),
        (Balance.clank_burst * burst(CLANK_BURST_SECONDS, CLANK_BAND, CLANK_BURST_DECAY, randomness), CLANK_DELAY),
        (Balance.rattle * rattle(randomness), RATTLE_DELAY),
    )
    for sound, moment in layers:
        laid_at(snap, sound, moment)
    return snap
