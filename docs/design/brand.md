# Brand

Sources: the tokens in `Assets/Unity/Resources/Hud/Brand.uss`, the HUD and title stylesheets beside it that read them, the title's painted backdrop (`Assets/Unity/Resources/Shaders/TitleBackdrop.shader`), and the poster in `docs/art/poster.png` that the dusk tones were read from; read 12 November 2026.

*Shallow Water: Locked* is a working canal in the autumn of 1938, seen at dusk. The brand has two families of colour. The **dusk** is what a picture is painted in: the sky, the brick, the water. The **kit** is what Askew holds: oilskin, brass, enamel, paper and pencil. The title screen speaks in the dusk; the HUD in play speaks in the kit; ink is the words on both.

Everything below is a token in `Brand.uss`, declared once on `.hud-screen`. Every other stylesheet reads the tokens and none states a colour, a typeface or a duration of its own. The painted backdrop reads the seven dusk tones from the same sheet (`DuskLook` hands them to the shader), so a change to `--sunset` moves the wordmark and the painted sun together.

## Palette

| Token | Brand name | Role — use for | Never for |
| --- | --- | --- | --- |
| `--night` | Night | The darkest tone of a painted picture, and the curtain a whole screen fades through | Words; the surface of a card (that is oilskin) |
| `--umber` | Umber | Painted pictures only: near silhouettes, brick in shadow | The interface |
| `--oxblood` | Oxblood | Painted pictures only: the high sky, the far wood, the shade under cloud | The interface |
| `--brick` | Brick | Painted pictures only: the low sky, lit masonry, haze | The interface: it sits too near signal red to mean anything there |
| `--ember` | Ember | The one mark that says where you are on a title page, the dash beside the entry in focus; the sun's glow in a painted picture | Words; a second mark in the same view |
| `--sunset` | Sunset | The wordmark, a title page's heading, and the entry in focus on a title menu | The HUD in play, whose accent is brass; running text |
| `--glow` | Glow | The instant an entry is pressed; the hottest light in a painted picture: the sun's core, lamplight, glints on water | A resting state |
| `--ink` | Ink | Words on any dark ground | Words on paper or enamel |
| `--ink-soft` | Soft ink | Second-rank words on dark: a tagline, a hint's words, the detail under the entry in focus | Headings |
| `--ink-faint` | Faint ink | Third-rank words on dark: eyebrows, details at rest, footers | Anything the player must read to act |
| `--rule` | Rule | Hairlines between rows on oilskin | The edge of a surface |
| `--oilskin` | Oilskin | The surface of every pill and small card (`hud-glass`) | A full-screen backdrop |
| `--oilskin-deep` | Deep oilskin | The surface of a card that holds reading: the controls, speech | Pills |
| `--edge` | Edge | The border of an oilskin surface | Rules inside it |
| `--edge-lit` | Lit edge | The top border of an oilskin surface, as if lit from above | The other three sides |
| `--brass` | Brass | The HUD's one accent in play: ahead on the telegraph, and the mouse button or wheel a prompt asks for | Decoration; the title, whose accent is sunset |
| `--signal-red` | Signal red | Astern on the telegraph | Decoration; any meaning but astern |
| `--unlit` | Unlit | A telegraph segment or stop that is not lit | Words |
| `--key-face` | Key face | The face of a key cap | Any other surface |
| `--key-ring` | Key ring | The nickel ring of a key cap and the outline of the mouse | The border of a card |
| `--key-ink` | Key ink | The letter on a key cap | Other words |
| `--enamel` | Enamel | The clock's plate | Other surfaces |
| `--enamel-ink` | Enamel ink | The clock's figures | Words on dark |
| `--enamel-ink-soft` | Soft enamel ink | The clock's colon | Figures |
| `--enamel-rim` | Enamel rim | The border of the clock's plate | Other borders |
| `--paper` | Paper | Paper itself: the job label, the pocket book's pages, the light parts of a sketch. Also Askew's hand on a dark ground: the chapter name on the title | Words in the game's own voice |
| `--paper-card` | Paper card | A card tucked into the pocket book | Pages |
| `--paper-edge` | Paper edge | The border of anything paper | Oilskin |
| `--pencil` | Pencil | Everything written or drawn on paper | Words on dark |
| `--pencil-soft` | Soft pencil | Second-rank pencil: asides, section names | Headings on paper |
| `--pencil-faint` | Faint pencil | Notes, and the outline of a spent round | Anything the player must read to act |
| `--pencil-rule` | Pencil rule | Ruled lines on a page | Borders |
| `--oilcloth` | Oilcloth | The pocket book's cover | HUD cards (those are oilskin) |
| `--oilcloth-edge` | Oilcloth edge | The border of the cover | Other borders |
| `--gutter` | Gutter | The shadow down the book's spine | Other shadows |
| `--table` | Table | The dimmed world behind the open book | A surface |
| `--scope-field` | Scope field | The black round the telescopic sight | Other surfaces |
| `--scope-wire` | Scope wire | The sight's cross wires | Other lines |

## Type

Five faces, each with one voice. All are under the SIL Open Font Licence, kept beside them in `Assets/Unity/Resources/Hud/Fonts`.

