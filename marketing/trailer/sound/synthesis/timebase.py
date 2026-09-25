"""The soundtrack's clock: the sample rate and the conversions between seconds and samples."""
import numpy as np

SAMPLE_RATE = 48000
NYQUIST = SAMPLE_RATE / 2


def samples_in(seconds: float) -> int:
    return int(round(seconds * SAMPLE_RATE))


def time_axis(length: int) -> np.ndarray:
    return np.arange(length) / SAMPLE_RATE

