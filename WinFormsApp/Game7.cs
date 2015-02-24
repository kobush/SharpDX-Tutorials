using SharpDX;
using SharpDX.Direct3D11;
using Color = SharpDX.Color;

namespace WinFormsApp
{
    using SharpDX.Toolkit;
    using SharpDX.Toolkit.Graphics;

    public class Game7 : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        private BasicEffect _effect;
        private Texture2D _texture;

        private GeometricPrimitive[] _primitives;

        public Game7()
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

            _primitives = new[]
            {
                GeometricPrimitive.Cube.New(GraphicsDevice, 1.5f),
                GeometricPrimitive.Cylinder.New(GraphicsDevice, 2f),
                GeometricPrimitive.Sphere.New(GraphicsDevice, 2f),
                GeometricPrimitive.GeoSphere.New(GraphicsDevice, 2f),
                GeometricPrimitive.Plane.New(GraphicsDevice, 2f, 2f),
                GeometricPrimitive.Torus.New(GraphicsDevice, 2f, 0.6f),
                GeometricPrimitive.Teapot.New(GraphicsDevice, 2f),
            };


// ================================== NEW CODE START =================================
            // load effect file
            _effect = new BasicEffect(GraphicsDevice);
            _effect.EnableDefaultLighting();
            _effect.PreferPerPixelLighting = true;

            // 3 directional lights
            _effect.DirectionalLight0.Enabled = true;
            _effect.DirectionalLight1.Enabled = true;
            _effect.DirectionalLight2.Enabled = true;
// ================================== NEW CODE END ===================================

            // load texture resource
            //_texture = ToDisposeContent(Content.Load<Texture2D>("WoodCrate01"));
            _texture = ToDisposeContent(Content.Load<Texture2D>("GeneticaMortarlessBlocks"));
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // view matrix: eye position, target, up vector
            var eye = new Vector3(0, 3, -8);
            var view = Matrix.LookAtRH(eye, new Vector3(0, 0, 0), Vector3.UnitY);

            // projection matrix: field-of-view, aspect ration, near field, far field
            var projection = Matrix.PerspectiveFovRH(MathUtil.DegreesToRadians(45), GraphicsDevice.Viewport.AspectRatio, 0.125f, 100f);

// ================================== NEW CODE START =================================
            _effect.View = view;
            _effect.Projection = projection;

            _effect.DiffuseColor = (Vector4) Color.Orange;
            _effect.PreferPerPixelLighting = true;

            _effect.TextureEnabled = true;
            _effect.Texture = _texture;
// ================================== NEW CODE END ===================================
        }

        /// <summary>
        /// Executes for each frame.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // clear screen
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var time = (float)(gameTime.TotalGameTime.TotalSeconds * 0.8f);
            var world = Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f);
            //var world = Matrix.RotationY(time);

            int cols = 4;
            int rows = 2;
            float space = 3f;

            // show wireframe only
            //GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.WireFrameCullNone);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    int i = y*cols + x;
                    if (i < _primitives.Length)
                    {
// ================================== NEW CODE START =================================
                        _effect.World = world *
                            Matrix.Translation((cols/2f - x - 0.5f)*space, (rows/2f - y - 0.5f)*space, 0);

                        _primitives[i].Draw(_effect);
// ================================== NEW CODE END ===================================
                    }
                }
            }
        }
    }
}
