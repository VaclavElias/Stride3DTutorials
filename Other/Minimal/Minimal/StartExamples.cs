public class StartExamples
{
    public void Main0()
    {
        using var game = new Game();

        game.Run(start: Start);

        void Start(Scene rootScene)
        {
            game.SetupBase3DScene();

            var entity = new Entity(new Vector3(1f, 0.5f, 3f))
            {
                new ModelComponent(new CubeProceduralModel().Generate(game.Services)),
                new RotationComponentScript()
            };

            entity.Scene = rootScene;
        }
    }

    public void Main1()
    {
        using var game = new Game();

        game.Run(start: Start);

        void Start(Scene rootScene)
        {
            game.SetupBase3DScene();

            var model = new CubeProceduralModel().Generate(game.Services);

            model.Materials.Add(new Material());

            var entity = new Entity(new Vector3(1f, 0.5f, 3f))
            {
                new ModelComponent(new CubeProceduralModel().Generate(game.Services)),
                new MotionComponentScript()
            };

            entity.Scene = rootScene;
        }
    }

    public void Main2()
    {
        using (var game = new Game())
        {
            var _entity = new Entity(new Vector3(1f, 0.5f, 3f));
            var _angle = 0f;
            var initialPosition = _entity.Transform.Position;

            game.Run(start: Start, update: Update);

            void Start(Scene rootScene)
            {
                //game.Window.AllowUserResizing = true;

                game.SetupBase3DScene();

                var model = new CubeProceduralModel().Generate(game.Services);

                model.Materials.Add(game.NewDefaultMaterial());

                _entity.Components.Add(new ModelComponent(model));

                _entity.Scene = rootScene;
            }

            void Update(Scene rootScene, GameTime time)
            {
                _angle += 1f * (float)time.Elapsed.TotalSeconds;

                var offset = new Vector3((float)Math.Sin(_angle), 0, (float)Math.Cos(_angle)) * 1f;

                _entity.Transform.Position = initialPosition + offset;
            }
        }
    }

    public void Main3()
    {
        using (var game = new Game())
        {
            var _entity = new Entity(new Vector3(1f, 0.5f, 3f));
            var _angle = 0f;
            var initialPosition = _entity.Transform.Position;

            game.Run(start: Start, update: Update);

            void Start(Scene rootScene)
            {
                // adds default camera, camera script, skybox, ground, ..like through UI
                game.SetupBase3DScene();

                var model = new CubeProceduralModel().Generate(game.Services);

                model.Materials.Add(game.NewDefaultMaterial());

                _entity.Components.Add(new ModelComponent(model));
                //_entity.Components.Add(new GameProfiler());
                //_entity.Components.Add(new RotationComponentScript());

                //var _entity = new Entity(new Vector3(1f, 0.5f, 3f))
                //{
                //    new ModelComponent(new CubeProceduralModel().Generate(game.Services)),
                //    new MotionComponentScript()
                //};

                _entity.Scene = rootScene;
            }

            void Update(Scene rootScene, GameTime time)
            {
                _angle += 5f * (float)time.Elapsed.TotalSeconds;

                var offset = new Vector3((float)Math.Sin(_angle), 0, (float)Math.Cos(_angle)) * 1f;

                _entity.Transform.Position = initialPosition + offset;

                //if (Vector3.Distance(initialPosition, _entity.Transform.Position) <= 2)
                //{
                //    _entity.Transform.Position.Z += 0.03f;
                //}
            }
        }
    }

    public void Main4()
    {
        using (var game = new Game())
        {
            var entity = new Entity(new Vector3(2f, 0, 2f));
            var angle = 0f;
            var initialPosition = entity.Transform.Position;

            game.Run(start: Start);

            void Start(Scene rootScene)
            {
                game.SetupBase3DScene();

                //var gizmo = new TransformationGizmo

                var vertices = new VertexPositionTexture[4];
                vertices[0].Position = new Vector3(0f, 0f, 1f);
                vertices[1].Position = new Vector3(0f, 1f, 0f);
                vertices[2].Position = new Vector3(0f, 1f, 1f);
                //vertices[3].Position = new Vector3(1f, 0f, 1f);
                var vertexBuffer = Stride.Graphics.Buffer.Vertex.New(game.GraphicsDevice, vertices,
                                                                     GraphicsResourceUsage.Dynamic);
                int[] indices = { 0, 2, 1 };
                var indexBuffer = Stride.Graphics.Buffer.Index.New(game.GraphicsDevice, indices);

                var customMesh = new Mesh
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

                model.Meshes.Add(customMesh);

                model.Materials.Add(game.NewDefaultMaterial());

                entity.Components.Add(new ModelComponent(model));

                entity.Scene = rootScene;
            }
        }
    }

    //public class MyGizmo : TransformationGizmo
    //{
    //}
}