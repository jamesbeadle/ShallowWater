"""The finale: the chain parts, the orchestra takes the theme from the wireless, and the title lands on D."""
from ..synthesis import noise
from ..synthesis.pitches import frequency
from ..timeline.names import Cues
from . import dynamics, orchestra, theme

THEME_BARS = (5, 7)
DOWNBEAT = "Dm"
TITLE_CHORD = "Dm9"
LANDING = "D5"
DOWNBEAT_TOUCH = 1.0
THEME_TOUCH = 0.65
TITLE_TOUCH = 1.0
ROLL_BEATS = 2.0
TITLE_ROLL_BEATS = 3.0
HELD_TO_END = 0.95
TITLE_FALLBACK_BARS = 3


def score(canvas, edit, section) -> None:
    """The downbeat on the snapping chain, the theme's last phrase, and the title chord ringing away to the end."""
    randomness = noise.generator("finale")
    tempo = edit.tempo
    title = edit.moment_of(Cues.title_hit, section.end - tempo.bars(TITLE_FALLBACK_BARS))
    snap = edit.moment_of(Cues.chain_snap, section.start)
    first_bar, last_bar = THEME_BARS
    theme_start = title - tempo.bars(last_bar - first_bar + 1)
    orchestra.strike(canvas, DOWNBEAT, snap, randomness)
    orchestra.chord(canvas, DOWNBEAT, snap, theme_start - snap, DOWNBEAT_TOUCH, randomness, dynamics.sforzando)
    theme_bars(canvas, theme_start, tempo, randomness)
    orchestra.timpani_into(canvas, TITLE_CHORD, title, tempo.beats(TITLE_ROLL_BEATS), randomness)
    held = (section.end - title) * HELD_TO_END
    orchestra.chord(canvas, TITLE_CHORD, title, held, TITLE_TOUCH, randomness, dynamics.ringing_away)
    orchestra.melody_note(canvas, frequency(LANDING), title, held, TITLE_TOUCH, randomness, dynamics.ringing_away)


def theme_bars(canvas, start: float, tempo, randomness) -> None:
    """Bars five to seven of the theme in full: the tune on top, a chord a bar under it, a roll into every bar line."""
    first_bar, last_bar = THEME_BARS
    melody = theme.laid_out(theme.bars_of(theme.THEME, first_bar, last_bar))
    chords = theme.laid_out(theme.bars_of(theme.THEME_HARMONY, first_bar, last_bar))
    for chord in chords:
        moment = start + tempo.beats(chord.beat)
        shape = dynamics.making_way if chord is chords[-1] else orchestra.steady
        orchestra.timpani_into(canvas, chord.name, moment, tempo.beats(ROLL_BEATS), randomness)
        orchestra.chord(canvas, chord.name, moment, tempo.beats(chord.length), THEME_TOUCH, randomness, shape)
    for note in melody:
        orchestra.melody_note(canvas, frequency(note.name), start + tempo.beats(note.beat), tempo.beats(note.length), THEME_TOUCH, randomness)
