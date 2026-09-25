"""The engine's beat through a section: every firing the edit cues there, heard the way the section hears it."""
import numpy as np

from ..effects.engine import firing
from ..synthesis import noise
from ..timeline.names import Cues

SILENCE = -60.0
MILLISECONDS = 1000
SHORTEST_FADE = 1e-9
FADE_CURVE = 2


def fading(beats: tuple, fade_from: float, fade_to: float) -> np.ndarray:
    """Decibels for each beat: full until fade_from, then falling away to silence by fade_to."""
    progress = np.clip((np.asarray(beats) - fade_from) / max(fade_to - fade_from, SHORTEST_FADE), 0.0, 1.0)
    return SILENCE * progress ** FADE_CURVE


def place_engine(canvas, edit, section, style) -> None:
    beats = edit.beats_within(section, Cues.engine_beat)
    fade_from = edit.moment_of(style.engine_fades_after, section.end) if style.engine_fades_after else section.end
    for moment, fade in zip(beats, fading(beats, fade_from, section.end)):
        randomness = noise.generator("engine beat", int(round(moment * MILLISECONDS)))
        canvas.place(firing(style.engine, randomness), moment, level=style.engine_level + fade, sends=dict(style.engine_sends))
