"""The clients' stings: a dark, restrained accent as each face appears, drawn in by a reverse swell."""
import numpy as np

from ..cinematic.swells import reverse_swell
from ..instruments import piano, strings
from ..synthesis import noise
from ..synthesis.layering import laid_at
from ..synthesis.pitches import frequency
from ..synthesis.timebase import samples_in
from . import theme

SWELL_SECONDS = 0.9
RING_SECONDS = 4.5
LOW_NOTES = ("D1", "D2")
MUTED_STAB = ("D3", "A3", "D4", "F4")
MUTED = strings.Bowing(attack=0.008, release=0.4, brightness=850.0, floor=90.0)
STAB_SECONDS = 0.16
MOTIF_LENGTH = 4
GROWING = (0.8, 0.85, 0.9, 1.0)
LOW_TOUCH = 0.75
HIGH_TOUCH = 0.5
HELD_SHARE = 0.5


class Balance:
    """The sting's parts against each other."""

    swell = 0.35
    low_piano = 1.0
    high_piano = 0.3
    stab = 0.6


def motif_note(ordinal: int) -> float:
    """Each client in turn gets the next note of the theme's opening: A, D, F, E."""
    opening = theme.laid_out(theme.THEME)[:MOTIF_LENGTH]
    return frequency(opening[ordinal % MOTIF_LENGTH].name)


def client_sting(ordinal: int) -> np.ndarray:
    """The sting for the ordinal-th client, starting SWELL_SECONDS before the moment it lands."""
    randomness = noise.generator("client sting", ordinal)
    sting = np.zeros((2, samples_in(SWELL_SECONDS + RING_SECONDS)))
    laid_at(sting, Balance.swell * reverse_swell(SWELL_SECONDS), 0.0)
    held = RING_SECONDS * HELD_SHARE
    low = sum(piano.note(frequency(name), held, LOW_TOUCH, piano.CONCERT_GRAND, randomness) for name in LOW_NOTES)
    laid_at(sting, Balance.low_piano * low, SWELL_SECONDS)
    laid_at(sting, Balance.high_piano * piano.note(motif_note(ordinal), held, HIGH_TOUCH, piano.CONCERT_GRAND, randomness), SWELL_SECONDS)
    stab = strings.chord([frequency(name) for name in MUTED_STAB], STAB_SECONDS, MUTED, strings.VIOLINS, randomness)
    laid_at(sting, Balance.stab * stab, SWELL_SECONDS)
    return sting * GROWING[min(ordinal, len(GROWING) - 1)]
