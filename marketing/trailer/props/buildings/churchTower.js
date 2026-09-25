import { BoxGeometry, Group, Mesh } from 'three';
import { applyWorldUvs } from '../../world/worldUvs.js';
import { buildHouse } from './house.js';

const Tower = { width: 6.5, height: 23, merlon: 0.9, merlonHeight: 1.1 };
const Nave = { width: 22, depth: 9.5, storeys: 1, storeyHeight: 7.5, pitch: 0.6, bays: 5, chimneys: [] };

function battlements(material) {
    const merlons = [];
    const step = Tower.width / 4;
    for (let index = 0; index <= 4; index += 1) {
        const offset = -Tower.width / 2 + index * step;
        [[offset, Tower.width / 2], [offset, -Tower.width / 2], [Tower.width / 2, offset], [-Tower.width / 2, offset]].forEach(([x, z]) => {
            const merlon = new Mesh(new BoxGeometry(Tower.merlon, Tower.merlonHeight, Tower.merlon), material);
            merlon.position.set(x, Tower.height + Tower.merlonHeight / 2, z);
            merlons.push(merlon);
        });
    }
    return merlons;
}

export function buildChurch(materials) {
    const church = new Group();
    const tower = new Mesh(applyWorldUvs(new BoxGeometry(Tower.width, Tower.height, Tower.width), 2.4), materials.stone);
    tower.position.setY(Tower.height / 2);
    tower.castShadow = true;
    const nave = buildHouse(Nave, materials);
    nave.position.set(Tower.width / 2 + Nave.width / 2, 0, 0);
    church.add(tower, nave, ...battlements(materials.stone));
    return { church, towerTop: Tower.height };
}
