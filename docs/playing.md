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
| Show or hide the controls | H, or click the **H Controls** hint at the top left | select (view) button |
| Open or put away the pocket book | Tab or I, or click the **Tab Pocket book** hint at the top left; Esc puts it away | east button |

The lever stays where it is put: full astern, half astern, stop, dead slow, half ahead and full ahead. It sets the engine's revolutions, not her speed (`BoatHandling`). The Bolinder has no reverse gear, so going from ahead to astern the engine stops, pauses a second and a half, and runs the other way (`Engine`). The beat is one a second at tickover and quickens with the revolutions (`EngineSound`, from `BolinderBeat`).

*Sparrow* is steered in the world, not along the canal: the bends have to be steered round. She is handled as a loaded working boat of eighteen tonnes in a shallow channel, worked out from the forces on her ahead, sideways and round (`BoatMotion`):

- **The screw** (`Propeller`) pushes hardest from rest and less as she gathers way; astern it gives little over half the push. Settled speeds are about 2.2, 3.4 and 4.4 metres a second at dead slow, half and full ahead; astern she makes about 1.6 and 3.
- **The canal holds her back** (`HullDrag`): drag climbs steeply as she nears the speed the shallow, narrow channel allows, so opening up past half ahead buys little. Stern first she is bluff and slow. Knocked out of gear at dead slow she carries her way for over a hundred metres; full astern stops her inside a length from dead slow and in about two lengths from full ahead.
- **The rudder only bites on water running aft past it** (`RudderForce`). That water is her own way ahead and the wash thrown back by the screw going ahead. Going astern the screw pulls water forward off the blade and the hull drags it the wrong way, so **there is no steering astern at all**. Lying still with the engine stopped, the tiller does nothing; a kick ahead with the tiller over swings her stern before she has moved. Putting her astern while she still has way on kills the steering too.
- **Prop walk.** The right-handed screw walks her stern to port when it goes astern, hardest as it first bites and fading as she gathers sternway, so going astern her head falls off to starboard. To hold her straight astern, give her short kicks ahead with the tiller over. Ahead it leans her stern very slightly to starboard.
- **Turning.** Hard over, she turns in a circle about forty metres across, pivoting about a point some four metres ahead of her middle, with the stern sweeping out. She sheds speed through a turn as the hull slides sideways and the blade drags (`HullCrossFlow`).
- **The banks.** Near a bank at speed, the water squeezed between hull and bank pulls her stern in and pushes her bow off (`BankSuction`). Run close along the bank and her head sheers out towards the middle. `Banks` stops her at the banks, the stop planks and the lock gates, pushing on the hull where it touches: she glances off, turns parallel and slides along.

## Ashore

When she lies within a metre and a half of either bank and is barely moving, E steps Askew ashore beside the stern (`Landing`); the lever goes to stop and she lies where she is. Near the stern again, E steps him back into the hatch at the tiller.

At the tiller he stands on the step in the back cabin hatchway, with the back doors pinned open behind him and the slide pushed open, the roof at his hips, his right hand on the tiller and his left on the slide (`Hatchway`, `BackDoors`, `TillerHold`). The tiller swings with the rudder, the opposite way to the turn, and he pushes it across: stepping, leaning and reaching after the handle, with a straight arm when it is hard over away from him.

## Tying up

*Sparrow* carries a centre line: made fast to the T-stud on the top planks amidships and laid aft along the planks and the cabin roof, with the tail coiled at the back of the roof by the hatch (`CentreStud`, `CentreLineDrape`). When Askew steps ashore he takes it with him, and from then until he steps back aboard it runs from the stud, over the side cloth, to his hand (`Mooring`, `CentreLine`, `LineInHand`): he holds it in the hand nearer the boat, hanging back at his hip, and the slack sags into the cut. With the line in hand he walks her along the bank: she follows him once the line comes taut, eleven metres from the stud, and the line holds him back from walking further (`LinePull`, `MooringPull`). She is eighteen tonnes, so she gathers way over ten seconds or so and carries it after he stops.

T ties the centre line off where he stands (`LineWork`): to the nearest bollard if one is within two and a half metres, or to a mooring pin he knocks into the bank in front of him (`MooringPosts`, `MooringPin`), kneeling to work the line on (`TyingPose`). She now lies on the centre line, and can still swing on it. Within a line's length (nine metres) of the bow T-stud, T ties the bow line to a post the same way; within nine metres of the stern dolly, T ties the stern line (`BowLine`, `SternLine`). With all three lines on she is made fast and lies still, the engine running down as it would.

