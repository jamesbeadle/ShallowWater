"""How each section sounds as a whole: its score, how the engine is heard there, its level and how it lets go at its end."""
from dataclasses import dataclass
from typing import Callable

from ..effects.engine import EngineVoice
from ..effects.engine_voices import EngineVoices
from ..music import button, engine_hole, finale, night_drive, on_the_cut, tension
from ..space.rooms import Rooms
from ..timeline.names import Cues, Sections


def unscored(canvas, edit, section) -> None:
    """A section carried by its cues alone."""


@dataclass(frozen=True)
class SectionStyle:
    """A section's score and engine, its level in decibels, how long it may ring past its end, and how early it may begin."""

    score: Callable = unscored
    engine: EngineVoice = EngineVoices.on_the_bank
    engine_level: float = -12.0
    engine_sends: tuple = ()
    engine_fades_after: str = ""
    level: float = 0.0
    tail: float = 1.5
    lead: float = 1.0


STYLES = {
    Sections.cold_open: SectionStyle(tail=0.3),
    Sections.the_engine: SectionStyle(engine_hole.score, EngineVoices.catching, 2.0, ((Rooms.engine_room, -2.0),), tail=6.0),
    Sections.the_cut: SectionStyle(on_the_cut.score, EngineVoices.on_the_bank, -12.0, ((Rooms.valley, -10.0),), tail=2.0),
    Sections.the_clients: SectionStyle(tension.score, EngineVoices.up_the_hill, -6.0, ((Rooms.hall, -8.0),), tail=0.0),
    Sections.the_night: SectionStyle(night_drive.score, EngineVoices.in_the_drop, 1.0, ((Rooms.hall, -18.0),), tail=0.0),
    Sections.the_shot: SectionStyle(tail=0.3),
    Sections.the_water: SectionStyle(finale.score, EngineVoices.in_the_water, 2.0, ((Rooms.cathedral, -8.0),), Cues.title_hit, tail=1.0),
    Sections.the_button: SectionStyle(button.score, EngineVoices.alone, -18.0, ((Rooms.valley, -2.0),), tail=0.0),
}

DEFAULT_STYLE = SectionStyle()
