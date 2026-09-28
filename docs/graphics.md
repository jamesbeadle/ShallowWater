# How the pound is drawn

Nothing in the scene is a texture file. The shapes come from the plain C# in `Assets/Game`, and the Unity layer gives every surface a look: a shader in `Assets/Unity/Resources/Shaders` that draws its detail from where it is in the world, so the same look holds from the helm to the far bank and a new surface needs no artwork.

## From a surface to a look

Every shape the game builds is filed under a `Surface` (`Assets/Game/Shapes/Surface.cs`): grass, towpath, coping, water, brick wall, slate roof, hull, cabin and the rest. `Assets/Unity/Looks/SurfaceLooks.cs` gives each surface its look, in one table, and `Finishes.For(surface)` makes that material once and shares it.

| Look | Draws | Used for |
| --- | --- | --- |
| Weathered | colour, worn patches and grain in world space | grass, banks, dirt towpath, tar, gravel, timber, iron, bark, and on the boat hull, deck, cloths, cratch and brass |
| Stone | laid stones with mortar between | coping, setts on the paved towpath, kerbs, flagstones |
| Brick | stretcher-bond brick, and on cottage walls sash windows, curtains, sills, a panelled door and a step | cottage walls, gables, chimney stacks, the lock chamber |
| Slate | lapped slate courses with moss near the eaves | cottage roofs |
| Hedge | leaf clumps, autumn turning and haws | hedgerows and garden hedges |
| Crown | leaf clumps, each tree its own autumn colour, stirring in the wind | tree crowns in the woods |
| Livery | painted panels with a coach line | *Sparrow*'s cabin sides and back doors |
| Railway | ballast, sleepers and rails | both railways |
| Fields | a patchwork of pasture, stubble and plough with hedgerow lines | the countryside under everything |
| Water | ripples, the sky and its clouds reflected, the banks darkening the edges, silt and fallen leaves | the canal, the canals beyond the pound, the Tame |
| Sky | the sky gradient, sun, glow and drifting cloud | the skybox |
| Grade | exposure, October warmth, tone curve and vignette | the finished picture |

The colours are named in the palettes beside the looks (`CountryPalette`, `RoadPalette`, `StonePalette`, `BuildingPalette`, `BoatPalette`, `WaterPalette`, `SkyPalette`), written as they would be picked, and turned into linear light by Unity.

## Where a point sits on its surface

Some looks need to know where they are on the thing they cover, not just in the world: a window belongs in the middle of a wall, a rail a set distance from the middle of the track, the water's banks at its edges. Every point of a `Shape` carries a `SurfacePlace` for that, sent to the shader as the mesh's first texture coordinates:

- a ribbon (canal, road, railway) measures along its line and across from its middle;
- a cottage wall measures from the middle of the wall and up from its foot;
- a roof slope measures along the ridge and up the slope from the eaves;
- the boat measures along its length and up from the waterline.

## Things that move

World-space detail would slide over anything that moves, so the boat's looks are made with `WeatheredLook.Aboard`, which measures from the boat instead of the world, and the livery measures from its surface places. A new moving thing takes its looks the same way.

## The settings the look depends on

- Linear colour space (`ProjectSettings.asset`), so light adds up as light does.
- The camera renders in HDR with anti-aliasing and hands the frame to `PictureGrade`, which brings it down to the screen.
- Fog variants are kept (`m_FogStripping: 1` in `GraphicsSettings.asset`) and instancing variants are kept (`m_InstancingStripping: 2`), because every material is made at runtime and a build cannot see which fog mode or instancing it will need.
- Shadows reach 120 metres on High (the web), 160 on Very High and 220 on Ultra (PC and consoles), in two or four cascades (`QualitySettings.asset`).
- The shaders live in a `Resources` folder so every build carries them, and `LookShaders.Made` names the file when one is missing.
