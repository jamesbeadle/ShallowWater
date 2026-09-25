"""Where the soundtrack reads the edit from and where it writes the render."""
from pathlib import Path

TRAILER_DIRECTORY = Path(__file__).resolve().parents[1]
EDIT_FILE = TRAILER_DIRECTORY / "edit.json"
SOUNDTRACK_FILE = TRAILER_DIRECTORY / "output" / "soundtrack.wav"
