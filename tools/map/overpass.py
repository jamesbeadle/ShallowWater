"""The one request to OpenStreetMap's Overpass API that every layer is cut from: the box asked for, widened to cover the
whole period map, and within it the water, the railways, the roads, the woods and the buildings of the four villages.
Public Overpass servers are often busy, so each is asked in turn, for a few rounds, until one answers in full.
"""
from __future__ import annotations

import json
import time
import urllib.parse
import urllib.request

from .period_map import Box, periodMapBox

INTERPRETERS = (
    "https://overpass-api.de/api/interpreter",
    "https://overpass.private.coffee/api/interpreter",
    "https://maps.mail.ru/osm/tools/overpass/api/interpreter",
)
PAUSES_BEFORE_EACH_ROUND_SECONDS = (0, 60, 180)
REQUESTED_BOX = Box(south=52.567, west=-1.868, north=52.685, east=-1.692)
ROAD_CLASSES = ("primary", "secondary", "tertiary", "unclassified")
VILLAGES = ("Hopwas", "Whittington", "Fazeley", "Huddlesford")
VILLAGE_RADIUS_METRES = 900
QUERY_TIMEOUT_SECONDS = 180
WAIT_FOR_ANSWER_SECONDS = 240
CUT_SHORT = "runtime error"
IDENTITY = "ShallowWater map regeneration (tools/map)"


class OverpassUnreachable(Exception):
    def __init__(self, failures: list[str]):
        super().__init__("\n".join(failures))


def fetchBox() -> Box:
    return REQUESTED_BOX.widenedTo(periodMapBox())


def anyOf(values: tuple[str, ...]) -> str:
    return "^(" + "|".join(values) + ")$"


def query() -> str:
    box = fetchBox()
    return "\n".join([
        f"[out:json][timeout:{QUERY_TIMEOUT_SECONDS}][bbox:{box.south},{box.west},{box.north},{box.east}];",
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


def askOnce(address: str, body: bytes) -> dict:
    request = urllib.request.Request(address, data=body, headers={"User-Agent": IDENTITY})
    with urllib.request.urlopen(request, timeout=WAIT_FOR_ANSWER_SECONDS) as response:
        answer = json.load(response)
    remark = answer.get("remark", "")
    if CUT_SHORT in remark:
        raise ValueError(f"the answer was cut short: {remark}")
    return answer


def firstAnswer(body: bytes, failures: list[str]) -> dict | None:
    for address in INTERPRETERS:
        print(f"Asking {address} ...", flush=True)
        try:
            return askOnce(address, body)
        except (OSError, ValueError) as failure:
            failures.append(f"{address}: {failure}")
            print(f"  no answer: {failure}", flush=True)
    return None


def fetch() -> dict:
    body = urllib.parse.urlencode({"data": query()}).encode()
    failures: list[str] = []
    for pause in PAUSES_BEFORE_EACH_ROUND_SECONDS:
        time.sleep(pause)
        answer = firstAnswer(body, failures)
        if answer is not None:
            return answer
    raise OverpassUnreachable(failures)
