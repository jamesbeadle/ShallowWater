# Shallow Water: The Pound — announcement trailer

An 84-second cinematic trailer, rendered in 3D from code: every tree, boat, figure, lamp and raindrop is built procedurally in [three.js](https://threejs.org), each frame is rendered in headless Chromium, and the score and sound design are synthesised in Python. Nothing is hand-animated or sampled, so the trailer re-renders exactly the same every time and changes by editing the code.

The cut follows the 1938 story in `docs/story/`: the run through Hopwas Wood before the red flags go up, the engine hole, the three clients who hire Askew, *find it by day, use it once, leave by water*, the shot over Fazeley at night, the chain falling, and the Halcyon leaving under the bridge into the sunrise.

## Rendering it

From `marketing/trailer/`:

```sh
npm install                                      # three.js, the fonts and Playwright
pip install -r sound/requirements.txt            # numpy and scipy for the soundtrack
npm run sound                                    # output/soundtrack.wav
npm run frames -- --workers 2                    # output/frames/frame-00000.png … frame-02519.png
FFMPEG=/path/to/ffmpeg npm run video             # output/shallow-water-the-pound-trailer.mp4
```

The frames take a few seconds each on a CPU (Chromium renders with SwiftShader, so no GPU is needed); `--workers` shares them between browsers. To look at part of the trailer, render only that part:

```sh
npm run frames -- --shots redFlags,picketAtTheBridge   # every frame of those shots
npm run frames -- --times 19.5,23,45                    # single frames at those seconds
npm run frames -- --step 6                              # every sixth frame of the whole cut
```

`FFMPEG` can be any ffmpeg with libx264; `python3 -c "import imageio_ffmpeg; print(imageio_ffmpeg.get_ffmpeg_exe())"` prints one that pip can install.

## How it is put together

- `edit.json` is the edit: every shot, card and sound cue with its start and end. The pictures and the soundtrack both read it, so they cannot drift apart.
- `stage/` is the page Chromium opens: the renderer, the post-processing (bloom, depth of field, the grade, grain and letterbox) and the frame delivery.
- `shots/` holds one file per shot in the edit, registered in `shots/shotBuilders.js`. Each builds its scene, its camera move and its look.
- `sets/` are the two places the trailer is shot in, Hopwas and Fazeley, built from `world/` (canal, terrain, water, trees, sky, mist and light), `props/` (the narrowboats, figures, buildings, bridges, lorries, rifles, the chain) and `effects/` (rain, smoke, braziers, muzzle flash, lamp haze).
- `titles/` draws the cards: the opening super, the lines, the clients' name cards and subtitles, the three statements and the canal-signwritten title.
- `sound/` composes and mixes the soundtrack from the same edit.
- `render/` serves the stage, captures the frames and encodes the video with the soundtrack.
