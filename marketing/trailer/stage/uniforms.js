export function setUniforms(uniforms, values) {
    for (const [name, value] of Object.entries(values)) {
        uniforms[name].value = value;
    }
}
