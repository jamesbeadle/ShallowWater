function lowerIndex(samples, value) {
    let low = 0;
    let high = samples.length - 1;
    while (high - low > 1) {
        const middle = (low + high) >> 1;
        const isBelow = samples[middle] <= value;
        low = isBelow ? middle : low;
        high = isBelow ? high : middle;
    }
    return low;
}

function cornerHeights(heightAt, xs, acrosses, column, row) {
    const [x0, x1] = [xs[column], xs[column + 1]];
    const [a0, a1] = [acrosses[row], acrosses[row + 1]];
    return { h00: heightAt(x0, a0), h10: heightAt(x1, a0), h01: heightAt(x0, a1), h11: heightAt(x1, a1) };
}

export function createTerrainSampler(heightAt, xs, acrosses) {
    return (x, across) => {
        const column = lowerIndex(xs, x);
        const row = lowerIndex(acrosses, across);
        const u = (x - xs[column]) / (xs[column + 1] - xs[column]);
        const v = (across - acrosses[row]) / (acrosses[row + 1] - acrosses[row]);
        const { h00, h10, h01, h11 } = cornerHeights(heightAt, xs, acrosses, column, row);
        const isFirstTriangle = u + v <= 1;
        return isFirstTriangle ? h00 + u * (h10 - h00) + v * (h01 - h00) : h11 + (1 - u) * (h01 - h11) + (1 - v) * (h10 - h11);
    };
}