Casting off is the same in reverse: R at a post lets go the bow or stern line there, and once both ends are loose R at the centre post casts her off, which puts the centre line back in his hand. He steps aboard with it, and it lies draped along the roof again. He cannot step aboard while the centre line is on its post: the hint says to let go the lines first. To shoulder the rifle with the line in his hand he lays it down at his feet, and he takes it up again when he lowers the rifle; she lies where she is meanwhile, and he cannot step aboard, tie up or let go until the rifle is down.

| Control | Keyboard | Gamepad |
| --- | --- | --- |
| Walk | W A S D or arrows, relative to the camera | left stick |
| Run | Shift | left stick press or right trigger |
| Look round | right mouse drag, scroll to zoom | right stick |
| Step aboard | E | south button |
| Tie her up, then the bow and stern lines | T | north button |
| Let go a line, at its post | R | west button |
| Talk, by the farmer | E | south button |
| Shoulder the rifle, ashore | hold the right mouse button | hold the left trigger |
| Fire, shouldered | Space or the left mouse button | right trigger |
| Show or hide the controls | H, or click the **H Controls** hint at the top left | select (view) button |
| Open or put away the pocket book | Tab or I, or click the **Tab Pocket book** hint at the top left; Esc puts it away | east button |

## The rifle and the paddock

Askew carries his own rook rifle (`RookRifle`, `RifleInHand`): at the trail whenever he is ashore, stowed below when he steps aboard. Holding the right mouse button shoulders it (`Marksman`, `AimingPose`): he turns to face where the camera was looking, the line goes down at his feet if it was in his hand, and the view goes down the sights, from his right eye on the stock along the barrel to the foresight under the ring in the middle of the screen (`CameraFraming.Sighting`, `Looking.DownTheSights`). The rifle is placed from that eye, so the barrel points where the ring is at every pitch, and the mouse lays the aim at a third of its usual rate, up to sixty degrees up or down. Space or the left button fires one shot before the bolt is worked. A shot flies as far as the first thing in its way (`SightLine`, `Shot`): a hedge, a wall, a gatepost or the ground stops it short, so a bird behind a hedge is safe until he finds the gateway. Every shot startles the birds within a couple of hundred yards (`Flock`, `Roaming`): they lift off their perches, calling, and settle somewhere else, so he waits for them to come down before the next.

South of Hopwas bridge, on the towpath side, a hedged paddock lies between the canal and the lane (`Paddock`, `PaddockShapes`): pasture, a stone trough, three oaks in the hedge and an open gate towards the water, with eight crows and two rooks (`PaddockStock`) feeding on the grass and sitting on the gate, the trough and the oaks. The birds beat their wings in flight and fold them when they land (`Wings`, `WingPose`), and call now and then (`Calling`, `CrowCall`). A bird that is hit falls (`Carcass`): it is given weight and a body and tumbles onto whatever is below it, the grass, the gate rail, the towpath or the water.

Everything built from shapes is solid (`Solidity`): the fields and their hedgerows, the roads and their hedges, the river and the railway, the houses, the canal and its towpath, the woodland floor, the village turf, the farm and the paddock each carry a collider, so a falling bird lands on them and the camera is kept out of them (`CameraClearance`): when a hedge or a wall stands between Askew and where the camera wants to be, the camera comes in to his side of it.

## The farm job

Dunstall Farm stands on the west bank 1,750 metres south of Hopwas bridge (`Farmyard`): a fenced stackyard with two hayricks and a muck heap, its gate towards the water, and the farmer inside it. Within a few yards of him E starts the talk (`Talker`, `Conversation`); his words come up on a card at the bottom of the screen and E takes them a line at a time. He sees the rifle and asks for the yard to be cleared (`FarmJob`). The farm's quarry (`FarmyardStock`) sit on the posts and ricks and run between the rick feet; the card at the top counts the cull, and when the last one is down the farmer keeps his word: a pub lead goes on the card. Only the farm's own birds and rats count; the paddock's crows are Askew's to shoot as he likes. The story of the job is in `docs/story/rooks-and-rats.md`.

