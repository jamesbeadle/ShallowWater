export const Palette = {
    cream: 0xf1e4c4,
    parchment: 0xf6f0e2,
    signwritingRed: 0x9e201a,
    shadowRed: 0x4a0d0a,
    companyGreen: 0x1c402c,
    brass: 0xc9a052,
    ink: 0x0b0b0b,
    black: 0x000000,
};

export function cssColour(colour, opacity = 1) {
    const red = (colour >> 16) & 255;
    const green = (colour >> 8) & 255;
    const blue = colour & 255;
    return `rgba(${red}, ${green}, ${blue}, ${opacity})`;
}
