// This is a placeholder
public class Example2
{
    public Example2()
    {
        var builder = GameApplication.CreateBuilder();

        var game = builder.Build3D();

        builder.AddAction(() => GameEntities());

        game.Run();

        void GameEntities()
        {
            var entity = new Entity(new Vector3(1, 0.5f, 3))
            {
                new ModelComponent(builder.GetCube()),
                new MotionComponent()
            };

            game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);
        }
    }
}