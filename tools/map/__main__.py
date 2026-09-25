"""Regenerates the map the game is built on into Assets/StreamingAssets/map, from the period map in docs/map and from
OpenStreetMap. Nobody edits those files by hand; run this from the repository root instead.

    python3 -m tools.map                          fetch from Overpass and write every layer
    python3 -m tools.map --answer overpass.json   write every layer from a saved Overpass answer
"""
from __future__ import annotations

import argparse
import json
from pathlib import Path
from urllib.error import URLError

from .features import layers
from .layer_files import writeFeatures, writeRecord
from .overpass import INTERPRETER_ADDRESS, fetch, query
from .period_map import copyImage, groundPlacement

REPOSITORY_ROOT = Path(__file__).resolve().parents[2]
MAP_FOLDER = REPOSITORY_ROOT / "Assets" / "StreamingAssets" / "map"


def writeGround() -> None:
    MAP_FOLDER.mkdir(parents=True, exist_ok=True)
    copyImage(REPOSITORY_ROOT, MAP_FOLDER)
    writeRecord(MAP_FOLDER, "ground", groundPlacement())


def unreachable(failure: URLError) -> str:
    return (f"Could not reach {INTERPRETER_ADDRESS} ({failure}). The ground was written; the layers were not. "
            f"Save the answer to this query and pass it with --answer:\n\n{query()}")


def overpassAnswer(savedAnswer: str | None) -> dict:
    if savedAnswer:
        return json.loads(Path(savedAnswer).read_text(encoding="utf-8"))
    try:
        return fetch()
    except URLError as failure:
        raise SystemExit(unreachable(failure))


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    parser.add_argument("--answer", help="a saved Overpass answer to write the layers from instead of fetching")
    arguments = parser.parse_args()
    writeGround()
    answer = overpassAnswer(arguments.answer)
    for layer, (key, features) in layers(answer["elements"]).items():
        writeFeatures(MAP_FOLDER, layer, key, features)


main()
