"""The spaces the trailer's sounds are heard in, from a steel engine hole to a whole valley."""
from dataclasses import dataclass


@dataclass(frozen=True)
class Room:
    """A space's reverberation: decay times for lows and highs, how soon it answers and how it builds."""

    name: str
    low_decay: float
    high_decay: float
    predelay: float
    build: float
    early_taps: int
    early_span: float
    brightness: float
    resonances: tuple = ()


class Rooms:
    """Every space the soundtrack uses."""

    engine_room = Room("engine room", 0.9, 0.5, 0.002, 0.004, 18, 0.022, 7000.0, ((620.0, 6.0), (1480.0, 9.0), (2650.0, 12.0)))
    cabin = Room("cabin", 0.35, 0.2, 0.001, 0.003, 10, 0.012, 3500.0)
    hall = Room("hall", 3.0, 1.7, 0.024, 0.02, 14, 0.09, 9000.0)
    cathedral = Room("cathedral", 5.5, 2.8, 0.04, 0.05, 12, 0.14, 6500.0)
    valley = Room("valley", 3.6, 1.4, 0.07, 0.16, 6, 0.25, 3000.0)
    wood = Room("wood", 2.2, 0.9, 0.05, 0.08, 8, 0.18, 2400.0)
