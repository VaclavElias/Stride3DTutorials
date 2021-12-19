// Can we make it simplier, cleaner and elegant?
// Can we make GameApplication simplier?
// What name adjustments would you suggest?
// Could we add this as a new optional official Stride NuGet once finalised maybe as a preview package?
// Could we extend Stride Game object to simplify my implementation, I am not sure about the MinimalGame class, Ideally I wouldn't have MinimalGame but we might need some other Event in Game object to hook to add Entities easier? I would probably add new Game() directly in the GameApplication but then I am not able to do BeginRun override unless there is some other option I am not aware of?
// Can anyone help/suggest how to improve the visual output which, could be as default or optional but I would prefer something much visually nicer, so anyone working with this NuGet would be very impressed by Stride
// What else could we add as optional? e.g. like AddSkybox, better ligtning and so on.

var builder = GameApplication.CreateBuilder();

// Here we could set 2D or 3D as parameter to position the default camera
var game = builder.Build(); // or Build2D(), Build3D()

// Maybe these below should be before the Build() but we don't have a game object till here, unless there is another solution or we could expose some new event from Game object?

// These 3 could be inside Build() with an option to overwrite?
builder.AddGround(); //  GameWorldType.Simple (Default), GameWorldType.Ocean, GameWordlType.Grass, Stone
builder.AddSkybox();
builder.AddCameraController();

builder.Add(GetCubeEntity(),new CubeProceduralModel());
builder.Add(new SphereProceduralModel());

game.Run();

Entity GetCubeEntity()
{
    var entity = new Entity();

    entity.Transform.Position = new Vector3(1,0,3);

    entity.Add(new MotionComponent());

    return entity;
}

// Ideally the above should look as simple as this

//using var game = new Game();

//// Example 1 - Learn basic Stride shapes
//var cube = new CubeProceduralModel();
//var entity = new Entity();
//entity.AddChild(cube);

//game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity);


//// Example 2 - Learn "for" loop in C#
//for (int i = 0; i < 4; i++)
//{
//    var model = new CubeProceduralModel();
//    var entity2 = new Entity();

//    entity2.Transform.Position.X = 5 * i;
//    entity2.AddChild(model);

//    game.SceneSystem.SceneInstance.RootScene.Entities.Add(entity2);
//}

//game.Run();
