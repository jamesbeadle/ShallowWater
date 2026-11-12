"""The old inns of the demo area, each placed by hand off the period map where the story and the villages put it: the
Plough at Huddlesford, the Red Lion and the Tame Otter at Hopwas bridge, the Dog and the Bell in Whittington and the
Three Tuns at Fazeley. Each stands on a plot wider than a house, fronting its nearest road or water at the set back,
and the rows of houses give way to it.
"""
from __future__ import annotations

from .period_map import pixelPoint
from .plane import Point, opposite, rightAngleClockwise
from .rows import ROW_DEPTH_METRES, row
from .set_backs import Obstacle, setBack
from .street_side import streetward

INNS = "inns"
INN_FRONTAGE_METRES = 16.0
HALF = 0.5


class Inn:
    def __init__(self, name: str, corners: list[Point]):
        self.name = name
        self.corners = corners


def plotFacing(anchor: Point, obstacles: list[Obstacle]) -> list[Point]:
    backwards = opposite(streetward(anchor, obstacles))
    along = opposite(rightAngleClockwise(backwards))
    return row(anchor, along, backwards, INN_FRONTAGE_METRES)


def standingInns(tracing: dict, obstacles: list[Obstacle]) -> list[Inn]:
    inns = []
    for feature in tracing.get(INNS, []):
        anchor = pixelPoint(*feature["pixel"])
        placed = setBack(plotFacing(anchor, obstacles), obstacles)
        if placed is None:
            print(f"  {feature['name']} could not stand back from the roads and water there, and is left out")
            continue
        inns.append(Inn(feature["name"], placed))
    return inns
