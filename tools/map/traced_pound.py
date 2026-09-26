"""The pound traced by hand from the 1900 map, for as long as OpenStreetMap cannot be reached: the canal's middle read
off the stitched sheet as pixels from Huddlesford Junction to Fazeley Junction, turned into metres and smoothed.
"""
from __future__ import annotations

import json
from pathlib import Path

from .curves import smoothThrough
from .ground import toDecimetres
from .layer_files import lineRecord
from .period_map import pixelPoint
from .pound import CANAL_KIND

TRACING = Path("docs") / "map" / "pound-traced-from-period-map.json"
SPACING_METRES = 10


def tracedPound(repositoryRoot: Path) -> dict:
    tracing = json.loads((repositoryRoot / TRACING).read_text(encoding="utf-8"))
    points = [pixelPoint(x, y) for x, y in tracing["pixels"]]
    curve = smoothThrough(points, SPACING_METRES)
    return lineRecord(CANAL_KIND, [toDecimetres(coordinate) for point in curve for coordinate in point])