`Land` knows where he can go: the canal, its arms and the Tame are water (`WaterLines`); houses, tree trunks, hedges, gateposts, the trough and the hayricks are in the way (`Obstacles`, `Hedge`); and he stands at the height of what is under him (`Relief`, `Floors`): the banks, towpath and verge, the woodland floor, the village turf, the farmyard floor, the paddock and the roads with their verges. `Walker` takes him up to a walk or a run and turns him to face his way, and `Gait` swings his thighs, shins, arms and forearms for the stride. He is built from shapes like everything else (`AskewShapes`).

## The day and the radar

The clock runs a game minute to each real second, so a day passes in 24 minutes (`GameClock`), starting at half past eight in the morning. The sun follows its mid-October path over Tamworth (`SunPath`): it rises about a quarter to seven in the east-south-east, stands 28 degrees up at noon and sets about a quarter past five. Between the sun at 12 degrees and the sun 10 degrees below the horizon the sky passes through twilight (`DayStages`); once the sun is 2 degrees down, the full moon opposite it takes over the light (`Lighting`) and the stars come out.

The time of day is in the top right corner. The radar in the bottom left shows 400 metres of the 1900 map round Askew or *Sparrow*, whichever the camera follows, turned so that the way the camera looks is up; the arrow is you and the N on the rim is north.

## The HUD

Everything on the screen but the radar is drawn with UI Toolkit (`Assets/Unity/Overlay`), scaled from a 1920 by 1080 screen by its height so it holds its shape on every platform. `OverlayScreen` raises it, and each piece is a small widget the player components drive:

- **Engine** (`TelegraphCard`, `TelegraphMeter`), beside the radar while you are at the tiller: the order the lever is at, in brass ahead and signal red astern, over a meter that fills from stop towards full ahead or full astern.
- **Prompts** (`PromptColumn`, `PromptPill`), bottom right: the key and what it does, such as **E** *Step ashore*, sliding in as they become possible.
- **Controls** (`ControlsPanel`), top left: the **H** *Controls* hint opens a panel of what every key does at the tiller or ashore; H, the gamepad's select or a click on **H** *Close* puts it away.
- **Clock** (`ClockFace`), top right, an enamel plate with each figure in its own cell so the time does not shift as it changes.
- **The job** (`JobCard`), top centre, a paper label in pencil: the farm and how the cull stands, then the lead once Pryce gives it.
- **The pocket book** (`PocketBookScreen`, driven by `PocketBook`), on Tab, I or the gamepad's east button: Askew's oilcloth notebook laid open over the dimmed world, which stands still while he reads it (`WorldPause`, and `Attention` keeps every key from the world until it is put away). The left page is his kit: the gun locker (`GunLocker`, three hooks, the rook rifle on one, his own, with whether it is loaded and in hand or stowed below), the box of fifty .300 Rook cartridges with every round drawn and the spent ones left as outlines (`CartridgeBox`, from `Assets/Game/Kit`), his purse in pounds, shillings and pence (`Purse`, `OldMoney`), his pocket watch with its hands at the game's time (`PocketWatch`), and the papers he has gathered: the lead to Holland on a card (`PaperCard`). The right page is the day: the job and where it stands (`JobEntry`), *Sparrow* with her engine order and each mooring line (`BoatEntry`), and where she lies on the pound in furlongs and yards from the nearest landmark (`WaterEntry`, from `Whereabouts` and `OldDistance`). Every item has a pencil sketch (`Sketches`), and the rifle is fired from that box: when the fifty are spent it will not fire.

The materials are the period's. Cards are oilskin (a dark waxed canvas, with the weave in `Materials/Weave.png`), the key caps are round black typewriter keys with a nickel ring, the job label and the book's pages are worn paper (`Materials/Paper.png`, generated, tileable) and the book's cover is oilcloth. Headings are stencilled in Stardos Stencil, the pencil hand is Caveat, the words are Cabin and the clock's figures Josefin Sans; all four are under the SIL Open Font License, kept beside them in `Assets/Unity/Resources/Hud/Fonts`. The look lives in the stylesheets in `Assets/Unity/Resources/Hud` (`Hud.uss` holds the colours and imports one sheet per widget; `PocketBook.uss` and `Sketches.uss` are the book's). Keys are drawn as those caps and the mouse as a mouse with the button or wheel you use in brass (`KeyGlyphs`, `MouseGlyph`).
