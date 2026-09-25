"""The 1900 one-inch map laid on the ground: the rectangle of metres its stitched image covers, worked out from the
zoom-15 Web Mercator tiles it was stitched from, and a copy of the image beside the layers so the game can load it.
"""
from __future__ import annotations

import math
import shutil
from pathlib import Path
from typing import NamedTuple

from .ground import metresEast, metresNorth, toDecimetres

TILE_ZOOM = 15
FIRST_TILE_COLUMN = 16220
FIRST_TILE_ROW = 10720
TILES_ACROSS = 16
TILES_DOWN = 16
FULL_TURN_DEGREES = 360.0
HALF_TURN_DEGREES = 180.0
SOURCE_IMAGE = Path("docs") / "map" / "one-inch-1900-huddlesford-to-fazeley.jpg"
IMAGE_NAME = "period-map.jpg"


class Box(NamedTuple):
    south: float
    west: float
    north: float
    east: float

    def widenedTo(self, other: Box) -> Box:
        return Box(
            south=min(self.south, other.south),
            west=min(self.west, other.west),
            north=max(self.north, other.north),
            east=max(self.east, other.east),
        )


def tilesAroundTheWorld() -> int:
    return 2 ** TILE_ZOOM


def tileLongitude(column: float) -> float:
    return column / tilesAroundTheWorld() * FULL_TURN_DEGREES - HALF_TURN_DEGREES


def tileLatitude(row: float) -> float:
    mercatorNorthing = math.pi * (1 - 2 * row / tilesAroundTheWorld())
    return math.degrees(math.atan(math.sinh(mercatorNorthing)))


def periodMapBox() -> Box:
    return Box(
        south=tileLatitude(FIRST_TILE_ROW + TILES_DOWN),
        west=tileLongitude(FIRST_TILE_COLUMN),
        north=tileLatitude(FIRST_TILE_ROW),
        east=tileLongitude(FIRST_TILE_COLUMN + TILES_ACROSS),
    )


def groundPlacement() -> dict:
    box = periodMapBox()
    return {
        "image": IMAGE_NAME,
        "west": toDecimetres(metresEast(box.west)),
        "east": toDecimetres(metresEast(box.east)),
        "south": toDecimetres(metresNorth(box.south)),
        "north": toDecimetres(metresNorth(box.north)),
    }


def copyImage(repositoryRoot: Path, mapFolder: Path) -> None:
    shutil.copyfile(repositoryRoot / SOURCE_IMAGE, mapFolder / IMAGE_NAME)
