namespace Stride.GameDefaults
{
    // Ignore this, this is just for illustration
    public static class PrimitiveProceduralModelBaseExtension
    {
        public static Model Generate(this PrimitiveProceduralModelBase proceduralModel, IServiceRegistry services)
        {
            var model = new Model();

            proceduralModel.Generate(services, model);

            return model;
        }
    }
}
