# Shallow Water

The master repository for the Shallow Water game series. Series-wide story, world, and planning documents live here, alongside the source and build files for each game in the series.

## The Series

Shallow Water is a series of open-world 3D games built in Unity, set on the canals of Varana — a modern island nation whose waterways are run by a corrupt private consortium, the Charter, and lived on by the people it calls bandits. The player is Wren, a lone narrowboater drawn into vigilante justice on behalf of the community that took him in. The story documentation is the single source of truth for every game in the series — see [docs/story.md](docs/story.md).

| Game | Status | Location |
| --- | --- | --- |
| Shallow Water Demo | Planning | [games/shallow-water-demo](games/shallow-water-demo) |

## Repository Layout

```
docs/                       Series-wide story, world, and planning documents
  story.md                  The Shallow Water story — source of truth for the series
  shallow-water-demo/       Planning documents for the first game
    plan.md                 Build plan: stories, scope, milestones
games/
  shallow-water-demo/       Unity project for the first game
CLAUDE.md                   Code style and working methodology for this repository
```

## Engine

All games in the series are built with **Unity 6.3 LTS** (3D, Universal Render Pipeline). Each game is a self-contained Unity project under `games/`; open the game folder — not the repository root — in Unity Hub.

## Working on This Repository

Read [CLAUDE.md](CLAUDE.md) before writing any code. It defines how code in this repository is written: the five-stage approach (user stories → UX → site map → data → systems), naming rules, file-size limits, and the prose-first philosophy.

The flow for any new feature:

1. Express it as a player story in the game's plan document.
2. Design the player experience it delivers.
3. Trace it through the scene map, data, and systems.
4. Only then write code.
