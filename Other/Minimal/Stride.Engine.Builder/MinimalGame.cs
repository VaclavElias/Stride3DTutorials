namespace Stride.Engine.Builder;

public class MinimalGame : Game
{
    public List<Action> BeginRunActions { get; set; } = new();

    protected override void BeginRun()
    {
        base.BeginRun();

        Window.AllowUserResizing = true;

        foreach (var action in BeginRunActions)
        {
            action?.Invoke();
        }
    }

    public void AddAction(Action? action)
    {
        if (action == null) return;

        BeginRunActions.Add(action);
    }
}

public class MinimalGame2 : Game
{
    public event EventHandler<EventArgs>? OnBeginRun;

    protected override void BeginRun()
    {
        base.BeginRun();

        Window.AllowUserResizing = true;

        OnBeginRun?.Invoke(this, EventArgs.Empty);
    }
}

public class MinimalGame3 : Game
{
    public event EventHandler<EventArgs>? OnBeginRun;

    public void SetDefaults()
    {
        var gameDefaults = new GameDefaults(this);

        gameDefaults.Set3D();
    }

    protected override void BeginRun()
    {
        base.BeginRun();

        Window.AllowUserResizing = true;

        OnBeginRun?.Invoke(this, EventArgs.Empty);
    }
}