| Token / class | Size · weight · line | Role |
| --- | --- | --- |
| `--face-headline` (League Gothic) | 204 the wordmark · 88 a page heading · 40 a menu entry · 28 a saved game or a credit's name; one weight, capitals, tracked 1.5 to 2 | The title's voice: a condensed jobbing printer's gothic, the lettering of a canal company's notices. The wordmark, page headings and menu entries. Never for running text, never in the HUD in play |
| `--face-stencil`, `--face-stencil-bold` (Stardos Stencil); class `hud-eyebrow` | 11 to 15 regular, capitals, tracked 2.5 to 4.5 · 24 to 36 bold | What is stencilled or stamped: eyebrows above a heading, a panel's title, the telegraph's order, section names in the book. Never for a sentence |
| `--face-hand`, `--face-hand-bold` (Caveat) | 25 to 28 on paper · 80 the chapter name · 44 light, the words on the curtain | Askew's own hand: everything pencilled in the pocket book and on the job label, the chapter name, *Casting off…*. Never for what the game says in its own voice |
| `--face-text`, `--face-text-medium`, `--face-text-bold` (Cabin); class `hud-text` | 15 to 22 · medium for prompts, speech and names · bold only on key caps, 12 to 16 | The reading face and the default of every label: prompts, speech, details, credits |
| `--face-figures` (Josefin Sans SemiBold); class `hud-display` | 32 on the clock | Figures that must not shift as they change |

A label is one line unless its sheet says otherwise (`hud-text` does not wrap; the credits do).

## Spacing

The screen is 1080 units tall at every size and shape (`HudLayout`, matched to the height), so a unit is the same share of the height everywhere and the width is what varies.

- **27**, the HUD's margin (`HudLayout.MarginShare`, 2.5% of the height): the safe area every HUD widget sits in.
- **132**, the title's margin (`--title-margin`): the left edge of its pages and both ends of its footer. The title is a poster, and takes a poster's margin.
- **10**, the gap between two pills in a row or a column.
- Inside a control the padding is the control's own, in its sheet.

## Radius · Elevation · Motion

**Radius.** A card is 6 (`hud-glass`). A pill is round: half its height. Paper is nearly square, 2 to 4. These are conventions the sheets follow, not tokens.

**Elevation.** There is none. Nothing casts a shadow: surfaces are told apart by tone, and an oilskin surface by its lit top edge.

**Motion.** Everything eases out (`ease-out-cubic`); only a whole screen leaving eases in (`ease-in-cubic`). A small thing slides 8 to 16 units as it arrives and a panel 24 to 28, never further. Words never change size and surfaces never change colour in motion.

| Token | Lasts | Role — what animates | Never for |
| --- | --- | --- | --- |
| `--quick` | 0.12 s | A change under the pointer or the thumb: focus moving to an entry, a press, the sight | Anything arriving |
| `--brisk` | 0.2 s | One thing arriving or leaving: a prompt, a hint, a card, a page of the title, the curtain drawn across it | A whole screen |
| `--unhurried` | 0.45 s | A whole screen giving way: the title fading to the world. `TitleScreen` waits the same 450 ms before it takes the title away, so the two change together | A response to a key |
| `--slow` | 0.8 s | The title's arrival and nothing else: the curtain lifting, the wordmark, then the menu and the footer | Anything the player is waiting on |

## Pieces every screen shares

| Piece | Made by | What it is |
| --- | --- | --- |
| `hud-text` | `OverlayElements.Words` | Any words: Cabin, ink, one line |
| `hud-eyebrow` | `OverlayElements.Eyebrow` | The stencilled line above a heading, and the version on the title |
| `hud-glass` | `OverlayElements.Glass` | The oilskin surface with its lit edge |
| `hud-prompt`, `hud-hint` | `PromptPill.Of`, `PromptPill.Hint` | A key and what it does, as a prompt in play or a standing hint in a row: the HUD's hints and the title's footer are the same pill |
| `hud-cap`, `hud-mouse` | `KeyGlyphs.For` | A key cap or the mouse |
| `title-button` | `TitleButton.Of` | An entry on a title page: words, an optional detail, and the ember mark when it is in focus |
| `title-page` | `TitlePage.Front`, `TitlePage.Inside` | The title's front page under the wordmark, or an inner page under its running head with Back at its foot. A new page (settings, say) is one `TitlePage.Inside` opened with `TitlePages.Open` |

## Unmapped

- **Tokens with no role in the interface.** `--umber`, `--oxblood` and `--brick` are read only by the painted backdrop.
- **Raw values that are not tokens.** Every type size (from 11 to 204), tracking, radius and padding is written in its component's sheet: there is no size or spacing scale yet. The title's arrival is staggered by two delays written in its sheets, 0.35 s for the menu and 0.6 s for the footer.
- **Colours outside this sheet.** The world is lit from its own palettes (`SkyPalette` and its neighbours in `Assets/Unity/Looks`, described in `docs/graphics.md`). They light a scene and are tuned by eye; the twilight sky there was matched to the poster, not to these tokens.
- **Keys only.** The hints show keyboard keys and the mouse, in play and on the title. There are no gamepad glyphs yet.
