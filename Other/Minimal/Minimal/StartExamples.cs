public class StartExamples
{
    public void Main3()
    {
        using (var game = new Game())
        {
            var _entity = new Entity(new Vector3(1f, 0.5f, 3f));
            var _angle = 0f;
            var initialPosition = _entity.Transform.Position;

            game.Run(start: Start, update: Update);

            void Start(Scene rootScene, IServiceRegistry services)
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

            void Update(Scene rootScene, IServiceRegistry services, GameTime time)
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

    public void Main1()
    {

    }

    public void Main2()
    {

    }
}