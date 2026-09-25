"""The layer files the game reads, stated once: the shape of each record, in the field names Unity's JsonUtility reads
into Assets/Unity/Map/MapRecords.cs, and the writing of each layer as compact JSON with one feature to a line.
"""
from __future__ import annotations

import json
from pathlib import Path

COMPACT_SEPARATORS = (",", ":")
POINTS = "points"


def lineRecord(kind: str, points: list[float]) -> dict:
    return {"kind": kind, POINTS: points}


def areaRecord(outline: list[float], holes: list[list[float]]) -> dict:
    return {"outline": outline, "holes": [{POINTS: hole} for hole in holes]}


def pointsOf(record: dict) -> list[float]:
    return record[POINTS]


def compactJson(content: dict) -> str:
    return json.dumps(content, separators=COMPACT_SEPARATORS)


def layerPath(mapFolder: Path, name: str) -> Path:
    return mapFolder / f"{name}.json"


def writeRecord(mapFolder: Path, name: str, record: dict) -> None:
    layerPath(mapFolder, name).write_text(compactJson(record) + "\n", encoding="utf-8")


def writeFeatures(mapFolder: Path, name: str, key: str, features: list[dict]) -> None:
    body = ",\n".join(compactJson(feature) for feature in features)
    layerPath(mapFolder, name).write_text(f'{{"{key}":[\n{body}\n]}}\n', encoding="utf-8")
