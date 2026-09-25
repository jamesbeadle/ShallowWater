"""The musical clock: common time at the edit's tempo, with the engine beating on one and three."""
from dataclasses import dataclass

SECONDS_PER_MINUTE = 60.0
BEATS_PER_BAR = 4


@dataclass(frozen=True)
class Tempo:
    """Beats and bars in seconds."""

    beats_per_minute: float

    @property
    def beat(self) -> float:
        return SECONDS_PER_MINUTE / self.beats_per_minute

    @property
    def bar(self) -> float:
        return self.beat * BEATS_PER_BAR

    def beats(self, count: float) -> float:
        return count * self.beat

    def bars(self, count: float) -> float:
        return count * self.bar
