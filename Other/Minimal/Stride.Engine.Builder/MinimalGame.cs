namespace Stride.Engine.Builder;

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
}

