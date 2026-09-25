"""Placing the edit's cues: each becomes a sound at its moment, with its own level, position and rooms."""
from dataclasses import dataclass
from typing import Callable

from ..space.ducking import ducked


@dataclass(frozen=True)
class CueMoment:
    """One cue as its sound needs to know it: when, how long, which of its kind, the beat, and the cuts around it."""

    start: float
    length: float
    ordinal: int
    beat: float
    starts_on_cut: bool
    ends_on_cut: bool


@dataclass(frozen=True)
class Placement:
    """How a cue is heard: the sound it makes, how loud, where, how far ahead of its moment it begins, and its rooms."""

    sound: Callable
    level: float
    pan: float = 0.0
    lead: float = 0.0
    sends: tuple = ()
    ducked_by: tuple = ()


def moments(edit, cue, ordinal: int, section) -> list:
    """The cue's moments inside the section: one for a single cue, one per repeat for a repeating one."""
    starts_on_cut = edit.is_cut(cue.start)
    ends_on_cut = edit.is_cut(cue.end)
    return [
        CueMoment(moment, cue.length, ordinal + index, edit.beat, starts_on_cut, ends_on_cut)
        for index, moment in enumerate(cue.times)
        if section.holds(moment)
    ]


def windows_of(edit, sounds: tuple) -> list:
    return [(cue.start, cue.end) for sound in sounds for cue in edit.cues_of(sound)]


def place_cues(canvas, edit, section, placements: dict) -> None:
    """Every cue in the section that the placements know how to sound, making way for any cue it is ducked by."""
    for sound, placement in placements.items():
        for ordinal, cue in enumerate(edit.cues_of(sound)):
            for moment in moments(edit, cue, ordinal, section):
                begins = moment.start - placement.lead
                heard = ducked(placement.sound(moment), begins, windows_of(edit, placement.ducked_by))
                canvas.place(heard, begins, level=placement.level, pan=placement.pan, sends=dict(placement.sends))
