namespace Stride.GameDefaults;

public static class GameExtensions
{
    public const string CameraEntityName = "Camera";
    public const string GroundEntityName = "Ground";
    //public const string SkyboxEntityName = "Skybox";
    //public const string SunEntityName = "Directional light";
    private const string SkyboxTexture = "skybox_texture_hdr.dds";
    private const string Profiler = "Profiler";

    public static void Run(this Game game, GameContext? context = null, Action<Scene>? start = null, Action<Scene, GameTime>? update = null)
    {
        game.Script.Scheduler.Add(RootScript);

        game.Run(context);

        async Task RootScript()
        {
            start?.Invoke(GetRootScene());
            if (update == null)
                return;
            do
            {
                update.Invoke(GetRootScene(), game.UpdateTime);
                await game.Script.NextFrame();
            } while (true);
        }

        Scene GetRootScene() => game.SceneSystem.SceneInstance.RootScene;
    }

    public static void SetupBase(this Game game)
    {
        AddGraphicsCompositor(game);
        AddScene(game);
    }

    public static void SetupBase3DScene(this Game game)
    {
        game.SetupBase();

        game.AddSkybox();
        game.AddMouseLookCamera();
        game.AddGround();
        game.AddSphere(); // Do we want this Sphere?
    }

    private static void AddGraphicsCompositor(Game game)
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

    private static void AddScene(Game game)
    {
        var scene = SceneHDRFactory.Create(game.SceneSystem.SceneInstance.RootScene);

        var cameraEntity = scene.Entities.Single(x => x.Name == SceneBaseFactory.CameraEntityName);

        cameraEntity.Components.Get<CameraComponent>().Slot = game.SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId();

        //game.SceneSystem.SceneInstance = new(game.Services, scene);
    }

    /// <summary>
    /// Add ground with default Size 10,10
    /// </summary>
    /// <param name="game"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static Entity AddGround(this Game game, Vector2? size = null)
    {
        var groundEntity = game.SceneSystem.SceneInstance.RootScene.Entities.Single(x => x.Name == GroundEntityName);

        if (groundEntity is not null) return groundEntity;

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

        var groundModel = new PlaneProceduralModel
        {
            Size = size ?? new Vector2(10.0f, 10.0f),
            MaterialInstance = { Material = material }
        };

        var model = groundModel.Generate(game.Services);

        var entity = new Entity(GroundEntityName) { new ModelComponent(model) };

        game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);

        return entity;
    }

    /// <summary>
    /// Adds static collider to default Ground
    /// </summary>
    /// <param name="game"></param>
    public static void AddGroundCollider(this Game game)
    {
        var ground = game.SceneSystem.SceneInstance.RootScene.Entities.Single(x => x.Name == GroundEntityName);

        if (ground is null) return;

        var modelComponent = ground.Get<ModelComponent>();

        if (modelComponent is null) return;

        var distance = modelComponent.Model.BoundingBox.Maximum.X - modelComponent.Model.BoundingBox.Minimum.X;

        var component = new StaticColliderComponent();

        component.ColliderShapes.Add(new BoxColliderShapeDesc()
        {
            Size = new Vector3(distance, 1, distance),
            LocalOffset = new Vector3(0, -0.5f, 0)
        });

        ground.Add(component);
    }

    public static void AddSkybox(this Game game)
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

    public static void AddMouseLookCamera(this Game game)
    {
        var cameraEntity = game.SceneSystem.SceneInstance.RootScene.Entities.Single(w => w.Name == CameraEntityName);

        cameraEntity?.Add(new BasicCameraController());
    }

    public static Material NewDefaultMaterial(this Game game, Color? color = null)
        => new DefaultMaterial(game.GraphicsDevice).Get(color);

    // This is similar to one in Unity, which I think returns Entity
    public static Entity CreatePrimitive(this Game game, PrimtiveModelType type)
    {
        PrimitiveProceduralModelBase proceduralModel = type switch
        {
            PrimtiveModelType.Plane => new PlaneProceduralModel(),
            PrimtiveModelType.Sphere => new SphereProceduralModel(),
            PrimtiveModelType.Cube => new CubeProceduralModel(),
            PrimtiveModelType.Cylinder => new CylinderProceduralModel(),
            PrimtiveModelType.Torus => new TorusProceduralModel(),
            PrimtiveModelType.Teapot => new TeapotProceduralModel(),
            PrimtiveModelType.Cone => new ConeProceduralModel(),
            PrimtiveModelType.Capsule => new CapsuleProceduralModel(),
            _ => throw new NotImplementedException(),
        };

        var model = proceduralModel.Generate(game.Services);

        return new Entity() { new ModelComponent(model) };

        //switch (type)
        //{
        //    case PrimtiveModelType.Plane:
        //        model = new PlaneProceduralModel
        //        {
        //            //MaterialInstance = { Material = new DefaultMaterial(game.GraphicsDevice).Get(color) },
        //            //Tessellation = 30,
        //        };
        //        break;
        //    case PrimtiveModelType.Sphere:
        //        model = new SphereProceduralModel
        //        {
        //            //MaterialInstance = { Material = new DefaultMaterial(game.GraphicsDevice).Get(color) },
        //            Tessellation = 30,
        //        };
        //        break;
        //    case PrimtiveModelType.Cube:
        //        model = new CubeProceduralModel
        //        {
        //            //MaterialInstance = { Material = new DefaultMaterial(game.GraphicsDevice).Get(color) },
        //            //Tessellation = 30,
        //        };
        //        break;
        //    case PrimtiveModelType.Cylinder:
        //        break;
        //    case PrimtiveModelType.Torus:
        //        break;
        //    case PrimtiveModelType.Teapot:
        //        break;
        //    case PrimtiveModelType.Cone:
        //        break;
        //    case PrimtiveModelType.Capsule:
        //        break;
        //}


    }

    public static void AddProfiler(this Game game)
    {
        var profilerEntity = game.SceneSystem.SceneInstance.RootScene.Entities.Single(w => w.Name == Profiler);

        if (profilerEntity is not null) return;

        var entity = new Entity(Profiler) { new GameProfiler() };

        game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
    }

    public static void AddRaycast(this Game game)
    {
        var camera = game.SceneSystem.SceneInstance.RootScene.Entities.Single(x => x.Name == CameraEntityName);

        if (camera is null) return;

        camera.Add(new CameraRaycast());
    }


    ////////////////////////////////////////////////
    //                Do we need theses?          //   
    ////////////////////////////////////////////////
    
    public static void AddSplashScreen(this Game game) { }

    // If we want this, refactor with CreatePrimitive if that is approved
    public static Entity AddSphere(this Game game, Color? color = null)
    {
        var sphereModel = new SphereProceduralModel
        {
            MaterialInstance = { Material = new DefaultMaterial(game.GraphicsDevice).Get(color) },
            Tessellation = 30,
        };

        var model = sphereModel.Generate(game.Services);

        var entity = new Entity("Sphere") { new ModelComponent(model) };

        entity.Transform.Position = new Vector3(0, 0.5f, 0);

        game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);

        return entity;
    }
}
