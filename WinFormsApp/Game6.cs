using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using Color = SharpDX.Color;
using Texture2D = SharpDX.Toolkit.Graphics.Texture2D;

namespace WinFormsApp
{
    public class Game6 : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        private Effect _effect;
        private Texture2D _texture;

        private GeometricPrimitive[] _primitives;

        public Game6()
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
// ================================== NEW CODE END ==================================

            // load effect file
            _effect = ToDisposeContent(Content.Load<Effect>("ShaderGame5"));

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

            // Bind resources 
            //TODO enable texture
            //_effect.Parameters["Texture"].SetResource(_texture);
            //_effect.Parameters["UseTexture"].SetValue(true);*/
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

// ================================== NEW CODE START =================================
            int cols = 4;
            int rows = 2;
            float space = 3f;

            // show wireframe only
            GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.WireFrameCullNone);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    int i = y*cols + x;
                    if (i < _primitives.Length)
                    {
                        _effect.Parameters["WorldMatrix"].SetValue(
                            world*Matrix.Translation((cols/2f - x - 0.5f)*space, (rows/2f - y - 0.5f)*space, 0));
                        _effect.Techniques[1].Passes[0].Apply();
                        _primitives[i].Draw();
                    }
                }
            }
// ================================== NEW CODE END ==================================
        }
    }
}
