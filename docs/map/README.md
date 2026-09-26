# The period map

Ordnance Survey one-inch to the mile, second edition, surveyed and revised around 1898 to 1904, from the National Library of Scotland's tile service. It is the last full survey before the game's decade, and the ground is the 1938 ground: the same canal, the same railway alongside it, the same villages, farms, mills and woods.

`one-inch-1900-huddlesford-to-fazeley.jpg` is the whole stretch stitched, a mile and more either side of the water. `tiles/` holds the 256 source tiles as downloaded, for a map overlay in the game.

## Georeferencing the tiles

The tiles are Web Mercator, zoom 15, named `x_y.png`. They cover x 16220 to 16235 and y 10720 to 10735, which is longitude -1.8018 to -1.6260 and latitude 52.5897 to 52.6964: from just west of Streethay to Amington, and from Drayton Bassett to Elford. Tile (16220, 10720) is the top-left corner. Each tile is 256 pixels and about 740 metres across at this latitude. To place a point, convert its longitude and latitude to zoom-15 tile coordinates the usual way and subtract the origin.

## Licence

The National Library of Scotland publishes these tiles under Creative Commons Attribution-NonCommercial-ShareAlike 4.0, so they are fine for design reference and for the build while the game is not sold. The underlying Ordnance Survey map is out of Crown copyright. Before the map ships in a sold game, either license the scans from the library or source a public-domain scan of the same sheets.

## What is 1938 and not on the map

- Whittington Heath is drawn as the old racecourse; by 1938 it is the barracks golf course.
- The Wyrley and Essington arm west from Huddlesford is drawn live; by 1938 it is silted and hardly used.
- Drayton Manor, the Peel house west of Fazeley, was demolished in 1929. The park and lodges remain.
- Watling Street is drawn as a country road. By 1938 it is the A5 and carries the lorries.
- Tamworth has grown a little towards Fazeley along the Bonehill road.

## The ground the game is built on

The game is laid out in metres on flat ground: x east and z north of Hopwas bridge, where the Lichfield to Tamworth road crosses the canal (longitude -1.7373, latitude 52.6433), one metre to one Unity unit. Everything the game reads about the map is in these metres, in `Assets/StreamingAssets/map/`, and nothing in the game knows about latitude.

Those files are generated, never edited by hand. From the repository root:

```
python3 -m tools.map                          # fetch from Overpass and write every layer
python3 -m tools.map --answer overpass.json   # write every layer from a saved Overpass answer
python3 -m tools.map --traced                 # write every layer traced by hand from the period map
```

Public Overpass servers are often busy and answer 504 or 429. The script asks overpass-api.de, overpass.private.coffee and maps.mail.ru in turn, for three rounds a few minutes apart, and refuses an answer the server cut short. If none answers in full, it prints the query: run that at overpass-turbo.eu, export the raw data, and pass the file back with `--answer`.

`ground.json` says which rectangle of metres the stitched period map covers, worked out from the tiles above, and `period-map.jpg` is a copy of it for the game to load. The map is laid as one flat quad, so the Mercator rows sit up to four metres from where they belong in the middle of the sheet, round Hopwas; everything else is exact to the decimetre the layers are written to.

The layers come from one Overpass request for the box longitude -1.868 to -1.692, latitude 52.567 to 52.685, widened to cover the whole period map: the canals and the rivers, railways, roads from primary down to unclassified, woods and forest, and the buildings within 900 metres of Hopwas, Whittington, Fazeley and Huddlesford. `pound.json` is the canal from Huddlesford Junction to Fazeley Junction as one line, north to south, the water Sparrow steers along, found as the shortest way along the canals between the two places where three of them meet; `canal.json` is the rest of the canals, the water beyond the two junctions. The script prints how many features each layer holds and how long the pound came out: about 11 kilometres is right. Trunk roads and motorways are left out: the A5 and A38 dual carriageways and the M6 Toll came after 1938.

