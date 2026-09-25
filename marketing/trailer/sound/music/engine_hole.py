"""The engine hole: its close steel air, and a low D drone that wakes as the flywheel is kicked."""
from ..effects.room_tone import room_tone
from ..instruments import strings
from ..synthesis import noise
from ..synthesis.pitches import frequency
from ..timeline.names import Cues

DRONE = ("D1", "D2")
DRONE_SECONDS = 5.0
DRONE_BOW = strings.Bowing(attack=1.5, release=3.0, brightness=260.0, floor=25.0)


class Balance:
    """The engine hole's quiet layers, in decibels."""

    room = -30.0
    drone = -17.0


def score(canvas, edit, section) -> None:
    canvas.place(room_tone(section.length), section.start, level=Balance.room)
    waking = edit.moment_of(Cues.flywheel_kick, section.start)
    drone = strings.chord([frequency(name) for name in DRONE], DRONE_SECONDS, DRONE_BOW, strings.LOW_STRINGS, noise.generator("drone"))
    canvas.place(drone, waking, level=Balance.drone)
