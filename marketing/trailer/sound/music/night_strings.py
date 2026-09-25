"""The night's strings and bass: a spiccato ostinato on the chords, a gritty saw bass in quavers and a sub beneath."""
import numpy as np
from numpy.random import Generator

from ..instruments import spiccato, strings, synth_bass
from ..space.ducking import ducked
from ..space.rooms import Rooms
from ..synthesis.pitches import frequency, transposed

VOICINGS = {"Dm": ("D4", "F4", "A4"), "Bb": ("D4", "F4", "Bb4"), "Gm": ("D4", "G4", "Bb4"), "A": ("C#4", "E4", "A4")}
FIGURE = (0, 2, 1, 2, 0, 2, 1, 2)
ACCENTS = (1.0, 0.7, 0.85, 0.7, 0.95, 0.7, 0.85, 0.75)
ROOTS = {"Dm": "D2", "Bb": "Bb1", "Gm": "G1", "A": "A1"}
QUAVERS_PER_BAR = 8
MAKING_ROOM = (0.45, 1.0, 1.0, 1.0, 0.45, 1.0, 1.0, 1.0)
KICKS_PER_BAR = 2
TREMOLO_SWELL = (0.3, 1.0)
PEDAL_SWELL = (0.35, 1.0)
OCTAVE = 12
BRIGHTNESS = (3200.0, 7000.0)
BASS_BRIGHTNESS = (700.0, 1600.0)
QUAVER_SECONDS = 0.22
PEDAL = ("A1", "A2", "E3")
TREMOLO = ("A3", "E4", "A4")


class Balance:
    """The strings and bass, in decibels, and the room they play in."""

    ostinato = 0.0
    lifted = -3.0
    bass = -8.0
    sub = -14.0
    pedal = -2.0
    tremolo = -7.0
    room = {Rooms.hall: -10.0}


LAYERS = ((0, Balance.ostinato, -0.1), (OCTAVE, Balance.lifted, 0.3))
LIFT = 3.0


def ostinato(canvas, chord: str, bar_start: float, quaver: float, is_lifted: bool, randomness: Generator) -> None:
    """Eight bounced quavers on the chord; once the night lifts, the violins double them an octave up."""
    pitches = [frequency(name) for name in VOICINGS[chord]]
    brightness = BRIGHTNESS[int(is_lifted)]
    layers = LAYERS if is_lifted else LAYERS[:1]
    for number, (tone, accent) in enumerate(zip(FIGURE, ACCENTS)):
        for shift, level, position in layers:
            stroke = spiccato.stroke(transposed(pitches[tone], shift), accent, brightness, randomness)
            canvas.place(stroke, bar_start + number * quaver, level=level + LIFT * is_lifted, pan=position, sends=Balance.room)


def bass(canvas, chord: str, bar_start: float, quaver: float, is_lifted: bool, randomness: Generator) -> None:
    """Saw quavers on the root, pulled back on the engine's beats, and a sub that ducks under every one of them."""
    root = frequency(ROOTS[chord])
    for number, room in enumerate(MAKING_ROOM):
        pulse = synth_bass.pulse(root, QUAVER_SECONDS, BASS_BRIGHTNESS[int(is_lifted)], randomness) * room
        canvas.place(pulse, bar_start + number * quaver, level=Balance.bass + LIFT * is_lifted)
    bar_seconds = quaver * QUAVERS_PER_BAR
    sub = synth_bass.sub(root, bar_seconds)
    kicks = [number * bar_seconds / KICKS_PER_BAR for number in range(KICKS_PER_BAR)]
    canvas.place(sub * synth_bass.pumping(len(sub), kicks), bar_start, level=Balance.sub)


def held_breath(canvas, start: float, end: float, breaths: list, randomness: Generator) -> None:
    """The riser's floor: low strings on A, violins trembling above them, swelling to the cut and giving way to a breath."""
    low = strings.chord([frequency(name) for name in PEDAL], end - start, strings.FULL_BOW, strings.LOW_STRINGS, randomness)
    high = strings.chord([frequency(name) for name in TREMOLO], end - start, strings.SLOW_BOW, strings.VIOLINS, randomness)
    swelling = np.linspace(*TREMOLO_SWELL, high.shape[1]) ** 2
    canvas.place(ducked(low * np.linspace(*PEDAL_SWELL, low.shape[1]), start, breaths), start, level=Balance.pedal)
    canvas.place(ducked(high * swelling, start, breaths), start, level=Balance.tremolo, sends=Balance.room)
