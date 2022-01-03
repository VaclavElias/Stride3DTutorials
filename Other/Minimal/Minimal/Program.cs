using (var game = new MinimalGame3())
{
    // Option 1 - Defaults set here
    // adds default camera, camera script, skybox, ground, ..like through UI
    //game.SetDefaults();

    // Option 2 - Defaults set as optional parameter
    game.Run(/*new GameDefaults(),*/ start: Start);

    void Start()
    {
        // Option 3 - Defaults set here
        var defaults = new GameDefaults(game).Set3DAfterStart();

        var entity = new Entity(new Vector3(1f, 0.5f, 3f))
        {
            new ModelComponent(new CubeProceduralModel().Generate(game.Services)),
            new MotionComponentScript()
        };

        entity.Scene = game.SceneSystem.SceneInstance.RootScene;
    }
}

using (var game = new MinimalGame3())
{
    // adds default camera, camera script, skybox, ground, ..like through UI
    var defaults = new GameDefaults(game).Set3DBeforeStart();

    game.Run(start: Start);

    void Start()
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