"""The pound Sparrow steers: the canal from Huddlesford Junction to Fazeley Junction as one line, north to south,
found as the shortest way along the canal between the two places where three canals meet.
"""
from __future__ import annotations

import heapq
import math
from collections import defaultdict

from .elements import pointOf, placesOf
from .ground import groundPoint

HUDDLESFORD_JUNCTION = groundPoint(-1.776, 52.684)
FAZELEY_JUNCTION = groundPoint(-1.699, 52.615)
JUNCTION_SEARCH_METRES = 1000.0
CANALS_AT_A_JUNCTION = 3


class CanalNetwork:
    def __init__(self, canalWays: list[dict]):
        self.positions: dict[int, tuple[float, float]] = {}
        self.neighbours: dict[int, set[int]] = defaultdict(set)
        for way in canalWays:
            self.addWay(way["nodes"], placesOf(way))

    def addWay(self, nodes: list[int], places: list[dict]) -> None:
        for node, place in zip(nodes, places):
            self.positions[node] = pointOf(place)
        for first, second in zip(nodes, nodes[1:]):
            self.neighbours[first].add(second)
            self.neighbours[second].add(first)

    def distanceTo(self, node: int, point: tuple[float, float]) -> float:
        return math.dist(self.positions[node], point)

    def junctionNear(self, point: tuple[float, float]) -> int:
        nearby = [node for node in self.neighbours if self.distanceTo(node, point) <= JUNCTION_SEARCH_METRES]
        junctions = [node for node in nearby if len(self.neighbours[node]) >= CANALS_AT_A_JUNCTION]
        candidates = junctions or list(self.neighbours)
        return min(candidates, key=lambda node: self.distanceTo(node, point))

    def shortestRoute(self, start: int, end: int) -> list[int]:
        distances = {start: 0.0}
        previous: dict[int, int] = {}
        frontier = [(0.0, start)]
        while frontier:
            distance, node = heapq.heappop(frontier)
            for neighbour in self.neighbours[node]:
                reached = distance + math.dist(self.positions[node], self.positions[neighbour])
                if reached < distances.get(neighbour, math.inf):
                    distances[neighbour] = reached
                    previous[neighbour] = node
                    heapq.heappush(frontier, (reached, neighbour))
        return routeBack(previous, start, end)


def routeBack(previous: dict[int, int], start: int, end: int) -> list[int]:
    if end not in previous:
        raise SystemExit("No canal joins Huddlesford Junction to Fazeley Junction in the Overpass answer.")
    route = [end]
    while route[-1] != start:
        route.append(previous[route[-1]])
    return list(reversed(route))


def poundLine(canalWays: list[dict]) -> dict:
    network = CanalNetwork(canalWays)
    start = network.junctionNear(HUDDLESFORD_JUNCTION)
    end = network.junctionNear(FAZELEY_JUNCTION)
    route = network.shortestRoute(start, end)
    return {"kind": "canal", "points": [coordinate for node in route for coordinate in network.positions[node]]}
