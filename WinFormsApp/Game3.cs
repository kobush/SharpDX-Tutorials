using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using Buffer = SharpDX.Toolkit.Graphics.Buffer;
using Color = SharpDX.Color;

namespace WinFormsApp
{
    public class Game3 : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private Buffer<VertexPositionColor> _vertexBuffer;
        private VertexInputLayout _inputLayout;
        private Effect _effect;

        public Game3()
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

// ================================== NEW CODE START =================================

            //var colors = new[] { Color.Red, Color.Red, Color.Green, Color.Green, Color.Blue, Color.Blue };
            //var colors = new[] { Color.Orange, Color.Orange, Color.OrangeRed, Color.OrangeRed, Color.DarkOrange, Color.DarkOrange };
            var colors = new[] { Color.Orange, Color.Orange, Color.Orange, Color.Orange, Color.Orange, Color.Orange };

            // Creates vertices for the cube
            var cubeVertices =
                new[]
                    {
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), colors[0]), // Back
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), colors[0]),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), colors[0]),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), colors[0]),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), colors[0]),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), colors[0]),

                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), colors[1]), // Front
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), colors[1]),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), colors[1]),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), colors[1]),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), colors[1]),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), colors[1]),

                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), colors[2]), // Top
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), colors[2]),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), colors[2]),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), colors[2]),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), colors[2]),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), colors[2]),

                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), colors[3]), // Bottom
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), colors[3]),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), colors[3]),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), colors[3]),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), colors[3]),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), colors[3]),

                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), colors[4]), // Left
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), colors[4]),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), colors[4]),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), colors[4]),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), colors[4]),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), colors[4]),

                        new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), colors[5]), // Right
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), colors[5]),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), colors[5]),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), colors[5]),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), colors[5]),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), colors[5]),
                    };

            // create vertex buffer
            _vertexBuffer = ToDisposeContent(Buffer.Vertex.New(GraphicsDevice, cubeVertices));

            _inputLayout = VertexInputLayout.FromBuffer(0, _vertexBuffer);
// ================================== NEW CODE END ==================================

            // load effect file
            _effect = ToDisposeContent(Content.Load<Effect>("ShaderGame2"));
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var time = (float) (gameTime.TotalGameTime.TotalSeconds*0.8f);
            var world = Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f);

            // view matrix: eye position, target, up vector
            var view = Matrix.LookAtRH(new Vector3(0, 0, -5), new Vector3(0, 0, 0), Vector3.UnitY);

            // projection matrix: field-of-view, aspect ration, near field, far field
            var projection = Matrix.PerspectiveFovRH(MathUtil.DegreesToRadians(45), GraphicsDevice.Viewport.AspectRatio, 0.125f, 100f);

            _effect.Parameters["WorldMatrix"].SetValue(world);
            _effect.Parameters["ViewMatrix"].SetValue(view);
            _effect.Parameters["ProjectionMatrix"].SetValue(projection);
        }

        /// <summary>
        /// Executes for each frame.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // clear screen
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // draw triangle
            GraphicsDevice.SetVertexInputLayout(_inputLayout);
            GraphicsDevice.SetVertexBuffer(_vertexBuffer);

            // make both sides visible
            //TODO GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.CullNone);

            // show wireframe only
            //TODO GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.WireFrameCullNone);

            _effect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.Draw(PrimitiveType.TriangleList, _vertexBuffer.ElementCount);
        }
    }
}
