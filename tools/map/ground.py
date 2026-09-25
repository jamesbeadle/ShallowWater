"""The flat ground the game stands on: metres east and north of Hopwas bridge, where the Lichfield to Tamworth road
crosses the canal. Every layer the game reads is written in these metres, so nothing in the game knows about latitude.
"""
from __future__ import annotations

import math

EARTH_RADIUS_METRES = 6378137.0
HOPWAS_BRIDGE_LONGITUDE = -1.7373
HOPWAS_BRIDGE_LATITUDE = 52.6433
DECIMETRE_PLACES = 1


def metresEast(longitude: float) -> float:
    parallelRadius = EARTH_RADIUS_METRES * math.cos(math.radians(HOPWAS_BRIDGE_LATITUDE))
    return math.radians(longitude - HOPWAS_BRIDGE_LONGITUDE) * parallelRadius


def metresNorth(latitude: float) -> float:
    return math.radians(latitude - HOPWAS_BRIDGE_LATITUDE) * EARTH_RADIUS_METRES


def toDecimetres(metres: float) -> float:
    return round(metres, DECIMETRE_PLACES)


def groundPoint(longitude: float, latitude: float) -> tuple[float, float]:
    return toDecimetres(metresEast(longitude)), toDecimetres(metresNorth(latitude))
