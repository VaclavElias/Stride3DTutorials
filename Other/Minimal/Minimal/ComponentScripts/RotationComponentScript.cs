namespace Minimal.ComponentScripts;

public class RotationComponentScript : SyncScript
{
    private Vector3 _initialPosition = Vector3.Zero;
    private float _rotateSpeed = 5f;
    private float _radius = 1f;
    private float _angle = 0f;

    public override void Start()
    {
        Log.Warning("Start Logging");

        _initialPosition = Entity.Transform.Position;
    }

    public override void Update()
    {
        _angle += _rotateSpeed * (float)Game.UpdateTime.Elapsed.TotalSeconds;

        var offset = new Vector3((float)Math.Sin(_angle), 0, (float)Math.Cos(_angle)) * _radius;

        Entity.Transform.Position = _initialPosition + offset;
    }
}