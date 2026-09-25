"""The night's drums: taiko patterns that play around the engine's kick, changing every two bars, and a gated snare on two and four."""
from numpy.random import Generator

from ..instruments import snare, taiko
from ..space.rooms import Rooms

SIXTEENTHS_PER_BEAT = 4
TAIKO_PITCHES = (60.0, 94.0)
BARS_PER_PATTERN = 2
BACKBEATS = (1, 3)
DRUM_POSITIONS = (-0.12, 0.12)
PATTERNS = (
    ((3, 0.6, 1), (6, 0.8, 0), (10, 0.85, 0), (14, 0.65, 1)),
    ((2, 0.55, 1), (3, 0.6, 1), (6, 0.8, 0), (11, 0.7, 1), (12, 0.75, 0), (14, 0.8, 0), (15, 0.6, 1)),
    ((6, 0.75, 0), (7, 0.5, 1), (10, 0.85, 0), (13, 0.55, 1), (14, 0.8, 0)),
    ((2, 0.55, 1), (4, 0.7, 0), (6, 0.8, 0), (10, 0.7, 1), (12, 0.85, 0), (13, 0.8, 1), (14, 0.9, 0), (15, 1.0, 1)),
)


class Balance:
    """The drums against each other, in decibels, and the room they are thrown into."""

    taiko = -5.0
    snare = 1.0
    room = {Rooms.hall: -12.0}
    snare_room = {Rooms.cathedral: -16.0}


def taiko_bar(canvas, bar: int, bar_start: float, beat: float, randomness: Generator) -> None:
    pattern = PATTERNS[(bar // BARS_PER_PATTERN) % len(PATTERNS)]
    sixteenth = beat / SIXTEENTHS_PER_BEAT
    for position, velocity, drum in pattern:
        hit = taiko.drum(TAIKO_PITCHES[drum], velocity, randomness)
        canvas.place(hit, bar_start + position * sixteenth, level=Balance.taiko, pan=DRUM_POSITIONS[drum], sends=Balance.room)


def backbeat(canvas, bar: int, bar_start: float, beat: float) -> None:
    for number in BACKBEATS:
        canvas.place(snare.gated_snare(f"night snare {bar} {number}"), bar_start + number * beat, level=Balance.snare, sends=Balance.snare_room)


def drums(canvas, bar: int, bar_start: float, beat: float, randomness: Generator) -> None:
    taiko_bar(canvas, bar, bar_start, beat, randomness)
    backbeat(canvas, bar, bar_start, beat)
