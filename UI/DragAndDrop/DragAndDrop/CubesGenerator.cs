using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Physics;
using Stride.Rendering;
using Stride.Rendering.ProceduralModels;
using System;

namespace DragAndDrop
{
    public class CubesGenerator
    {
        private readonly Random _random = new();
        private readonly CubeProceduralModel _cubeModel = new();
        private readonly IServiceRegistry _services;

        public CubesGenerator(IServiceRegistry services)
        {
            _services = services;
        }

        public Entity GetCube()
        {
            var model = new Model();
            _cubeModel.Generate(_services, model);

            var entity = new Entity();
            entity.Transform.Scale = new Vector3(0.1f);
            entity.Transform.Position = new Vector3(
                -3 + (float)(_random.NextDouble() * 6),
                (float)(_random.NextDouble() * 1) + 2,
                -3 + (float)(_random.NextDouble() * 6));

            entity.GetOrCreate<ModelComponent>().Model = model;
            var rigidBody = entity.GetOrCreate<RigidbodyComponent>();
            rigidBody.Gravity = new Vector3(0, 0.5f, 0);
            rigidBody.ColliderShape = new BoxColliderShape(false, new Vector3(0.1f));

            return entity;
        }
    }
}
