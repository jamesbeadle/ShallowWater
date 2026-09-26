"""Whether two cottages' footprints overlap. Both are convex, so they are apart exactly when the edge of one or the
other has every corner of the pair on its own side: the two outlines, seen along that edge's normal, do not meet.
"""
from __future__ import annotations

Point = tuple[float, float]


def normalsOf(corners: list[Point]) -> list[Point]:
    return [(end[1] - start[1], start[0] - end[0]) for start, end in zip(corners, corners[1:] + corners[:1])]


def shadow(corners: list[Point], axis: Point) -> tuple[float, float]:
    lengths = [east * axis[0] + north * axis[1] for east, north in corners]
    return min(lengths), max(lengths)


def isSeparatedAlong(first: list[Point], second: list[Point], axis: Point) -> bool:
    firstLow, firstHigh = shadow(first, axis)
    secondLow, secondHigh = shadow(second, axis)
    return firstHigh <= secondLow or secondHigh <= firstLow


def overlaps(first: list[Point], second: list[Point]) -> bool:
    axes = normalsOf(first) + normalsOf(second)
    return not any(isSeparatedAlong(first, second, axis) for axis in axes)
