import { ShaderChunk } from 'three';
import { shaderNoise } from './shaderNoise.js';

export const waterFragmentShader = /* glsl */ `
    uniform sampler2D tDiffuse;
    uniform sampler2D normalMap;
    uniform float time, rippleScale, rippleStrength, distortion, baseReflectance, wakeHeading, glintStrength;
    uniform vec3 bodyColour, sunDirection, sunColour, wakeOrigin;
    varying vec4 mirrorCoordinates;
    varying vec3 vWorldPosition;
    ${ShaderChunk.fog_pars_fragment}
    ${shaderNoise}

    vec2 rippleTilt(vec2 place) {
        vec2 first = texture2D(normalMap, place * rippleScale + vec2(time * 0.011, time * 0.007)).rg;
        vec2 second = texture2D(normalMap, place * rippleScale * 0.37 - vec2(time * 0.006, -time * 0.009)).rg;
        return (first + second * 0.8 - 0.9) * 2.0 * rippleStrength;
    }

    vec3 wakeOf(vec2 place) {
        vec2 forward = vec2(cos(wakeHeading), -sin(wakeHeading));
        vec2 right = vec2(-forward.y, forward.x);
        vec2 relative = place - wakeOrigin.xz;
        float behind = -dot(relative, forward);
        float side = dot(relative, right);
        float isBehind = step(0.0, behind);
        float arm = abs(abs(side) - 0.34 * behind);
        float spread = 0.45 + 0.035 * behind;
        float armStrength = exp(-(arm * arm) / (spread * spread)) * exp(-behind / 60.0) * isBehind;
        float wave = sin(behind * 2.6 - time * 5.0 + abs(side) * 1.7);
        float wash = exp(-(side * side) / 1.6) * exp(-behind / 16.0) * isBehind;
        float churn = noiseAt(vec3(place * 1.7, time * 1.3)) - 0.5;
        vec2 tilt = right * sign(side) * wave * armStrength * 0.5 + vec2(churn, -churn) * wash * 1.4;
        return vec3(tilt, wash * (churn + 0.5));
    }

    void main() {
        vec3 wake = wakeOf(vWorldPosition.xz);
        vec2 tilt = rippleTilt(vWorldPosition.xz) + wake.xy;
        vec3 normal = normalize(vec3(tilt.x, 1.0, tilt.y));
        vec3 toEye = normalize(cameraPosition - vWorldPosition);
        float fresnel = baseReflectance + (1.0 - baseReflectance) * pow(1.0 - max(dot(toEye, normal), 0.0), 5.0);
        vec4 shifted = mirrorCoordinates + vec4(tilt * distortion * mirrorCoordinates.w, 0.0, 0.0);
        vec3 reflection = texture2DProj(tDiffuse, shifted).rgb;
        float glint = pow(max(dot(normal, normalize(sunDirection + toEye)), 0.0), 400.0);
        vec3 foam = vec3(0.16, 0.15, 0.13) * wake.z;
        gl_FragColor = vec4(mix(bodyColour + foam, reflection, fresnel) + sunColour * glint * glintStrength, 1.0);
        ${ShaderChunk.fog_fragment}
    }`;
