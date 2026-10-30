
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class PixelationRendererFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public Material material;

        [Range(1, 16)]
        public int pixelSize = 4;
    }

    public Settings settings = new Settings();

    private PixelationPass _pass;

    public override void Create()
    {
        _pass = new PixelationPass(settings);
        _pass.renderPassEvent =
            RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void AddRenderPasses(
        ScriptableRenderer renderer,
        ref RenderingData renderingData)
    {
        if (settings.material == null)
            return;

        if (renderingData.cameraData.cameraType != CameraType.Game)
            return;

        renderer.EnqueuePass(_pass);
    }

    private class PixelationPass : ScriptableRenderPass
    {
        private readonly Settings _settings;

        private class PassData
        {
            public TextureHandle source;
            public Material material;
            public int pixelSize;
        }

        public PixelationPass(Settings settings)
        {
            _settings = settings;
        }

        public override void RecordRenderGraph(
            RenderGraph renderGraph,
            ContextContainer frameData)
        {
            if (_settings.material == null)
                return;

            UniversalResourceData resources =
                frameData.Get<UniversalResourceData>();

            UniversalCameraData cameraData =
                frameData.Get<UniversalCameraData>();

            if (resources.isActiveTargetBackBuffer)
                return;

            TextureHandle source =
                resources.activeColorTexture;

            var descriptor =
                renderGraph.GetTextureDesc(source);

            descriptor.name = "_PixelationTemporary";
            descriptor.clearBuffer = false;
            descriptor.depthBufferBits = 0;
            descriptor.msaaSamples = MSAASamples.None;

            TextureHandle destination =
                renderGraph.CreateTexture(descriptor);

            using (var builder =
                renderGraph.AddRasterRenderPass<PassData>(
                    "Pixelation Pass",
                    out var passData))
            {
                passData.source = source;
                passData.material = _settings.material;
                passData.pixelSize = _settings.pixelSize;

                builder.UseTexture(
                    source,
                    AccessFlags.Read
                );

                builder.SetRenderAttachment(
                    destination,
                    0,
                    AccessFlags.Write
                );

                builder.SetRenderFunc(
                    (PassData data, RasterGraphContext context) =>
                    {
                        data.material.SetFloat(
                            "_PixelSize",
                            data.pixelSize
                        );

                        Blitter.BlitTexture(
                            context.cmd,
                            data.source,
                            new Vector4(1, 1, 0, 0),
                            data.material,
                            0
                        );
                    }
                );
            }

            resources.cameraColor = destination;
        }
    }
}