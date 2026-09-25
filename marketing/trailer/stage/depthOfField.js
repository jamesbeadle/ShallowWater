import { PerspectiveCamera, Scene } from 'three';
import { BokehPass } from 'three/addons/postprocessing/BokehPass.js';
import { setUniforms } from './uniforms.js';

export class DepthOfField {
    constructor() {
        this.pass = new BokehPass(new Scene(), new PerspectiveCamera(), {});
        this.pass.enabled = false;
    }

    focusOn(take, focus) {
        this.pass.enabled = Boolean(focus);
        if (!focus) {
            return;
        }
        this.pass.scene = take.scene;
        this.pass.camera = take.camera;
        setUniforms(this.pass.uniforms, { focus: focus.distance, aperture: focus.aperture, maxblur: focus.maximumBlur });
    }
}
