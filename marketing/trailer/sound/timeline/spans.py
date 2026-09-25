"""The stretches and moments of the edit: sections and shots as spans, sounds as cues."""
import math
from dataclasses import dataclass

ONE_SHOT = 0.0
STEP_TOLERANCE = 1e-4


@dataclass(frozen=True)
class Span:
    """A named stretch of the trailer: a section or a shot."""

    name: str
    start: float
    end: float

    @property
    def length(self) -> float:
        return self.end - self.start

    @property
    def middle(self) -> float:
        return (self.start + self.end) / 2.0

    def holds(self, moment: float) -> bool:
        return self.start <= moment < self.end


@dataclass(frozen=True)
class Cue:
    """A sound the edit asks for: once at its start, over a span, or repeating every so often until its end."""

    sound: str
    start: float
    end: float
    every: float

    @property
    def length(self) -> float:
        return self.end - self.start

    @property
    def times(self) -> tuple:
        is_repeating = self.every > ONE_SHOT
        count = math.ceil(self.length / self.every - STEP_TOLERANCE) if is_repeating else 1
        return tuple(self.start + index * self.every for index in range(count))
