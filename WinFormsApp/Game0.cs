using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;

namespace WinFormsApp
{
    public class Game0 : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        public Game0()
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
        /// Executes for each frame.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // clear screen
            GraphicsDevice.Clear(Color.Coral);
        }
    }
}
