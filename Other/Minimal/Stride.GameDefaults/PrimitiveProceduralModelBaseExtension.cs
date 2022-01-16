namespace Stride.GameDefaults
{
    // Ignore this, this is just for illustration, will be added to Engine #1258
    public static class PrimitiveProceduralModelBaseExtension
    {
        public static Model Generate2(this PrimitiveProceduralModelBase proceduralModel, IServiceRegistry services)
        {
            var model = new Model();

            proceduralModel.Generate(services, model);

            return model;
        }
    }
}
