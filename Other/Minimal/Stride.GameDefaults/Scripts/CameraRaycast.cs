namespace Stride.GameDefaults.Scripts;

public class CameraRaycast : SyncScript
{
    private Simulation? _simulation;
    private CameraComponent? _camera;

    public override void Start()
    {
        _camera = Entity.Get<CameraComponent>();
        _simulation = this.GetSimulation();
        //_simulation.ColliderShapesRendering = true;
    }

    public override void Update()
    {
        if (_camera is null || _simulation is null) return;

        if (Input.HasMouse && Input.IsMouseButtonPressed(MouseButton.Left))
        {
            Log.Warning($"Mouse button clicked on {Entity.Name}");

            var result = ScreenPositionToWorldPositionRaycast(Input.MousePosition, _camera, _simulation);

            if (result.Succeeded)
            {
                Log.Warning($"Entity hit: {result.Collider.Entity.Name}");

                Log.Error(result.Collider.Entity.Name);

                //_ = result.Collider.Entity.Name switch
                //{
                //    "Building" => BuildingAction(result.Collider.Entity),
                //    "Vehicle" => VehicleAction(result.Collider.Entity),
                //    _ => Empty()
                //};

                //HighlightItem(vehicle);
            }
        }
    }

    public HitResult ScreenPositionToWorldPositionRaycast(Vector2 mousePosition, CameraComponent camera, Simulation simulation)
    {
        var invertedMatrix = Matrix.Invert(camera.ViewProjectionMatrix);

        Vector3 position;
        position.X = mousePosition.X * 2f - 1f;
        position.Y = 1f - mousePosition.Y * 2f;
        position.Z = 0f;

        Vector4 vectorNear = Vector3.Transform(position, invertedMatrix);
        vectorNear /= vectorNear.W;

        position.Z = 1f;

        Vector4 vectorFar = Vector3.Transform(position, invertedMatrix);
        vectorFar /= vectorFar.W;

        return simulation.Raycast(vectorNear.XYZ(), vectorFar.XYZ());
    }
}

