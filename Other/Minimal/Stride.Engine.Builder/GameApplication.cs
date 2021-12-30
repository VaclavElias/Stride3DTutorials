// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
using Stride.Core;
using Stride.Core.IO;
using Stride.Graphics.GeometricPrimitives;
using Stride.Rendering.Skyboxes;

namespace Stride.Engine.Builder;

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
    /// Adds Ground and CamereController
    /// </summary>
    /// <returns></returns>
    public Game Build3D()
    {
        var game = Build();

        _game.BeginRunActions.Add(() =>
        {
            CreateAndSetGround();
            AddSkybox();
            GetSpecialSphere(null);
        });

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

    public GameApplication AddGround()
    {
        _game.BeginRunActions.Add(() => CreateAndSetGround());

        return this;
    }

    public static Task<Entity> CreateEntityWithComponent(string name, EntityComponent component, params EntityComponent[] additionalComponents)
    {
        var newEntity = new Entity { Name = name };
        newEntity.Components.Add(component);
        if (additionalComponents != null)
        {
            foreach (var additionalComponent in additionalComponents)
            {
                newEntity.Components.Add(additionalComponent);
            }
        }
        return Task.FromResult(newEntity);
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

        entity.Transform.Position = new Vector3(1, 0.5f, 4);

        _game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
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
    public void AddSkybox()
    {
        var skyboxFilename = "skybox_texture_hdr.dds";

        using var stream = new FileStream($"Resources\\{skyboxFilename}", FileMode.Open, FileAccess.Read);

        var texture = Texture.Load(_game.GraphicsDevice, stream);
        
        var skyboxEntity = _game.SceneSystem.SceneInstance.RootScene.Entities.Single(x => x.Name == SceneBaseFactory.SkyboxEntityName);

        var texture2 = CubemapFromTextureRenderer.GenerateCubemap(_game.Services, new RenderDrawContext(_game.Services, RenderContext.GetShared(_game.Services), _game.GraphicsContext), texture, 32);

        skyboxEntity.Get<BackgroundComponent>().Texture = texture;

        var test = Texture.NewCube(_game.GraphicsDevice, 256, 1, PixelFormat.R8G8B8A8_UNorm, TextureFlags.RenderTarget | TextureFlags.ShaderResource);


        //var skyboxGeneratorContext = new SkyboxGeneratorContext(_game.GraphicsDevice, _game.Services, _game.GraphicsContext, _game.Services?.GetSafeServiceAs<IDatabaseFileProviderService>());

        //var skybox = new Skybox();

        //skybox = SkyboxGenerator.Generate(skybox, skyboxGeneratorContext, texture);

        //skyboxEntity.Get<LightComponent>().Type = new LightSkybox
        //{
        //    Skybox = skybox,

        //};
    }

    // Simple Camera Movement
    public void AddCameraController()
    {
        _game.BeginRunActions.Add(() =>
        {

            var cameraEntity = _game.SceneSystem.SceneInstance.RootScene.Entities.Single(w => w.Name == CameraEntityName);

            cameraEntity.Add(new BasicCameraController());
        });
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

    private void CreateAndSetGround()
    {
        var materialDescription = new MaterialDescriptor
        {
            Attributes =
                {
                    Diffuse = new MaterialDiffuseMapFeature(new ComputeColor(Color.FromBgra(0xFF242424))),
                    DiffuseModel = new MaterialDiffuseLambertModelFeature(),
                    Specular =  new MaterialMetalnessMapFeature(new ComputeFloat(0.0f)),
                    SpecularModel = new MaterialSpecularMicrofacetModelFeature(),
                    MicroSurface = new MaterialGlossinessMapFeature(new ComputeFloat(0.1f))
                }
        };

        var material = Material.New(_game.GraphicsDevice, materialDescription);

        var model = new Model();

        var sphereModel = new PlaneProceduralModel
        {
            MaterialInstance = { Material = material },
            Size = new Vector2(10.0f, 10.0f),
        };

        sphereModel.Generate(_game.Services, model);

        var entity = new Entity("Ground") { new ModelComponent(model) };

        _game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
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
