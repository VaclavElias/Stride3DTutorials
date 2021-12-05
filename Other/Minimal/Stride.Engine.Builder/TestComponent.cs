using Stride.Engine;

namespace Minimal
{
    public class TestComponent : SyncScript
    {
        public override void Start()
        {
            Log.Warning("Start Logging");
        }

        public override void Update() {
            Log.Warning("Test Logging");
        }
    }
}
