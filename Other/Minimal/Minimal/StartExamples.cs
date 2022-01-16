public class StartExamples
{
    public void Main1()
    {
        using var game = new Game();

        game.Run(start: Start, update: null);

        void Start(Scene rootScene)
        {
            game.SetupBase3DScene();

            var entity = new Entity(new Vector3(1f, 0.5f, 3f))
            {
                new ModelComponent(new CubeProceduralModel().Generate(game.Services)),
                new MotionComponentScript()
            };

            entity.Scene = game.SceneSystem.SceneInstance.RootScene;
        }
    }

    public void Main2()
    {
        using (var game = new Game())
        {
            var _entity = new Entity(new Vector3(1f, 0.5f, 3f));
            var _angle = 0f;
            var initialPosition = _entity.Transform.Position;

            game.OnInitialize += () => System.Console.WriteLine("Hello, manio143");

            game.Run(start: Start, update: Update);

            void Start(Scene rootScene)
            {
                //game.Window.AllowUserResizing = true;

                game.SetupBase3DScene();

                var model = new CubeProceduralModel().Generate(game.Services);

                model.Materials.Add(game.NewDefaultMaterial());

                _entity.Components.Add(new ModelComponent(model));

                _entity.Scene = game.SceneSystem.SceneInstance.RootScene;
            }

            void Update(Scene rootScene, GameTime time)
            {
                _angle += 1f * (float)time.Elapsed.TotalSeconds;

                var offset = new Vector3((float)Math.Sin(_angle), 0, (float)Math.Cos(_angle)) * 1f;

                _entity.Transform.Position = initialPosition + offset;
            }
        }
    }

    public void Main3()
    {
        using (var game = new Game())
        {
            var _entity = new Entity(new Vector3(1f, 0.5f, 3f));
            var _angle = 0f;
            var initialPosition = _entity.Transform.Position;

            game.Run(start: Start, update: Update);

            void Start(Scene rootScene)
            {
                game.SetupBase3DScene();

                var model = new CubeProceduralModel().Generate(game.Services);

                model.Materials.Add(game.NewDefaultMaterial());

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

            void Update(Scene rootScene, GameTime time)
            {
                _angle += 5f * (float)time.Elapsed.TotalSeconds;

                var offset = new Vector3((float)Math.Sin(_angle), 0, (float)Math.Cos(_angle)) * 1f;

                _entity.Transform.Position = initialPosition + offset;

                //if (Vector3.Distance(initialPosition, _entity.Transform.Position) <= 2)
                //{
                //    _entity.Transform.Position.Z += 0.03f;
                //}
            }
        }
    }
}