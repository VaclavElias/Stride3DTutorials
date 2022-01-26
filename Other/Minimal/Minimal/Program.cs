using (var game = new Game())
{
    game.Run(start: Start);

    void Start(Scene rootScene)
    {
        game.SetupBase();
        game.AddSkybox();
        game.AddMouseLookCamera();
        game.AddGround();

        var entity = new Entity(new Vector3(1f, 0.5f, 3f))
            {
                new ModelComponent(new CubeProceduralModel().Generate(game.Services)),
                new RotationComponentScript()
            };

        entity.Scene = rootScene;

        var cylinder = game.CreatePrimitive(PrimtiveModelType.Torus);

        cylinder.Scene = rootScene;
    }
}

using (var game = new Game())
{
    var entity = new Entity(new Vector3(1f, 0.5f, 3f));
    var cubeGenerator = new CubesGenerator(game.Services);

    game.Run(start: Start, update: Update);

    void Start(Scene rootScene)
    {
        game.SetupBase3DScene();
        game.AddGroundCollider();
        //game.AddProfiler();
        game.AddRaycast();

        var model = new CubeProceduralModel().Generate(game.Services);

        model.Materials.Add(game.NewDefaultMaterial());

        entity.Components.Add(new ModelComponent(model));

        entity.Scene = rootScene;

        for (int i = 0; i < 1000; i++)
        {
            entity.AddChild(cubeGenerator.GetCube());
        }
    }

    void Update(Scene rootScene, GameTime time)
    {

    }
}

using (var game = new Game())
{
    var entity = new Entity(new Vector3(1f, 0.5f, 3f));
    var angle = 0f;
    var initialPosition = entity.Transform.Position;

    game.Run(start: Start, update: Update);

    void Start(Scene rootScene)
    {
        game.SetupBase3DScene();
        game.AddProfiler();

        var model = new CubeProceduralModel().Generate(game.Services);

        model.Materials.Add(game.NewDefaultMaterial());

        entity.Components.Add(new ModelComponent(model));

        entity.Scene = rootScene;
    }

    void Update(Scene rootScene, GameTime time)
    {
        angle += 1f * (float)time.Elapsed.TotalSeconds;

        var offset = new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle)) * 1f;

        entity.Transform.Position = initialPosition + offset;
    }
}


using (var game = new Game())
{
    game.Run(start: Start);

    void Start(Scene rootScene)
    {
        // adds default camera, camera script, skybox, ground, ..like through UI
        game.SetupBase3DScene();
        game.AddProfiler();

        var model = new CubeProceduralModel().Generate(game.Services);

        model.Materials.Add(game.NewDefaultMaterial());

        var entity = new Entity(new Vector3(1f, 0.5f, -3f))
        {
            new ModelComponent(model),
            new MotionComponentScript()
        };

        entity.Scene = rootScene;

        var entity2 = new Entity(new Vector3(1, 0.5f, 3))
            {
                new ModelComponent(new CubeProceduralModel().Generate(game.Services)),
                new MotionComponentScript()
            };

        entity2.Scene = rootScene;
    }
}