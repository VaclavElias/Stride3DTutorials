namespace Stride.Engine.Builder;

// Credit https://github.com/IceReaper/StrideTest
public static class GraphicsCompositorBuilder
{
    public static GraphicsCompositor Create()
    {
        var opaqueRenderStage = new RenderStage("Opaque", "Main") { SortMode = new StateChangeSortMode() };
        var transparentRenderStage = new RenderStage("Transparent", "Main") { SortMode = new BackToFrontSortMode() };
        var shadowCasterRenderStage = new RenderStage("ShadowMapCaster", "ShadowMapCaster") { SortMode = new FrontToBackSortMode() };
        var shadowCasterCubeMapRenderStage = new RenderStage("ShadowMapCasterCubeMap", "ShadowMapCasterCubeMap") { SortMode = new FrontToBackSortMode() };

        var shadowCasterParaboloidRenderStage =
            new RenderStage("ShadowMapCasterParaboloid", "ShadowMapCasterParaboloid") { SortMode = new FrontToBackSortMode() };

        var postProcessingEffects = new PostProcessingEffects();
        postProcessingEffects.DisableAll();

        var renderer = new ForwardRenderer
        {
            Clear = { Color = Color.Black },
            OpaqueRenderStage = opaqueRenderStage,
            TransparentRenderStage = transparentRenderStage,
            ShadowMapRenderStages = { shadowCasterRenderStage, shadowCasterParaboloidRenderStage, shadowCasterCubeMapRenderStage },
            PostEffects = postProcessingEffects
        };

        var cameraSlot = new SceneCameraSlot { Name = "Main" };

        return new()
        {
            Cameras = { cameraSlot },
            RenderStages =
                {
                    opaqueRenderStage,
                    transparentRenderStage,
                    shadowCasterRenderStage,
                    shadowCasterParaboloidRenderStage,
                    shadowCasterCubeMapRenderStage
                },
            RenderFeatures =
                {
                    new MeshRenderFeature
                    {
                        RenderFeatures =
                        {
                            new TransformRenderFeature(),
                            new SkinningRenderFeature(),
                            new MaterialRenderFeature(),
                            new ShadowCasterRenderFeature(),
                            new ForwardLightingRenderFeature
                            {
                                LightRenderers =
                                {
                                    new LightAmbientRenderer(),
                                    new LightSkyboxRenderer(),
                                    new LightDirectionalGroupRenderer(),
                                    new LightPointGroupRenderer(),
                                    new LightSpotGroupRenderer(),
                                    new LightClusteredPointSpotGroupRenderer()
                                },
                                ShadowMapRenderer = new ShadowMapRenderer
                                {
                                    Renderers =
                                    {
                                        new LightDirectionalShadowMapRenderer { ShadowCasterRenderStage = shadowCasterRenderStage },
                                        new LightSpotShadowMapRenderer { ShadowCasterRenderStage = shadowCasterRenderStage },
                                        new LightPointShadowMapRendererParaboloid
                                        {
                                            ShadowCasterRenderStage = shadowCasterParaboloidRenderStage
                                        },
                                        new LightPointShadowMapRendererCubeMap
                                        {
                                            ShadowCasterRenderStage = shadowCasterCubeMapRenderStage
                                        }
                                    }
                                }
                            }
                        },
                        RenderStageSelectors =
                        {
                            new MeshTransparentRenderStageSelector
                            {
                                EffectName = "StrideForwardShadingEffect",
                                OpaqueRenderStage = opaqueRenderStage,
                                TransparentRenderStage = transparentRenderStage,
                                RenderGroup = RenderGroupMask.All
                            },
                            new ShadowMapRenderStageSelector
                            {
                                EffectName = "StrideForwardShadingEffect.ShadowMapCaster",
                                ShadowMapRenderStage = shadowCasterRenderStage,
                                RenderGroup = RenderGroupMask.All
                            },
                            new ShadowMapRenderStageSelector
                            {
                                EffectName = "StrideForwardShadingEffect.ShadowMapCasterParaboloid",
                                ShadowMapRenderStage = shadowCasterParaboloidRenderStage,
                                RenderGroup = RenderGroupMask.All
                            },
                            new ShadowMapRenderStageSelector
                            {
                                EffectName = "StrideForwardShadingEffect.ShadowMapCasterCubeMap",
                                ShadowMapRenderStage = shadowCasterCubeMapRenderStage,
                                RenderGroup = RenderGroupMask.All
                            }
                        },
                        PipelineProcessors =
                        {
                            new MeshPipelineProcessor { TransparentRenderStage = transparentRenderStage },
                            new ShadowMeshPipelineProcessor { ShadowMapRenderStage = shadowCasterRenderStage },
                            new ShadowMeshPipelineProcessor { ShadowMapRenderStage = shadowCasterParaboloidRenderStage, DepthClipping = true },
                            new ShadowMeshPipelineProcessor { ShadowMapRenderStage = shadowCasterCubeMapRenderStage, DepthClipping = true }
                        }
                    },
                    new ParticleEmitterRenderFeature
                    {
                        RenderStageSelectors =
                        {
                            new ParticleEmitterTransparentRenderStageSelector
                            {
                                OpaqueRenderStage = opaqueRenderStage, TransparentRenderStage = transparentRenderStage
                            }
                        }
                    },
                    new UIRenderFeature
                    {
                        RenderStageSelectors =
                        {
                            new SimpleGroupToRenderStageSelector { RenderStage = transparentRenderStage, EffectName = "Test" }
                        }
                    },
                    new BackgroundRenderFeature
                    {
                        RenderStageSelectors =
                        {
                            new SimpleGroupToRenderStageSelector { RenderStage = transparentRenderStage, EffectName = "Test" }
                        }
                    }
                },
            Game = new SceneCameraRenderer { Child = renderer, Camera = cameraSlot },
            Editor = renderer,
            SingleView = renderer
        };
    }
}

