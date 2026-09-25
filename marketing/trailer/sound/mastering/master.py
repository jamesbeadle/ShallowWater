"""The master: the soundtrack brought to its loudness, held under its ceiling, and let go into silence at the very end."""
import numpy as np

from ..synthesis import envelopes, filters
from ..synthesis.shaping import decibels, level_of
from ..synthesis.timebase import SAMPLE_RATE
from .limiter import limited, true_peaks
from .loudness import integrated_loudness

TARGET_LOUDNESS = -16.0
CEILING = -1.3
PASSES = 3
SUBSONIC = 24.0
LAST_BREATH = 0.8


def finished(track: np.ndarray) -> np.ndarray:
    """Gain found by measuring, limiting and measuring again, since limiting takes a little loudness away."""
    track = filters.highpass(track, SUBSONIC)
    gain = TARGET_LOUDNESS - integrated_loudness(track)
    for _ in range(PASSES):
        gain += TARGET_LOUDNESS - integrated_loudness(limited(track * decibels(gain), decibels(CEILING)))
    mastered = limited(track * decibels(gain), decibels(CEILING))
    return envelopes.fade_edges(mastered, 0.0, LAST_BREATH)


def described(stereo: np.ndarray) -> str:
    """How long, how loud and how high the peaks: the three figures a master is judged by."""
    peak = level_of(np.max(true_peaks(stereo)))
    return f"{stereo.shape[-1] / SAMPLE_RATE:.3f} s, {integrated_loudness(stereo):.2f} LUFS, true peak {peak:.2f} dBTP"
