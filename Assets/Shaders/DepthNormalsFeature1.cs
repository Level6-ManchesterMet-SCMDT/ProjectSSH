using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DepthNormalsFeature1 : ScriptableRendererFeature
{
    class Pass : ScriptableRenderPass
    {
        private Material material;
        private List<ShaderTagId> shaderTags;
        private FilteringSettings filteringSettings;
        private RenderTargetHandle destinationHandle;
        public Pass(Material material)
        {
            this.material = material;

            this.shaderTags = new List<ShaderTagId>()
            {
                new ShaderTagId("DepthOnly"),
                //new ShaderTagId("SRPDefaultUnlit"),
                //new ShaderTagId("UniversalForward"),
                //new ShaderTagId("LightweightForward"),

            };
            this.filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            destinationHandle.Init("_DepthNormalsTexture");
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(destinationHandle.id, cameraTextureDescriptor, FilterMode.Point);
            ConfigureTarget(destinationHandle.Identifier());
            ConfigureClear(ClearFlag.All, Color.black);
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var drawStettings = CreateDrawingSettings(shaderTags, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
            drawStettings.overrideMaterial = material;
            context.DrawRenderers(renderingData.cullResults, ref drawStettings, ref filteringSettings);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(destinationHandle.id);
        }

    }

    private Pass pass;

    /// <inheritdoc/>
    public override void Create()
    {
        Material material = CoreUtils.CreateEngineMaterial("Hidden/Internal-DepthNormalsTexture");
        this.pass = new Pass(material);

        // Configures where the render pass should be injected.
        pass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}


