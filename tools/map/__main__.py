"""Regenerates the map the game is built on into Assets/StreamingAssets/map, from the period map in docs/map and from
OpenStreetMap. Nobody edits those files by hand; run this from the repository root instead.

    python3 -m tools.map                          fetch from Overpass and write every layer
    python3 -m tools.map --answer overpass.json   write every layer from a saved Overpass answer
    python3 -m tools.map --traced                 write every layer traced by hand from the period map
"""
from __future__ import annotations

import argparse
import json
import math
from pathlib import Path

from .features import layers
from .layer_files import pointsOf, writeFeatures, writeRecord
from .overpass import OverpassUnreachable, fetch, query
from .period_map import copyImage, groundPlacement
from .traced_layers import tracedLayers

REPOSITORY_ROOT = Path(__file__).resolve().parents[2]
MAP_FOLDER = REPOSITORY_ROOT / "Assets" / "StreamingAssets" / "map"
POUND_LAYER = "pound"
METRES_IN_A_KILOMETRE = 1000
TURBO_ADDRESS = "https://overpass-turbo.eu"


def writeGround() -> None:
    MAP_FOLDER.mkdir(parents=True, exist_ok=True)
    copyImage(REPOSITORY_ROOT, MAP_FOLDER)
    writeRecord(MAP_FOLDER, "ground", groundPlacement())


def unreachable(failures: OverpassUnreachable) -> str:
    return (f"No Overpass server answered in full:\n{failures}\n\nThe ground was written; the layers were not. "
            f"Try again later, or paste the query below into {TURBO_ADDRESS}, run it, export the raw data and pass "
            f"the downloaded file with --answer:\n\n{query()}")


def overpassAnswer(savedAnswer: str | None) -> dict:
    if savedAnswer:
        return json.loads(Path(savedAnswer).read_text(encoding="utf-8"))
    try:
        return fetch()
    except OverpassUnreachable as failures:
        raise SystemExit(unreachable(failures))


def lengthInKilometres(line: dict) -> float:
    points = pointsOf(line)
    places = list(zip(points[0::2], points[1::2]))
    return sum(math.dist(first, second) for first, second in zip(places, places[1:])) / METRES_IN_A_KILOMETRE


def report(layer: str, features: list[dict]) -> None:
    print(f"{layer}: {len(features)} features")
    if layer == POUND_LAYER:
        print(f"  the pound runs {lengthInKilometres(features[0]):.1f} km from Huddlesford Junction to Glascote locks")


def chosenLayers(arguments: argparse.Namespace) -> dict[str, tuple[str, list[dict]]]:
    if arguments.traced:
        return tracedLayers(REPOSITORY_ROOT)
    answer = overpassAnswer(arguments.answer)
    return layers(answer["elements"])


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    parser.add_argument("--answer", help="a saved Overpass answer to write the layers from instead of fetching")
    parser.add_argument("--traced", action="store_true", help="write the layers traced by hand from the period map")
    arguments = parser.parse_args()
    writeGround()
    for layer, (key, features) in chosenLayers(arguments).items():
        writeFeatures(MAP_FOLDER, layer, key, features)
        report(layer, features)


main()
