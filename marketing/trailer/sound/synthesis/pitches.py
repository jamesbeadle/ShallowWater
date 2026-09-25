"""Pitch names to frequencies, in equal temperament from A4 at 440 Hz."""
import re

CONCERT_A = 440.0
CONCERT_A_NUMBER = 69
SEMITONES_PER_OCTAVE = 12
CENTS_PER_SEMITONE = 100
LETTERS = {"C": 0, "D": 2, "E": 4, "F": 5, "G": 7, "A": 9, "B": 11}
ACCIDENTALS = {"#": 1, "b": -1, "": 0}
NOTE_NAME = re.compile(r"([A-G])([#b]?)(-?\d)")


def note_number(name: str) -> int:
    letter, accidental, octave = NOTE_NAME.fullmatch(name).groups()
    return (int(octave) + 1) * SEMITONES_PER_OCTAVE + LETTERS[letter] + ACCIDENTALS[accidental]


def frequency(name: str) -> float:
    return transposed(CONCERT_A, note_number(name) - CONCERT_A_NUMBER)


def transposed(pitch, semitones):
    return pitch * 2.0 ** (semitones / SEMITONES_PER_OCTAVE)


def detuned(pitch, cents):
    return transposed(pitch, cents / CENTS_PER_SEMITONE)


def in_octave(pitch_class: str, octave: int) -> str:
    """A pitch class placed in an octave: Bb and 2 make Bb2."""
    return f"{pitch_class}{octave}"
