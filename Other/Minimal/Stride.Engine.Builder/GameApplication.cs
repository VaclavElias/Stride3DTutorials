using Stride.Core.Mathematics;
using Stride.Engine.Processors;
using Stride.Graphics;
using Stride.Graphics.GeometricPrimitives;
using Stride.Rendering;
using Stride.Rendering.Colors;
using Stride.Rendering.Lights;
using Stride.Rendering.Materials;
using Stride.Rendering.Materials.ComputeColors;
using Stride.Rendering.ProceduralModels;

namespace Stride.Engine.Builder
{
    // Or GameEngine?
    // I am following a bit this https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio
    // This will bootstrap all boilerplate
    // Maybe it could have these options
    // GameType.2D, GameType.3D (Default)
    // GameWorldType.Simple (Default), GameWorldType.Ocean, GameWordlType.Grass
    //  - where this would bring better lighting, added ground and sky box
    public class GameApplication
    {
        public static GameApplication CreateBuilder() => new();

        private readonly MinimalGame _game = new();

        public Action? SomeAction2 { get; set; }

        public GameApplication()
        {
            // These can be here or in BeginRun()
            _game.SceneSystem.GraphicsCompositor = GraphicsCompositorBuilder.Create();
            _game.SceneSystem.SceneInstance = new(_game.Services, new());
        }

        public Game Build()
        {
            _game.WindowCreated += OnWindowCreated;

            return _game;
        }

        private void OnWindowCreated(object? sender, EventArgs e)
        {
            // This could be in BeginRun or GameStarted
            _game.SceneSystem.SceneInstance.RootScene.Entities.Add(GetCamera(_game.SceneSystem));
            _game.SceneSystem.SceneInstance.RootScene.Entities.Add(GetAmbientLight());

            //SomeAction2?.Invoke();
        }
        public void AddEntity(Entity entity)
        {
            _game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
        }

        public void AddAction(Action? action)
        {
            _game.Actions.Add(action);
        }

        public void AddAction2(Action? action)
        {
            _game.Actions.Add(action);
        }

        public void AddGround()
        {
            _game.Actions.Add(() => GetGround());
        }

        public void GetGround()
        {
            var model = new Model();

            var plane = new PlaneProceduralModel();

            plane.Size = new Vector2(10, 10);

            var materialDescription = new MaterialDescriptor
            {
                Attributes = {
                    DiffuseModel = new MaterialDiffuseLambertModelFeature(),
                    Diffuse = new MaterialDiffuseMapFeature(new ComputeColor { Key = MaterialKeys.DiffuseValue })
                }
            };

            var material = Material.New(_game.GraphicsDevice, materialDescription);

            material.Passes[0].Parameters.Set(MaterialKeys.DiffuseValue, Color.Red);

            model.Materials.Add(material);

            plane.Generate(_game.Services, model);

            var entity = new Entity();
            entity.Transform.Position = new Vector3(0, -2, 0);
            entity.GetOrCreate<ModelComponent>().Model = model;

            _game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
        }

        public Model GetCube()
        {
            var model = new Model();

            var cube = new CubeProceduralModel();

            cube.Generate(_game.Services, model);

            return model;
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