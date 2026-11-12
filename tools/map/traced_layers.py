"""The map drawn by hand off the 1900 sheet, the game's own 1938 ground: each layer traced from the stitched map as
pixels and kept in docs/map/traced, turned here into metres, with every line smoothed round its bends, the old inns
placed where they stood, and the rows of houses raised on every building block the sheet draws and along every
village street, each facing its street and set back from the roads and the water.
"""
from __future__ import annotations

import json
from pathlib import Path

from .curves import smoothThrough
from .ground import toDecimetres
from .inns import INNS, Inn, standingInns
from .layer_files import areaRecord, lineRecord
from .set_backs import SET_BACK_METRES_BY_LAYER, Obstacle, obstacle
from .standing_rows import BLOCKS, metresOf, standingRows

TRACED_FOLDER = Path("docs") / "map" / "traced"
LINES = "lines"
AREAS = "areas"
VILLAGES = "villages"
ROADS = "roads"
SPACING_METRES = 10
POINTS_PER_OBSTACLE = 12
SHARED_POINT = 1


def flatDecimetres(points: list[tuple[float, float]]) -> list[float]:
    return [toDecimetres(coordinate) for point in points for coordinate in point]


def curveOf(feature: dict) -> list[tuple[float, float]]:
    return smoothThrough(metresOf(feature["pixels"]), SPACING_METRES)


def piecesOf(curve: list[tuple[float, float]], setBackMetres: float) -> list[Obstacle]:
    step = POINTS_PER_OBSTACLE - SHARED_POINT
    return [obstacle(curve[start:start + POINTS_PER_OBSTACLE], setBackMetres) for start in range(0, len(curve) - 1, step)]


def obstaclesIn(tracings: dict[str, dict]) -> list[Obstacle]:
    obstacles = []
    for layer, setBackMetres in SET_BACK_METRES_BY_LAYER.items():
        features = tracings.get(layer, {}).get(LINES, [])
        obstacles += [piece for feature in features for piece in piecesOf(curveOf(feature), setBackMetres)]
    return obstacles


def streetsIn(tracings: dict[str, dict]) -> tuple[list, list]:
    villages = [(area["name"], metresOf(area["pixels"])) for area in tracings.get(VILLAGES, {}).get(AREAS, [])]
    roads = [(line["name"], curveOf(line)) for line in tracings.get(ROADS, {}).get(LINES, [])]
    return villages, roads


def innRecord(inn: Inn) -> dict:
    record = areaRecord(flatDecimetres(inn.corners), [])
    record["name"] = inn.name
    return record


def tracedLayer(tracing: dict, tracings: dict[str, dict], inns: list[Inn], obstacles: list[Obstacle]) -> tuple[str, list[dict]]:
    if LINES in tracing:
        return LINES, [lineRecord(feature["kind"], flatDecimetres(curveOf(feature))) for feature in tracing[LINES]]
    if INNS in tracing:
        return AREAS, [innRecord(inn) for inn in inns]
    if BLOCKS in tracing:
        rows = standingRows(tracing, streetsIn(tracings), [inn.corners for inn in inns], obstacles)
        return AREAS, [areaRecord(flatDecimetres(row), []) for row in rows]
    return AREAS, [areaRecord(flatDecimetres(metresOf(feature["pixels"])), []) for feature in tracing[AREAS]]


def tracedLayers(repositoryRoot: Path) -> dict[str, tuple[str, list[dict]]]:
    paths = sorted((repositoryRoot / TRACED_FOLDER).glob("*.json"))
    tracings = {path.stem: json.loads(path.read_text(encoding="utf-8")) for path in paths}
    obstacles = obstaclesIn(tracings)
    inns = standingInns(tracings.get(INNS, {}), obstacles)
    return {layer: tracedLayer(tracing, tracings, inns, obstacles) for layer, tracing in tracings.items()}
