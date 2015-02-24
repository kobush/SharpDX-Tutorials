using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using Color = SharpDX.Color;

namespace WinFormsApp
{
    public class Game8 : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        private Model _model;

        private Matrix _view;
        private Matrix _projection;
        private Matrix _world;

        public Game8()
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
            // Load the model (by default the model is loaded with a BasicEffect. 
            // Use ModelContentReaderOptions to change the behavior at loading time.
            //_model = Content.Load<Model>("Duck");
            //_model = Content.Load<Model>("Happy");
            _model = Content.Load<Model>("Skull");


            // Enable default lighting  on model.
            BasicEffect.EnableDefaultLighting(_model, true);

            base.LoadContent();
        }

        const float MaxModelSize = 10.0f;

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Calculate the bounds of this model
            BoundingSphere modelBounds = _model.CalculateBounds();

            // Calculates the world and the view based on the model size
            float scaling = MaxModelSize / modelBounds.Radius;

            _view = Matrix.LookAtRH(new Vector3(0, 0, -MaxModelSize * 2.5f), new Vector3(0, 0, 0), Vector3.UnitY);
            _projection = Matrix.PerspectiveFovRH(0.9f, GraphicsDevice.Viewport.AspectRatio, 0.1f, MaxModelSize * 10.0f);
            _world = 
                Matrix.Translation(-modelBounds.Center.X, -modelBounds.Center.Y, -modelBounds.Center.Z) * 
                Matrix.Scaling(scaling) * 
                Matrix.RotationY((float)gameTime.TotalGameTime.TotalSeconds);
        }

        /// <summary>
        /// Executes for each frame.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Clears the screen
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(Color.Transparent);

            // Draw the model
            _model.Draw(GraphicsDevice, _world, _view, _projection);

            // Handle base.Draw
            base.Draw(gameTime);
        }
    }
}
