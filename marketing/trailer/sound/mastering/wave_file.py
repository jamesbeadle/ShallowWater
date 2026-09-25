"""The soundtrack on disk: 48 kHz, stereo, 24-bit PCM."""
import wave
from pathlib import Path

import numpy as np

from ..synthesis.timebase import SAMPLE_RATE

CHANNELS = 2
BYTES_PER_SAMPLE = 3
BYTES_PER_WORD = 4
FULL_SCALE = 2 ** 23 - 1


def pcm_frames(stereo: np.ndarray) -> bytes:
    """Interleaved little-endian 24-bit samples: the low three bytes of each 32-bit integer."""
    integers = np.round(np.clip(stereo, -1.0, 1.0) * FULL_SCALE).astype("<i4")
    interleaved = np.ascontiguousarray(integers.T).reshape(-1)
    return interleaved.view(np.uint8).reshape(-1, BYTES_PER_WORD)[:, :BYTES_PER_SAMPLE].tobytes()


def write(path: Path, stereo: np.ndarray) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    with wave.open(str(path), "wb") as output:
        output.setnchannels(CHANNELS)
        output.setsampwidth(BYTES_PER_SAMPLE)
        output.setframerate(SAMPLE_RATE)
        output.writeframes(pcm_frames(stereo))
