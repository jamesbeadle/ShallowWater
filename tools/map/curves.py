"""Smooth curves through hand-traced points: centripetal Catmull-Rom, which passes through every traced point without
looping at a tight corner, sampled every few metres so a traced line runs round its bends instead of cutting them.
"""
from __future__ import annotations

import math

CENTRIPETAL = 0.5
MIRROR = 2


def knotAfter(time: float, start: tuple[float, float], end: tuple[float, float]) -> float:
    return time + math.dist(start, end) ** CENTRIPETAL


def blend(first: tuple[float, float], second: tuple[float, float], start: float, end: float, time: float) -> tuple:
    weight = (time - start) / (end - start)
    return first[0] + (second[0] - first[0]) * weight, first[1] + (second[1] - first[1]) * weight


def curvePoint(corners: list[tuple[float, float]], fraction: float) -> tuple[float, float]:
    before, start, end, after = corners
    startTime = knotAfter(0.0, before, start)
    endTime = knotAfter(startTime, start, end)
    afterTime = knotAfter(endTime, end, after)
    time = startTime + (endTime - startTime) * fraction
    towardsStart = blend(before, start, 0.0, startTime, time)
    along = blend(start, end, startTime, endTime, time)
    towardsAfter = blend(end, after, endTime, afterTime, time)
    first = blend(towardsStart, along, 0.0, endTime, time)
    second = blend(along, towardsAfter, startTime, afterTime, time)
    return blend(first, second, startTime, endTime, time)


def beyond(behind: tuple[float, float], last: tuple[float, float]) -> tuple[float, float]:
    return MIRROR * last[0] - behind[0], MIRROR * last[1] - behind[1]


def smoothThrough(points: list[tuple[float, float]], spacingMetres: float) -> list[tuple[float, float]]:
    padded = [beyond(points[1], points[0])] + points + [beyond(points[-2], points[-1])]
    curve = [points[0]]
    for index in range(1, len(padded) - 2):
        corners = padded[index - 1:index + 3]
        steps = max(1, math.ceil(math.dist(corners[1], corners[2]) / spacingMetres))
        curve += [curvePoint(corners, step / steps) for step in range(1, steps + 1)]
    return curve
