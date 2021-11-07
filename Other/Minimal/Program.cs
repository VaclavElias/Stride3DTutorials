using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Processors;
using Stride.Rendering;
using Stride.Rendering.Colors;
using Stride.Rendering.Lights;
using Stride.Rendering.ProceduralModels;

namespace Minimal
{
    static class Program
    {
        static void Main(string[] args)
        {
            using var game = new Game();

            Scene scene = new Scene();

            var cube = new CubeProceduralModel();

            var model = new Model();
            cube.Generate(game.Services, model);

            var entity = new Entity();
            entity.GetOrCreate<ModelComponent>().Model = model;

            var camera = new CameraComponent();
            camera.Projection = CameraProjectionMode.Perspective;
            var cameraEntity = new Entity();
            cameraEntity.Transform.Position = new Vector3(0, 25, 50);
            cameraEntity.Transform.Scale = new Vector3(1);
            cameraEntity.Transform.Rotation = new Quaternion(-0.34202012f, 0, 0, 0.9396926f);
            cameraEntity.Transform.Scale = new Vector3(1);
            cameraEntity.Add(camera);

            // Direction Light
            var directionalLightEntity = new Entity();
            directionalLightEntity.Transform.Position = new Vector3(11.803946f, 50.0f, 0.65027833f);
            directionalLightEntity.Transform.Rotation = new Quaternion(0.11697774f, 0.88302225f, 0.32139382f, 0.3213937f);
            directionalLightEntity.Transform.Scale = new Vector3(0);
            var directionalLightComponent = directionalLightEntity.GetOrCreate<LightComponent>();

            var light = new LightPoint();
            light.Color = new ColorRgbProvider(new Color(1, 1, 1));
            light.Radius = 200;

            directionalLightComponent.Type = light;
            directionalLightComponent.Intensity = 800;

            var skyboxEntity = new Entity();
            skyboxEntity.Transform.Position = new Vector3(0, 2, -2);
            skyboxEntity.Transform.Scale = new Vector3(1);
            var lightComponent = skyboxEntity.GetOrCreate<LightComponent>();

            var light2 = new LightSkybox();
            //light2.Color = new ColorRgbProvider(new Color(1, 1, 1));
            //light2.Radius = 200;

            lightComponent.Type = light2;
            //lightComponent.Intensity = 800;

            scene.Entities.Add(entity);
            scene.Entities.Add(directionalLightEntity);
            scene.Entities.Add(cameraEntity);
            scene.Entities.Add(skyboxEntity);

            game.SceneSystem.SceneInstance = new SceneInstance(game.Services, scene);

            //game.SceneSystem.SceneInstance.RootScene.Children.Add(scene);

            game.Run();
        }
    }
}
