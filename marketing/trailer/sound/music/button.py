"""The button: the picture gone quiet, the engine still beating, and the wireless whispering its crackle."""
from ..effects.crackle import crackle
from ..synthesis import envelopes

CRACKLE_LEVEL = -40.0
FADE_SECONDS = 1.0
WIRELESS_SIDE = 0.3


def score(canvas, edit, section) -> None:
    whisper = envelopes.fade_edges(crackle(section.length, "button crackle"), FADE_SECONDS, FADE_SECONDS)
    canvas.place(whisper, section.start, level=CRACKLE_LEVEL, pan=WIRELESS_SIDE)
