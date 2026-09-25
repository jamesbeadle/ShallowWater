"""A stretch of the soundtrack under construction: a dry stereo bus and a send to each room it uses."""
import numpy as np

from ..synthesis.layering import mix_into
from ..synthesis.shaping import decibels
from ..synthesis.timebase import samples_in
from . import reverb, stereo


class Canvas:
    """Sounds placed sample-accurately in time, level, stereo position and room."""

    def __init__(self, start: float, end: float):
        self.start = start
        self.end = end
        self.length = samples_in(end - start)
        self.dry = np.zeros((2, self.length))
        self.sends = {}

    def place(self, sound: np.ndarray, at: float, level: float = 0.0, pan: float = 0.0, sends=None) -> None:
        """Adds a sound starting at a time in seconds, level in decibels, with sends in decibels per room."""
        placed = stereo.as_stereo(sound, pan) * decibels(level)
        offset = samples_in(at - self.start)
        mix_into(self.dry, placed, offset)
        for room, send_level in (sends or {}).items():
            mix_into(self.send_to(room), placed * decibels(send_level), offset)

    def send_to(self, room) -> np.ndarray:
        if room not in self.sends:
            self.sends[room] = np.zeros((2, self.length))
        return self.sends[room]

    def mixdown(self) -> np.ndarray:
        returns = [reverb.reverberate(send, room) for room, send in self.sends.items()]
        return self.dry + sum(returns)
