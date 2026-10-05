"""Points and directions on the flat ground, in metres east and north: the few sums the map's rows of houses are laid out
with.
"""
from __future__ import annotations

import math

Point = tuple[float, float]


def difference(end: Point, start: Point) -> Point:
    return end[0] - start[0], end[1] - start[1]


def stepped(point: Point, direction: Point, distance: float) -> Point:
    return point[0] + direction[0] * distance, point[1] + direction[1] * distance


def size(vector: Point) -> float:
    return math.hypot(vector[0], vector[1])


def unit(vector: Point) -> Point:
    return vector[0] / size(vector), vector[1] / size(vector)


def rightAngleClockwise(vector: Point) -> Point:
    return vector[1], -vector[0]


def centreOf(corners: list[Point]) -> Point:
    return sum(east for east, _ in corners) / len(corners), sum(north for _, north in corners) / len(corners)


def opposite(vector: Point) -> Point:
    return -vector[0], -vector[1]


def dot(first: Point, second: Point) -> float:
    return first[0] * second[0] + first[1] * second[1]
