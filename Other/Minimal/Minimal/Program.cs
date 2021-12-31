var builder = GameApplication2.CreateBuilder();

var game = builder.Build3D();

game.OnBeginRun += (s, e) =>
{
    var entity = new Entity(new Vector3(1, 0.5f, 3))
    {
        new ModelComponent(builder.GetCube()),
        new MotionComponent()
    };

    game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
};

game.Run();