using (var game = new Game())
{
    game.Run(start: Start);

    void Start(Scene rootScene)
    {
        game.SetupBase3DScene();

        // Approach 1

        var myModel = new MyProceduralModel();
        var model2 = myModel.Generate(game.Services);
        model2.Materials.Add(game.NewDefaultMaterial());

        var meshEntity2 = new Entity(new Vector3(1, 1, 1));
        meshEntity2.Components.Add(new ModelComponent(model2));
        meshEntity2.Scene = rootScene;


        // Approach 2

        var vertices = new VertexPositionTexture[4];
        vertices[0].Position = new Vector3(0f, 0.5f, 0f); // Orange
        vertices[1].Position = new Vector3(0f, 1f, 0f); // Blue
        vertices[2].Position = new Vector3(0f, 1f, 1f); // Green
        vertices[3].Position = new Vector3(0f, 0f, 1f); // Red

        var vertexBuffer = Stride.Graphics.Buffer.Vertex.New(game.GraphicsDevice, vertices,
                                                                     GraphicsResourceUsage.Dynamic);
        // clock wise direction of vertices
        // 1,3,2
        // 0,3,2
        // 0,3,1
        // 2,1,0
        // 2,1,3
        // 2,0,3
        // 3,1,0
        int[] indices = { 0, 3, 2, 0, 2, 1 };
        var indexBuffer = Stride.Graphics.Buffer.Index.New(game.GraphicsDevice, indices);

        var mesh = new Mesh
        {
            Draw = new MeshDraw
            {
                /* Vertex buffer and index buffer setup */
                PrimitiveType = PrimitiveType.TriangleList,
                DrawCount = indices.Length,
                IndexBuffer = new IndexBufferBinding(indexBuffer, true, indices.Length),
                VertexBuffers = new[] { new VertexBufferBinding(vertexBuffer,
                                  VertexPositionTexture.Layout, vertexBuffer.ElementCount) },
            }
        };

        var model = new Model();
        model.Meshes.Add(mesh);
        model.Materials.Add(game.NewDefaultMaterial());

        var meshEntity = new Entity(new Vector3(0, 0, 0));
        meshEntity.Components.Add(new ModelComponent(model));
        meshEntity.Scene = rootScene;


        var entityDot1 = game.CreatePrimitive(PrimitiveModelType.Sphere, material: game.NewDefaultMaterial(Color.Orange), includeCollider: false);
        entityDot1.Transform.Scale = new Vector3(0.1f);
        entityDot1.Transform.Position = vertices[0].Position;
        meshEntity.AddChild(entityDot1);

        var entityDot2 = game.CreatePrimitive(PrimitiveModelType.Sphere, material: game.NewDefaultMaterial(Color.Blue), includeCollider: false);
        entityDot2.Transform.Scale = new Vector3(0.1f);
        entityDot2.Transform.Position = vertices[1].Position;
        meshEntity.AddChild(entityDot2);

        var entityDot3 = game.CreatePrimitive(PrimitiveModelType.Sphere, material: game.NewDefaultMaterial(Color.Green), includeCollider: false);
        entityDot3.Transform.Scale = new Vector3(0.1f);
        entityDot3.Transform.Position = vertices[2].Position;
        meshEntity.AddChild(entityDot3);

        var entityDot4 = game.CreatePrimitive(PrimitiveModelType.Sphere, material: game.NewDefaultMaterial(Color.Red), includeCollider: false);
        entityDot4.Transform.Scale = new Vector3(0.1f);
        entityDot4.Transform.Position = vertices[3].Position;
        meshEntity.AddChild(entityDot4);

        var entity = game.CreatePrimitive(PrimitiveModelType.Capsule);
        entity.Transform.Position = new Vector3(0, 8, 0);
        entity.Scene = rootScene;
    }
}

public class MyProceduralModel : PrimitiveProceduralModelBase
{
    // A custom property that shows up in Game Studio
    /// <summary>
    /// Gets or sets the size of the model.
    /// </summary>
    public Vector3 Size { get; set; } = Vector3.One;

    protected override GeometricMeshData<VertexPositionNormalTexture> CreatePrimitiveMeshData()
    {
        // First generate the arrays for vertices and indices with the correct size
        var vertexCount = 4;
        var indexCount = 6;
        var vertices = new VertexPositionNormalTexture[vertexCount];
        var indices = new int[indexCount];

        // Create custom vertices, in this case just a quad facing in Y direction
        var normal = Vector3.UnitZ;
        vertices[0] = new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, 0) * Size, normal, new Vector2(0, 0));
        vertices[1] = new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, 0) * Size, normal, new Vector2(1, 0));
        vertices[2] = new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, 0) * Size, normal, new Vector2(0, 1));
        vertices[3] = new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, 0) * Size, normal, new Vector2(1, 1));

        // Create custom indices
        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 2;
        indices[3] = 1;
        indices[4] = 3;
        indices[5] = 2;

        // Create the primitive object for further processing by the base class
        return new GeometricMeshData<VertexPositionNormalTexture>(vertices, indices, isLeftHanded: false) { Name = "MyModel" };
    }
}

