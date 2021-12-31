// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
namespace Stride.Engine.Builder;

public class GameApplication
{
    public const string CameraEntityName = "Camera";
    public const string GroundEntityName = "Ground";
    public const string SkyboxEntityName = "Skybox";
    public const string SunEntityName = "Directional light";

    private const string SkyboxTexture = "skybox_texture_hdr.dds";
    private readonly MinimalGame _game = new();

    public GameApplication()
    {
        //_game.SceneSystem.GraphicsCompositor = GraphicsCompositorHelper.CreateDefault(true, clearColor: Color.Green, graphicsProfile: GraphicsProfile.Level_11_2);
        _game.SceneSystem.GraphicsCompositor = GraphicsCompositorBuilder.Create();

        CreateAndSetNewScene();
    }

    public static GameApplication CreateBuilder() => new();

    public Game Build()
    {
        _game.WindowCreated += OnWindowCreated;

        return _game;
    }

    /// <summary>
    /// Adds Ground, SkyBox, CameraScript
    /// </summary>
    /// <returns></returns>
    public Game Build3D()
    {
        var game = Build();

        _game.BeginRunActions.Add(() =>
        {
            CreateAndSetGround();
            CreateAndSetSkybox();
            CreateAndSetCameraScript();
            GetSpecialSphere(null);
        });

        return _game;
    }

    public Game Build2D() => throw new NotImplementedException();

    public GameApplication AddGround()
    {
        _game.BeginRunActions.Add(() => CreateAndSetGround());

        return this;
    }

    public GameApplication AddSkybox()
    {
        _game.BeginRunActions.Add(() => CreateAndSetSkybox());

        return this;
    }

    public GameApplication AddCameraScript()
    {
        _game.BeginRunActions.Add(() => CreateAndSetCameraScript());

        return this;
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
    //public static Task<Entity> CreateEntityWithComponent(string name, EntityComponent component, params EntityComponent[] additionalComponents)
    //{
    //    var newEntity = new Entity { Name = name };
    //    newEntity.Components.Add(component);
    //    if (additionalComponents != null)
    //    {
    //        foreach (var additionalComponent in additionalComponents)
    //        {
    //            newEntity.Components.Add(additionalComponent);
    //        }
    //    }
    //    return Task.FromResult(newEntity);
    //}
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

    private void CreateAndSetNewScene()
    {
        var scene = SceneHDRFactory.Create();

        var cameraEntity = scene.Entities.Single(x => x.Name == SceneBaseFactory.CameraEntityName);

        cameraEntity.Components.Get<CameraComponent>().Slot = _game.SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId();

        _game.SceneSystem.SceneInstance = new(_game.Services, scene);
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

        var groundModel = new PlaneProceduralModel
        {
            Size = new Vector2(10.0f, 10.0f),
            MaterialInstance = { Material = material }
        };

        groundModel.Generate(_game.Services, model);

        var entity = new Entity(GroundEntityName) { new ModelComponent(model) };

        _game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
    }

    private void CreateAndSetSkybox()
    {
        using var stream = new FileStream($"Resources\\{SkyboxTexture}", FileMode.Open, FileAccess.Read);

        var texture = Texture.Load(_game.GraphicsDevice, stream, TextureFlags.ShaderResource, GraphicsResourceUsage.Dynamic);

        var skyboxEntity = _game.SceneSystem.SceneInstance.RootScene.Entities.Single(x => x.Name == SceneBaseFactory.SkyboxEntityName);

        skyboxEntity.Get<BackgroundComponent>().Texture = texture;

        var skyboxGeneratorContext = new SkyboxGeneratorContext(_game);

        var skybox = new Skybox();

        skybox = SkyboxGenerator.Generate(skybox, skyboxGeneratorContext, texture);

        skyboxEntity.Get<LightComponent>().Type = new LightSkybox
        {
            Skybox = skybox,
        };
    }

    private void CreateAndSetCameraScript()
    {
        var cameraEntity = _game.SceneSystem.SceneInstance.RootScene.Entities.Single(w => w.Name == CameraEntityName);

        cameraEntity.Add(new BasicCameraController());
    }

    private void OnWindowCreated(object? sender, EventArgs e) { }

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
