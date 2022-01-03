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

    //private Action? _startAction2;
    //private Action<Scene, ServiceRegistry>? _startAction;

    //public void Run(Action start)
    //{
    //    _startAction2 = start;

    //    base.Run();
    //}

    //public void Run(Action<Scene, ServiceRegistry> start)
    //{
    //    _startAction = start;

    //    base.Run();
    //}

    public GameDefaults SetDefaults()
    {
        var gameDefaults = new GameDefaults(this);

        gameDefaults.Set3D();

        return gameDefaults;
    }

    protected override void BeginRun()
    {
        OnBeginRun?.Invoke(this, EventArgs.Empty);

        base.BeginRun();

        //_startAction?.Invoke(SceneSystem.SceneInstance.RootScene, Services);

        //_startAction2?.Invoke();
    }
}