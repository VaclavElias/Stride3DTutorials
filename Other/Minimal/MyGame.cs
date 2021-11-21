using Stride.Core.Diagnostics;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Processors;
using Stride.Games;
using Stride.Graphics;
using Stride.Graphics.GeometricPrimitives;
using Stride.Rendering;
using Stride.Rendering.Colors;
using Stride.Rendering.Lights;
using Stride.Rendering.ProceduralModels;
using System;
using System.Threading.Tasks;

namespace Minimal
{
    public class MyGame2 : Game
    {
        public MyGame2()
        {
            GameStarted += MyGame2_GameStarted;
        }

        private void MyGame2_GameStarted(object? sender, EventArgs e)
        {
            Log.Warning("Hello");

            var scene = new Scene();

            scene.Entities.Add(GetCamera());
            scene.Entities.Add(GetLight());

            var model = new Model();
            var cube = new CubeProceduralModel();

            cube.Generate(Services, model);

            var cubeEntity = new Entity();
            cubeEntity.Transform.Scale = new Vector3(1);
            cubeEntity.Transform.Position = new Vector3(1);

            cubeEntity.GetOrCreate<ModelComponent>().Model = model;

            cubeEntity.Add(new TestComponent());

            scene.Entities.Add(cubeEntity);

            SceneSystem.SceneInstance = new SceneInstance(Services, scene);

            //SceneSystem.SceneInstance.RootScene.Entities.Add(cubeEntity);
        }

        public Entity GetCamera()
        {
            var camera = new CameraComponent();
            camera.Projection = CameraProjectionMode.Perspective;

            var cameraEntity = new Entity();
            cameraEntity.Transform.Position = new Vector3(0, 25, 50);
            cameraEntity.Transform.Scale = new Vector3(1);
            cameraEntity.Transform.Rotation = new Quaternion(-0.34202012f, 0, 0, 0.9396926f);
            cameraEntity.Transform.Scale = new Vector3(1);
            cameraEntity.Add(camera);

            return cameraEntity;
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
            directionalLightComponent.Intensity = 800;

            return directionalLightEntity;
        }
    }

    public class MyGame : Game
    {
        private Matrix view = Matrix.LookAtRH(new Vector3(0, 0, 5), new Vector3(0, 0, 0), Vector3.UnitY);
        private EffectInstance simpleEffect;
        private GeometricPrimitive teapot;

        protected async override Task LoadContent()
        {
            await base.LoadContent();

            var model = new Model();
            var cube = new CubeProceduralModel();

            cube.Generate(this.Services, model);

            var cubeEntity = new Entity();
            cubeEntity.Transform.Scale = new Vector3(1);
            cubeEntity.Transform.Position = new Vector3(1);

            cubeEntity.GetOrCreate<ModelComponent>().Model = model;

            teapot = GeometricPrimitive.Cube.New(GraphicsDevice);


            // Prepare effect/shader
            simpleEffect = new EffectInstance(new Effect(GraphicsDevice, SpriteEffect.Bytecode));
            // Load texture
            //using (var stream = new FileStream("small_uv.png", FileMode.Open, FileAccess.Read, FileShare.Read))
            //    simpleEffect.Parameters.Set(TexturingKeys.Texture0, Texture.Load(GraphicsDevice, stream));
            // Initialize teapot
            //teapot = GeometricPrimitive.Teapot.New(GraphicsDevice);

            //teapot.Draw(GraphicsContext, simpleEffect);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Clear screen
            GraphicsContext.CommandList.Clear(GraphicsDevice.Presenter.BackBuffer, Color.CornflowerBlue);
            GraphicsContext.CommandList.Clear(GraphicsDevice.Presenter.DepthStencilBuffer, DepthStencilClearOptions.DepthBuffer | DepthStencilClearOptions.Stencil);

            // Set render target
            GraphicsContext.CommandList.SetRenderTargetAndViewport(GraphicsDevice.Presenter.DepthStencilBuffer, GraphicsDevice.Presenter.BackBuffer);

            var time = (float)gameTime.Total.TotalSeconds;

            // Compute matrices
            var world = Matrix.Scaling((float)Math.Sin(time * 1.5f) * 0.2f + 1.0f) * Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f) * Matrix.Translation(0, 0, 0);
            var projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.Presenter.BackBuffer.ViewWidth / GraphicsDevice.Presenter.BackBuffer.ViewHeight, 0.1f, 100.0f);

            // Setup effect/shader
            //simpleEffect.Parameters.Set(SpriteBaseKeys.MatrixTransform, Matrix.Multiply(world, Matrix.Multiply(view, projection)));
            //simpleEffect.UpdateEffect(GraphicsDevice);

            //teapot.Draw(GraphicsContext, simpleEffect);
        }
    }
}
