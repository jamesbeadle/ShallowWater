"""Reads the trailer's edit: the sections, shots and cues that every sound is timed from."""
import json
from dataclasses import dataclass
from pathlib import Path

from .spans import ONE_SHOT, Cue, Span
from .tempo import Tempo

CUT_TOLERANCE = 1e-3


@dataclass(frozen=True)
class Edit:
    """The whole edit, as the soundtrack needs it."""

    duration: float
    tempo: Tempo
    sections: tuple
    shots: tuple
    cues: tuple

    @property
    def beat(self) -> float:
        return self.tempo.beat

    @property
    def bar(self) -> float:
        return self.tempo.bar

    def cues_of(self, sound: str) -> tuple:
        return tuple(cue for cue in self.cues if cue.sound == sound)

    def cues_within(self, span: Span, sound: str) -> tuple:
        return tuple(cue for cue in self.cues_of(sound) if span.holds(cue.start))

    def beats_within(self, span: Span, sound: str) -> tuple:
        """Every repetition of a repeating cue that falls inside a span, whichever cue it belongs to."""
        return tuple(moment for cue in self.cues_of(sound) for moment in cue.times if span.holds(moment))

    def is_cut(self, moment: float) -> bool:
        """Whether the picture cuts at this moment: a shot starts or ends exactly there."""
        return any(abs(shot.start - moment) < CUT_TOLERANCE or abs(shot.end - moment) < CUT_TOLERANCE for shot in self.shots)

    def moment_of(self, sound: str, fallback: float) -> float:
        """When a cue first sounds, or the fallback if the edit no longer asks for it."""
        cues = self.cues_of(sound)
        return cues[0].start if cues else fallback


def span_from(raw: dict) -> Span:
    return Span(raw["name"], raw["start"], raw["end"])


def cue_from(raw: dict) -> Cue:
    start = raw.get("at", raw.get("start"))
    return Cue(raw["sound"], start, raw.get("end", start), raw.get("every", ONE_SHOT))


def load(path: Path) -> Edit:
    raw = json.loads(path.read_text())
    sound = raw["sound"]
    return Edit(
        duration=raw["duration"],
        tempo=Tempo(raw["beatsPerMinute"]),
        sections=tuple(span_from(section) for section in sound["sections"]),
        shots=tuple(span_from(shot) for shot in raw["shots"]),
        cues=tuple(cue_from(cue) for cue in sound["cues"]),
    )
