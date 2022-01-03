using (var game = new MinimalGame3())
{
    // adds default camera, camera script, skybox, ground, ..like through UI
    game.SetDefaults();

    // or this option to access other defaults, e.g default Material
    // var defaults = game.SetDefaults();

    game.Run(start: Start);

    void Start()
    {
        var model = new Model();

        var proceduralModel = new CubeProceduralModel();

        proceduralModel.Generate(game.Services, model);

        var entity = new Entity(new Vector3(1f, 0.5f, 3f))
        {
            new ModelComponent(model),
            new MotionComponent()
        };

        entity.Scene = game.SceneSystem.SceneInstance.RootScene;
    }
}

using (var game = new MinimalGame3())
{
    // adds default camera, camera script, skybox, ground, ..like through UI
    var defaults = new GameDefaults(game).Set3D();

    game.Run(start: Start);

    void Start(Scene rootScene, ServiceRegistry services)
    {
        var model = new Model();

        var proceduralModel = new CubeProceduralModel
        {
            MaterialInstance = { Material = defaults.DefaultMaterial }
        };

        proceduralModel.Generate(game.Services, model);

        var entity = new Entity(new Vector3(1f, 0.5f, 3f))
        {
            new ModelComponent(model),
            new MotionComponent()
        };

        entity.Scene = rootScene;

        var entity2 = new Entity(new Vector3(1, 0.5f, 1))
            {
                new ModelComponent(defaults.GetCube()),
                new MotionComponent()
            };

        entity2.Scene = rootScene;
    }
}