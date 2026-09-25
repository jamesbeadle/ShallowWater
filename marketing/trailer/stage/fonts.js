const fontFiles = "../node_modules/@fontsource";

export const Typefaces = {
    poster: 'Oswald',
    serif: 'Playfair Display',
    typewriter: 'Special Elite',
    signwriting: 'Alfa Slab One',
};

const faces = [
    [Typefaces.poster, 'oswald/files/oswald-latin-700-normal.woff2', { weight: '700' }],
    [Typefaces.poster, 'oswald/files/oswald-latin-400-normal.woff2', { weight: '400' }],
    [Typefaces.serif, 'playfair-display/files/playfair-display-latin-400-italic.woff2', { style: 'italic', weight: '400' }],
    [Typefaces.serif, 'playfair-display/files/playfair-display-latin-700-normal.woff2', { weight: '700' }],
    [Typefaces.typewriter, 'special-elite/files/special-elite-latin-400-normal.woff2', { weight: '400' }],
    [Typefaces.signwriting, 'alfa-slab-one/files/alfa-slab-one-latin-400-normal.woff2', { weight: '400' }],
];

async function loadFace([family, file, descriptors]) {
    const face = new FontFace(family, `url(${fontFiles}/${file})`, descriptors);
    await face.load();
    document.fonts.add(face);
}

export async function loadFonts() {
    await Promise.all(faces.map(loadFace));
}
