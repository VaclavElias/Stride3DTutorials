namespace Minimal.Components;

public class MotionComponentScript : SyncScript
{
    private Vector3 _initialPosition = Vector3.Zero;

    public override void Start()
    {
        Log.Warning("Start Logging");

        _initialPosition = Entity.Transform.Position;
    }

    public override void Update()
    {
        if (Vector3.Distance(_initialPosition, Entity.Transform.Position) <= 1)
            Entity.Transform.Position.X += 0.03f;
    }
}

