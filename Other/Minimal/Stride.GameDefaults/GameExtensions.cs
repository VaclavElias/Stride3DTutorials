namespace Stride.GameDefaults;

public static class GameExtensions
{
    public const string CameraEntityName = "Camera";
    public const string GroundEntityName = "Ground";
    public const string SkyboxEntityName = "Skybox";
    public const string SunEntityName = "Directional light";
    private const string SkyboxTexture = "skybox_texture_hdr.dds";
    public static Material DefaultMaterial { get; set; } = new();

    public static void Run(this Game game, Action<Scene, Core.IServiceRegistry>? start, Action<Scene, Core.IServiceRegistry, GameTime>? update)
    {
        if (start != null || update != null)
        {
            var rootScript = new RootScript(start, update);

            game.OnInitialize += () => game.Script.Add(rootScript);
        }

        game.Run();
    }

    public static void SetupBase3DScene(this Game game)
    {
        CreateAndSetDefaultGraphicsCompositor(game);
        CreateAndSetNewScene(game);

        CreateAndSetSkybox(game);
        CreateAndSetCameraScript(game);
        CreateAndSetGround(game);

        SetDefaultMaterial(game);
        CreateAndSetDefaultSphere(game);
    }

    //public static void AddSplashScreen();

    private static void CreateAndSetDefaultGraphicsCompositor(Game game)
    {
        // This is already build in Stride engine
        //var graphicsCompositor = GraphicsCompositorHelper.CreateDefault(true);

        // Just some extra things added
        //((ForwardRenderer)graphicsCompositor.SingleView).PostEffects = (PostProcessingEffects?)new PostProcessingEffects
        //{
        //    DepthOfField = { Enabled = false },
        //    ColorTransforms = { Transforms = { new ToneMap() } },
        //};


        // This might be used instead, I would like to update it with Clean UI
        // https://github.com/herocrab/StrideCleanUI
        var graphicsCompositor = GraphicsCompositorBuilder.Create();

        game.SceneSystem.GraphicsCompositor = graphicsCompositor;
    }

    private static void CreateAndSetNewScene(Game game)
    {
        var scene = SceneHDRFactory.Create();

        var cameraEntity = scene.Entities.Single(x => x.Name == SceneBaseFactory.CameraEntityName);

        cameraEntity.Components.Get<CameraComponent>().Slot = game.SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId();

        game.SceneSystem.SceneInstance = new(game.Services, scene);
    }

    private static Entity CreateAndSetGround(Game game)
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

        var material = Material.New(game.GraphicsDevice, materialDescription);

        var model = new Model();

        var groundModel = new PlaneProceduralModel
        {
            Size = new Vector2(10.0f, 10.0f),
            MaterialInstance = { Material = material }
        };

        groundModel.Generate(game.Services, model);

        var entity = new Entity(GroundEntityName) { new ModelComponent(model) };

        game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);

        return entity;
    }

    private static void CreateAndSetSkybox(Game game)
    {
        using var stream = new FileStream($"Resources\\{SkyboxTexture}", FileMode.Open, FileAccess.Read);

        var texture = Texture.Load(game.GraphicsDevice, stream, TextureFlags.ShaderResource, GraphicsResourceUsage.Dynamic);

        var skyboxEntity = game.SceneSystem.SceneInstance.RootScene.Entities.Single(x => x.Name == SceneBaseFactory.SkyboxEntityName);

        skyboxEntity.Get<BackgroundComponent>().Texture = texture;

        var skyboxGeneratorContext = new SkyboxGeneratorContext(game);

        var skybox = new Skybox();

        skybox = SkyboxGenerator.Generate(skybox, skyboxGeneratorContext, texture);

        skyboxEntity.Get<LightComponent>().Type = new LightSkybox
        {
            Skybox = skybox,
        };
    }

    private static void CreateAndSetCameraScript(Game game)
    {
        var cameraEntity = game.SceneSystem.SceneInstance.RootScene.Entities.Single(w => w.Name == CameraEntityName);

        cameraEntity.Add(new BasicCameraController());
    }

    public static void AddEntity(Game game, Entity entity)
        => game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);

    private static void SetDefaultMaterial(Game game, Color? color = null)
        => DefaultMaterial = new DefaultMaterial(game.GraphicsDevice).Get(color);

    private static void CreateAndSetDefaultSphere(Game game, Color? color = null)
    {
        var model = new Model();

        var sphereModel = new SphereProceduralModel
        {
            MaterialInstance = { Material = DefaultMaterial },
            Tessellation = 30,
        };

        sphereModel.Generate(game.Services, model);

        var entity = new Entity("Sphere") { new ModelComponent(model) };

        entity.Transform.Position = new Vector3(0, 0.5f, 0);

        game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
    }

    private static void AddGameProfiler() { }

    public static Material NewDefaultMaterial(this Game game, Color? color = null)
    {
        return new DefaultMaterial(game.GraphicsDevice).Get(color);
    }
}

