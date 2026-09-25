"""Tension under the clients' offers: a low D pedal, a muted clock on the quavers, and the build into the suck-back."""
import numpy as np
from numpy.random import Generator

from ..cinematic.swells import reverse_swell
from ..instruments import clock, strings, timpani
from ..instruments.strings import PEDAL_BOW
from ..space.rooms import Rooms
from ..synthesis import envelopes, noise
from ..synthesis.pitches import frequency
from ..timeline.names import Cues

PEDAL = ("D2", "A2", "D3")
BUILD_CHORDS = (("D4", "F4", "A4"), ("D4", "F4", "Bb4"), ("C#4", "E4", "A4"))
SUCK_BACK_SECONDS = 1.0
QUAVER = 0.5
BUILD_FROM = (-20.0, -2.0)
BUILD_SWELL = (0.5, 1.0)
ROLL_SECONDS = 2.5
ROLL_NOTE = "A1"


class Balance:
    """The bed's parts, in decibels."""

    pedal = -11.0
    tick = -27.0
    tock = -30.0
    suck_back = 2.0
    roll = -6.0
    room = {Rooms.hall: -6.0}


def pedal(canvas, section, randomness: Generator) -> None:
    held = strings.chord([frequency(name) for name in PEDAL], section.length - PEDAL_BOW.release, PEDAL_BOW, strings.LOW_STRINGS, randomness)
    canvas.place(held, section.start, level=Balance.pedal)


def ticking(canvas, section, beat: float, randomness: Generator) -> None:
    """Tick and tock on every quaver until the suck-back takes the air out of the room."""
    quavers = np.arange(section.start, section.end - SUCK_BACK_SECONDS, beat * QUAVER)
    for number, moment in enumerate(quavers):
        is_tock = bool(number % 2)
        canvas.place(clock.tick(is_tock, randomness), moment, level=Balance.tock if is_tock else Balance.tick)


def build(canvas, start: float, end: float, randomness: Generator) -> None:
    """Strings climbing through D minor, B flat and A, louder and brighter, towards the drop."""
    step = (end - start) / len(BUILD_CHORDS)
    levels = np.linspace(*BUILD_FROM, len(BUILD_CHORDS))
    for number, voicing in enumerate(BUILD_CHORDS):
        chord = strings.chord([frequency(name) for name in voicing], step, strings.FULL_BOW, strings.VIOLINS, randomness)
        rising = envelopes.curve(chord.shape[1], *BUILD_SWELL)
        canvas.place(chord * rising, start + number * step, level=levels[number], sends=Balance.room)


def score(canvas, edit, section) -> None:
    randomness = noise.generator("tension")
    stings = edit.cues_within(section, Cues.client_sting)
    build_from = stings[-1].start if stings else section.middle
    pedal(canvas, section, randomness)
    ticking(canvas, section, edit.beat, randomness)
    build(canvas, build_from, section.end - SUCK_BACK_SECONDS, randomness)
    rolling = timpani.roll(frequency(ROLL_NOTE), ROLL_SECONDS, randomness)
    canvas.place(rolling, section.end - SUCK_BACK_SECONDS - ROLL_SECONDS, level=Balance.roll, sends=Balance.room)
    canvas.place(reverse_swell(SUCK_BACK_SECONDS), section.end - SUCK_BACK_SECONDS, level=Balance.suck_back)
