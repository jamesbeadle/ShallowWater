"""The foxtrot on the wireless: as many bars of the theme as the cue holds, running on into the tag, played dry."""
import numpy as np

from ..effects import wireless
from ..space.canvas import Canvas
from ..synthesis import envelopes, noise
from ..synthesis.timebase import samples_in
from ..timeline.tempo import BEATS_PER_BAR
from . import band_parts, theme

RING_OUT = 1.0
SWITCHED_ON = 0.01


def arrangement(bars: int) -> tuple:
    """The melody and chords for a performance bars long: the theme, running on into the tag if there is room."""
    melody = theme.bars_of(theme.THEME + theme.DANCE_BAND_TAG, 1, bars)
    harmony = theme.bars_of(theme.THEME_HARMONY + theme.DANCE_BAND_TAG_HARMONY, 1, bars)
    return theme.laid_out(melody), theme.laid_out(harmony)


def foxtrot(seconds: float, beat: float) -> np.ndarray:
    """The band in the studio, before the wireless gets at it."""
    bars = max(round(seconds / (beat * BEATS_PER_BAR)), 1)
    melody, chords = arrangement(bars)
    band = Canvas(0.0, seconds + RING_OUT)
    randomness = noise.generator("dance band")
    band_parts.melody(band, melody, beat, randomness)
    band_parts.rhythm_section(band, chords, beat, randomness)
    band_parts.brush_sweeps(band, bars, beat, randomness)
    band_parts.saxes(band, chords, beat, randomness)
    return band.mixdown()


def on_the_wireless(seconds: float, beat: float) -> np.ndarray:
    """The foxtrot as the cabin's wireless gives it out, turned down over its last bar."""
    heard = wireless.broadcast(foxtrot(seconds, beat))[:samples_in(seconds)]
    return envelopes.fade_edges(heard, SWITCHED_ON, beat * BEATS_PER_BAR)
