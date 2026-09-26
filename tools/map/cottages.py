"""Cottages on the building blocks of the 1900 map. The one-inch sheet draws a village's buildings as solid blocks at
about twice their true size, so each block picked off it becomes a row of cottages along its length, or two rows back
to back when it is deep enough for both, each cottage the size of one standing in the villages today.
"""
from __future__ import annotations

import math
from typing import NamedTuple

COTTAGE_FRONTAGE_METRES = 8.0
COTTAGE_DEPTH_METRES = 6.0
GAP_BETWEEN_COTTAGES_METRES = 2.0
YARD_BETWEEN_ROWS_METRES = 4.0
ROWS_BACK_TO_BACK = 2
FEWEST_COTTAGES = 1
HALF = 0.5
CORNER_SIDES = ((-1, -1), (1, -1), (1, 1), (-1, 1))

Point = tuple[float, float]


class Block(NamedTuple):
    centre: Point
    along: Point
    across: Point
    length: float
    width: float


def difference(end: Point, start: Point) -> Point:
    return end[0] - start[0], end[1] - start[1]


def stepped(point: Point, direction: Point, distance: float) -> Point:
    return point[0] + direction[0] * distance, point[1] + direction[1] * distance


def size(vector: Point) -> float:
    return math.hypot(vector[0], vector[1])


def unit(vector: Point) -> Point:
    return vector[0] / size(vector), vector[1] / size(vector)


def centreOf(corners: list[Point]) -> Point:
    return sum(east for east, _ in corners) / len(corners), sum(north for _, north in corners) / len(corners)


def blockOf(corners: list[Point]) -> Block:
    shortSide, longSide = sorted((difference(corners[1], corners[0]), difference(corners[3], corners[0])), key=size)
    return Block(centreOf(corners), unit(longSide), unit(shortSide), size(longSide), size(shortSide))


def rowOffsets(block: Block) -> list[float]:
    fitsTwoRows = block.width >= ROWS_BACK_TO_BACK * COTTAGE_DEPTH_METRES + YARD_BETWEEN_ROWS_METRES
    if not fitsTwoRows:
        return [0.0]
    offset = HALF * (block.width - COTTAGE_DEPTH_METRES)
    return [-offset, offset]


def placesAlong(length: float) -> list[float]:
    pitch = COTTAGE_FRONTAGE_METRES + GAP_BETWEEN_COTTAGES_METRES
    count = max(FEWEST_COTTAGES, math.floor((length + GAP_BETWEEN_COTTAGES_METRES) / pitch))
    first = -HALF * (count - 1) * pitch
    return [first + index * pitch for index in range(count)]


def cottage(block: Block, along: float, across: float) -> list[Point]:
    alongTheRow = stepped(block.centre, block.along, along)
    middle = stepped(alongTheRow, block.across, across)
    halfFrontage = HALF * min(COTTAGE_FRONTAGE_METRES, block.length)
    halfDepth = HALF * min(COTTAGE_DEPTH_METRES, block.width)
    corners = []
    for alongSide, acrossSide in CORNER_SIDES:
        endOfTheFront = stepped(middle, block.along, alongSide * halfFrontage)
        corners.append(stepped(endOfTheFront, block.across, acrossSide * halfDepth))
    return corners


def cottagesOn(corners: list[Point]) -> list[list[Point]]:
    block = blockOf(corners)
    return [cottage(block, along, across) for across in rowOffsets(block) for along in placesAlong(block.length)]
