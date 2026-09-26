"""The map drawn by hand off the 1900 sheet, the game's own 1938 ground: each layer traced from the stitched map as
pixels and kept in docs/map/traced, turned here into metres, with every line smoothed round its bends.
"""
from __future__ import annotations

import json
from pathlib import Path

from .curves import smoothThrough
from .ground import toDecimetres
from .layer_files import areaRecord, lineRecord
from .period_map import pixelPoint

TRACED_FOLDER = Path("docs") / "map" / "traced"
LINES = "lines"
AREAS = "areas"
SPACING_METRES = 10


def metresOf(pixels: list[list[float]]) -> list[tuple[float, float]]:
    return [pixelPoint(x, y) for x, y in pixels]


def flatDecimetres(points: list[tuple[float, float]]) -> list[float]:
    return [toDecimetres(coordinate) for point in points for coordinate in point]


def tracedLine(feature: dict) -> dict:
    curve = smoothThrough(metresOf(feature["pixels"]), SPACING_METRES)
    return lineRecord(feature["kind"], flatDecimetres(curve))


def tracedArea(feature: dict) -> dict:
    return areaRecord(flatDecimetres(metresOf(feature["pixels"])), [])


def tracedLayer(tracing: dict) -> tuple[str, list[dict]]:
    if LINES in tracing:
        return LINES, [tracedLine(feature) for feature in tracing[LINES]]
    return AREAS, [tracedArea(feature) for feature in tracing[AREAS]]


def tracedLayers(repositoryRoot: Path) -> dict[str, tuple[str, list[dict]]]:
    tracings = sorted((repositoryRoot / TRACED_FOLDER).glob("*.json"))
    return {path.stem: tracedLayer(json.loads(path.read_text(encoding="utf-8"))) for path in tracings}
