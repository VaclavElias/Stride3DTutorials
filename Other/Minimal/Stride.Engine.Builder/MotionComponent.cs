using Stride.Core.Mathematics;
using Stride.Engine;

namespace Minimal
{
    public class MotionComponent : SyncScript
    {
        Vector3 _initialPosition = Vector3.Zero;

        public override void Start()
        {
            Log.Warning("Start Logging");

            _initialPosition = Entity.Transform.Position;
        }

        public override void Update()
        {

            Log.Warning("Test Logging");

            if (Vector3.Distance(_initialPosition, Entity.Transform.Position) <= 1)
            {
                Entity.Transform.Position.X += 0.03f;
            }
        }
    }
}
