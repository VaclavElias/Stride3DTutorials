namespace Stride.Engine.Builder;

public class MinimalGame : Game
{
    // Approach 1
    public List<Action> BeginRunActions { get; set; } = new();

    // Approach 2
    public event EventHandler<EventArgs>? OnBeginRun;

    protected override void BeginRun()
    {
        base.BeginRun();

        Window.AllowUserResizing = true;

        // Approach 1
        foreach (var action in BeginRunActions)
        {
            action.Invoke();
        }

        // Approach 2
        OnBeginRun?.Invoke(this, EventArgs.Empty);
    }

    // Approach 1
    public void AddAction(Action? action)
    {
        if (action == null) return;

        BeginRunActions.Add(action);
    }
}

