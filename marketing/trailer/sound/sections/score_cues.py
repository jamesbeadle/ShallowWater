"""The score's cues: the reveal, the stings, the braam, the hits, the riser and the title, each placed where the edit puts it."""
from ..cinematic.braam import braam
from ..cinematic.impacts import hit
from ..cinematic.riser import riser
from ..cinematic.title import REVERSE_SECONDS, title_impact
from ..music.reveal import reveal_swell
from ..music.stings import SWELL_SECONDS, client_sting
from ..space.rooms import Rooms
from ..timeline.names import Cues
from .cue_placement import Placement

SCORE = {
    Cues.reveal_swell: Placement(lambda cue: reveal_swell(cue.length, cue.beat), -6.0, sends=((Rooms.hall, -4.0),)),
    Cues.client_sting: Placement(lambda cue: client_sting(cue.ordinal), -9.0, lead=SWELL_SECONDS, sends=((Rooms.cathedral, -7.0),)),
    Cues.braam: Placement(lambda cue: braam(), -3.0, sends=((Rooms.cathedral, -8.0),)),
    Cues.hit: Placement(lambda cue: hit(f"hit {cue.ordinal}"), -4.0, sends=((Rooms.cathedral, -9.0),)),
    Cues.riser: Placement(lambda cue: riser(cue.length), 5.0, sends=((Rooms.hall, -12.0),), ducked_by=(Cues.exhale,)),
    Cues.title_hit: Placement(lambda cue: title_impact(), -6.0, lead=REVERSE_SECONDS, sends=((Rooms.cathedral, -6.0),)),
}
