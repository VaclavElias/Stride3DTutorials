using Stride.Core.Diagnostics;
using Stride.Core.Mathematics;
using Stride.Engine.Processors;
using Stride.Rendering;
using Stride.Rendering.Colors;
using Stride.Rendering.Lights;
using Stride.Rendering.ProceduralModels;

namespace Stride.Engine.Builder
{
    public class MinimalGame : Game
    {
        public Action? SomeAction { get; set; }

        protected override void BeginRun()
        {
            base.BeginRun();

            Window.AllowUserResizing = true;

            RunTheMethod(SomeAction);
        }

        public void RunTheMethod(Action? myMethodName)
        {
            myMethodName?.Invoke();
        }
    }
}
