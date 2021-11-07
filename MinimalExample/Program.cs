using Stride.Engine;
using Stride.Rendering;
using Stride.Rendering.ProceduralModels;

namespace MinimalExample
{
    class Program
    {
        static void Main(string[] args)
        {
            using var game = new Game();

            Scene scene = new Scene();


            var cube = new CubeProceduralModel();
            var model = new Model();
            cube.Generate(game.Services, model);

            var entity = new Entity();
            entity.GetOrCreate<ModelComponent>().Model = model;

            scene.Entities.Add(entity);

            game.SceneSystem.SceneInstance = new SceneInstance(game.Services, scene);

            //game.SceneSystem.SceneInstance.RootScene.Children.Add(scene);

            game.Run();
        }
    }
}
