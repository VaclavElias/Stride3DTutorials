using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Games;
using Stride.Graphics;
using Stride.Graphics.GeometricPrimitives;
using Stride.Rendering;
using Stride.Rendering.ProceduralModels;
using System;
using System.Threading.Tasks;

namespace Minimal
{

    public class MyGame : Game
    {
        private Matrix view = Matrix.LookAtRH(new Vector3(0, 0, 5), new Vector3(0, 0, 0), Vector3.UnitY);
        private EffectInstance simpleEffect;
        private GeometricPrimitive teapot;

        protected async override Task LoadContent()
        {
            await base.LoadContent();

            var model = new Model();
            var cube = new CubeProceduralModel();

            cube.Generate(this.Services, model);

            var cubeEntity = new Entity();
            cubeEntity.Transform.Scale = new Vector3(1);
            cubeEntity.Transform.Position = new Vector3(1);

            cubeEntity.GetOrCreate<ModelComponent>().Model = model;

            teapot = GeometricPrimitive.Cube.New(GraphicsDevice);


            // Prepare effect/shader
            simpleEffect = new EffectInstance(new Effect(GraphicsDevice, SpriteEffect.Bytecode));
            // Load texture
            //using (var stream = new FileStream("small_uv.png", FileMode.Open, FileAccess.Read, FileShare.Read))
            //    simpleEffect.Parameters.Set(TexturingKeys.Texture0, Texture.Load(GraphicsDevice, stream));
            // Initialize teapot
            //teapot = GeometricPrimitive.Teapot.New(GraphicsDevice);

            //teapot.Draw(GraphicsContext, simpleEffect);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Clear screen
            GraphicsContext.CommandList.Clear(GraphicsDevice.Presenter.BackBuffer, Color.CornflowerBlue);
            GraphicsContext.CommandList.Clear(GraphicsDevice.Presenter.DepthStencilBuffer, DepthStencilClearOptions.DepthBuffer | DepthStencilClearOptions.Stencil);

            // Set render target
            GraphicsContext.CommandList.SetRenderTargetAndViewport(GraphicsDevice.Presenter.DepthStencilBuffer, GraphicsDevice.Presenter.BackBuffer);

            var time = (float)gameTime.Total.TotalSeconds;

            // Compute matrices
            var world = Matrix.Scaling((float)Math.Sin(time * 1.5f) * 0.2f + 1.0f) * Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f) * Matrix.Translation(0, 0, 0);
            var projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.Presenter.BackBuffer.ViewWidth / GraphicsDevice.Presenter.BackBuffer.ViewHeight, 0.1f, 100.0f);

            // Setup effect/shader
            //simpleEffect.Parameters.Set(SpriteBaseKeys.MatrixTransform, Matrix.Multiply(world, Matrix.Multiply(view, projection)));
            //simpleEffect.UpdateEffect(GraphicsDevice);

            //teapot.Draw(GraphicsContext, simpleEffect);
        }
    }
}
