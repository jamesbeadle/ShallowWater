"""The harmony of the score: which pitch classes each chord holds and where its bass notes fall."""
from ..synthesis.pitches import frequency, in_octave, transposed

TONES = {
    "Dm": ("D", "F", "A"),
    "Gm": ("G", "Bb", "D"),
    "A7": ("A", "C#", "E", "G"),
    "A": ("A", "C#", "E"),
    "Bb": ("Bb", "D", "F"),
}
FIFTH_BELOW = -5


def root(chord: str, octave: int) -> float:
    return frequency(in_octave(TONES[chord][0], octave))


def fifth_below(chord: str, octave: int) -> float:
    """The chord's fifth, taken below its root: the 'pah' a stride left hand drops to."""
    return transposed(root(chord, octave), FIFTH_BELOW)

