using (var game = new MinimalGame3())
{
    // Option 1 - Defaults set here
    game.SetupBase3DScene();

    var _entity = new Entity(new Vector3(1f, 0.5f, 3f));
    var _angle = 0f;
    var initialPosition = _entity.Transform.Position;

    game.Run3(start: Start, update: Update);

    void Start(Scene rootScene, IServiceRegistry services)
    {
        //game.Window.AllowUserResizing = true;

        //game.SetupBase3DScene();

        var model = new CubeProceduralModel().Generate(services);

        model.Materials.Add(game.NewDefaultMaterial());

        _entity.Components.Add(new ModelComponent(model));

        _entity.Scene = game.SceneSystem.SceneInstance.RootScene;
    }

    void Update(Scene rootScene, IServiceRegistry services, GameTime time)
    {
        _angle += 5f * (float)time.Elapsed.TotalSeconds;

        var offset = new Vector3((float)Math.Sin(_angle), 0, (float)Math.Cos(_angle)) * 1f;

        _entity.Transform.Position = initialPosition + offset;
    }
}

using (var game = new MinimalGame3())
{
    // adds default camera, camera script, skybox, ground, ..like through UI
    var defaults = new GameDefaults(game).Set3DBeforeStart();

    game.Run3(start: Start, update: null);

    void Start(Scene rootScene, IServiceRegistry services)
    {
        var model = new CubeProceduralModel().Generate(game.Services);

        model.Materials.Add(defaults.DefaultMaterial);

        var entity = new Entity(new Vector3(1f, 0.5f, -3f))
        {
            new ModelComponent(model),
            new MotionComponentScript()
        };

        entity.Scene = game.SceneSystem.SceneInstance.RootScene;

        var entity2 = new Entity(new Vector3(1, 0.5f, 3))
            {
                new ModelComponent(defaults.GetCube()),
                new MotionComponentScript()
            };

        entity2.Scene = game.SceneSystem.SceneInstance.RootScene;
    }
}