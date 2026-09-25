"""The full orchestra of the finale: chords in horns and strings, the tune in trumpets and violins, roots in the basses."""
import numpy as np

from ..cinematic.impacts import low_boom
from ..instruments import brass, strings, timpani
from ..space.rooms import Rooms
from ..synthesis.pitches import frequency, transposed

CHORD_VOICINGS = {
    "Dm": ("D3", "A3", "D4", "F4"),
    "Gm": ("D3", "G3", "Bb3", "D4"),
    "A7": ("C#3", "G3", "A3", "E4"),
    "Dm9": ("D3", "A3", "D4", "E4", "F4", "A4"),
}
STRING_LIFT = {"Dm": 0, "Gm": 0, "A7": 0, "Dm9": 12}
ROOTS = {"Dm": ("D2", "D3"), "Gm": ("G1", "G2"), "A7": ("A1", "A2"), "Dm9": ("D2", "D3")}
TIMPANI_NOTES = {"Dm": "D2", "Gm": "D2", "A7": "A1", "Dm9": "D2"}
OCTAVE = 12


class Balance:
    """The orchestra's sections, in decibels, and the hall around them."""

    horns = -9.0
    strings = -8.0
    basses = -7.0
    trumpets = -6.0
    violins = -9.0
    low_horns = -11.0
    timpani = -6.0
    boom = -3.0
    hall = {Rooms.hall: -7.0, Rooms.cathedral: -13.0}


def steady(length: int) -> np.ndarray:
    return np.ones(length)


def chord(canvas, name: str, moment: float, seconds: float, velocity: float, randomness, dynamics=steady) -> None:
    """One chord from the whole orchestra, shaped by dynamics: horns and strings in the middle, cellos and basses on the root."""
    pitches = [frequency(pitch) for pitch in CHORD_VOICINGS[name]]
    horns = sum(brass.note(pitch, seconds, velocity, brass.HORNS, randomness) for pitch in pitches) / np.sqrt(len(pitches))
    bed = strings.chord([transposed(pitch, STRING_LIFT[name]) for pitch in pitches], seconds, strings.FULL_BOW, strings.VIOLINS, randomness)
    basses = strings.chord([frequency(pitch) for pitch in ROOTS[name]], seconds, strings.FULL_BOW, strings.LOW_STRINGS, randomness)
    for section, level in ((horns, Balance.horns), (bed * velocity, Balance.strings), (basses * velocity, Balance.basses)):
        canvas.place(section * dynamics(section.shape[-1]), moment, level=level, sends=Balance.hall)


def melody_note(canvas, pitch: float, moment: float, seconds: float, velocity: float, randomness, dynamics=steady) -> None:
    """A note of the tune in trumpets, doubled an octave up in the violins and an octave down in the horns."""
    trumpets = brass.note(pitch, seconds, velocity, brass.TRUMPETS, randomness)
    violins = strings.bowed(transposed(pitch, OCTAVE), seconds, strings.FULL_BOW, strings.VIOLINS, randomness) * velocity
    low_horns = brass.note(transposed(pitch, -OCTAVE), seconds, velocity, brass.HORNS, randomness)
    for section, level in ((trumpets, Balance.trumpets), (violins, Balance.violins), (low_horns, Balance.low_horns)):
        canvas.place(section * dynamics(section.shape[-1]), moment, level=level, sends=Balance.hall)


def strike(canvas, name: str, moment: float, randomness) -> None:
    """The downbeat's weight: a full timpani stroke and a deep boom under the chord."""
    canvas.place(timpani.stroke(frequency(TIMPANI_NOTES[name]), 1.0, randomness), moment, level=Balance.timpani + 2.0, sends=Balance.hall)
    canvas.place(low_boom(), moment, level=Balance.boom, sends=Balance.hall)


def timpani_into(canvas, name: str, moment: float, seconds: float, randomness) -> None:
    """A roll growing into the bar line, and the stroke that lands on it."""
    pitch = frequency(TIMPANI_NOTES[name])
    canvas.place(timpani.roll(pitch, seconds, randomness), moment - seconds, level=Balance.timpani, sends=Balance.hall)
    canvas.place(timpani.stroke(pitch, 1.0, randomness), moment, level=Balance.timpani, sends=Balance.hall)
