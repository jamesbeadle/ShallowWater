import { Vector2, Vector3 } from 'three';

const neutral = () => new Vector3(1, 1, 1);

export const FinishShader = {
    name: 'FinishShader',
    uniforms: {
        tDiffuse: { value: null },
        resolution: { value: new Vector2(1, 1) },
        time: { value: 0 },
        contrast: { value: 1 },
        saturation: { value: 1 },
        lift: { value: new Vector3() },
        gamma: { value: neutral() },
        gain: { value: neutral() },
        shadowTint: { value: neutral() },
        highlightTint: { value: neutral() },
        vignette: { value: 0 },
        grain: { value: 0 },
        aberration: { value: 0 },
        fade: { value: 0 },
        flash: { value: 0 },
        scope: { value: 0 },
        scopeRadius: { value: 0.46 },
        glowColour: { value: new Vector3() },
        glowCentre: { value: new Vector2(0.5, 0.5) },
        glowRadius: { value: 0.5 },
    },
    vertexShader: /* glsl */ `
        varying vec2 vUv;
        void main() {
            vUv = uv;
            gl_Position = projectionMatrix * modelViewMatrix * vec4(position, 1.0);
        }`,
    fragmentShader: /* glsl */ `
        uniform sampler2D tDiffuse;
        uniform vec2 resolution;
        uniform float time, contrast, saturation, vignette, grain, aberration, fade, flash, scope, scopeRadius;
        uniform vec3 lift, gamma, gain, shadowTint, highlightTint, glowColour;
        uniform vec2 glowCentre;
        uniform float glowRadius;
        varying vec2 vUv;
        const vec3 lumaWeights = vec3(0.2126, 0.7152, 0.0722);

        float hash(vec2 point) {
            return fract(sin(dot(point, vec2(12.9898, 78.233))) * 43758.5453);
        }

        vec3 sampleSplit(vec2 uv) {
            vec2 offset = (uv - 0.5) * aberration;
            return vec3(texture2D(tDiffuse, uv + offset).r, texture2D(tDiffuse, uv).g, texture2D(tDiffuse, uv - offset).b);
        }

        vec3 grade(vec3 color) {
            float luma = dot(color, lumaWeights);
            vec3 toned = mix(vec3(luma), color, saturation);
            toned = (toned - 0.5) * contrast + 0.5;
            toned *= mix(shadowTint, highlightTint, smoothstep(0.05, 0.85, luma));
            toned = lift + toned * (gain - lift);
            return pow(max(toned, 0.0), 1.0 / gamma);
        }

        vec2 pixelFromCentre() {
            return (vUv - 0.5) * resolution;
        }

        float reticle(vec2 pixel) {
            float crossbar = (1.0 - smoothstep(1.0, 2.2, abs(pixel.y))) * step(14.0, abs(pixel.x));
            float postWidth = mix(0.8, 6.0, clamp(-pixel.y / 70.0, 0.0, 1.0));
            float post = (1.0 - smoothstep(postWidth, postWidth + 1.2, abs(pixel.x))) * step(pixel.y, 0.0);
            return max(crossbar, post);
        }

        vec3 throughScope(vec3 color) {
            vec2 pixel = pixelFromCentre();
            float edge = length(pixel) / (scopeRadius * resolution.y);
            float opening = 1.0 - smoothstep(0.965, 1.0, edge);
            vec3 darkened = color * (1.0 - 0.6 * pow(edge, 4.0));
            vec3 marked = mix(darkened, vec3(0.01), reticle(pixel)) * opening;
            return mix(color, marked, scope);
        }

        void main() {
            vec3 color = grade(sampleSplit(vUv));
            vec2 fromGlow = (vUv - glowCentre) * vec2(resolution.x / resolution.y, 1.0);
            color += glowColour * exp(-dot(fromGlow, fromGlow) / (glowRadius * glowRadius));
            vec2 centred = vUv - 0.5;
            color *= 1.0 - vignette * dot(centred, centred) * 2.4;
            float speck = hash(vUv * resolution + fract(time * 7.13) * 311.0) - 0.5;
            color += grain * speck * (0.6 + 0.4 * (1.0 - dot(color, lumaWeights)));
            color = throughScope(color);
            color = mix(color, vec3(1.0), flash);
            color *= 1.0 - fade;
            gl_FragColor = vec4(clamp(color, 0.0, 1.0), 1.0);
        }`,
};
