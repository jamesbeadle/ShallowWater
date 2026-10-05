"""The map drawn by hand off the 1900 sheet, the game's own 1938 ground: each layer traced from the stitched map as
pixels and kept in docs/map/traced, turned here into metres, with every line smoothed round its bends and every
building block the sheet draws raised as the rows of houses standing on it, each facing its street and set back from
the roads and the water; a row that cannot stand back far enough is halved until its parts can, or left out.
"""
from __future__ import annotations

import json
from pathlib import Path

from .curves import smoothThrough
from .ground import toDecimetres
from .layer_files import areaRecord, lineRecord
from .overlaps import overlaps
from .period_map import pixelPoint
from .rows import SHORTEST_ROW_METRES, centreOf, frontageOf, halves, rowsOn
from .set_backs import SET_BACK_METRES_BY_LAYER, Obstacle, obstacle, setBack
from .street_side import streetward

TRACED_FOLDER = Path("docs") / "map" / "traced"
LINES = "lines"
AREAS = "areas"
BLOCKS = "blocks"
SPACING_METRES = 10
POINTS_PER_OBSTACLE = 12
PARTS_OF_A_HALVED_ROW = 2
SHARED_POINT = 1


def metresOf(pixels: list[list[float]]) -> list[tuple[float, float]]:
    return [pixelPoint(x, y) for x, y in pixels]


def flatDecimetres(points: list[tuple[float, float]]) -> list[float]:
    return [toDecimetres(coordinate) for point in points for coordinate in point]


def curveOf(feature: dict) -> list[tuple[float, float]]:
    return smoothThrough(metresOf(feature["pixels"]), SPACING_METRES)


def tracedLine(feature: dict) -> dict:
    return lineRecord(feature["kind"], flatDecimetres(curveOf(feature)))


def tracedArea(feature: dict) -> dict:
    return areaRecord(flatDecimetres(metresOf(feature["pixels"])), [])


def piecesOf(curve: list[tuple[float, float]], setBackMetres: float) -> list[Obstacle]:
    step = POINTS_PER_OBSTACLE - SHARED_POINT
    return [obstacle(curve[start:start + POINTS_PER_OBSTACLE], setBackMetres) for start in range(0, len(curve) - 1, step)]


def obstaclesIn(tracings: dict[str, dict]) -> list[Obstacle]:
    obstacles = []
    for layer, setBackMetres in SET_BACK_METRES_BY_LAYER.items():
        features = tracings.get(layer, {}).get(LINES, [])
        obstacles += [piece for feature in features for piece in piecesOf(curveOf(feature), setBackMetres)]
    return obstacles


def placedRows(row: list[tuple[float, float]], obstacles: list[Obstacle]) -> list[list[tuple[float, float]]]:
    placed = setBack(row, obstacles)
    if placed is not None:
        return [placed]
    isTooShortToHalve = frontageOf(row) < PARTS_OF_A_HALVED_ROW * SHORTEST_ROW_METRES
    if isTooShortToHalve:
        return []
    return [part for half in halves(row) for part in placedRows(half, obstacles)]


def standingRows(tracing: dict, obstacles: list[Obstacle]) -> list[dict]:
    standing = []
    for feature in tracing[BLOCKS]:
        corners = metresOf(feature["pixels"])
        for row in rowsOn(corners, streetward(centreOf(corners), obstacles)):
            for part in placedRows(row, obstacles):
                isClear = not any(overlaps(part, other) for other in standing)
                standing += [part] if isClear else []
    return [areaRecord(flatDecimetres(row), []) for row in standing]


def tracedLayer(tracing: dict, obstacles: list[Obstacle]) -> tuple[str, list[dict]]:
    if LINES in tracing:
        return LINES, [tracedLine(feature) for feature in tracing[LINES]]
    if BLOCKS in tracing:
        return AREAS, standingRows(tracing, obstacles)
    return AREAS, [tracedArea(feature) for feature in tracing[AREAS]]


def tracedLayers(repositoryRoot: Path) -> dict[str, tuple[str, list[dict]]]:
    paths = sorted((repositoryRoot / TRACED_FOLDER).glob("*.json"))
    tracings = {path.stem: json.loads(path.read_text(encoding="utf-8")) for path in paths}
    obstacles = obstaclesIn(tracings)
    return {layer: tracedLayer(tracing, obstacles) for layer, tracing in tracings.items()}
