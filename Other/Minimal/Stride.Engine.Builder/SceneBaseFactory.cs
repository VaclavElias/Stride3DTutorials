// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
namespace Stride.Engine.Builder;

// This is taken from Stride.Assets.Entities, SceneBaseFactory
public abstract class SceneBaseFactory
{
    public const string SkyboxEntityName = "Skybox";
    public const string CameraEntityName = "Camera";
    public const string SunEntityName = "Directional light";

    /// <summary>
    /// Creates default skybox, camera, light (with shadows)
    /// </summary>
    /// <param name="skyIntensity"></param>
    /// <param name="sunIntensity"></param>
    /// <returns></returns>
    protected static Scene CreateBase(float skyIntensity, float sunIntensity)
    {
        var skyboxEntity = new Entity(SkyboxEntityName) {
                new BackgroundComponent { Intensity = skyIntensity },
            };
        skyboxEntity.Transform.Position = new Vector3(0.0f, 2.0f, -2.0f);

        var cameraEntity = new Entity(CameraEntityName) { new CameraComponent { Projection = CameraProjectionMode.Perspective } };
        cameraEntity.Transform.Position = new(6, 6, 6);
        cameraEntity.Transform.Rotation = Quaternion.RotationYawPitchRoll(
            MathUtil.DegreesToRadians(45),
            MathUtil.DegreesToRadians(-30),
            MathUtil.DegreesToRadians(0));

        var lightEntity = new Entity(SunEntityName) { new LightComponent
            {
                Intensity = sunIntensity,
                Type = new LightDirectional
                {
                    Shadow =
                    {
                        Enabled = true,
                        Size = LightShadowMapSize.Large,
                        Filter = new LightShadowMapFilterTypePcf { FilterSize = LightShadowMapFilterTypePcfSize.Filter5x5 },
                    }
                }
            } };
        lightEntity.Transform.Position = new Vector3(0, 2.0f, 0);
        lightEntity.Transform.Rotation = Quaternion.RotationX(MathUtil.DegreesToRadians(-30.0f)) * Quaternion.RotationY(MathUtil.DegreesToRadians(-180.0f));

        var scene = new Scene();

        scene.Entities.Add(cameraEntity);
        scene.Entities.Add(lightEntity);
        scene.Entities.Add(skyboxEntity);

        return scene;
    }
}

public class SceneHDRFactory : SceneBaseFactory
{
    private const float SkyIntensity = 1.0f;
    private const float SunIntensity = 20.0f;

    public static Scene Create()
    {
        var sceneAsset = CreateBase(SkyIntensity, SunIntensity);

        // Add a sky light to the scene
        var skyboxEntity = sceneAsset.Entities.Single(x => x.Name == SkyboxEntityName);

        skyboxEntity.Add(new LightComponent
        {
            Intensity = 1.0f,
            Type = new LightSkybox(),
        });

        return sceneAsset;
    }
}

