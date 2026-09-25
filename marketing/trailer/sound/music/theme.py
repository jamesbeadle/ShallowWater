"""The game's melody: eight bars in D minor, first heard on the wireless and last on the whole orchestra."""
from dataclasses import dataclass

from ..timeline.tempo import BEATS_PER_BAR

REST = None

THEME = (
    (("A4", 1), ("D5", 1), ("F5", 1), ("E5", 0.5), ("D5", 0.5)),
    (("C5", 1.5), ("Bb4", 0.5), ("A4", 2)),
    (("G4", 1), ("A4", 1), ("C#5", 1), ("E5", 1)),
    (("D5", 3), (REST, 1)),
    (("A4", 1), ("D5", 1), ("F5", 1), ("A5", 1)),
    (("G5", 1.5), ("F5", 0.5), ("E5", 1), ("D5", 1)),
    (("E5", 1), ("C#5", 1), ("A4", 1), ("G4", 0.5), ("E4", 0.5)),
    (("D4", 4),),
)

THEME_HARMONY = tuple(((chord, BEATS_PER_BAR),) for chord in ("Dm", "Gm", "A7", "Dm", "Dm", "Gm", "A7", "Dm"))

DANCE_BAND_TAG = (
    (("F5", 1), ("E5", 1), ("D5", 1), ("C#5", 1)),
    (("D5", 2), ("A4", 1), ("D4", 1)),
)

DANCE_BAND_TAG_HARMONY = ((("Gm", 2), ("A7", 2)), (("Dm", BEATS_PER_BAR),))


@dataclass(frozen=True)
class Event:
    """A note or a chord: what it is, the beat it starts on from the top of the phrase, and how many beats it lasts."""

    name: str
    beat: float
    length: float


def laid_out(bars) -> tuple:
    """Bars of (name, beats) pairs laid end to end as timed events, rests left out."""
    events = []
    for number, bar in enumerate(bars):
        beat = number * BEATS_PER_BAR
        for name, length in bar:
            events.append(Event(name, beat, length))
            beat += length
    return tuple(event for event in events if event.name is not REST)


def bars_of(bars, first: int, last: int) -> tuple:
    """Bars first to last of a phrase, counting from one as a musician would."""
    return bars[first - 1:last]
