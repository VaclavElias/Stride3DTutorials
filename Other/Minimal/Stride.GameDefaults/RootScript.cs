namespace Stride.GameDefaults;

public class RootScript : SyncScript
{
    private readonly Action<Scene, IServiceRegistry>? startAction;
    private readonly Action<Scene, IServiceRegistry, GameTime>? updateAction;

    public RootScript(Action<Scene, IServiceRegistry>? start = null, Action<Scene, IServiceRegistry, GameTime>? update = null)
    {
        startAction = start;
        updateAction = update;
    }

    public override void Start()
    {
        //SceneSystem.SceneInstance ??= new SceneInstance(Services) { RootScene = new Scene() };

        startAction?.Invoke(SceneSystem.SceneInstance?.RootScene, Services);
    }

    public override void Update()
    {
        updateAction?.Invoke(SceneSystem.SceneInstance?.RootScene, Services, Game.UpdateTime);
    }
}

