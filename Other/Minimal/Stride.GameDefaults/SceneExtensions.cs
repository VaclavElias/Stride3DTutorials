// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
namespace Stride.GameDefaults;

// Some code was taken from Stride.Assets.Entities, SceneBaseFactory
// Maybe we shall just move it to GameExtension.cs?
public static class SceneExtensions
{
    public const string SkyboxEntityName = "Skybox";
    public const string CameraEntityName = "Camera";
    public const string SunEntityName = "Directional light";

    /// <summary>
    /// Creates default skybox, camera, light (with shadows)
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public static void AddBaseEntities(this Scene? scene)
    {
        if (scene == null) return;

        //AddSkybox(scene);

        AddCamera(scene);

        AddLight(scene);
    }

    private static void AddSkybox(Scene? scene)
    {
        if (scene == null) return;

        var entity = new Entity(SkyboxEntityName) {
                new BackgroundComponent { Intensity = 1.0f },
                new LightComponent {
                    Intensity = 1.0f,
                    Type = new LightSkybox()}
        };

        entity.Transform.Position = new Vector3(0.0f, 2.0f, -2.0f);

        scene.Entities.Add(entity);
    }

    private static void AddCamera(Scene? scene)
    {
        if (scene == null) return;

        var entity = new Entity(CameraEntityName) { new CameraComponent {
            Projection = CameraProjectionMode.Perspective } };

        entity.Transform.Position = new(6, 6, 6);
        entity.Transform.Rotation = Quaternion.RotationYawPitchRoll(
            MathUtil.DegreesToRadians(45),
            MathUtil.DegreesToRadians(-30),
            MathUtil.DegreesToRadians(0));


        scene.Entities.Add(entity);
    }

    private static void AddLight(Scene? scene)
    {
        if (scene == null) return;

        var entity = new Entity(SunEntityName) { new LightComponent
            {
                Intensity =  20.0f,
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

        entity.Transform.Position = new Vector3(0, 2.0f, 0);
        entity.Transform.Rotation = Quaternion.RotationX(MathUtil.DegreesToRadians(-30.0f)) * Quaternion.RotationY(MathUtil.DegreesToRadians(-180.0f));

        scene.Entities.Add(entity);
    }
}