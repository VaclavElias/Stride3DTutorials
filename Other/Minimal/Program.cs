using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Rendering;
using Stride.Rendering.ProceduralModels;

namespace Minimal
{
    // This opens black screen
    static class Program
    {
        static void Main(string[] args)
        {
            using var game = new Game();

            // How do we create programatically a new scene and attatch to Game?
            Scene scene = new Scene();

            var model = new Model();
            var cube = new CubeProceduralModel();

            cube.Generate(game.Services, model);

            var cubeEntity = new Entity();
            cubeEntity.Transform.Scale = new Vector3(1);
            cubeEntity.Transform.Position = new Vector3(1);

            cubeEntity.GetOrCreate<ModelComponent>().Model = model;

            // How do we attach a Sync script? 
            // This is causing an error
            // Error: Service [IContentManager] not found'
            //cubeEntity.Add(new TestComponent());

            scene.Entities.Add(cubeEntity);

            game.SceneSystem.SceneInstance = new SceneInstance(game.Services, scene);

            game.Run();
        }
    }
}
