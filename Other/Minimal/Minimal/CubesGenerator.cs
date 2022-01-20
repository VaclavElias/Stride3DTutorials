namespace Minimal;

public class CubesGenerator
{
    private readonly Random _random = new();
    private readonly CubeProceduralModel _cubeModel = new();
    private readonly IServiceRegistry _services;
    private readonly int _squareSize = 4;
    private readonly int _height = 4;

    public CubesGenerator(IServiceRegistry services) => _services = services;

    public Entity GetCube()
    {
        var model = _cubeModel.Generate(_services);

        var entity = new Entity();

        entity.Transform.Scale = new Vector3(0.3f);
        entity.Transform.Position = new Vector3(
            GetRandomPosition(),
            (float)(_random.NextDouble() * 1) + _height,
            GetRandomPosition());

        entity.GetOrCreate<ModelComponent>().Model = model;

        var rigidBody = entity.GetOrCreate<RigidbodyComponent>();
        rigidBody.ColliderShape = new BoxColliderShape(false, new Vector3(1));

        return entity;

        float GetRandomPosition()
        {
            return -_squareSize + (float)(_random.NextDouble() * _squareSize * 2);
        }
    }
}

