namespace Stride.GameDefaults;

public static class GameExtensions
{
    private const string CameraEntityName = "Camera";
    private const string GroundEntityName = "Ground";
    private const string SunEntityName = "Directional light";
    private const string SkyboxTexture = "skybox_texture_hdr.dds";
    private const string SkyboxEntityName = "Skybox";
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

    /// <summary>
    /// Sets up the mininum: Graphics Compositor, Camera and Light
    /// </summary>
    /// <param name="game"></param>
    public static void SetupBase(this Game game)
    {
        AddGraphicsCompositor(game);
        AddCamera(game);
        AddLight(game);
    }

    /// <summary>
    /// Sets up the default scene similarly like in Stride.Assets.Entities, SceneBaseFactory; Graphics Compositor, Camera and Light, Skybox, MouseLookCamera, Ground, Sphere
    /// </summary>
    /// <param name="game"></param>
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

    public static void AddCamera(this Game game)
    {
        var cameraEntity = game.SceneSystem.SceneInstance.RootScene.Entities.SingleOrDefault(x => x.Name == CameraEntityName);

        if (cameraEntity is not null) return;

        var entity = new Entity(CameraEntityName) { new CameraComponent {
            Projection = CameraProjectionMode.Perspective,
            Slot =  game.SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId()}
        };

        entity.Transform.Position = new(6, 6, 6);
        entity.Transform.Rotation = Quaternion.RotationYawPitchRoll(
            MathUtil.DegreesToRadians(45),
            MathUtil.DegreesToRadians(-30),
            MathUtil.DegreesToRadians(0));


        game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
    }

    public static void AddLight(this Game game)
    {
        var sunEntity = game.SceneSystem.SceneInstance.RootScene.Entities.SingleOrDefault(x => x.Name == SunEntityName);

        if (sunEntity is not null) return;

        var entity = new Entity(SunEntityName) { new LightComponent
            {
                Intensity =  20.0f,
                Type = new LightDirectional
                {
                    Shadow =
                    {
                        Enabled = true,
                        Size = LightShadowMapSize.Large,
                        Filter = new LightShadowMapFilterTypePcf { FilterSize = LightShadowMapFilterTypePcfSize.Filter5x5 },
                    }
                }
            } };

        entity.Transform.Position = new Vector3(0, 2.0f, 0);
        entity.Transform.Rotation = Quaternion.RotationX(MathUtil.DegreesToRadians(-30.0f)) * Quaternion.RotationY(MathUtil.DegreesToRadians(-180.0f));

        game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
    }

    public static void AddSkybox(this Game game)
    {
        var skyboxEntity = game.SceneSystem.SceneInstance.RootScene.Entities.SingleOrDefault(x => x.Name == SkyboxEntityName);

        if (skyboxEntity is not null) return;

        using var stream = new FileStream($"Resources\\{SkyboxTexture}", FileMode.Open, FileAccess.Read);

        var texture = Texture.Load(game.GraphicsDevice, stream, TextureFlags.ShaderResource, GraphicsResourceUsage.Dynamic);

        var skyboxGeneratorContext = new SkyboxGeneratorContext(game);

        var skybox = new Skybox();

        skybox = SkyboxGenerator.Generate(skybox, skyboxGeneratorContext, texture);

        var entity = new Entity(SkyboxEntityName) {
                new BackgroundComponent { Intensity = 1.0f, Texture = texture },
                new LightComponent {
                    Intensity = 1.0f,
                    Type = new LightSkybox() { Skybox = skybox } }
        };

        entity.Transform.Position = new Vector3(0.0f, 2.0f, -2.0f);

        game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
    }

    /// <summary>
    /// The camera entity can be moved using W, A, S, D, Q and E, arrow keys, a gamepad's left stick or dragging/scaling using multi-touch.
    /// Rotation is achieved using the Numpad, the mouse while holding the right mouse button, a gamepad's right stick, or dragging using single-touch.
    /// </summary>
    public static void AddMouseLookCamera(this Game game)
    {
        var cameraEntity = game.SceneSystem.SceneInstance.RootScene.Entities.Single(w => w.Name == CameraEntityName);

        cameraEntity?.Add(new BasicCameraController());
    }

    /// <summary>
    /// Add ground with default Size 10,10. You can use AddGroundCollider() if needed.
    /// </summary>
    /// <param name="game"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static Entity AddGround(this Game game, Vector2? size = null)
    {
        var groundEntity = game.SceneSystem.SceneInstance.RootScene.Entities.SingleOrDefault(x => x.Name == GroundEntityName);

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
        var ground = game.SceneSystem.SceneInstance.RootScene.Entities.SingleOrDefault(x => x.Name == GroundEntityName);

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

    /// <summary>
    /// Basic default material
    /// </summary>
    /// <param name="game"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Material NewDefaultMaterial(this Game game, Color? color = null)
        => new DefaultMaterial(game.GraphicsDevice).Get(color);

    // This is similar to one in Unity, which I think returns Entity
    public static Entity CreatePrimitive(this Game game, PrimtiveModelType type, Material? material = null)
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

        model.Materials.Add(material);

        return new Entity() { new ModelComponent(model) };
    }

    /// <summary>
    /// Toggle profiling Left Shift + Left Ctrl + P, Toggle filtering mode F1
    /// </summary>
    /// <param name="game"></param>
    public static void AddProfiler(this Game game)
    {
        var profilerEntity = game.SceneSystem.SceneInstance.RootScene.Entities.SingleOrDefault(w => w.Name == Profiler);

        if (profilerEntity is not null) return;

        var entity = new Entity(Profiler) { new GameProfiler() };

        game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
    }

    /// <summary>
    /// Returns a HitResult based on a ray going from camera through a screen point. The ray is in world space, starting on the near plane of the camera and going through position's (x,y) pixel coordinates on the screen.
    /// </summary>
    /// <param name="game"></param>
    /// <param name="mousePosition"></param>
    /// <returns></returns>
    public static HitResult ScreenPointToRay(this Game game, Vector2? mousePosition = null)
    {
        var validMousePosition = mousePosition ?? game.Input.MousePosition;

        var simulation = game.SceneSystem.SceneInstance.GetProcessor<PhysicsProcessor>()?.Simulation;

        if (simulation is null) return new HitResult();

        var camera = game.SceneSystem.SceneInstance.RootScene.Entities.SingleOrDefault(x => x.Name == CameraEntityName)?.Get<CameraComponent>();

        if (camera is null) return new HitResult();

        var invertedMatrix = Matrix.Invert(camera.ViewProjectionMatrix);

        Vector3 position;
        position.X = validMousePosition.X * 2f - 1f;
        position.Y = 1f - validMousePosition.Y * 2f;
        position.Z = 0f;

        Vector4 vectorNear = Vector3.Transform(position, invertedMatrix);
        vectorNear /= vectorNear.W;

        position.Z = 1f;

        Vector4 vectorFar = Vector3.Transform(position, invertedMatrix);
        vectorFar /= vectorFar.W;

        return simulation.Raycast(vectorNear.XYZ(), vectorFar.XYZ());
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
