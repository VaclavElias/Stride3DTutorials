var builder = GameApplication.CreateBuilder();

var game = builder.Build3D();

//builder.AddAction(() => GameEntities());

game.OnBeginRun += StartGame;

void StartGame(object? sender, EventArgs e)
{
    GameEntities();
}

game.Run();

void GameEntities()
{
    var entity = new Entity(new Vector3(1, 0.5f, 3))
    {
        new ModelComponent(builder.GetCube()),
        new MotionComponent()
    };

    builder.AddEntity(entity);
}