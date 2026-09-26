"""Reading the elements of an Overpass answer: whether each is a way or a relation, its tags, and the places on the
ground it passes through, in metres from Hopwas bridge.
"""
from __future__ import annotations

from .ground import groundPoint

WAY = "way"
RELATION = "relation"


def isWay(element: dict) -> bool:
    return element.get("type") == WAY


def isRelation(element: dict) -> bool:
    return element.get("type") == RELATION


def tagsOf(element: dict) -> dict:
    return element.get("tags", {})


def placesOf(element: dict) -> list[dict]:
    return element.get("geometry", [])


def pointOf(place: dict) -> tuple[float, float]:
    return groundPoint(place["lon"], place["lat"])


def flatPoints(places: list[dict]) -> list[float]:
    return [coordinate for place in places for coordinate in pointOf(place)]