The game's map is drawn by hand off the period map, which is the 1938 ground; OpenStreetMap is today's, with the post-war estates and bypasses. `traced/` here holds each layer as points read off the stitched sheet in pixels, one file per layer: `pound.json` (the canal from Huddlesford Junction round Fazeley Junction to the foot of Glascote locks in Tamworth, 12.9 kilometres), `canal.json` (the Coventry Canal towards Fradley and the Birmingham and Fazeley towards Drayton Bassett), `river.json` (the Tame from Elford to Dosthill), `railway.json` (the Trent Valley line and the line from Lichfield towards Burton), `roads.json` (the seventeen roads of the demo area, from Watling Street and the Lichfield to Tamworth road down to the farm track over the canal below Huddlesford, each with its class: primary, secondary, tertiary or unclassified), `villages.json` (the outlines of Whittington, Hopwas, Fazeley, Tamworth, Kettlebrook and Glascote, and Wilnecote, where a road becomes a street with kerbs and pavements), `woods.json` (Hopwas Hays Wood with Tamhorn Park, Packington, Comberford Hall, Wigginton Hall, Kendall's Wood, the Bonehill strip, Drayton Manor park and the Hints woods) and `buildings.json` (the blocks of buildings in Whittington, Hopwas and Fazeley). `python3 -m tools.map --traced` turns them into metres, with every line smoothed into a point every ten metres, and writes the layers the game reads. Every line is snapped onto the feature as the sheet prints it, and checked at close zoom against the sheet: the canal onto the middle of its band of parallel lines, the Tame onto the middle of its water lines, each railway onto its chequered band, and each road onto the middle of its two printed edges. The sheet draws a road about forty metres wide and the canal about twenty-five, so the game's roads and water, drawn at their real widths, run down the middle of the printed ones and look narrower. Every road that joins another ends on that road's middle line. The one-inch sheet draws buildings solid black at about twice their size and runs a row of them into one block, so each block in `buildings.json` is the rectangle of one black block on the sheet, and the script stands a row of cottages eight metres by six along it, or two rows back to back where the block is deep enough for both. Because the sheet draws the blocks and the roads beside them far wider than they are, a cottage can land on a road or at the water's edge; the script moves it straight back until it stands its distance from every road, canal, river and railway, and leaves it out if it cannot, or if it would then overlap a neighbour. Huddlesford's few buildings are lost among the canal, the railway and the lanes at this scale, so none are drawn there. Fazeley Junction is taken where the Birmingham and Fazeley turns south under Watling Street; from there the pound runs east along the Coventry Canal, over the Tame aqueduct and past Kettlebrook, and ends where the Glascote road crosses above the locks. The game builds whichever layers it finds and says in the console which are missing; a full run of `python3 -m tools.map` would replace them all with OpenStreetMap's.

## The demo area

The demo is the canal from Whittington through Hopwas and Fazeley to Tamworth, and `demo-area-on-the-1900-map.webp` is its boundary, drawn in green on this map. The land's natural edges are the roads and the river marked there:

- the Lichfield road from Whittington Heath to Hopwas
- the lane south from Hopwas to Bonehill
- Watling Street through Fazeley and Wilnecote
- the Tame from Elford down to Coton
- the road north of Tamworth by Wigginton to Amington
- the road from Bonehill towards Bassett's Pole

On the water, obstacles close every way out upstream: the stop-planks at Huddlesford, the closed bottom gates of Glascote locks, and the chain across the Birmingham and Fazeley at Fazeley Junction.

## OpenStreetMap

The layers in `Assets/StreamingAssets/map/` other than `ground.json` and `period-map.jpg` are derived from OpenStreetMap: © OpenStreetMap contributors, available under the Open Database Licence (https://www.openstreetmap.org/copyright). The layers are a derived database and stay under the ODbL; the game that draws them is a produced work and must credit "© OpenStreetMap contributors" wherever it is shown or shipped.
