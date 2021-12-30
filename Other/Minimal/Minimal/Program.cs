var builder = GameApplication.CreateBuilder();

//builder.AddGround();
//builder.AddSkybox();
//builder.AddCameraScript();

var game = builder.Build3D();

builder.Add(GetCubeEntity(),new CubeProceduralModel());
builder.Add(new SphereProceduralModel());

game.Run();

Entity GetCubeEntity()
{
    var entity = new Entity();

    entity.Transform.Position = new Vector3(1,0,3);

    //entity.Add(new MotionComponent());

    return entity;
}