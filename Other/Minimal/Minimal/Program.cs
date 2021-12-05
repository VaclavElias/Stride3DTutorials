using Stride.Core.Mathematics;
using Stride.Engine.Builder;
using Stride.Rendering;
using Stride.Rendering.ProceduralModels;

var builder = GameApplication.CreateBuilder();

var game = builder.Build();

// Should be before but we don't have a game till here
builder.AddAction(() => AddCube(game));

game.Run();

void AddCube(Game game)
{
    var model = new Model();
    var cube = new CubeProceduralModel();

    cube.Generate(game.Services, model);

    var cubeEntity = new Entity();
    cubeEntity.Transform.Scale = new Vector3(1);
    cubeEntity.Transform.Position = new Vector3(1);

    cubeEntity.GetOrCreate<ModelComponent>().Model = model;

    //cubeEntity.Add(new TestComponent());

    game.SceneSystem.SceneInstance.RootScene.Entities.Add(cubeEntity);
}

//using var game = new MinimalGame2();

//game.Run();

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
