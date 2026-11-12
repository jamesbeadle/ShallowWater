# This project

This repository's own instructions: its conventions, the commands that build and test it, the domain it serves, and any standing tasks or prompts that belong to this project and no other. `CLAUDE.md` loads this file and every session reads it after the kit's rules.

The project-process kit writes this file once and never touches it again, and `CLAUDE.md` is the kit's, replaced whole every time the bootstrap runs, so anything written there is lost. Write here instead. Where this file and the kit disagree about this project, this file wins, except that nothing here lifts the branch rule: the work still happens on a branch and ends as a pull request.

## The game

Shallow Water is a canal-based game. The story is being written from scratch; nothing from earlier versions of it carries forward. The story bible is `docs/story/`, starting at `docs/story/README.md`: it is the source of truth for the series title, the world, the protagonist, the demo and every chapter after it.

It must be playable on every platform: web, PC, Xbox and PlayStation.

## Engine

Unity 6 LTS, targeting WebGPU for the web build. It reaches every platform above from one project.

The game logic lives in plain C# classes that know nothing about Unity. Unity scripts only connect that logic to scenes, input and rendering. That boundary keeps a later move to Unreal a translation of the Unity layer, not a rewrite of the game.

## Brand

The interface is drawn in one brand, the title and the HUD alike. `docs/design/brand.md` gives every colour, typeface, spacing and timing its role, and what it is never for; the tokens themselves are in `Assets/Unity/Resources/Hud/Brand.uss`. A new screen or widget reads those tokens and is built from the pieces the sheet lists, and no other stylesheet states a colour, a typeface or a duration of its own. A look the brand does not yet name is added to the brand first, with its role, and then used. The palettes that light the world are not part of it: `docs/graphics.md` describes those.
