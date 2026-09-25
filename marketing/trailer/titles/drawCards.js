import { drawButton } from './buttonCard.js';
import { drawLine } from './lineCard.js';
import { drawName } from './nameCard.js';
import { drawPlatforms } from './platformsCard.js';
import { drawStatement } from './statementCard.js';
import { drawSubtitle } from './subtitleCard.js';
import { drawSuper } from './superCard.js';
import { drawTitle } from './titleCard.js';

const drawersByStyle = {
    super: drawSuper,
    line: drawLine,
    name: drawName,
    subtitle: drawSubtitle,
    statement: drawStatement,
    title: drawTitle,
    platforms: drawPlatforms,
    button: drawButton,
};

export function drawCards(context, shape, cards, time) {
    cards.forEach((card) => drawersByStyle[card.style](context, shape, card, time));
}
