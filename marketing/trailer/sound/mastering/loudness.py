"""Loudness as ITU-R BS.1770 measures it: K-weighted, gated, and integrated over the programme, in LUFS."""
import numpy as np
from scipy import signal

from ..synthesis.timebase import samples_in

SHELF = ((1.53512485958697, -2.69169618940638, 1.19839281085285), (1.0, -1.69065929318241, 0.73248077421585))
HIGH_PASS = ((1.0, -2.0, 1.0), (1.0, -1.99004745483398, 0.99007225036621))
BLOCK_SECONDS = 0.4
STEP_SECONDS = 0.1
ABSOLUTE_GATE = -70.0
RELATIVE_GATE = -10.0
LOUDNESS_OFFSET = -0.691
SMALLEST_POWER = 1e-20
DECIBELS_PER_POWER_DECADE = 10.0


def k_weighted(stereo: np.ndarray) -> np.ndarray:
    """The programme as the ear weighs it: a lift above 2 kHz and the lowest bass let go (coefficients for 48 kHz)."""
    shelved = signal.lfilter(*SHELF, stereo, axis=-1)
    return signal.lfilter(*HIGH_PASS, shelved, axis=-1)


def block_powers(stereo: np.ndarray, block_seconds: float = BLOCK_SECONDS) -> np.ndarray:
    """Mean-square power in overlapping blocks, summed over both channels."""
    squared = k_weighted(stereo) ** 2
    block = samples_in(block_seconds)
    running = np.concatenate([np.zeros((squared.shape[0], 1)), np.cumsum(squared, axis=-1)], axis=-1)
    starts = np.arange(0, squared.shape[-1] - block + 1, samples_in(STEP_SECONDS))
    means = (running[:, starts + block] - running[:, starts]) / block
    return means.sum(axis=0)


def loudness_of(power) -> np.ndarray:
    return LOUDNESS_OFFSET + DECIBELS_PER_POWER_DECADE * np.log10(np.maximum(power, SMALLEST_POWER))


def integrated_loudness(stereo: np.ndarray) -> float:
    """Integrated loudness: silence gated out absolutely, then everything more than ten units below the rest."""
    powers = block_powers(stereo)
    audible = powers[loudness_of(powers) > ABSOLUTE_GATE]
    threshold = loudness_of(np.mean(audible)) + RELATIVE_GATE
    counted = audible[loudness_of(audible) > threshold]
    return float(loudness_of(np.mean(counted)))
