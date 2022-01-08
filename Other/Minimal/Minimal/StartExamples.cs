public class StartExamples
{
    public void Main3()
    {
        using (var game = new MinimalGame3())
        {
            var _entity = new Entity(new Vector3(1f, 0.5f, 3f));
            var _angle = 0f;
            var initialPosition = _entity.Transform.Position;

            game.Run(start: Start, update: Update);

            void Start()
            {
                var defaults = new GameDefaults(game).Set3D();

                var model = new CubeProceduralModel().Generate(game.Services);

                model.Materials.Add(defaults.DefaultMaterial);

                _entity.Components.Add(new ModelComponent(model));

                _entity.Scene = game.SceneSystem.SceneInstance.RootScene;
            }

            void Update()
            {
                _angle += 5f * (float)game.UpdateTime.Elapsed.TotalSeconds;

                var offset = new Vector3((float)Math.Sin(_angle), 0, (float)Math.Cos(_angle)) * 1f;

                _entity.Transform.Position = initialPosition + offset;
            }
        }
    }

    public void Main1()
    {
        var builder = GameApplication.CreateBuilder();

        var game = builder.Build3D();

        game.AddAction(() =>
        {
            var entity = new Entity(new Vector3(1, 0.5f, 3))
            {
                new ModelComponent(builder.GetCube()),
                new MotionComponentScript()
            };

            game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
        });

        game.Run();
    }

    public void Main2()
    {
        var builder = GameApplication2.CreateBuilder();

        var game = builder.Build3D();

        game.OnBeginRun += (s, e) =>
        {
            var entity = new Entity(new Vector3(1, 0.5f, 3))
            {
                new ModelComponent(builder.GetCube()),
                new MotionComponentScript()
            };

            game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
        };

        game.Run();
    }
}