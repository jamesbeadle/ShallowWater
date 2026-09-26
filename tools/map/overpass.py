"""The one request to OpenStreetMap's Overpass API that every layer is cut from: the box asked for, widened to cover the
whole period map, and within it the water, the railways, the roads, the woods and the buildings of the four villages.
"""
from __future__ import annotations

import json
import urllib.parse
import urllib.request

from .period_map import Box, periodMapBox

INTERPRETER_ADDRESS = "https://overpass-api.de/api/interpreter"
REQUESTED_BOX = Box(south=52.567, west=-1.868, north=52.685, east=-1.692)
ROAD_CLASSES = ("primary", "secondary", "tertiary", "unclassified")
VILLAGES = ("Hopwas", "Whittington", "Fazeley", "Huddlesford")
VILLAGE_RADIUS_METRES = 900
TIMEOUT_SECONDS = 300
IDENTITY = "ShallowWater map regeneration (tools/map)"


def fetchBox() -> Box:
    return REQUESTED_BOX.widenedTo(periodMapBox())


def anyOf(values: tuple[str, ...]) -> str:
    return "^(" + "|".join(values) + ")$"


def query() -> str:
    box = fetchBox()
    return "\n".join([
        f"[out:json][timeout:{TIMEOUT_SECONDS}][bbox:{box.south},{box.west},{box.north},{box.east}];",
        f'node["place"]["name"~"{anyOf(VILLAGES)}"]->.villages;',
        "(",
        'way["waterway"~"^(canal|river)$"];',
        'way["railway"="rail"];',
        f'way["highway"~"{anyOf(ROAD_CLASSES)}"];',
        'way["natural"="wood"];',
        'way["landuse"="forest"];',
        'relation["natural"="wood"];',
        'relation["landuse"="forest"];',
        f'way["building"](around.villages:{VILLAGE_RADIUS_METRES});',
        ");",
        "out geom;",
    ])


def fetch() -> dict:
    body = urllib.parse.urlencode({"data": query()}).encode()
    request = urllib.request.Request(INTERPRETER_ADDRESS, data=body, headers={"User-Agent": IDENTITY})
    with urllib.request.urlopen(request, timeout=TIMEOUT_SECONDS) as response:
        return json.load(response)
