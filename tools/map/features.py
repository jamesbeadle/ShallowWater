"""Cuts the Overpass answer into the game's layers: each way sorted by what it is and turned into metres on the ground,
as lines for water, railways and roads, and as areas for woods and buildings.
"""
from __future__ import annotations

from .areas import areaFeatures, isBuilding, isWood
from .elements import flatPoints, isWay, placesOf, tagsOf
from .layer_files import lineRecord
from .overpass import ROAD_CLASSES
from .pound import PoundRoute

CANALS = ("waterway", ("canal",))
LINE_LAYERS = {
    "river": ("waterway", ("river",)),
    "railway": ("railway", ("rail",)),
    "roads": ("highway", ROAD_CLASSES),
}
AREA_LAYERS = {"woods": isWood, "buildings": isBuilding}


def kindIn(way: dict, rule: tuple[str, tuple[str, ...]]) -> str | None:
    key, kinds = rule
    kind = tagsOf(way).get(key)
    return kind if kind in kinds else None


def waysIn(ways: list[dict], rule: tuple[str, tuple[str, ...]]) -> list[dict]:
    return [way for way in ways if kindIn(way, rule)]


def lineFeatures(ways: list[dict], rule: tuple[str, tuple[str, ...]]) -> list[dict]:
    return [lineRecord(kindIn(way, rule), flatPoints(placesOf(way))) for way in waysIn(ways, rule)]


def layers(elements: list[dict]) -> dict[str, tuple[str, list[dict]]]:
    ways = [element for element in elements if isWay(element)]
    pound = PoundRoute(waysIn(ways, CANALS))
    content = {"pound": ("lines", [pound.line()]), "canal": ("lines", pound.canalsBeyond())}
    for layer, rule in LINE_LAYERS.items():
        content[layer] = ("lines", lineFeatures(ways, rule))
    for layer, belongs in AREA_LAYERS.items():
        content[layer] = ("areas", areaFeatures(elements, belongs))
    return content
