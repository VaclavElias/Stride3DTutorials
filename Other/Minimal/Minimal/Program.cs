var builder = GameApplication.CreateBuilder();

builder.Add(GetCubeEntity(),new CubeProceduralModel());
builder.Add(new SphereProceduralModel());

var game = builder.Build3D();

game.Run();

Entity GetCubeEntity()
{
    return new Entity(new Vector3(1, 0, 3))
    {
        new MotionComponent()
    };
}