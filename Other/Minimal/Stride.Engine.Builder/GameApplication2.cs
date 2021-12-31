// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
namespace Stride.Engine.Builder;

public class GameApplication2
{
    public const string CameraEntityName = "Camera";
    public const string GroundEntityName = "Ground";
    public const string SkyboxEntityName = "Skybox";
    public const string SunEntityName = "Directional light";

    private const string SkyboxTexture = "skybox_texture_hdr.dds";
    private readonly MinimalGame2 _game = new();
    private Material _defaultMaterial = new Material();
    private bool _isGround;
    private bool _isSkybox;
    private bool _isCameraScript;
    private bool _isSphere;

    public GameApplication2()
    {
        //var graphicsCompositor = GraphicsCompositorHelper.CreateDefault(true);
        var graphicsCompositor = GraphicsCompositorBuilder.Create();

        //((ForwardRenderer)graphicsCompositor.SingleView).PostEffects = (PostProcessingEffects?)new PostProcessingEffects
        //{
        //    DepthOfField = { Enabled = false },
        //    ColorTransforms = { Transforms = { new ToneMap() } },
        //};

        _game.OnBeginRun += OnBeginRun;
        _game.SceneSystem.GraphicsCompositor = graphicsCompositor;

        CreateAndSetNewScene();
    }

    public static GameApplication2 CreateBuilder() => new();

    public MinimalGame2 Build()
    {
        return _game;
    }

    private void OnBeginRun(object? sender, EventArgs e)
    {
        SetDefaultMaterial();
        if (_isGround) CreateAndSetGround();
        if (_isSkybox) CreateAndSetSkybox();
        if (_isCameraScript) CreateAndSetCameraScript();
        if (_isSphere) CreateAndSetDefaultSphere();
    }

    /// <summary>
    /// Adds Ground, SkyBox, CameraScript
    /// </summary>
    /// <returns></returns>
    public MinimalGame2 Build3D()
    {
        var game = Build();

        _isGround = true;
        _isSkybox = true;
        _isCameraScript = true;
        _isSphere = true;

        return game;
    }

    public MinimalGame2 Build2D() => throw new NotImplementedException();

    public GameApplication2 AddGround()
    {
        _isGround = true;

        return this;
    }

    public GameApplication2 AddSkybox()
    {
        _isSkybox = true;

        return this;
    }

    public GameApplication2 AddCameraScript()
    {
        _isCameraScript = true;

        return this;
    }

    public void CreateAndSetDefaultSphere(Color? color = null)
    {
        var model = new Model();

        var sphereModel = new SphereProceduralModel
        {
            MaterialInstance = { Material = _defaultMaterial },
            Tessellation = 30,
        };

        sphereModel.Generate(_game.Services, model);

        var entity = new Entity("Sphere") { new ModelComponent(model) };

        entity.Transform.Position = new Vector3(0, 0.5f, 0);

        AddEntity(entity);
    }

    public Model GetCube()
    {
        var model = new Model();

        var proceduralModel = new CubeProceduralModel
        {
            MaterialInstance = { Material = _defaultMaterial }
        };

        proceduralModel.Generate(_game.Services, model);

        return model;
    }

    private void SetDefaultMaterial(Color? color = null)
        => _defaultMaterial = new DefaultMaterial(_game.GraphicsDevice).Get(color);

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

        AddEntity(entity);
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

    public void AddEntity(Entity entity)
        => _game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
}
