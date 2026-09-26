"""The map drawn by hand off the 1900 sheet, the game's own 1938 ground: each layer traced from the stitched map as
pixels and kept in docs/map/traced, turned here into metres, with every line smoothed round its bends and every
building block the sheet draws raised as the cottages standing on it, each set back from the roads and the water.
"""
from __future__ import annotations

import json
from pathlib import Path

from .cottages import cottagesOn
from .curves import smoothThrough
from .ground import toDecimetres
from .layer_files import areaRecord, lineRecord
from .overlaps import overlaps
from .period_map import pixelPoint
from .set_backs import SET_BACK_METRES_BY_LAYER, Obstacle, obstacle, setBack

TRACED_FOLDER = Path("docs") / "map" / "traced"
LINES = "lines"
AREAS = "areas"
BLOCKS = "blocks"
SPACING_METRES = 10
POINTS_PER_OBSTACLE = 12
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


def standingCottages(tracing: dict, obstacles: list[Obstacle]) -> list[dict]:
    standing = []
    for feature in tracing[BLOCKS]:
        for cottage in cottagesOn(metresOf(feature["pixels"])):
            placed = setBack(cottage, obstacles)
            isLeftOut = placed is None or any(overlaps(placed, other) for other in standing)
            if not isLeftOut:
                standing.append(placed)
    return [areaRecord(flatDecimetres(cottage), []) for cottage in standing]


def tracedLayer(tracing: dict, obstacles: list[Obstacle]) -> tuple[str, list[dict]]:
    if LINES in tracing:
        return LINES, [tracedLine(feature) for feature in tracing[LINES]]
    if BLOCKS in tracing:
        return AREAS, standingCottages(tracing, obstacles)
    return AREAS, [tracedArea(feature) for feature in tracing[AREAS]]


def tracedLayers(repositoryRoot: Path) -> dict[str, tuple[str, list[dict]]]:
    paths = sorted((repositoryRoot / TRACED_FOLDER).glob("*.json"))
    tracings = {path.stem: json.loads(path.read_text(encoding="utf-8")) for path in paths}
    obstacles = obstaclesIn(tracings)
    return {layer: tracedLayer(tracing, obstacles) for layer, tracing in tracings.items()}
