# How Sparrow handles and how Askew walks

The game side lives in plain C# (`Assets/Game`); the Unity layer only reads the controls and puts things where the game says they are.

## At the tiller

| Control | Keyboard | Gamepad |
| --- | --- | --- |
| Lever one notch ahead | W or ↑ | right shoulder |
| Lever one notch astern | S or ↓ | left shoulder |
| Stop | Space | west button |
| Tiller | A / D or ← / → | left stick |
| Look round | right mouse drag, scroll to zoom | right stick |
| Step ashore | E | south button |

The lever stays where it is put: full astern, half astern, stop, dead slow, half ahead and full ahead, settling at about 1, 1.9 and 3 metres a second ahead. The Bolinder has no reverse gear, so going from ahead to astern the engine stops, pauses a second and a half, and runs the other way (`Engine`). The beat is one a second at tickover and quickens as she opens up (`EngineSound`, from `BolinderBeat`).

*Sparrow* is steered in the world, not along the canal: the bends have to be steered round. `BoatMotion` carries her way through the water, lets her side-slip into a turn and swing about a point a third of her length back from the bow, and takes the rudder's bite from her speed and the propeller's wash, so a kick of the engine turns a stopped boat. `Banks` stops her at the banks, the stop planks and the lock gates, pushing on the hull where it touches: she glances off, turns parallel and slides along.

## Ashore

When she lies within a metre and a half of either bank and is barely moving, E steps Askew ashore beside the stern (`Landing`); the lever goes to stop and she lies where she is. Near the stern again, E steps him back into the hatch at the tiller.

| Control | Keyboard | Gamepad |
| --- | --- | --- |
| Walk | W A S D or arrows, relative to the camera | left stick |
| Run | Shift | left stick press or right trigger |
| Look round | right mouse drag, scroll to zoom | right stick |
| Step aboard | E | south button |

`Land` knows where he can go: the canal, its arms and the Tame are water (`WaterLines`), houses and tree trunks are in the way (`Obstacles`), and the banks, towpath and woodland floor are at their own heights. `Walker` takes him up to a walk or a run and turns him to face his way, and `Gait` swings his thighs, shins, arms and forearms for the stride. He is built from shapes like everything else (`AskewShapes`).
