var builder = GameApplication.CreateBuilder();

builder.Add(GetCubeEntity(),new CubeProceduralModel());
builder.Add(new SphereProceduralModel());

var game = builder.Build3D();

game.Run();

Entity GetCubeEntity()
{
    var entity = new Entity(new Vector3(1, 0, 3));

    entity.Add(new MotionComponent());

    return entity;
}

//builder.AddGround();
//builder.AddSkybox();
//builder.AddCameraScript();