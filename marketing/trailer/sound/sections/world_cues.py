"""The world's cues: what each diegetic sound in the edit is, how loud, where it sits and which space it is heard in."""
from ..effects import flywheel, rifle
from ..effects.blowlamp import blowlamp
from ..effects.breath import exhale, running_breath
from ..effects.chain import chain_snap
from ..effects.dawn import dawn
from ..effects.footsteps import footfall
from ..effects.lorries import lorries
from ..effects.rain import rain
from ..effects.splash import splash
from ..effects.town import town_wakes
from ..effects.valley import valley_echo
from ..effects.whistle import police_whistle
from ..effects.wind import hilltop_wind
from ..music.dance_band import on_the_wireless
from ..space.rooms import Rooms
from ..timeline.names import Cues
from .cue_placement import Placement


WORLD = {
    Cues.blowlamp: Placement(lambda cue: blowlamp(cue.length, cue.starts_on_cut, cue.ends_on_cut), 1.0),
    Cues.dawn_ambience: Placement(lambda cue: dawn(cue.length), -12.0),
    Cues.footsteps: Placement(lambda cue: footfall(cue.ordinal), -4.0, sends=((Rooms.wood, -14.0),)),
    Cues.breathing: Placement(lambda cue: running_breath(cue.length), -11.0, sends=((Rooms.wood, -16.0),)),
    Cues.hilltop_wind: Placement(lambda cue: hilltop_wind(cue.length), 3.0),
    Cues.flywheel_kick: Placement(lambda cue: flywheel.kick_and_turn(), -6.0, sends=((Rooms.engine_room, -4.0),)),
    Cues.engine_cough: Placement(lambda cue: flywheel.cough(), -9.0, sends=((Rooms.engine_room, -4.0),)),
    Cues.wireless: Placement(lambda cue: on_the_wireless(cue.length, cue.beat), -19.0, 0.3, sends=((Rooms.cabin, -3.0),)),
    Cues.distant_rifle: Placement(lambda cue: rifle.distant_shot(cue.ordinal), 2.0, -0.35),
    Cues.lorry_engines: Placement(lambda cue: lorries(cue.length), 2.0, sends=((Rooms.valley, -12.0),)),
    Cues.rain: Placement(lambda cue: rain(cue.length), -7.0),
    Cues.exhale: Placement(lambda cue: exhale(cue.length), 3.0),
    Cues.rifle_shot: Placement(lambda cue: rifle.close_shot(), 12.0),
    Cues.valley_echo: Placement(lambda cue: valley_echo(cue.length), 4.0, sends=((Rooms.valley, -16.0),)),
    Cues.town_wakes: Placement(lambda cue: town_wakes(cue.length), -9.0, sends=((Rooms.valley, -3.0),)),
    Cues.police_whistle: Placement(lambda cue: police_whistle(), -15.0, 0.45, sends=((Rooms.valley, -9.0),)),
    Cues.chain_snap: Placement(lambda cue: chain_snap(), -4.0, sends=((Rooms.hall, -10.0),)),
    Cues.splash: Placement(lambda cue: splash(), -6.0, sends=((Rooms.hall, -10.0),)),
}
