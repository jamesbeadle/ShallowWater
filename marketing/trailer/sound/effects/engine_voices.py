"""The engine heard from each place the trailer stands: in the engine hole, on the bank, up the hill, in the drop."""
from dataclasses import replace

from .engine import EngineVoice


class EngineVoices:
    """The engine's character in each section."""

    catching = EngineVoice(strike_pitch=120.0, body=0.22, knock=-9.0, chuff=-4.0, ring=-12.0, chuff_length=0.045, drive=2.2, seconds=1.2)
    on_the_bank = EngineVoice(body=0.14, knock=-16.0, chuff=-9.0, ring=-22.0, brightness=1400.0, drive=1.3)
    up_the_hill = EngineVoice(body=0.12, knock=-18.0, chuff=-5.0, ring=-22.0, brightness=900.0, drive=1.6)
    in_the_drop = EngineVoice(strike_pitch=170.0, settle_pitch=52.0, sweep=0.022, body=0.12, knock=-3.0, chuff=-7.0, ring=-20.0, drive=3.0)
    in_the_water = EngineVoice(strike_pitch=110.0, settle_pitch=46.0, sweep=0.04, body=0.3, knock=-9.0, chuff=-4.0, drive=2.2, seconds=1.4)
    alone = replace(catching, drive=1.8, seconds=1.4)