//using (var game = new Game())
//{
//    game.Run(start: Start);

//    void Start(Scene rootScene)
//    {
//        game.SetupBase3DScene();

//        var entity = new Entity(new Vector3(1f, 0.5f, 3f))
//            {
//                new ModelComponent(new CubeProceduralModel().Generate(game.Services)),
//                new RotationComponentScript()
//            };

//        entity.Scene = rootScene;

//        var entity2 = game.CreatePrimitive(PrimitiveModelType.Teapot, material: game.NewDefaultMaterial(Color.Blue));
//        entity2.Scene = rootScene;

//        var entity3 = game.CreatePrimitive(PrimitiveModelType.Cube);
//        entity3.Transform.Position = new Vector3(0, 2, 0);
//        entity3.Scene = rootScene;

//        var entity4 = game.CreatePrimitive(PrimitiveModelType.Torus);
//        entity4.Transform.Position = new Vector3(0, 4, 0);
//        entity4.Scene = rootScene;

//        var entity5 = game.CreatePrimitive(PrimitiveModelType.Cone);
//        entity5.Transform.Position = new Vector3(0, 6, 0);
//        entity5.Scene = rootScene;

//        var entity6 = game.CreatePrimitive(PrimitiveModelType.Capsule);
//        entity6.Transform.Position = new Vector3(0, 8, 0);
//        entity6.Scene = rootScene;
//    }
//}

//using (var game = new Game())
//{
//    var entity = new Entity(new Vector3(1f, 0.5f, 3f));
//    var cubeGenerator = new CubesGenerator(game.Services);
//    var cameraEntityName = "Camera";

//    CameraComponent? cameraComponent = null;
//    Simulation? simulation = null;

//    game.Run(start: Start, update: Update);

//    void Start(Scene rootScene)
//    {
//        game.AddGraphicsCompositor();
//        game.AddMouseLookCamera(game.AddCamera(cameraEntityName));
//        game.AddLight();
//        game.AddSkybox();
//        game.AddGround();
//        game.AddProfiler();

//        cameraComponent = rootScene.Entities.SingleOrDefault(x => x.Name == cameraEntityName)?.Get<CameraComponent>();
//        simulation = game.SceneSystem.SceneInstance.GetProcessor<PhysicsProcessor>()?.Simulation;

//        var model = new CubeProceduralModel().Generate(game.Services);

//        model.Materials.Add(game.NewDefaultMaterial());

//        entity.Components.Add(new ModelComponent(model));

//        entity.Scene = rootScene;

//        for (int i = 0; i < 1000; i++)
//        {
//            entity.AddChild(cubeGenerator.GetCube());
//        }
//    }

//    void Update(Scene rootScene, GameTime time)
//    {
//        if (simulation is null || cameraComponent is null) return;

//        if (game.Input.HasMouse && game.Input.IsMouseButtonPressed(MouseButton.Left))
//        {
//            var ray = cameraComponent.ScreenPointToRay(game.Input.MousePosition);

//            var hitResult = simulation.Raycast(ray.VectorNear.XYZ(), ray.VectorFar.XYZ());

//            if (hitResult.Succeeded)
//            {
//                hitResult.Collider.Entity.Scene = null;
//            }
//        }
//    }
//}

//using (var game = new Game())
//{
//    var entity = new Entity(new Vector3(1f, 0.5f, 3f));
//    var angle = 0f;
//    var initialPosition = entity.Transform.Position;

//    game.Run(start: Start, update: Update);

//    void Start(Scene rootScene)
//    {
//        game.SetupBase3DScene();
//        game.AddProfiler();

//        var model = new CubeProceduralModel().Generate(game.Services);

//        model.Materials.Add(game.NewDefaultMaterial());

//        entity.Components.Add(new ModelComponent(model));

//        entity.Scene = rootScene;
//    }

//    void Update(Scene rootScene, GameTime time)
//    {
//        angle += 1f * (float)time.Elapsed.TotalSeconds;

//        var offset = new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle)) * 1f;

//        entity.Transform.Position = initialPosition + offset;
//    }
//}


//using (var game = new Game())
//{
//    game.Run(start: Start);

//    void Start(Scene rootScene)
//    {
//        // adds default camera, camera script, skybox, ground, ..like through UI
//        game.SetupBase3DScene();
//        game.AddProfiler();

//        var model = new CubeProceduralModel().Generate(game.Services);

//        model.Materials.Add(game.NewDefaultMaterial());

//        var entity = new Entity(new Vector3(1f, 0.5f, -3f))
//        {
//            new ModelComponent(model),
//            new MotionComponentScript()
//        };

//        entity.Scene = rootScene;

//        var entity2 = new Entity(new Vector3(1, 0.5f, 3))
//            {
//                new ModelComponent(new CubeProceduralModel().Generate(game.Services)),
//                new MotionComponentScript()
//            };

//        entity2.Scene = rootScene;
//    }
//}