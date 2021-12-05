using Stride.Core.Diagnostics;
using Stride.Core.Mathematics;
using Stride.Engine.Processors;
using Stride.Rendering;
using Stride.Rendering.Colors;
using Stride.Rendering.Lights;
using Stride.Rendering.ProceduralModels;

namespace Stride.Engine.Builder
{
    public class MinimalGame2 : Game
    {
        public MinimalGame2()
        {
            //GameStarted += MyGame2_GameStarted;
        }

        protected override void BeginRun()
        {
            this.Window.AllowUserResizing = true;

            this.SceneSystem.GraphicsCompositor = GraphicsCompositorBuilder.Create();

            this.SceneSystem.SceneInstance = new(this.Services, new());
            this.SceneSystem.SceneInstance.RootScene.Entities.Add(GetCamera(this.SceneSystem));

            var scene = this.SceneSystem.SceneInstance.RootScene;

            //scene.Entities.Add(GetCamera(SceneSystem));
            //scene.Entities.Add(GetLight());
            scene.Entities.Add(GetAmbientLight());

            var model = new Model();
            var cube = new CubeProceduralModel();

            cube.Generate(Services, model);

            var cubeEntity = new Entity();
            cubeEntity.Transform.Scale = new Vector3(1);
            cubeEntity.Transform.Position = new Vector3(1);

            cubeEntity.GetOrCreate<ModelComponent>().Model = model;

            //cubeEntity.Add(new TestComponent());

            scene.Entities.Add(cubeEntity);
        }

        private void MyGame2_GameStarted(object? sender, EventArgs e)
        {
            Log.Warning("Hello");

            SceneSystem.GraphicsCompositor = GraphicsCompositorBuilder.Create();

            var scene = new Scene();

            SceneSystem.SceneInstance = new SceneInstance(Services, scene);

            scene.Entities.Add(GetCamera(SceneSystem));
            scene.Entities.Add(GetLight());
            scene.Entities.Add(GetAmbientLight());

            var model = new Model();
            var cube = new CubeProceduralModel();

            cube.Generate(Services, model);

            var cubeEntity = new Entity();
            cubeEntity.Transform.Scale = new Vector3(1);
            cubeEntity.Transform.Position = new Vector3(1);

            cubeEntity.GetOrCreate<ModelComponent>().Model = model;

            //cubeEntity.Add(new TestComponent());

            scene.Entities.Add(cubeEntity);


            //SceneSystem.SceneInstance.RootScene.Entities.Add(cubeEntity);
        }

        public Entity GetCamera(SceneSystem sceneSystem)
        {
            var camera = new CameraComponent();
            camera.Projection = CameraProjectionMode.Perspective;
            camera.Slot = sceneSystem.GraphicsCompositor.Cameras[0].ToSlotId();

            var cameraEntity = new Entity();
            cameraEntity.Transform.Position = new(6, 6, 6);

            cameraEntity.Transform.Rotation = Quaternion.RotationYawPitchRoll(
                MathUtil.DegreesToRadians(45),
                MathUtil.DegreesToRadians(-30),
                MathUtil.DegreesToRadians(0)
            );
            //cameraEntity.Transform.Position = new Vector3(0, 25, 50);
            //cameraEntity.Transform.Scale = new Vector3(1);
            //cameraEntity.Transform.Rotation = new Quaternion(-0.34202012f, 0, 0, 0.9396926f);
            //cameraEntity.Transform.Scale = new Vector3(1);
            cameraEntity.Add(camera);

            return cameraEntity;
        }

        public Entity GetAmbientLight()
        {
            var directionalLightEntity = new Entity();
            //directionalLightEntity.Transform.Position = new Vector3(11.803946f, 50.0f, 0.65027833f);
            //directionalLightEntity.Transform.Rotation = new Quaternion(0.11697774f, 0.88302225f, 0.32139382f, 0.3213937f);
            //directionalLightEntity.Transform.Scale = new Vector3(0);

            var directionalLightComponent = directionalLightEntity.GetOrCreate<LightComponent>();

            //scene.Entities.Add(directionalLightEntity);

            var light = new LightAmbient();
            light.Color = new ColorRgbProvider(Color.White);

            directionalLightComponent.Type = light;
            directionalLightComponent.Intensity = 0.5f;

            return directionalLightEntity;
        }

        public Entity GetLight()
        {
            var directionalLightEntity = new Entity();
            directionalLightEntity.Transform.Position = new Vector3(11.803946f, 50.0f, 0.65027833f);
            directionalLightEntity.Transform.Rotation = new Quaternion(0.11697774f, 0.88302225f, 0.32139382f, 0.3213937f);
            directionalLightEntity.Transform.Scale = new Vector3(0);

            var directionalLightComponent = directionalLightEntity.GetOrCreate<LightComponent>();

            //scene.Entities.Add(directionalLightEntity);

            var light = new LightPoint();
            light.Color = new ColorRgbProvider(new Color(1, 1, 1));
            light.Radius = 200;

            directionalLightComponent.Type = light;
            directionalLightComponent.Intensity = 0.5f;

            return directionalLightEntity;
        }
    }
}
