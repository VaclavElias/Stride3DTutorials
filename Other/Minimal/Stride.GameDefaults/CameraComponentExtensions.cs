namespace Stride.GameDefaults;

public static class CameraComponentExtensions
{
    public static (Vector4 VectorNear, Vector4 VectorFar) ScreenPointToRay(this CameraComponent cameraComponent, Vector2 mousePosition)
    {
        var validMousePosition = mousePosition;

        var invertedMatrix = Matrix.Invert(cameraComponent.ViewProjectionMatrix);

        Vector3 position;
        position.X = validMousePosition.X * 2f - 1f;
        position.Y = 1f - validMousePosition.Y * 2f;
        position.Z = 0f;

        Vector4 vectorNear = Vector3.Transform(position, invertedMatrix);
        vectorNear /= vectorNear.W;

        position.Z = 1f;

        Vector4 vectorFar = Vector3.Transform(position, invertedMatrix);
        vectorFar /= vectorFar.W;

        return (vectorNear, vectorFar);
    }
}