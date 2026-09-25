const neutralFinish = {
    contrast: 1,
    saturation: 1,
    lift: [0, 0, 0],
    gamma: [1, 1, 1],
    gain: [1, 1, 1],
    shadowTint: [1, 1, 1],
    highlightTint: [1, 1, 1],
    vignette: 0.4,
    grain: 0.04,
    aberration: 0.0018,
    fade: 0,
    flash: 0,
    scope: 0,
    glowColour: [0, 0, 0],
    glowCentre: [0.5, 0.5],
    glowRadius: 0.5,
};

const neutralLook = { exposure: 1, bloom: { strength: 0.4, radius: 0.5, threshold: 0.85 }, focus: null };

export const Grades = {
    dawn: {
        glowColour: [0.16, 0.08, 0.02], glowCentre: [0.0, 0.85], glowRadius: 0.75, contrast: 1.08, saturation: 1.0,
        lift: [0.025, 0.03, 0.05], gain: [1.03, 0.99, 0.93], shadowTint: [0.88, 0.97, 1.1], highlightTint: [1.1, 1.0, 0.86],
    },
    morning: { contrast: 1.05, saturation: 0.9, lift: [0.02, 0.025, 0.035], shadowTint: [0.92, 0.98, 1.06], highlightTint: [1.06, 1.01, 0.92] },
    night: { contrast: 1.12, saturation: 0.8, lift: [0.004, 0.012, 0.026], shadowTint: [0.8, 0.95, 1.2], highlightTint: [1.12, 0.98, 0.82] },
    firelight: { contrast: 1.15, saturation: 1.05, lift: [0.01, 0.005, 0.0], shadowTint: [0.85, 0.9, 1.1], highlightTint: [1.12, 0.96, 0.8] },
    black: { fade: 1 },
};

export function lookOf(gradeName, { exposure, bloom, focus, finish } = {}) {
    return {
        exposure: exposure ?? neutralLook.exposure,
        bloom: { ...neutralLook.bloom, ...bloom },
        focus: focus ?? neutralLook.focus,
        finish: { ...neutralFinish, ...Grades[gradeName], ...finish },
    };
}
