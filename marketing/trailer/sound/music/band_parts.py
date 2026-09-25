"""The dance band's players: clarinet lead, stride piano, string bass, brushes and saxes."""
from ..instruments import brushes, clarinet, piano, reeds, upright_bass
from ..synthesis.pitches import frequency
from . import harmony

OFFBEAT = 0.5
SWING = 0.07
LEGATO = 0.9
STRONG_BEATS = 2
PIANO_CHORDS = {"Dm": ("F3", "A3", "D4"), "Gm": ("G3", "Bb3", "D4"), "A7": ("G3", "C#4", "E4")}
REED_CHORDS = {"Dm": ("F3", "A3", "D4"), "Gm": ("D3", "G3", "Bb3"), "A7": ("E3", "G3", "C#4")}
BASS_OCTAVE = 2
STRIDE_SECONDS = 0.42
CHORD_SECONDS = 0.28


class Balance:
    """How loud each player sits in the band, in decibels."""

    clarinet = 0.0
    bass = -4.0
    left_hand = -8.0
    right_hand = -10.0
    brush_tap = -15.0
    brush_sweep = -25.0
    saxes = -16.0


class Touch:
    """How hard each player plays, from nought to one."""

    clarinet = 0.9
    bass = 0.9
    left_hand = 0.7
    right_hand = 0.55
    accent = 1.0
    ghost = 0.35


def swung(beat: float) -> float:
    """Quavers played long-short, as a 1938 dance band would: the off-beat quaver arrives late."""
    is_offbeat = beat % 1.0 == OFFBEAT
    return beat + SWING * is_offbeat


def melody(band, notes, beat: float, randomness) -> None:
    for note in notes:
        sound = clarinet.note(frequency(note.name), note.length * beat * LEGATO, Touch.clarinet, randomness)
        band.place(sound, swung(note.beat) * beat, level=Balance.clarinet)


def beats_under(chords) -> list:
    """Every beat under the chords: the chord, how far into it, and whether the beat is one or three."""
    return [(chord, offset, (chord.beat + offset) % STRONG_BEATS == 0) for chord in chords for offset in range(int(chord.length))]


def oom(band, chord, offset: int, moment: float, randomness) -> None:
    """One and three: string bass and the piano's left hand together, the root and then the fifth below."""
    is_first = offset < STRONG_BEATS
    pitch = harmony.root(chord.name, BASS_OCTAVE) if is_first else harmony.fifth_below(chord.name, BASS_OCTAVE)
    band.place(upright_bass.pluck(pitch, STRIDE_SECONDS, Touch.bass, randomness), moment, level=Balance.bass)
    band.place(piano.note(pitch, STRIDE_SECONDS, Touch.left_hand, piano.PUB_UPRIGHT, randomness), moment, level=Balance.left_hand)
    band.place(brushes.tap(Touch.ghost, randomness), moment, level=Balance.brush_tap)


def pah(band, chord, offset: int, moment: float, randomness) -> None:
    """Two and four: the piano's right hand on the chord, with the brush landing on it."""
    names = PIANO_CHORDS[chord.name]
    right_hand = sum(piano.note(frequency(name), CHORD_SECONDS, Touch.right_hand, piano.PUB_UPRIGHT, randomness) for name in names)
    band.place(right_hand, moment, level=Balance.right_hand)
    band.place(brushes.tap(Touch.accent, randomness), moment, level=Balance.brush_tap)


def rhythm_section(band, chords, beat: float, randomness) -> None:
    for chord, offset, is_strong in beats_under(chords):
        play = oom if is_strong else pah
        play(band, chord, offset, (chord.beat + offset) * beat, randomness)


def brush_sweeps(band, bars: int, beat: float, randomness) -> None:
    for half_bar in range(bars * 2):
        band.place(brushes.sweep(beat * STRONG_BEATS, randomness), half_bar * STRONG_BEATS * beat, level=Balance.brush_sweep)


def saxes(band, chords, beat: float, randomness) -> None:
    for chord in chords:
        for name in REED_CHORDS[chord.name]:
            band.place(reeds.held_note(frequency(name), chord.length * beat, randomness), chord.beat * beat, level=Balance.saxes)
