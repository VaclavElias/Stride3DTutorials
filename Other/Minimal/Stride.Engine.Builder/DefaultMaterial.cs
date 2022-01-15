namespace Stride.GameDefaults
{
    public class DefaultMaterial
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Color _defaulColour = Color.FromBgra(0xFF8C8C8C);

        public DefaultMaterial(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null) throw new ArgumentNullException(nameof(graphicsDevice));

            _graphicsDevice = graphicsDevice;
        }

        public Material Get(Color? color = null)
        {
            var materialDescription = new MaterialDescriptor
            {
                Attributes =
                {
                    Diffuse = new MaterialDiffuseMapFeature(new ComputeColor(color ?? _defaulColour)),
                    DiffuseModel = new MaterialDiffuseLambertModelFeature(),
                    Specular =  new MaterialMetalnessMapFeature(new ComputeFloat(1.0f)),
                    SpecularModel = new MaterialSpecularMicrofacetModelFeature(),
                    MicroSurface = new MaterialGlossinessMapFeature(new ComputeFloat(0.65f))
                }
            };

            return Material.New(_graphicsDevice, materialDescription);
        }
    }
}
