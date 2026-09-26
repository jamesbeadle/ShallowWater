"""Cottages stand back from what runs past them. A cottage raised on a building block of the 1900 map, which draws both
the block and the road beside it far wider than they are, can land on the road or at the water's edge; it is moved
straight back until it stands its distance from every road, canal, river and railway, and left out if it cannot.
"""
from __future__ import annotations

import math
from typing import NamedTuple

SET_BACK_METRES_BY_LAYER = {"roads": 9.0, "pound": 15.0, "canal": 9.0, "river": 11.0, "railway": 7.0}
CLEARANCE_BEYOND_THE_SET_BACK_METRES = 0.5
MOVES_BEFORE_GIVING_UP = 6
NOTHING = 0.0

Point = tuple[float, float]


class Obstacle(NamedTuple):
    points: list[Point]
    setBackMetres: float
    west: float
    east: float
    south: float
    north: float

    def isNear(self, place: Point) -> bool:
        east, north = place
        reach = self.setBackMetres
        return self.west - reach <= east <= self.east + reach and self.south - reach <= north <= self.north + reach


def obstacle(points: list[Point], setBackMetres: float) -> Obstacle:
    easts = [east for east, _ in points]
    norths = [north for _, north in points]
    return Obstacle(points, setBackMetres, min(easts), max(easts), min(norths), max(norths))


def footOnSegment(place: Point, start: Point, end: Point) -> Point:
    runEast, runNorth = end[0] - start[0], end[1] - start[1]
    lengthSquared = runEast * runEast + runNorth * runNorth
    isAPoint = lengthSquared == NOTHING
    if isAPoint:
        return start
    fraction = ((place[0] - start[0]) * runEast + (place[1] - start[1]) * runNorth) / lengthSquared
    fraction = min(1.0, max(NOTHING, fraction))
    return start[0] + runEast * fraction, start[1] + runNorth * fraction


def nearestFoot(place: Point, points: list[Point]) -> Point:
    feet = [footOnSegment(place, start, end) for start, end in zip(points, points[1:])]
    return min(feet, key=lambda foot: math.dist(place, foot))


def centreOf(corners: list[Point]) -> Point:
    return sum(east for east, _ in corners) / len(corners), sum(north for _, north in corners) / len(corners)


def awayFrom(line: Obstacle, centre: Point) -> Point | None:
    foot = nearestFoot(centre, line.points)
    away = (centre[0] - foot[0], centre[1] - foot[1])
    length = math.hypot(*away)
    isOnTheLine = length == NOTHING
    if isOnTheLine:
        return None
    return away[0] / length, away[1] / length


def shortfall(corners: list[Point], line: Obstacle) -> tuple[float, Point | None]:
    centre = centreOf(corners)
    away = awayFrom(line, centre)
    if away is None:
        return line.setBackMetres, None
    foot = nearestFoot(centre, line.points)
    wanted = line.setBackMetres + CLEARANCE_BEYOND_THE_SET_BACK_METRES
    distances = [(east - foot[0]) * away[0] + (north - foot[1]) * away[1] for east, north in corners]
    return wanted - min(distances), away


def movedBack(corners: list[Point], line: Obstacle) -> tuple[list[Point], bool]:
    missing, away = shortfall(corners, line)
    isClear = missing <= NOTHING
    if isClear or away is None:
        return corners, not isClear
    step = (away[0] * missing, away[1] * missing)
    return [(east + step[0], north + step[1]) for east, north in corners], True


def setBack(corners: list[Point], obstacles: list[Obstacle]) -> list[Point] | None:
    near = [line for line in obstacles if any(line.isNear(corner) for corner in corners)]
    for _ in range(MOVES_BEFORE_GIVING_UP):
        moves = 0
        for line in near:
            corners, hasMoved = movedBack(corners, line)
            moves += hasMoved
        if moves == 0:
            return corners
    return None
