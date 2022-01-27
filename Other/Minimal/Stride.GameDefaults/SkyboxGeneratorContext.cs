namespace Stride.GameDefaults;

// Taken from Stride.Assets.Skyboxes
public class SkyboxGeneratorContext : ShaderGeneratorContext
{
    public IServiceRegistry Services { get; private set; }

    //public EffectSystem EffectSystem { get; private set; }

    public GraphicsDevice GraphicsDevice { get; private set; }

    //public IGraphicsDeviceService GraphicsDeviceService { get; private set; }

    public RenderContext RenderContext { get; private set; }

    public RenderDrawContext RenderDrawContext { get; private set; }

    public SkyboxGeneratorContext(Game game)
    {
        Services = game.Services;
        //Services.AddService(fileProviderService);
        //Content = new ContentManager(Services);

        //Services.AddService<IContentManager>(Content);
        //Services.AddService(Content);

        GraphicsDevice = game.GraphicsDevice;
        //GraphicsDeviceService = new GraphicsDeviceServiceLocal(Services, GraphicsDevice);
        //Services.AddService(GraphicsDeviceService);

        //var graphicsContext = new GraphicsContext(GraphicsDevice);
        //Services.AddService(graphicsContext);

        //EffectSystem = new EffectSystem(Services);
        //EffectSystem.Compiler = EffectCompilerFactory.CreateEffectCompiler(Content.FileProvider, EffectSystem);

        //Services.AddService(EffectSystem);
        //EffectSystem.Initialize();
        //((IContentable)EffectSystem).LoadContent();
        //((EffectCompilerCache)EffectSystem.Compiler).CompileEffectAsynchronously = false;

        RenderContext = RenderContext.GetShared(Services);
        RenderDrawContext = new RenderDrawContext(Services, RenderContext, game.GraphicsContext);
    }
}

