using (var game = new Game())
{
    // Option 1 - Defaults set here
    //game.SetupBase3DScene();

    var _entity = new Entity(new Vector3(1f, 0.5f, 3f));
    var _angle = 0f;
    var initialPosition = _entity.Transform.Position;

    game.Run(start: Start, update: Update);

    void Start(Scene rootScene, IServiceRegistry services)
    {
        //game.Window.AllowUserResizing = true;

        game.SetupBase3DScene();

        var model = new CubeProceduralModel().Generate(services);

        model.Materials.Add(game.NewDefaultMaterial());

        _entity.Components.Add(new ModelComponent(model));

        _entity.Scene = game.SceneSystem.SceneInstance.RootScene;
    }

    void Update(Scene rootScene, IServiceRegistry services, GameTime time)
    {
        _angle += 1f * (float)time.Elapsed.TotalSeconds;

        var offset = new Vector3((float)Math.Sin(_angle), 0, (float)Math.Cos(_angle)) * 1f;

        _entity.Transform.Position = initialPosition + offset;
    }
}

using (var game = new Game())
{
    game.Run(start: Start, update: null);

    void Start(Scene rootScene, IServiceRegistry services)
    // adds default camera, camera script, skybox, ground, ..like through UI
    {
        game.SetupBase3DScene();

        var model = new CubeProceduralModel().Generate(game.Services);

        model.Materials.Add(game.NewDefaultMaterial());

        var entity = new Entity(new Vector3(1f, 0.5f, -3f))
        {
            new ModelComponent(model),
            new MotionComponentScript()
        };

        entity.Scene = game.SceneSystem.SceneInstance.RootScene;

        var entity2 = new Entity(new Vector3(1, 0.5f, 3))
            {
                new ModelComponent(new CubeProceduralModel().Generate(game.Services)),
                new MotionComponentScript()
            };

        entity2.Scene = game.SceneSystem.SceneInstance.RootScene;
    }
}