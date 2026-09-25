"""The lift: trombones and horns swelling on the chord under the octave-up ostinato, pushing the night towards the riser."""
import numpy as np
from numpy.random import Generator

from ..instruments import brass
from ..space.rooms import Rooms
from ..synthesis.layering import summed
from ..synthesis.pitches import frequency

VOICINGS = {"A": ("A2", "E3", "A3", "C#4"), "Dm": ("D3", "A3", "D4", "F4"), "Gm": ("G2", "D3", "G3", "Bb3"), "Bb": ("Bb2", "F3", "Bb3", "D4")}
TOUCH = 0.8
LEVEL = -7.0
SWELL_FROM = 0.25
SWELL_CURVE = 1.5
ROOM = {Rooms.hall: -8.0}


def swell(canvas, chord: str, start: float, end: float, randomness: Generator) -> None:
    """One chord held from start to end by trombones and horns, growing all the way."""
    seconds = end - start
    section = summed(brass.note(frequency(name), seconds, TOUCH, timbre, randomness) for name in VOICINGS[chord] for timbre in (brass.TROMBONES, brass.HORNS))
    growing = np.linspace(SWELL_FROM, 1.0, section.shape[-1]) ** SWELL_CURVE
    canvas.place(section * growing, start, level=LEVEL, sends=ROOM)
