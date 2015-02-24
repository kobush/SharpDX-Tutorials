using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using Buffer = SharpDX.Toolkit.Graphics.Buffer;
using Color = SharpDX.Color;

namespace WinFormsApp
{
    public class Game2 : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private Buffer<VertexInputType> _vertexBuffer;
        private VertexInputLayout _inputLayout;
        private Effect _effect;

        public Game2()
        {
            // Creates a graphics manager. This is mandatory.
            _graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Enable debug device
            _graphicsDeviceManager.DeviceCreationFlags = DeviceCreationFlags.Debug;

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
        }

        // Simple vertex structure with position and color.
        internal struct VertexInputType
        {
            [VertexElement("SV_POSITION")]
            public Vector3 Position;

            [VertexElement("COLOR")]
            public Color Color;

            public VertexInputType(Vector3 position, Color color)
            {
                Position = position;
                Color = color;
            }
        }

        /// <summary>
        /// Executes once on device creation. 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // define vertices for single triangle
            var vertices = new[]
            {
                new VertexInputType(new Vector3(-1.0f, -1.0f, 0.0f), Color.Red),
                new VertexInputType(new Vector3(0.0f, 1.0f, 0.0f), Color.Green),
                new VertexInputType(new Vector3(1.0f, -1.0f, 0.0f), Color.Blue),
            };

            // create vertex buffer
            _vertexBuffer = ToDisposeContent(Buffer.Vertex.New(GraphicsDevice, vertices));
            _vertexBuffer.Name = "triangle_vb";

            // create input layout
            _inputLayout = VertexInputLayout.FromBuffer(0, _vertexBuffer);

            // load effect file
			_effect = ToDisposeContent(Content.Load<Effect>("ShaderGame2"));
            _effect.Name = "triangle_fx";
		}

// ================================== NEW CODE START =================================
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //var world = Matrix.Identity;
            //TODO: rotate on Y axis
            var world = Matrix.RotationY((float) (gameTime.TotalGameTime.TotalSeconds*0.8f));

            // view transform: eye position, target, up vector
            // http://msdn.microsoft.com/en-us/library/windows/desktop/bb206342%28v=vs.85%29.aspx
            var view = Matrix.LookAtLH(new Vector3(0, 0, -4), new Vector3(0, 0, 0), Vector3.UnitY);

            // projection transform: field-of-view, aspect ration, near field, far field
            // http://msdn.microsoft.com/en-us/library/windows/desktop/bb147302%28v=vs.85%29.aspx
            var projection = Matrix.PerspectiveFovLH(MathUtil.DegreesToRadians(45), GraphicsDevice.Viewport.AspectRatio, 0.125f, 1000f);

            _effect.Parameters["WorldMatrix"].SetValue(world);
            _effect.Parameters["ViewMatrix"].SetValue(view);
            _effect.Parameters["ProjectionMatrix"].SetValue(projection);
        }
// ================================== NEW CODE END ==================================

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

// ================================== NEW CODE START =================================
            //TODO: make both sides visible
            //GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.CullNone);

            //TODO: show wireframe only
            GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.WireFrameCullNone);
// ================================== NEW CODE END ==================================

            // apply effect
            _effect.CurrentTechnique.Passes[0].Apply();

            // draw triangle
            GraphicsDevice.Draw(PrimitiveType.TriangleList, _vertexBuffer.ElementCount);
        }
    }
}
