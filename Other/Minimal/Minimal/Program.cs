var builder = GameApplication.CreateBuilder();

var game = builder.Build();

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