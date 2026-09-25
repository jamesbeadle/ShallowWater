"""Writes each layer as compact JSON with one feature to a line, so a regenerated layer reads and diffs feature by
feature, in the shape Unity's JsonUtility reads straight into its records.
"""
from __future__ import annotations

import json
from pathlib import Path

COMPACT_SEPARATORS = (",", ":")


def compactJson(content: dict) -> str:
    return json.dumps(content, separators=COMPACT_SEPARATORS)


def layerPath(mapFolder: Path, name: str) -> Path:
    return mapFolder / f"{name}.json"


def writeRecord(mapFolder: Path, name: str, record: dict) -> None:
    layerPath(mapFolder, name).write_text(compactJson(record) + "\n", encoding="utf-8")


def writeFeatures(mapFolder: Path, name: str, key: str, features: list[dict]) -> None:
    body = ",\n".join(compactJson(feature) for feature in features)
    layerPath(mapFolder, name).write_text(f'{{"{key}":[\n{body}\n]}}\n', encoding="utf-8")
