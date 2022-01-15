namespace Stride.GameDefaults;

public class RootScript : SyncScript
{
    private readonly Action<Scene, Core.IServiceRegistry>? startAction;
    private readonly Action<Scene, Core.IServiceRegistry, GameTime>? updateAction;

    public RootScript(Action<Scene, Core.IServiceRegistry>? start = null, Action<Scene, Core.IServiceRegistry, GameTime>? update = null)
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

