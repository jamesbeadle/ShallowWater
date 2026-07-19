# Shallow Water Demo — Build Plan

The plan for the first game in the series. It follows the five-stage approach from [CLAUDE.md](../../CLAUDE.md): each stage is derived from the one above it, and nothing exists in a lower stage that isn't demanded by a higher one. The chain for a game reads: player stories → player experience → scene map → data structure → systems.

The demo tells the "Tapley Junction" chapter of [the story](../story.md): Wren versus District Steward Voss, ending with the impounded boats freed and the Charter's wax stamp revealed.

## 1. Player Stories

Every feature expressed as: as a player, I want X, so that Y. This set defines the demo's scope — nothing more, nothing less. Each story traces to the Tapley Junction chapter.

1. As a player, I want to pilot *Last Orders* along the Cut — steering, mooring, and working locks — so that travelling the water is itself the play.
2. As a player, I want to step off the boat and explore Tapley Junction on foot, so that the world is a place and not a corridor.
3. As a player, I want to take missions from Maud and the junction folk, so that the story advances through work I choose to do.
4. As a player, I want to earn money from odd jobs and challenges — cargo runs, breakdown rescues, time trials through the locks — so that progress comes from living on the water.
5. As a player, I want to spend my earnings doing up *Last Orders* — paint, engine, fittings — so that the boat reflects my time in the world.
6. As a player, I want to run sabotage and heist missions against Voss's stewards, so that vigilante justice is something I do, not something I watch.
7. As a player, I want the junction community to respond to what I've done for them, so that belonging is earned and visible.
8. As a player, I want to finish the chapter — Voss ruined, the boats freed, the wax stamp found — so that the demo tells a complete story and promises a bigger one.

## 2. Player Experience

*Each story delivered through a concrete moment of play: what the player sees, what they press, what responds. Known patterns, no invention for invention's sake.*

## 3. Scene Map

*Stories don't live in isolation. Every story maps to a scene or a moment within one; the scenes map to each other — where play enters, what leads where. Any scene no story reaches shouldn't exist; any story no scene delivers is a gap.*

## 4. Data Structure

*The play reveals the domain. Entities and their properties fall out of what the unified set of scenes demands. Derive what's derivable; store what isn't.*

## 5. Systems

*By this point the hard decisions are made. Systems translate the data structure into the play the scenes demand. Named intentions, one responsibility each, small surfaces.*

## Technical Setup

- **Engine:** Unity 6.3 LTS, 3D, Universal Render Pipeline.
- **Project location:** `games/shallow-water-demo/` — created via Unity Hub with the 3D (URP) template, into that folder.
- **Version control:** the repository root `.gitignore` and `.gitattributes` already cover Unity projects under `games/`. In Unity: Edit → Project Settings → Editor → set Asset Serialization to *Force Text* and Version Control to *Visible Meta Files* (both are Unity's defaults, worth confirming).

## Milestones

| Milestone | Definition of done |
| --- | --- |
| 0 — Repository ready | Monorepo skeleton, docs, Unity-ready git configuration. |
| 1 — Story documented | `docs/story.md` written; demo chapter defined. |
| 2 — Plan complete | Stages 1–5 above filled in and traced to the story. |
| 3 — Unity project created | Empty URP project committed under `games/shallow-water-demo/`. |
| 4 — Greybox | The scene map playable with placeholder geometry. |
| 5 — Vertical slice | One story polished end-to-end. |
| 6 — Demo | Every player story playable; the demo's chapter told. |
