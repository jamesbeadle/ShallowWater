"""The whole soundtrack: every section rendered from the edit, laid in place, mastered and written to disk."""
import time

import numpy as np

from . import paths
from .mastering import master, wave_file
from .sections import render
from .synthesis import layering, timebase
from .timeline import edit_file


def rendered(edit) -> np.ndarray:
    track = np.zeros((2, timebase.samples_in(edit.duration)))
    for section in edit.sections:
        began = time.perf_counter()
        start, sound = render.render_section(edit, section)
        layering.laid_at(track, sound, start)
        print(f"{section.name:12s} {section.start:5.1f}-{section.end:5.1f} s rendered in {time.perf_counter() - began:5.1f} s")
    return track


def render_to_file() -> None:
    edit = edit_file.load(paths.EDIT_FILE)
    track = rendered(edit)
    print(f"Mixed: {master.described(track)}")
    finished = master.finished(track)
    wave_file.write(paths.SOUNDTRACK_FILE, finished)
    print(f"Wrote {paths.SOUNDTRACK_FILE}: {master.described(finished)}")
