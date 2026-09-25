# The period map

Ordnance Survey one-inch to the mile, second edition, surveyed and revised around 1898 to 1904, from the National Library of Scotland's tile service. It is the last full survey before the game's decade, and the ground is the 1938 ground: the same canal, the same railway alongside it, the same villages, farms, mills and woods.

`one-inch-1900-huddlesford-to-fazeley.jpg` is the whole stretch stitched, a mile and more either side of the water. `tiles/` holds the 256 source tiles as downloaded, for a map overlay in the game.

## Georeferencing the tiles

The tiles are Web Mercator, zoom 15, named `x_y.png`. They cover x 16220 to 16235 and y 10720 to 10735, which is roughly longitude -1.868 to -1.692 and latitude 52.567 to 52.685. Tile (16220, 10720) is the top-left corner. Each tile is 256 pixels and about 740 metres across at this latitude. To place a point, convert its longitude and latitude to zoom-15 tile coordinates the usual way and subtract the origin.

## Licence

The National Library of Scotland publishes these tiles under Creative Commons Attribution-NonCommercial-ShareAlike 4.0, so they are fine for design reference and for the build while the game is not sold. The underlying Ordnance Survey map is out of Crown copyright. Before the map ships in a sold game, either license the scans from the library or source a public-domain scan of the same sheets.

## What is 1938 and not on the map

- Whittington Heath is drawn as the old racecourse; by 1938 it is the barracks golf course.
- The Wyrley and Essington arm west from Huddlesford is drawn live; by 1938 it is silted and hardly used.
- Drayton Manor, the Peel house west of Fazeley, was demolished in 1929. The park and lodges remain.
- Watling Street is drawn as a country road. By 1938 it is the A5 and carries the lorries.
- Tamworth has grown a little towards Fazeley along the Bonehill road.
