using SharpDX;
using SharpDX.Direct3D11;
using Color = SharpDX.Color;

namespace WinFormsApp
{
    using SharpDX.Toolkit;
    using SharpDX.Toolkit.Graphics;

    public class Game5 : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private Buffer<VertexPositionNormalTexture> _vertexBuffer;
        private VertexInputLayout _inputLayout;
        private Effect _effect;

// ================================== NEW CODE START =================================
        private Texture2D _texture;
// ================================== NEW CODE END ==================================

        public Game5()
        {
            // Creates a graphics manager. This is mandatory.
            _graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Enable debug device
            _graphicsDeviceManager.DeviceCreationFlags = DeviceCreationFlags.Debug;

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Executes once on device creation. 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // Creates vertices for the cube
            var cubeVertices = 
                new[]
                    {
                        new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(0,0,1), new Vector2(1,0)), // Back
                        new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(0,0,1), new Vector2(1,1)),
                        new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0,0,1), new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(0,0,1), new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0,0,1), new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), new Vector3(0,0,1), new Vector2(0,0)),

                        new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(0,0,-1), new Vector2(0,1)), // Front
                        new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), new Vector3(0,0,-1), new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), new Vector3(0,0,-1), new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(0,0,-1), new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), new Vector3(0,0,-1), new Vector2(1,1)),
                        new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), new Vector3(0,0,-1), new Vector2(1,0)),

                        new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(0,1,0), new Vector2(0,0)), // Top
                        new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), new Vector3(0,1,0), new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), new Vector3(0,1,0), new Vector2(1,1)),
                        new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(0,1,0), new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), new Vector3(0,1,0), new Vector2(1,1)),
                        new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0,1,0), new Vector2(1,0)),

                        new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(0,-1,0), new Vector2(1,1)), // Bottom
                        new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), new Vector3(0,-1,0), new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(0,-1,0), new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(0,-1,0), new Vector2(1,1)),
                        new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), new Vector3(0,-1,0), new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), new Vector3(0,-1,0), new Vector2(0,0)),

                        new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(-1,0,0), new Vector2(0,0)), // Left
                        new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(-1,0,0), new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), new Vector3(-1,0,0), new Vector2(1,1)),
                        new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(-1,0,0), new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), new Vector3(-1,0,0), new Vector2(1,1)),
                        new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(-1,0,0), new Vector2(1,0)),

                        new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), new Vector3(1,0,0), new Vector2(1,1)), // Right
                        new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), new Vector3(1,0,0), new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), new Vector3(1,0,0), new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), new Vector3(1,0,0), new Vector2(1,1)),
                        new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1,0,0), new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), new Vector3(1,0,0), new Vector2(0,0)),
                    };

            // create vertex buffer
            _vertexBuffer = ToDisposeContent(Buffer.Vertex.New(GraphicsDevice, cubeVertices));
            _vertexBuffer.Name = "cube_vb";

            _inputLayout = VertexInputLayout.FromBuffer(0, _vertexBuffer);

            // load effect file
            _effect = ToDisposeContent(Content.Load<Effect>("ShaderGame5"));
            _effect.Name = "cube_fx";

// ================================== NEW CODE START =================================
            // load texture resource
            _texture = ToDisposeContent(Content.Load<Texture2D>("WoodCrate01"));
            //TODO _texture = ToDisposeContent(Content.Load<Texture2D>("GeneticaMortarlessBlocks"));
            _texture.Name = "cube_tx";
// ================================== NEW CODE END ==================================
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // view matrix: eye position, target, up vector
            var eye = new Vector3(0, 3, -8);
            var view = Matrix.LookAtRH(eye, new Vector3(0, 0, 0), Vector3.UnitY);

            // projection matrix: field-of-view, aspect ration, near field, far field
            var projection = Matrix.PerspectiveFovRH(MathUtil.DegreesToRadians(45), GraphicsDevice.Viewport.AspectRatio, 0.125f, 100f);

            _effect.Parameters["ViewMatrix"].SetValue(view);
            _effect.Parameters["ProjectionMatrix"].SetValue(projection);

            // set lighting
            _effect.Parameters["DiffuseColor"].SetValue((Vector4)Color.Orange);
            _effect.Parameters["AmbientColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
            _effect.Parameters["DirLightColor"].SetValue(new Vector3(0.9f, 0.9f, 0.9f));
            var dirToLight = new Vector3(1,1,-2);
            dirToLight.Normalize();
            _effect.Parameters["DirToLight"].SetValue(dirToLight);

            _effect.Parameters["EyePosition"].SetValue(eye);
            _effect.Parameters["SpecularColor"].SetValue(new Vector3(1f, 1f, 1f));
            _effect.Parameters["SpecularExp"].SetValue(16f);

// ================================== NEW CODE START =================================
            // Bind resources 
            _effect.Parameters["Texture"].SetResource(_texture);
            _effect.Parameters["UseTexture"].SetValue(true);
// ================================== NEW CODE END ==================================
        }

        /// <summary>
        /// Executes for each frame.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // clear screen
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // set vertex buffer
            GraphicsDevice.SetVertexInputLayout(_inputLayout);
            GraphicsDevice.SetVertexBuffer(_vertexBuffer);

            var time = (float)(gameTime.TotalGameTime.TotalSeconds * 0.8f);
            var world = Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f);
            //var world = Matrix.RotationY(time);


            // cube 1: no lighting
            _effect.Parameters["WorldMatrix"].SetValue(world * Matrix.Translation(4,0,0));
            _effect.Techniques[0].Passes[0].Apply();
            GraphicsDevice.Draw(PrimitiveType.TriangleList, _vertexBuffer.ElementCount);


            // cube 2: lighting - no specular
            _effect.Parameters["SpecularExp"].SetValue(0);
            _effect.Parameters["WorldMatrix"].SetValue(world * Matrix.Translation(0, 0, 0));
            _effect.Techniques[1].Passes[0].Apply();
            GraphicsDevice.Draw(PrimitiveType.TriangleList, _vertexBuffer.ElementCount);


            // cube 3: lighting - with specular
            _effect.Parameters["SpecularExp"].SetValue(16f);
            _effect.Parameters["WorldMatrix"].SetValue(world * Matrix.Translation(-4, 0, 0));
            _effect.Techniques[1].Passes[0].Apply();
            GraphicsDevice.Draw(PrimitiveType.TriangleList, _vertexBuffer.ElementCount);
        }
    }
}
