public class StartExamples
{
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