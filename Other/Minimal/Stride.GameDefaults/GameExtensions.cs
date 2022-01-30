namespace Stride.GameDefaults;

public static class GameExtensions
{
    private const string CameraEntityName = "Camera";
    //private const string GroundEntityName = "Ground";
    //private const string SunEntityName = "Directional light";
    private const string SkyboxTexture = "skybox_texture_hdr.dds";
    //private const string SkyboxEntityName = "Skybox";
    //private const string Profiler = "Profiler";

    private static readonly Color _defaulColour = Color.FromBgra(0xFF8C8C8C);

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
        AddCamera(game, CameraEntityName);
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
    }

    public static GraphicsCompositor AddGraphicsCompositor(Game game)
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

        return graphicsCompositor;
    }

    public static Entity AddCamera(this Game game, string entityName)
    {
        var entity = new Entity(entityName) { new CameraComponent {
            Projection = CameraProjectionMode.Perspective,
            Slot =  game.SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId()}
        };

        entity.Transform.Position = new(6, 6, 6);
        entity.Transform.Rotation = Quaternion.RotationYawPitchRoll(
            MathUtil.DegreesToRadians(45),
            MathUtil.DegreesToRadians(-30),
            MathUtil.DegreesToRadians(0));

        game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);

        return entity;
    }

    public static Entity AddLight(this Game game)
    {
        var entity = new Entity() { new LightComponent
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

        return entity;
    }

    public static Entity AddSkybox(this Game game)
    {
        using var stream = new FileStream($"Resources\\{SkyboxTexture}", FileMode.Open, FileAccess.Read);

        var texture = Texture.Load(game.GraphicsDevice, stream, TextureFlags.ShaderResource, GraphicsResourceUsage.Dynamic);

        var skyboxGeneratorContext = new SkyboxGeneratorContext(game);

        var skybox = new Skybox();

        skybox = SkyboxGenerator.Generate(skybox, skyboxGeneratorContext, texture);

        var entity = new Entity() {
                new BackgroundComponent { Intensity = 1.0f, Texture = texture },
                new LightComponent {
                    Intensity = 1.0f,
                    Type = new LightSkybox() { Skybox = skybox } }
        };

        entity.Transform.Position = new Vector3(0.0f, 2.0f, -2.0f);

        game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);

        return entity;
    }

    /// <summary>
    /// The camera entity can be moved using W, A, S, D, Q and E, arrow keys, a gamepad's left stick or dragging/scaling using multi-touch.
    /// Rotation is achieved using the Numpad, the mouse while holding the right mouse button, a gamepad's right stick, or dragging using single-touch.
    /// </summary>
    /// <param name="game"></param>
    /// <param name="cameraEntityName"></param>
    public static void AddMouseLookCamera(this Game game, string cameraEntityName = CameraEntityName)
    {
        var cameraEntity = game.SceneSystem.SceneInstance.RootScene.Entities.Single(w => w.Name == cameraEntityName);

        cameraEntity?.Add(new BasicCameraController());
    }

    /// <summary>
    /// Adds a ground with default Size 10,10.
    /// </summary>
    /// <param name="game"></param>
    /// <param name="size"></param>
    /// <param name="isPhysical">Adds a collider</param>
    /// <returns></returns>
    public static Entity AddGround(this Game game, Vector2? size = null, bool isPhysical = true)
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

        var validSize = size ?? new Vector2(10.0f, 10.0f);

        var groundModel = new PlaneProceduralModel
        {
            Size = validSize,
            MaterialInstance = { Material = material }
        };

        var model = groundModel.Generate(game.Services);

        var entity = new Entity() { new ModelComponent(model) };

        if (isPhysical)
        {
            var groundCollider = new StaticColliderComponent();

            groundCollider.ColliderShapes.Add(new BoxColliderShapeDesc()
            {
                Size = new Vector3(validSize.X, 1, validSize.Y),
                LocalOffset = new Vector3(0, -0.5f, 0)
            });

            entity.Add(groundCollider);
        }

        game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);

        return entity;
    }

    /// <summary>
    /// Basic default material
    /// </summary>
    /// <param name="game"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Material NewDefaultMaterial(this Game game, Color? color = null)
    {
        var materialDescription = new MaterialDescriptor
        {
            Attributes =
                {
                    Diffuse = new MaterialDiffuseMapFeature(new ComputeColor(color ?? _defaulColour)),
                    DiffuseModel = new MaterialDiffuseLambertModelFeature(),
                    Specular =  new MaterialMetalnessMapFeature(new ComputeFloat(1.0f)),
                    SpecularModel = new MaterialSpecularMicrofacetModelFeature(),
                    MicroSurface = new MaterialGlossinessMapFeature(new ComputeFloat(0.65f))
                }
        };

        return Material.New(game.GraphicsDevice, materialDescription);
    }

    /// <summary>
    /// Creates a game object with a primitive mesh renderer.
    /// </summary>
    /// <param name="game"></param>
    /// <param name="type"></param>
    /// <param name="material"></param>
    /// <returns></returns>
    public static Entity CreatePrimitive(this Game game, PrimitiveModelType type, Material? material = null)
    {
        PrimitiveProceduralModelBase proceduralModel = type switch
        {
            PrimitiveModelType.Plane => new PlaneProceduralModel(),
            PrimitiveModelType.Sphere => new SphereProceduralModel(),
            PrimitiveModelType.Cube => new CubeProceduralModel(),
            PrimitiveModelType.Cylinder => new CylinderProceduralModel(),
            PrimitiveModelType.Torus => new TorusProceduralModel(),
            PrimitiveModelType.Teapot => new TeapotProceduralModel(),
            PrimitiveModelType.Cone => new ConeProceduralModel(),
            PrimitiveModelType.Capsule => new CapsuleProceduralModel(),
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
    public static Entity AddProfiler(this Game game)
    {
        var entity = new Entity() { new GameProfiler() };

        game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);

        return entity;
    }

    // This will be replaced with CameraComponentExtensions.ScreenPointToRay
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
}
