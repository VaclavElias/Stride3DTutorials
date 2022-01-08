using (var game = new MinimalGame3())
{
    // Option 1 - Defaults set here
    // adds default camera, camera script, skybox, ground, ..like through UI
    //game.SetDefaults();

    // Option 2 - Defaults set as optional parameter
    var _entity = new Entity(new Vector3(1f, 0.5f, 3f));
    var _angle = 0f;
    var initialPosition = _entity.Transform.Position;

    game.Run(/*new GameDefaults(),*/ start: Start, update: Update);

    void Start()
    {
        //game.Window.AllowUserResizing = true;

        // Option 3 - Defaults set here
        var defaults = new GameDefaults(game).Set3D();

        // or select what you want
        //var defaults2 = new GameDefaults(game).AddGround().AddSkybox().AddCameraScript().AddGameProfiler();

        var model = new CubeProceduralModel().Generate(game.Services);

        model.Materials.Add(defaults.DefaultMaterial);

        _entity.Components.Add(new ModelComponent(model));
        //_entity.Components.Add(new GameProfiler());
        //_entity.Components.Add(new RotationComponentScript());

        //var _entity = new Entity(new Vector3(1f, 0.5f, 3f))
        //{
        //    new ModelComponent(new CubeProceduralModel().Generate(game.Services)),
        //    new MotionComponentScript()
        //};

        _entity.Scene = game.SceneSystem.SceneInstance.RootScene;
    }

    void Update()
    {
        _angle += 5f * (float)game.UpdateTime.Elapsed.TotalSeconds;

        var offset = new Vector3((float)Math.Sin(_angle), 0, (float)Math.Cos(_angle)) * 1f;

        _entity.Transform.Position = initialPosition + offset;


        //if (Vector3.Distance(initialPosition, _entity.Transform.Position) <= 2)
        //{
        //    _entity.Transform.Position.Z += 0.03f;
        //}
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