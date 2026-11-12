"""The rows of houses that stand: those raised on the building blocks of the 1900 map, then those lining the village
streets, each set back from the roads and the water, halved until its parts can stand back if it cannot, and left out
where it would overlap an inn or a row already standing.
"""
from __future__ import annotations

from .overlaps import overlaps
from .period_map import pixelPoint
from .rows import SHORTEST_ROW_METRES, centreOf, frontageOf, halves, rowsOn
from .set_backs import Obstacle, setBack
from .street_frontage import streetRows
from .street_side import streetward

BLOCKS = "blocks"
PARTS_OF_A_HALVED_ROW = 2

Point = tuple[float, float]


def metresOf(pixels: list[list[float]]) -> list[Point]:
    return [pixelPoint(x, y) for x, y in pixels]


def placedRows(row: list[Point], obstacles: list[Obstacle]) -> list[list[Point]]:
    placed = setBack(row, obstacles)
    if placed is not None:
        return [placed]
    isTooShortToHalve = frontageOf(row) < PARTS_OF_A_HALVED_ROW * SHORTEST_ROW_METRES
    if isTooShortToHalve:
        return []
    return [part for half in halves(row) for part in placedRows(half, obstacles)]


def blockRows(tracing: dict, obstacles: list[Obstacle]) -> list[list[Point]]:
    blocks = [metresOf(feature["pixels"]) for feature in tracing[BLOCKS]]
    return [row for corners in blocks for row in rowsOn(corners, streetward(centreOf(corners), obstacles))]


def standingRows(tracing: dict, streets: tuple[list, list], taken: list[list[Point]], obstacles: list[Obstacle]) -> list[list[Point]]:
    villages, roads = streets
    standing = []
    for row in blockRows(tracing, obstacles) + streetRows(villages, roads):
        for part in placedRows(row, obstacles):
            isClear = not any(overlaps(part, other) for other in standing + taken)
            standing += [part] if isClear else []
    return standing
