# How the pound is drawn

Nothing in the scene is a texture file. The shapes come from the plain C# in `Assets/Game`, and the Unity layer gives every surface a look: a shader in `Assets/Unity/Resources/Shaders` that draws its detail from where it is in the world, so the same look holds from the helm to the far bank and a new surface needs no artwork.

## From a surface to a look

Every shape the game builds is filed under a `Surface` (`Assets/Game/Shapes/Surface.cs`): grass, towpath, coping, water, brick wall, slate roof, hull, cabin and the rest. `Assets/Unity/Looks/SurfaceLooks.cs` gives each surface its look, in one table, and `Finishes.For(surface)` makes that material once and shares it.

| Look | Draws | Used for |
| --- | --- | --- |
| Weathered | colour, worn patches, grain and soot | grass, banks, dirt towpath, tar, gravel, timber, iron, the leaf litter under the woods, and on the boat the deck, cloths, roof, tarnished brass and ironwork |
| Stone | laid stones with mortar between | coping, setts on the paved towpath, kerbs, flagstones |
| Brick | stretcher-bond brick, and on cottage walls sash windows, curtains, sills, a panelled door and a step | cottage walls, gables, chimney stacks, the lock chamber |
| Slate | lapped slate courses with moss near the eaves | cottage roofs |
| Hedge | leaf clumps, autumn turning and haws | hedgerows and garden hedges |
| Foliage | leaf cards turned to face the eye, each a cluster of oak, ash, birch or hazel leaves or pine needle tufts, each tree its own way into autumn, with light through the leaves and a breeze in them | the crowns of every tree and hazel in the woods |
| Bark | furrowed oak, netted ash, white birch with its black foot and lenticels, pine plates turning orange up the trunk, lichen and moss at the foot | the trunks and limbs of every tree |
| Paintwork | faded panels and coach lines, SPARROW signwritten with roses, roses and castles, diamonds, all under soot, streaks and chipped paint | *Sparrow*'s cabin sides, back doors, cratch, deck board and Buckby cans |
| Hull Plates | riveted iron plates lapped in strakes, tar, coal dust, rust streaks, scrapes and weed at the waterline | *Sparrow*'s hull |
| Rope | laid strands, or hair | cloth strings, Turk's heads, the mop, the fender and coiled line, the horse tail |
| Smoke | puffs that rise, grow, drift and fade, left behind as the boat moves | the stove chimney, and the Bolinder's exhaust at one puff a second |
| Railway | ballast, sleepers and rails | both railways |
| Fields | a patchwork of pasture, stubble and plough with hedgerow lines | the countryside under everything |
| Water | ripples, the sky and its clouds reflected, the banks darkening the edges, silt, fallen leaves, and *Sparrow*'s bow wave, her wake along the path she took, the prop wash and churned silt, and her wash breaking on the banks | the canal, the canals beyond the pound, the Tame |
| Sky | the sky gradient, sun, glow and drifting cloud | the skybox |
| Grade | exposure, October warmth, tone curve and vignette | the finished picture |

The colours are named in the palettes beside the looks (`CountryPalette`, `RoadPalette`, `StonePalette`, `BuildingPalette`, `BoatPalette`, `WaterPalette`, `SkyPalette`), written as they would be picked, and turned into linear light by Unity.

## Relief

Every look also shapes the light, not just the colour: mortar sits back from the brick, slates step down at their laps, rivets stand proud of the hull, leaf clumps bulge and furrows run across the plough. `Relief.cginc` turns a height in metres into a bent normal from the screen-space change in that height, so no normal maps are needed, and every mesh carries tangents (`ShapeMeshes`) so the bent normal can be handed to Unity's lighting. Fine relief fades with distance like the patterns do, so nothing sparkles far off.

## *Sparrow*

*Sparrow* is a 1912 FMC Josher motor as the story bible has her: FMC dark green and red under twenty years of coal dust, no brass kept bright, the name and the roses on the cabin side under the soot, roses and castles on the cabin doors, and a Bolinder beating once a second. The shapes are plain C# in `Assets/Game/Boat`, measured from the boat moored at the origin with her bow to the north, and `SparrowShapes` puts them together:

- `Hull`, `HullShapes`, `RubbingStrakes`: a fine raked bow and an overhanging counter, the decks dropped into wells inside a rising sheer, gunwale capping, two rubbing strakes sweeping up the bow
- `CabinShapes`, `RoofFittings`: tumblehome, a roof lip, handrails, the slide and the pigeon box
- `Stovepipes`, `Chain`, `RoofStowage`, `RopeAndMop`: the stove chimney with its bands and chain, the exhaust, two Buckby cans, a coiled line and a mop
- `HoldShapes`, `Sheeting`, `ClothStrings`: cloths sagging between their strings, the top plank and the mast
- `ForeEnd`, `BowFender`: the deck board, headlamp, T-stud and bow fender
- `SternGear`: the rudder, swan-neck tiller, Turk's heads and horse tail

