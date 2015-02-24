using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using Buffer = SharpDX.Toolkit.Graphics.Buffer;

namespace WinFormsApp
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

// ================================== NEW CODE START =================================
        private Buffer<VertexInputType> _vertexBuffer;
        private VertexInputLayout _inputLayout;
        private Effect _effect;
// ================================== NEW CODE END ==================================

        public Game1()
        {
            // Creates a graphics manager. This is mandatory.
            _graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Enable debug device
            _graphicsDeviceManager.DeviceCreationFlags = DeviceCreationFlags.Debug;

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
        }

// ================================== NEW CODE START =================================
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
            _effect = ToDisposeContent(Content.Load<Effect>("ShaderGame1"));
            _effect.Name = "triangle_fx";
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

// ================================== NEW CODE START =================================
            // set vertex buffer
            GraphicsDevice.SetVertexInputLayout(_inputLayout);
            GraphicsDevice.SetVertexBuffer(_vertexBuffer);

            // apply effect
            _effect.CurrentTechnique.Passes[0].Apply();

            // draw triangle
            GraphicsDevice.Draw(PrimitiveType.TriangleList, _vertexBuffer.ElementCount);
// ================================== NEW CODE END ==================================
        }
    }
}
