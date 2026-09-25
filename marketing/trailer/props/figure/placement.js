export function placeFigure(rig, position, heading) {
    const { root } = rig;
    root.position.copy(position);
    root.rotation.set(0, heading, 0);
    return rig;
}
