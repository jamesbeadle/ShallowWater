"""The cut at dawn: water against the hull, and warm strings out of the picture that follow the wireless's chords."""
from ..effects.water import lapping
from ..instruments import strings
from ..synthesis import noise
from ..synthesis.pitches import frequency
from ..timeline.names import Cues
from . import theme

VOICINGS = {"Dm": ("D3", "F3", "A3", "D4"), "Gm": ("D3", "G3", "Bb3", "D4"), "A7": ("C#3", "G3", "A3", "E4")}
STRINGS_LEVEL = -16.0
WATER_LEVEL = -26.0
ARRIVAL_SHARE = 0.5


def warm_strings(canvas, edit, section) -> None:
    """From the middle of the broadcast on, a chord under each of the band's chords."""
    wireless = edit.cues_within(section, Cues.wireless)
    start = wireless[0].start if wireless else section.start
    beat = edit.beat
    arrival = start + ARRIVAL_SHARE * (section.end - start)
    randomness = noise.generator("cut strings")
    bars = max(round((section.end - start) / edit.bar), 1)
    chords = theme.laid_out(theme.bars_of(theme.THEME_HARMONY + theme.DANCE_BAND_TAG_HARMONY, 1, bars))
    for chord in [chord for chord in chords if start + (chord.beat + chord.length) * beat > arrival]:
        entry = max(start + chord.beat * beat, arrival)
        pitches = [frequency(name) for name in VOICINGS[chord.name]]
        pad = strings.chord(pitches, start + (chord.beat + chord.length) * beat - entry, strings.SLOW_BOW, strings.VIOLINS, randomness)
        canvas.place(pad, entry, level=STRINGS_LEVEL)


def score(canvas, edit, section) -> None:
    canvas.place(lapping(section.length), section.start, level=WATER_LEVEL)
    warm_strings(canvas, edit, section)
