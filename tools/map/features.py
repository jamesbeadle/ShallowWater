"""Cuts the Overpass answer into the game's layers: each way sorted by what it is and turned into metres on the ground,
as lines for water, railways and roads, and as areas for woods and buildings.
"""
from __future__ import annotations

from .areas import areaFeatures, isBuilding, isWood
from .elements import flatPoints, isWay, placesOf, tagsOf
from .overpass import ROAD_CLASSES
from .pound import poundLine

LINE_LAYERS = {
    "canal": ("waterway", ("canal",)),
    "river": ("waterway", ("river",)),
    "railway": ("railway", ("rail",)),
    "roads": ("highway", ROAD_CLASSES),
}
AREA_LAYERS = {"woods": isWood, "buildings": isBuilding}


def kindIn(way: dict, layer: str) -> str | None:
    key, kinds = LINE_LAYERS[layer]
    kind = tagsOf(way).get(key)
    return kind if kind in kinds else None


def waysIn(ways: list[dict], layer: str) -> list[dict]:
    return [way for way in ways if kindIn(way, layer)]


def lineFeatures(ways: list[dict], layer: str) -> list[dict]:
    return [{"kind": kindIn(way, layer), "points": flatPoints(placesOf(way))} for way in waysIn(ways, layer)]


def layers(elements: list[dict]) -> dict[str, tuple[str, list[dict]]]:
    ways = [element for element in elements if isWay(element)]
    content = {layer: ("lines", lineFeatures(ways, layer)) for layer in LINE_LAYERS}
    content["pound"] = ("lines", [poundLine(waysIn(ways, "canal"))])
    for layer, belongs in AREA_LAYERS.items():
        content[layer] = ("areas", areaFeatures(elements, belongs))
    return content
