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

The lever stays where it is put: full astern, half astern, stop, dead slow, half ahead and full ahead. It sets the engine's revolutions, not her speed (`BoatHandling`). The Bolinder has no reverse gear, so going from ahead to astern the engine stops, pauses a second and a half, and runs the other way (`Engine`). The beat is one a second at tickover and quickens with the revolutions (`EngineSound`, from `BolinderBeat`).

*Sparrow* is steered in the world, not along the canal: the bends have to be steered round. She is handled as a loaded working boat of eighteen tonnes in a shallow channel, worked out from the forces on her ahead, sideways and round (`BoatMotion`):

- **The screw** (`Propeller`) pushes hardest from rest and less as she gathers way; astern it gives little over half the push. Settled speeds are about 1.1, 1.7 and 2.2 metres a second at dead slow, half and full ahead; astern she makes about 0.8 and 1.5.
- **The canal holds her back** (`HullDrag`): drag climbs steeply as she nears the speed the shallow, narrow channel allows, so opening up past half ahead buys little. Stern first she is bluff and slow. Knocked out of gear at dead slow she carries her way for some fifty metres; full astern stops her inside half a length from dead slow and in about a length from full ahead.
- **The rudder only bites on water running aft past it** (`RudderForce`). That water is her own way ahead and the wash thrown back by the screw going ahead. Going astern the screw pulls water forward off the blade and the hull drags it the wrong way, so **there is no steering astern at all**. Lying still with the engine stopped, the tiller does nothing; a kick ahead with the tiller over swings her stern before she has moved. Putting her astern while she still has way on kills the steering too.
- **Prop walk.** The right-handed screw walks her stern to port when it goes astern, hardest as it first bites and fading as she gathers sternway, so going astern her head falls off to starboard. To hold her straight astern, give her short kicks ahead with the tiller over. Ahead it leans her stern very slightly to starboard.
- **Turning.** Hard over, she turns in a circle about thirty metres across, pivoting about a point some four metres ahead of her middle, with the stern sweeping out. She sheds speed through a turn as the hull slides sideways and the blade drags (`HullCrossFlow`).
- **The banks.** Near a bank at speed, the water squeezed between hull and bank pulls her stern in and pushes her bow off (`BankSuction`). Run close along the bank and her head sheers out towards the middle. `Banks` stops her at the banks, the stop planks and the lock gates, pushing on the hull where it touches: she glances off, turns parallel and slides along.

## Ashore

When she lies within a metre and a half of either bank and is barely moving, E steps Askew ashore beside the stern (`Landing`); the lever goes to stop and she lies where she is. Near the stern again, E steps him back into the hatch at the tiller.

At the tiller he stands in the back cabin hatchway with the slide pushed open, the roof at his hips, his right hand on the tiller and his left on the slide (`Hatchway`, `TillerHold`).

## Tying up

Ashore, within a line's length (nine metres) of the stern, T ties her up (`TyingUp`). He goes to the nearest bollard if one is within two and a half metres, or knocks a mooring pin into the bank in front of him (`MooringPosts`, `MooringPin`), kneels and works the line on (`TyingPose`). The stern line runs from the dolly on the counter nearer him, with a sag (`SternLine`, `Dollies`). Once he starts she lies still against the line, the engine running down as it would. T at the post casts her off the same way. He cannot step aboard while she is made fast: the hint says to cast off first.

| Control | Keyboard | Gamepad |
| --- | --- | --- |
| Walk | W A S D or arrows, relative to the camera | left stick |
| Run | Shift | left stick press or right trigger |
| Look round | right mouse drag, scroll to zoom | right stick |
| Step aboard | E | south button |
| Tie up, cast off | T | north button |

`Land` knows where he can go: the canal, its arms and the Tame are water (`WaterLines`), houses and tree trunks are in the way (`Obstacles`), and the banks, towpath and woodland floor are at their own heights. `Walker` takes him up to a walk or a run and turns him to face his way, and `Gait` swings his thighs, shins, arms and forearms for the stride. He is built from shapes like everything else (`AskewShapes`).
