"""Areas on the ground, woods and buildings, as outlines with their holes: from closed ways, and from multipolygon
relations whose outer and inner rings are joined back together from the ways they were split into.
"""
from __future__ import annotations

from .elements import flatPoints, isRelation, isWay, placesOf, tagsOf

OUTER_ROLES = ("outer", "")
INNER_ROLES = ("inner",)
SMALLEST_RING = 4


def isWood(tags: dict) -> bool:
    return tags.get("natural") == "wood" or tags.get("landuse") == "forest"


def isBuilding(tags: dict) -> bool:
    return "building" in tags


def isClosed(places: list[dict]) -> bool:
    return len(places) >= SMALLEST_RING and places[0] == places[-1]


def ringPoints(places: list[dict]) -> list[float]:
    withoutClosingPlace = places[:-1]
    return flatPoints(withoutClosingPlace)


def areaFeature(outline: list[dict], holes: list[list[dict]]) -> dict:
    return {"outline": ringPoints(outline), "holes": [{"points": ringPoints(hole)} for hole in holes]}


def nextSegment(end: dict, remaining: list[list[dict]]) -> list[dict] | None:
    for index, segment in enumerate(remaining):
        if segment[0] == end:
            return remaining.pop(index)
        if segment[-1] == end:
            return list(reversed(remaining.pop(index)))
    return None


def closedRing(ring: list[dict], remaining: list[list[dict]]) -> list[dict]:
    while not isClosed(ring):
        follower = nextSegment(ring[-1], remaining)
        if follower is None:
            return []
        ring = ring + follower[1:]
    return ring


def joinedRings(segments: list[list[dict]]) -> list[list[dict]]:
    remaining = [list(segment) for segment in segments if segment]
    rings = []
    while remaining:
        rings.append(closedRing(remaining.pop(), remaining))
    return [ring for ring in rings if ring]


def memberRings(relation: dict, roles: tuple[str, ...]) -> list[list[dict]]:
    ways = [member for member in relation.get("members", []) if isWay(member)]
    return joinedRings([placesOf(way) for way in ways if way.get("role") in roles])


def elementAreas(element: dict) -> list[dict]:
    places = placesOf(element)
    if isRelation(element):
        holes = memberRings(element, INNER_ROLES)
        return [areaFeature(outline, holes) for outline in memberRings(element, OUTER_ROLES)]
    if isWay(element) and isClosed(places):
        return [areaFeature(places, [])]
    return []


def areaFeatures(elements: list[dict], belongs) -> list[dict]:
    return [area for element in elements if belongs(tagsOf(element)) for area in elementAreas(element)]
