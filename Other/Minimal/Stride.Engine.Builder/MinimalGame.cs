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
        public List<Action> Actions { get; set; } = new();

        protected override void BeginRun()
        {
            base.BeginRun();

            Window.AllowUserResizing = true;

            foreach (var action in Actions)
            {
                action.Invoke();
            }
        }

        //public void RunTheMethod(Action? myMethodName)
        //{
        //    myMethodName?.Invoke();
        //}
    }
}
