using Stride.Core.IO;
using Stride.Core.Serialization;
using Stride.Graphics;
using Stride.Rendering.Skyboxes;

namespace Stride.Engine.Builder;

// Or GameEngine?
// I am following a bit this https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio
// This will bootstrap some boilerplate
// Maybe it could have these options
// GameType.2D, GameType.3D (Default) or Build2D and Build3D.
// GameWorldType.Simple (Default), GameWorldType.Ocean, GameWordlType.Grass
//  - where this would bring better lighting, added ground and sky box
public class GameApplication
{
    public const string SkyboxEntityName = "Skybox";
    public const string CameraEntityName = "Camera";
    public const string SunEntityName = "Directional light";

    public static GameApplication CreateBuilder() => new();

    private readonly MinimalGame _game = new();

    public GameApplication()
    {
        // These can be here or in BeginRun()
        _game.SceneSystem.GraphicsCompositor = GraphicsCompositorBuilder.Create();

        CreateAndSetNewScene();
    }

    private void CreateAndSetNewScene()
    {
        var scene = SceneHDRFactory.Create();

        var cameraEntity = scene.Entities.Single(x => x.Name == SceneBaseFactory.CameraEntityName);

        cameraEntity.Components.Get<CameraComponent>().Slot = _game.SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId();

        _game.SceneSystem.SceneInstance = new(_game.Services, scene);
    }

    // Here we could set 2D or 3D as a parameter to position the default camera or we could use Build2D(), Build3D()
    public Game Build()
    {
        _game.WindowCreated += OnWindowCreated;

        return _game;
    }

    /// <summary>
    /// Creates an Entity and a Model, adds the Model to the Entity and adds the Entity to the Scene
    /// </summary>
    /// <param name="primitiveModel"></param>
    public void Add(PrimitiveProceduralModelBase primitiveModel)
    {
        _game.BeginRunActions.Add(() => GenerateModel(new Entity(), primitiveModel));
    }

    /// <summary>
    /// Creates a Model, adds the Model to the Entity and adds the Entity to the Scene
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="primitiveModel"></param>
    public void Add(Entity entity, PrimitiveProceduralModelBase primitiveModel)
    {
        _game.BeginRunActions.Add(() => GenerateModel(entity, primitiveModel));
    }

    private void AddEntityAction(Entity entity)
        => _game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);

    public void AddAction(Action? action)
    {
        if (action == null) return;

        _game.BeginRunActions.Add(action);
    }

    public void AddGround()
    {
        _game.BeginRunActions.Add(() => AddGroundEntity());
        _game.BeginRunActions.Add(() => GetSpecialSphere(null));
    }

    public void GetSpecialSphere(Color? color)
    {
        var materialDescription = new MaterialDescriptor
        {
            Attributes =
                {
                    Diffuse = new MaterialDiffuseMapFeature(new ComputeColor(Color.FromBgra(0xFF8C8C8C))),
                    DiffuseModel = new MaterialDiffuseLambertModelFeature(),
                    Specular =  new MaterialMetalnessMapFeature(new ComputeFloat(1.0f)),
                    SpecularModel = new MaterialSpecularMicrofacetModelFeature(),
                    MicroSurface = new MaterialGlossinessMapFeature(new ComputeFloat(0.65f))
                }
        };

        var material = Material.New(_game.GraphicsDevice, materialDescription);

        var model = new Model();

        var sphereModel = new SphereProceduralModel
        {
            MaterialInstance = { Material = material },
            Tessellation = 30,
        };

        sphereModel.Generate(_game.Services, model);

        var entity = new Entity("Sphere") { new ModelComponent(model) };

        entity.Transform.Position = new Vector3(1, 0, 4);

        _game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);

        var skyboxFilename = "skybox_texture_hdr.dds";

        using (FileStream fsSource = new FileStream("Resources\\skybox_texture_hdr.dds",
            FileMode.Open, FileAccess.Read))
        {
            var texture = Texture.Load(_game.GraphicsDevice, fsSource);
        }
    }

    public Material GetMaterial(Color? color)
    {
        var materialDescription = new MaterialDescriptor
        {
            Attributes = {
                    DiffuseModel = new MaterialDiffuseLambertModelFeature(),
                    Diffuse = new MaterialDiffuseMapFeature(new ComputeColor { Key = MaterialKeys.DiffuseValue })
                }
        };

        var material = Material.New(_game.GraphicsDevice, materialDescription);

        material.Passes[0].Parameters.Set(MaterialKeys.DiffuseValue, color ?? Color.White);

        return material;
    }

    // Simple Skybox
    public void AddSkybox() { }

    // Simple Camera Movement
    public void AddCameraController()
    {
        _game.BeginRunActions.Add(() =>
        {

            var cameraEntity = _game.SceneSystem.SceneInstance.RootScene.Entities.Where(w => w.Name == CameraEntityName).Single();

            cameraEntity.Add(new BasicCameraController());
        });
    }

    private Entity GetCamera(SceneSystem sceneSystem)
    {
        var camera = new CameraComponent();
        camera.Projection = CameraProjectionMode.Perspective;
        camera.Slot = sceneSystem.GraphicsCompositor.Cameras[0].ToSlotId();

        var cameraEntity = new Entity(CameraEntityName);
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

    private Entity GetAmbientLight()
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

    private Entity GetLight()
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

    private void AddGroundEntity()
    {
        var model = new Model();

        var plane = new PlaneProceduralModel();

        plane.Size = new Vector2(10, 10);

        model.Materials.Add(GetMaterial(Color.Red));

        plane.Generate(_game.Services, model);

        var entity = new Entity();
        entity.Transform.Position = new Vector3(0, -2, 0);
        entity.GetOrCreate<ModelComponent>().Model = model;

        _game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
    }

    private void OnWindowCreated(object? sender, EventArgs e)
    {
        // This could be in BeginRun or GameStarted
        //_game.SceneSystem.SceneInstance.RootScene.Entities.Add(GetCamera(_game.SceneSystem));
        _game.SceneSystem.SceneInstance.RootScene.Entities.Add(GetAmbientLight());
    }

    private void GenerateModel(Entity entity, PrimitiveProceduralModelBase primitiveModel)
    {
        var model = GeneratePrimitiveModel(primitiveModel);

        entity.GetOrCreate<ModelComponent>().Model = model;

        _game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
    }
    private Model GeneratePrimitiveModel(PrimitiveProceduralModelBase proceduralModel)
    {
        var model = new Model();

        proceduralModel.Generate(_game.Services, model);

        return model;
    }
}
