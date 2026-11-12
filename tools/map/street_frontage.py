"""The houses along the streets of the villages and towns. The one-inch sheet drops most small buildings and draws none
at all in Tamworth, Kettlebrook and Wilnecote, so the streets it does draw stand open. Every road through a village
outline is lined on both sides with rows of houses, fronting the road at its set back: a town's frontage built up
almost end to end with entries between, a village's broken by gardens, yards and paddocks.
"""
from __future__ import annotations

import random
from typing import NamedTuple

from .plane import Point, difference, opposite, rightAngleClockwise, size, stepped, unit
from .rows import ROW_DEPTH_METRES, row

FRONT_FROM_THE_ROAD_MIDDLE_METRES = 10.0
HALF = 0.5
SIDES = (1.0, -1.0)


class Character(NamedTuple):
    shareBuilt: float
    shortestRowMetres: float
    longestRowMetres: float
    narrowestGapMetres: float
    widestGapMetres: float


TOWN = Character(0.9, 18.0, 34.0, 3.0, 7.0)
VILLAGE = Character(0.72, 9.0, 26.0, 4.0, 22.0)
CHARACTER_BY_PLACE = {"Fazeley": TOWN, "Tamworth": TOWN, "Kettlebrook and Glascote": TOWN, "Wilnecote": TOWN}


def characterOf(place: str) -> Character:
    return CHARACTER_BY_PLACE.get(place, VILLAGE)


def isInside(point: Point, outline: list[Point]) -> bool:
    east, north = point
    crossings = 0
    for start, end in zip(outline, outline[1:] + outline[:1]):
        isStraddling = (start[1] > north) != (end[1] > north)
        if not isStraddling:
            continue
        crossingEast = start[0] + (north - start[1]) * (end[0] - start[0]) / (end[1] - start[1])
        crossings += crossingEast > east
    return crossings % 2 == 1


def distancesAlong(curve: list[Point]) -> list[float]:
    distances = [0.0]
    for start, end in zip(curve, curve[1:]):
        distances.append(distances[-1] + size(difference(end, start)))
    return distances


def pointAt(curve: list[Point], distances: list[float], wanted: float) -> Point:
    for index in range(1, len(curve)):
        isWithinThisStep = distances[index] >= wanted
        if not isWithinThisStep:
            continue
        stepLength = distances[index] - distances[index - 1]
        fraction = (wanted - distances[index - 1]) / stepLength if stepLength else 0.0
        return stepped(curve[index - 1], difference(curve[index], curve[index - 1]), fraction)
    return curve[-1]


def rowBeside(curve: list[Point], distances: list[float], start: float, length: float, side: float) -> list[Point]:
    first = pointAt(curve, distances, start)
    last = pointAt(curve, distances, start + length)
    along = unit(difference(last, first))
    backwards = rightAngleClockwise(along) if side > 0 else opposite(rightAngleClockwise(along))
    roadMiddle = stepped(first, difference(last, first), HALF)
    middle = stepped(roadMiddle, backwards, FRONT_FROM_THE_ROAD_MIDDLE_METRES + HALF * ROW_DEPTH_METRES)
    return row(middle, along, backwards, length)


def rowsAlongOneSide(curve: list[Point], outline: list[Point], character: Character, side: float, chance: random.Random) -> list[list[Point]]:
    distances = distancesAlong(curve)
    rows = []
    start = chance.uniform(0.0, character.widestGapMetres)
    while start + character.shortestRowMetres < distances[-1]:
        length = min(chance.uniform(character.shortestRowMetres, character.longestRowMetres), distances[-1] - start)
        isBuilt = chance.random() < character.shareBuilt
        candidate = rowBeside(curve, distances, start, length, side)
        isInTheVillage = all(isInside(corner, outline) for corner in candidate)
        rows += [candidate] if isBuilt and isInTheVillage else []
        start += length + chance.uniform(character.narrowestGapMetres, character.widestGapMetres)
    return rows


def streetRows(villages: list[tuple[str, list[Point]]], roads: list[tuple[str, list[Point]]]) -> list[list[Point]]:
    rows = []
    for place, outline in villages:
        character = characterOf(place)
        for road, curve in roads:
            for side in SIDES:
                chance = random.Random(f"{place}/{road}/{side}")
                rows += rowsAlongOneSide(curve, outline, character, side, chance)
    return rows
