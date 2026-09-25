"""The night: the engine as the kick of a driving track through D minor, B flat, G minor and A, then the held breath of the riser."""
from ..synthesis import noise
from ..timeline.names import Cues
from . import night_brass, night_drums, night_strings

PROGRESSION = ("Dm", "Dm", "Bb", "Bb", "Gm", "Gm", "A", "A")
LIFT_BAR = 6
QUAVER = 0.5


def score(canvas, edit, section) -> None:
    """Bar by bar until the riser starts, then the pedal on A holds everything up until the cut."""
    randomness = noise.generator("night drive")
    tempo = edit.tempo
    riser = edit.moment_of(Cues.riser, section.end)
    driven = [(bar, chord) for bar, chord in enumerate(PROGRESSION) if section.start + tempo.bars(bar) < riser]
    for bar, chord in driven:
        drive(canvas, bar, chord, section.start + tempo.bars(bar), tempo, bar >= LIFT_BAR, randomness)
    lift = section.start + tempo.bars(LIFT_BAR)
    night_brass.swell(canvas, PROGRESSION[LIFT_BAR], lift, max(riser, lift + tempo.bar), randomness)
    breaths = [(cue.start, cue.end) for cue in edit.cues_of(Cues.exhale)]
    night_strings.held_breath(canvas, riser, section.end, breaths, randomness)


def drive(canvas, bar: int, chord: str, bar_start: float, tempo, is_lifted: bool, randomness) -> None:
    night_drums.drums(canvas, bar, bar_start, tempo.beat, randomness)
    night_strings.ostinato(canvas, chord, bar_start, tempo.beat * QUAVER, is_lifted, randomness)
    night_strings.bass(canvas, chord, bar_start, tempo.beat * QUAVER, is_lifted, randomness)
