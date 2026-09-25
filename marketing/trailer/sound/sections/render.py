"""A section rendered whole: its cues, engine and score on one canvas, mixed, and cut dead or let ring at its end."""
import numpy as np

from ..space.canvas import Canvas
from ..synthesis import envelopes
from ..synthesis.shaping import decibels
from ..synthesis.timebase import samples_in
from .cue_placement import place_cues
from .engine_beats import place_engine
from .score_cues import SCORE
from .styles import DEFAULT_STYLE, STYLES
from .world_cues import WORLD

CUT_SECONDS = 0.002


def letting_go(canvas: Canvas, section, tail: float) -> np.ndarray:
    """Full until the section ends, then gone: at once for a hard cut, or faded over the tail it may ring for."""
    gain = np.ones(canvas.length)
    ending = samples_in(section.end - canvas.start)
    fade = samples_in(max(tail, CUT_SECONDS))
    closing = envelopes.rising(fade)[::-1]
    fade_start = ending - fade if tail < CUT_SECONDS else ending
    gain[fade_start:fade_start + fade] = closing[:len(gain[fade_start:fade_start + fade])]
    gain[fade_start + fade:] = 0.0
    return gain


def render_section(edit, section) -> tuple:
    """The section's sound and the moment it starts, which may be a little before the section for sounds that lead in."""
    style = STYLES.get(section.name, DEFAULT_STYLE)
    canvas = Canvas(max(section.start - style.lead, 0.0), min(section.end + style.tail, edit.duration))
    place_cues(canvas, edit, section, WORLD)
    place_cues(canvas, edit, section, SCORE)
    place_engine(canvas, edit, section, style)
    style.score(canvas, edit, section)
    return canvas.start, canvas.mixdown() * letting_go(canvas, section, style.tail) * decibels(style.level)
