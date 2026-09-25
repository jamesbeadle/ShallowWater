"""A pressurised paraffin blowlamp roaring close to the microphone, stopped dead by a cut or put out as the engine turns."""
import numpy as np
from numpy.random import Generator

from ..synthesis import envelopes, filters, noise
from ..synthesis.shaping import normalised
from ..synthesis.timebase import samples_in, time_axis

ROAR_RESONANCES = ((230.0, 1.6), (340.0, 2.5), (410.0, 4.0))
HISS_FLOOR = 1800.0
HISS_LEVEL = 0.22
RUMBLE_CUTOFF = 120.0
RUMBLE_LEVEL = 0.35
CEILING = 3600.0
FLUTTER_RANGE = (0.2, 2.0)
DYING_SPEED = 4.0
FLUTTER_RATE = 5.0
FLUTTER_DEPTH = 0.22
TURBULENCE_RATE = 23.0
TURBULENCE_DEPTH = 0.06
LIGHTING_SECONDS = 0.8
CUT_SECONDS = 0.006
DYING_SECONDS = 0.3
SHARED_SHARE = 0.8


def flame(length: int, randomness: Generator) -> np.ndarray:
    source = noise.white(length, randomness)
    roar = sum(filters.resonance(source, frequency, quality) for frequency, quality in ROAR_RESONANCES)
    hiss = HISS_LEVEL * filters.highpass(source, HISS_FLOOR)
    rumble = RUMBLE_LEVEL * normalised(filters.lowpass(noise.brown(length, randomness), RUMBLE_CUTOFF))
    return filters.lowpass(normalised(roar) + hiss + rumble, CEILING, order=filters.STEEP)


def flutter(length: int, randomness: Generator) -> np.ndarray:
    slow = FLUTTER_DEPTH * noise.wander(length, FLUTTER_RATE, randomness)
    quick = TURBULENCE_DEPTH * noise.wander(length, TURBULENCE_RATE, randomness)
    return np.clip(1.0 + slow + quick, *FLUTTER_RANGE)


def life(length: int, starts_on_cut: bool, ends_on_cut: bool) -> np.ndarray:
    """Roaring from the first frame of a cut or lit over a moment; stopped dead by a cut or dying as the valve closes."""
    moments = time_axis(length)
    lighting = envelopes.rising(samples_in(CUT_SECONDS if starts_on_cut else LIGHTING_SECONDS))
    opening = np.pad(lighting, (0, length - len(lighting)), constant_values=1.0)
    remaining = moments[-1] - moments
    dying = 1.0 - np.exp(-remaining * DYING_SPEED / (CUT_SECONDS if ends_on_cut else DYING_SECONDS))
    return opening * dying


def blowlamp(seconds: float, starts_on_cut: bool, ends_on_cut: bool) -> np.ndarray:
    """A stereo roar: mostly one flame heard by both ears, a little of each side's own turbulence."""
    length = samples_in(seconds)
    randomness = noise.generator("blowlamp")
    shared = flame(length, randomness)
    sides = [SHARED_SHARE * shared + (1.0 - SHARED_SHARE) * flame(length, randomness) for _ in range(2)]
    return np.stack(sides) * flutter(length, randomness) * life(length, starts_on_cut, ends_on_cut)
