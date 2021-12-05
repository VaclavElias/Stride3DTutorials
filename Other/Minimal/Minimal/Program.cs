namespace Minimal
{
    // This opens black screen
    static class Program
    {
        static void Main(string[] args)
        {
            using var game = new MinimalGame();

            game.Run();
        }
    }
}

//var game = new Game();

//// Example 1 - Learn basic Stride shapes
//var cube = Cube.New();
//var entity = new Entity();
//entity.AddChild(cube);

//game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);

//// Example 2 - Learn for loop
//for (int i = 0; i < 4; i++)
//{
//    var cube2 = Cube.New();
//    var entity2 = new Entity();

//    entity2.Transform.Position.X = 5 * i;
//    entity2.AddChild(cube2);

//    game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity2);
//}

//game.Run();
