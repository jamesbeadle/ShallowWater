"""The pound Sparrow steers: the canal from Huddlesford Junction to Fazeley Junction as one line, north to south, and
the rest of the canals in the box as the water beyond it, so the two never lie on top of each other.
"""
from __future__ import annotations

from .canal_network import CanalNetwork
from .elements import flatPoints, placesOf
from .ground import groundPoint
from .layer_files import lineRecord

HUDDLESFORD_JUNCTION = groundPoint(-1.776, 52.684)
FAZELEY_JUNCTION = groundPoint(-1.699, 52.615)
CANAL_KIND = "canal"


def runsOffTheRoute(way: dict, routeEdges: set[frozenset[int]]) -> list[list[dict]]:
    nodes, places = way["nodes"], placesOf(way)
    runs = [[]]
    for index, edge in enumerate(zip(nodes, nodes[1:])):
        if frozenset(edge) in routeEdges:
            runs.append([])
            continue
        runs[-1] = runs[-1] or [places[index]]
        runs[-1].append(places[index + 1])
    return [run for run in runs if run]


class PoundRoute:
    def __init__(self, canalWays: list[dict]):
        self.canalWays = canalWays
        self.network = CanalNetwork(canalWays)
        start = self.network.junctionNear(HUDDLESFORD_JUNCTION)
        end = self.network.junctionNear(FAZELEY_JUNCTION)
        self.nodes = self.network.shortestRoute(start, end)

    def points(self) -> list[tuple[float, float]]:
        return [self.network.positions[node] for node in self.nodes]

    def line(self) -> dict:
        return lineRecord(CANAL_KIND, [coordinate for point in self.points() for coordinate in point])

    def canalsBeyond(self) -> list[dict]:
        routeEdges = {frozenset(edge) for edge in zip(self.nodes, self.nodes[1:])}
        runs = [run for way in self.canalWays for run in runsOffTheRoute(way, routeEdges)]
        return [lineRecord(CANAL_KIND, flatPoints(run)) for run in runs]