`Lathe` (turned shapes) and `Tube` (ropes, rails, links) in `Assets/Game/Shapes` build most of the fittings.

In the Unity layer, `SparrowModel` launches her with `Riding` (a gentle roll and heave, squatting with speed and heeling as she swings), `WakeSignal` (which tells the water and the smoke where the boat is, how fast she is going and how hard the engine is working), `WakeTrail`, `EngineSound` and two `Plume`s of smoke.

Her wake follows where she has been, not where she is pointing. `WakeTrail` keeps the stern's last 24 places, one every 0.6 seconds, with her speed and the engine's work at each, and hands them to the water. `Kelvin.cginc` finds the nearest point of that trail for every patch of water and draws the Kelvin pattern from it: the transverse waves astern, and the diverging arms spreading at 19.5 degrees from the bow, their wavelength set by her speed and their height by its square, dying away over seven seconds. `Wake.cginc` adds the cushion of water heaped at the bow, the prop wash (streaky foam for a few seconds and churned silt for half a minute, as heavy as the engine was working) and foam on the crests when she goes faster than a walk; the water shader breaks the swell into foam where it reaches the banks. Turn her and the wake bends; stop her and it spreads out and settles.

## The woods

The woods are the ones on the map, planted the way Hopwas Hays and the smaller woods of the pound were in 1938. `WoodPlanting` sets out each wood in `Assets/Game/Woods`:

- `Compartments` divides it into compartments of a few hectares; some deep inside are Scots pine plantations in rows, and the rest is broadleaf.
- `Rides` cuts one or two straight rides through the larger woods.
- `SpeciesMix` plants the broadleaf mostly with oak, with patches of birch and ash and more birch along the edges; a fifth of the trees are young, and the oaks on the edge grow open, wide and low.
- `Understorey` sets hazel coppice under the canopy, thickest along the edges.
- `WoodFloor` lays leaf litter under every wood.

A tree is not a mesh from a file. Each species has a `Habit` (`OakHabits`, `AshHabits`, `BirchHabits`, `PineHabits`, `HazelHabits`): how tall it grows, where the trunk forks, how many limbs it throws and at what angle, how they bend and droop, and how its leaves cluster. `TreeGrowth` grows a tree from a habit and a seed, limb by limb; the trunk flares at its foot and ends in a rounded knuckle where its limbs fork from it, so no cut-off stub stands among them. `TreeForms` keeps the twenty-two forms the woods are planted with, four of the woodland oak that fills most of every wood and two or three of everything else, so a tree is not met again a few paces on. `TreeFigures` draws each form three ways: near, with every branch and every leaf clump; middle, with the main limbs and the clumps merged; and far, with the trunk and a few large clumps.

In the Unity layer, `WoodsRenderer` draws the woods for every camera that looks at them, the game's and the Scene view's. `WoodTiles` lays the trees in 200-metre tiles; for each camera, `WoodView` picks near, middle or far for every tree within 320 metres and far for the tiles beyond, draws nothing past three kilometres, and draws each form as one instanced batch per tile. Only the near and middle trees cast shadows.

## Where a point sits on its surface

Some looks need to know where they are on the thing they cover, not just in the world: a window belongs in the middle of a wall, a rail a set distance from the middle of the track, the water's banks at its edges. Every point of a `Shape` carries a `SurfacePlace` for that, sent to the shader as the mesh's first texture coordinates:

- a ribbon (canal, road, railway) measures along its line and across from its middle;
- a cottage wall measures from the middle of the wall and up from its foot;
- a roof slope measures along the ridge and up the slope from the eaves;
- the boat measures along its length and up from the waterline;
- a turned shape measures around it and up it, a tube along it and around it, and a limb along it and by the angle around it, so bark wraps without a seam.

## Things that move

World-space detail would slide over anything that moves, so the boat's looks are made with `WeatheredLook.Aboard`, which measures from the boat instead of the world, and the livery measures from its surface places. A new moving thing takes its looks the same way.

## The settings the look depends on

- Linear colour space (`ProjectSettings.asset`), so light adds up as light does.
- The camera renders in HDR with anti-aliasing and hands the frame to `PictureGrade`, which brings it down to the screen.
- Fog variants are kept (`m_FogStripping: 1` in `GraphicsSettings.asset`) and instancing variants are kept (`m_InstancingStripping: 2`), because every material is made at runtime and a build cannot see which fog mode or instancing it will need.
- Shadows reach 120 metres on High (the web), 160 on Very High and 220 on Ultra (PC and consoles), in two or four cascades (`QualitySettings.asset`).
- The shaders live in a `Resources` folder so every build carries them, and `LookShaders.Made` names the file when one is missing.
