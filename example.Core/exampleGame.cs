using System;
using System.Collections.Generic;
using System.Globalization;
using example.Core.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Net.Mime.MediaTypeNames;

namespace example.Core
{
    /// <summary>
    /// The main class for the game, responsible for managing game components, settings,
    /// and platform-specific configurations.
    /// </summary>
    public class exampleGame : Game
    {
        // Resources for drawing.
        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;
        private Texture2D texture1;
        private RenderTarget2D renderTarget;

        /// <summary>
        /// Initializes a new instance of the game. Configures platform-specific settings,
        /// initializes services like settings and leaderboard managers, and sets up the
        /// screen manager for screen transitions.
        /// </summary>
        public exampleGame()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            graphicsDeviceManager.GraphicsProfile = GraphicsProfile.HiDef;

            // Share GraphicsDeviceManager as a service.
            Services.AddService(typeof(GraphicsDeviceManager), graphicsDeviceManager);

            Content.RootDirectory = "Content";

            // Configure screen orientations.
            graphicsDeviceManager.SupportedOrientations =
                DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// <summary>
        /// Initializes the game, including setting up localization and adding the
        /// initial screens to the ScreenManager.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Load supported languages and set the default language.
            List<CultureInfo> cultures = LocalizationManager.GetSupportedCultures();
            var languages = new List<CultureInfo>();
            for (int i = 0; i < cultures.Count; i++)
            {
                languages.Add(cultures[i]);
            }

            // TODO You should load this from a settings file or similar,
            // based on what the user or operating system selected.
            var selectedLanguage = LocalizationManager.DEFAULT_CULTURE_CODE;
            LocalizationManager.SetCulture(selectedLanguage);
        }

        /// <summary>
        /// Loads game content, such as textures and particle systems.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture1 = Content.Load<Texture2D>("square");
            renderTarget = new RenderTarget2D(
                GraphicsDevice,
                graphicsDeviceManager.PreferredBackBufferWidth,
                graphicsDeviceManager.PreferredBackBufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.None
            );

            base.LoadContent();
        }

        /// <summary>
        /// Updates the game's logic, called once per frame.
        /// </summary>
        /// <param name="gameTime">
        /// Provides a snapshot of timing values used for game updates.
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            // Exit the game if the Back button (GamePad) or Escape key (Keyboard) is pressed.
            var keyboardState = Keyboard.GetState();
            if (
                GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || keyboardState.IsKeyDown(Keys.Escape)
            )
                Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game's graphics, called once per frame.
        /// </summary>
        /// <param name="gameTime">
        /// Provides a snapshot of timing values used for rendering.
        /// </param>
        protected override void Draw(GameTime gameTime)
        {
            // Pass 1: draw texture1 into the render target.
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(
                texture1,
                destinationRectangle: renderTarget.Bounds,
                color: Color.White
            );
            spriteBatch.End();

            // Pass 2: draw the render target to the backbuffer.
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.MonoGameOrange);

            spriteBatch.Begin();
            spriteBatch.Draw(
                renderTarget,
                destinationRectangle: GraphicsDevice.Viewport.Bounds,
                color: Color.White
            );
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
