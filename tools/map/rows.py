"""Rows of houses on the building blocks of the 1900 map. The one-inch sheet draws a village's buildings as solid blocks
at about twice their true size, and runs a terrace into one block, so each block picked off it becomes a row of houses
along its length: a terrace front to the street, or two rows back to back when it is deep enough for both, with a yard
between. A long block is broken into rows with an entry between them, as village streets were.

A row is written as its four corners, front first and then round clockwise: the first side is the street front, the
second runs back from its right-hand end. The game splits each row into the houses that stand on it.
"""
from __future__ import annotations

import math
from typing import NamedTuple

from .plane import Point, centreOf, difference, dot, opposite, rightAngleClockwise, size, stepped, unit

ROW_DEPTH_METRES = 9.5
YARD_BETWEEN_ROWS_METRES = 4.0
LONGEST_ROW_METRES = 34.0
ENTRY_BETWEEN_ROWS_METRES = 3.0
SHORTEST_ROW_METRES = 5.5
ROWS_BACK_TO_BACK = 2
HALF = 0.5
NOTHING = 0.0


class Block(NamedTuple):
    centre: Point
    along: Point
    across: Point
    length: float
    width: float


def blockOf(corners: list[Point]) -> Block:
    shortSide, longSide = sorted((difference(corners[1], corners[0]), difference(corners[3], corners[0])), key=size)
    return Block(centreOf(corners), unit(longSide), unit(shortSide), size(longSide), size(shortSide))


def rowPlaces(block: Block, towardsTheStreet: Point) -> list[tuple[float, Point]]:
    awayFromTheStreet = opposite(towardsTheStreet)
    fitsTwoRows = block.width >= ROWS_BACK_TO_BACK * ROW_DEPTH_METRES + YARD_BETWEEN_ROWS_METRES
    if not fitsTwoRows:
        return [(0.0, awayFromTheStreet)]
    offset = HALF * (block.width - ROW_DEPTH_METRES)
    return [(offset, awayFromTheStreet), (-offset, towardsTheStreet)]


def rowSpans(length: float) -> list[tuple[float, float]]:
    rows = max(1, math.ceil((length + ENTRY_BETWEEN_ROWS_METRES) / (LONGEST_ROW_METRES + ENTRY_BETWEEN_ROWS_METRES)))
    rowLength = (length - (rows - 1) * ENTRY_BETWEEN_ROWS_METRES) / rows
    first = -HALF * (length - rowLength)
    return [(first + index * (rowLength + ENTRY_BETWEEN_ROWS_METRES), rowLength) for index in range(rows)]


def row(middle: Point, along: Point, backwards: Point, length: float) -> list[Point]:
    isBackOnTheRight = dot(rightAngleClockwise(along), backwards) > NOTHING
    lengthways = along if isBackOnTheRight else opposite(along)
    frontMiddle = stepped(middle, backwards, -HALF * ROW_DEPTH_METRES)
    frontLeft = stepped(frontMiddle, lengthways, -HALF * length)
    frontRight = stepped(frontMiddle, lengthways, HALF * length)
    return [frontLeft, frontRight, stepped(frontRight, backwards, ROW_DEPTH_METRES), stepped(frontLeft, backwards, ROW_DEPTH_METRES)]


def rowsOn(corners: list[Point], streetward: Point) -> list[list[Point]]:
    block = blockOf(corners)
    isAcrossTowardsTheStreet = dot(block.across, streetward) >= NOTHING
    towardsTheStreet = block.across if isAcrossTowardsTheStreet else opposite(block.across)
    rows = []
    for offset, backwards in rowPlaces(block, towardsTheStreet):
        acrossTheBlock = stepped(block.centre, towardsTheStreet, offset)
        rows += [row(stepped(acrossTheBlock, block.along, along), block.along, backwards, length) for along, length in rowSpans(block.length)]
    return rows


def halves(corners: list[Point]) -> list[list[Point]]:
    frontLeft, frontRight, backRight, backLeft = corners
    frontMiddle = stepped(frontLeft, difference(frontRight, frontLeft), HALF)
    backMiddle = stepped(backLeft, difference(backRight, backLeft), HALF)
    return [[frontLeft, frontMiddle, backMiddle, backLeft], [frontMiddle, frontRight, backRight, backMiddle]]


def frontageOf(corners: list[Point]) -> float:
    return size(difference(corners[1], corners[0]))
