import { Vector2 } from 'three';
import { EffectComposer } from 'three/addons/postprocessing/EffectComposer.js';
import { OutputPass } from 'three/addons/postprocessing/OutputPass.js';
import { RenderPass } from 'three/addons/postprocessing/RenderPass.js';
import { ShaderPass } from 'three/addons/postprocessing/ShaderPass.js';
import { SMAAPass } from 'three/addons/postprocessing/SMAAPass.js';
import { UnrealBloomPass } from 'three/addons/postprocessing/UnrealBloomPass.js';
import { FinishShader } from './finishShader.js';
import { setUniforms } from './uniforms.js';
import { DepthOfField } from './depthOfField.js';

export class PostProcessing {
    constructor(renderer, shape) {
        this.renderer = renderer;
        this.shadowMap = renderer.shadowMap;
        this.resolution = [shape.width, shape.pictureHeight];
        this.composer = new EffectComposer(renderer);
        this.renderPass = new RenderPass();
        this.depthOfField = new DepthOfField();
        this.bloomPass = new UnrealBloomPass(new Vector2(shape.width, shape.pictureHeight));
        this.finishPass = new ShaderPass(FinishShader);
        const passes = [this.renderPass, this.depthOfField.pass, this.bloomPass, new OutputPass(), new SMAAPass(), this.finishPass];
        passes.forEach((pass) => this.composer.addPass(pass));
    }

    show(take, look, time) {
        this.renderPass.scene = take.scene;
        this.renderPass.camera = take.camera;
        this.renderer.toneMappingExposure = look.exposure;
        this.depthOfField.focusOn(take, look.focus);
        Object.assign(this.bloomPass, look.bloom);
        setUniforms(this.finishPass.uniforms, { ...look.finish, time, resolution: this.resolution });
    }

    render() {
        this.shadowMap.needsUpdate = true;
        this.composer.render();
    }
}
