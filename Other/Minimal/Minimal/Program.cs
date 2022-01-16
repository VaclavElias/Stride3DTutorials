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
    game.Run(start: Start, update: null);

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