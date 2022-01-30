using (var game = new Game())
{
    game.Run(start: Start);

    void Start(Scene rootScene)
    {
        game.SetupBase3DScene();

        var entity = new Entity(new Vector3(1f, 0.5f, 3f))
            {
                new ModelComponent(new CubeProceduralModel().Generate(game.Services)),
                new RotationComponentScript()
            };

        entity.Scene = rootScene;

        var cylinder = game.CreatePrimitive(PrimitiveModelType.Teapot, game.NewDefaultMaterial(Color.Blue));

        cylinder.Scene = rootScene;
    }
}

using (var game = new Game())
{
    var entity = new Entity(new Vector3(1f, 0.5f, 3f));
    var cubeGenerator = new CubesGenerator(game.Services);
    CameraComponent? cameraComponent = null;
    Simulation? simulation = null;

    game.Run(start: Start, update: Update);

    void Start(Scene rootScene)
    {
        game.SetupBase();
        game.AddSkybox();
        game.AddMouseLookCamera();
        game.AddGround();
        game.AddProfiler();

        cameraComponent = rootScene.Entities.SingleOrDefault(x => x.Name == "Camera")?.Get<CameraComponent>();
        simulation = game.SceneSystem.SceneInstance.GetProcessor<PhysicsProcessor>()?.Simulation;

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
        if (simulation is null || cameraComponent is null) return;

        if (game.Input.HasMouse && game.Input.IsMouseButtonPressed(MouseButton.Left))
        {
            var ray = cameraComponent.ScreenPointToRay(game.Input.MousePosition);

            var hitResult = simulation.Raycast(ray.VectorNear.XYZ(), ray.VectorFar.XYZ());

            if (hitResult.Succeeded)
            {
                hitResult.Collider.Entity.Scene = null;
            }

            //var result = game.ScreenPointToRay();

            //if (result.Succeeded)
            //{
            //    result.Collider.Entity.Scene = null;
            //}
        }
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