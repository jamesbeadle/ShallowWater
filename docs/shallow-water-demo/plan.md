# Shallow Water Demo — Build Plan

The plan for the first game in the series. It follows the five-stage approach from [CLAUDE.md](../../CLAUDE.md): each stage is derived from the one above it, and nothing exists in a lower stage that isn't demanded by a higher one. The chain for a game reads: player stories → player experience → scene map → data structure → systems.

Stages 1–5 stay empty until [the story](../story.md) is written — the demo's chapter of the story is what the player stories are derived from.

## 1. Player Stories

*Every feature expressed as: as a player, I want X, so that Y. The full set defines the demo's scope — nothing more, nothing less. Derived from the demo's chapter of the story.*

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
