"""Which way a building block faces: towards the nearest road, canal, river or railway line, the way a village's houses
front what passes them. A block with nothing near it faces south, to the sun.
"""
from __future__ import annotations

import math

from .set_backs import Obstacle, nearestFoot

LOOKING_METRES = 120.0
SOUTH = (0.0, -1.0)
NOTHING = 0.0

Point = tuple[float, float]


def isWithinSight(line: Obstacle, centre: Point) -> bool:
    east, north = centre
    return line.west - LOOKING_METRES <= east <= line.east + LOOKING_METRES and line.south - LOOKING_METRES <= north <= line.north + LOOKING_METRES


def streetward(centre: Point, obstacles: list[Obstacle]) -> Point:
    feet = [nearestFoot(centre, line.points) for line in obstacles if isWithinSight(line, centre)]
    if not feet:
        return SOUTH
    foot = min(feet, key=lambda place: math.dist(centre, place))
    distance = math.dist(centre, foot)
    if distance == NOTHING:
        return SOUTH
    return (foot[0] - centre[0]) / distance, (foot[1] - centre[1]) / distance
