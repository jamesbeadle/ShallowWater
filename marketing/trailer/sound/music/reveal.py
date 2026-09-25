"""The reveal from the hilltop: the theme's opening, soft and alone on a horn, over strings swelling in."""
import numpy as np
from numpy.random import Generator

from ..instruments import brass, strings
from ..space.canvas import Canvas
from ..synthesis import envelopes, noise
from ..synthesis.ensembles import Ensemble
from ..synthesis.pitches import frequency
from ..synthesis.timebase import samples_in
from . import theme

SOLO_HORN = brass.BrassTimbre(Ensemble(2, 3.0, 4.6, 7.0, 1.5, 0.25), 200.0, 900.0, 0.18, 0.12, 0.6, 1.2)
HORN_TOUCH = 0.55
HORN_LEVEL = -2.0
STRING_LEVEL = -7.0
REVEAL_BARS = 2
VOICINGS = {"Dm": ("D3", "A3", "D4", "F4"), "Gm": ("D3", "G3", "Bb3", "D4")}
CLOSING_CHORD = "Dm"
GONE_BEFORE_END = 0.2
FADING_SECONDS = 1.3
HORN_SIDE = -0.15


def horn_call(canvas: Canvas, beat: float, randomness: Generator) -> None:
    for note in theme.laid_out(theme.bars_of(theme.THEME, 1, REVEAL_BARS)):
        call = brass.note(frequency(note.name), note.length * beat, HORN_TOUCH, SOLO_HORN, randomness)
        canvas.place(call, note.beat * beat, level=HORN_LEVEL, pan=HORN_SIDE)


def string_bed(canvas: Canvas, seconds: float, beat: float, randomness: Generator) -> None:
    """The theme's first two chords, then home to D minor for as long as the reveal lasts."""
    chords = theme.laid_out(theme.bars_of(theme.THEME_HARMONY, 1, REVEAL_BARS))
    closing_beat = chords[-1].beat + chords[-1].length
    closing = theme.Event(CLOSING_CHORD, closing_beat, max(seconds / beat - closing_beat, 1.0))
    for chord in chords + (closing,):
        pitches = [frequency(name) for name in VOICINGS[chord.name]]
        bed = strings.chord(pitches, chord.length * beat, strings.SLOW_BOW, strings.VIOLINS, randomness)
        canvas.place(bed, chord.beat * beat, level=STRING_LEVEL)


def reveal_swell(seconds: float, beat: float) -> np.ndarray:
    """Swelling in with the view, and gone just before the cut back to the engine hole."""
    canvas = Canvas(0.0, seconds)
    randomness = noise.generator("reveal swell")
    horn_call(canvas, beat, randomness)
    string_bed(canvas, seconds, beat, randomness)
    heard = canvas.mixdown()[:, :canvas.length - samples_in(GONE_BEFORE_END)]
    return envelopes.fade_edges(heard, 0.0, FADING_SECONDS)
