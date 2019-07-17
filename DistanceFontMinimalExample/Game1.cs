using DistanceFont;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DistanceFontMinimalExample
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager graphics;
		private Font font;

		public Game1()
		{
			//Set window bounds and the content directory
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 600;
			Content.RootDirectory = "Content";
			
			//Enable shader support
			graphics.GraphicsProfile = GraphicsProfile.HiDef;
		}

		protected override void Initialize()
		{
			base.Initialize();
		}
		
		protected override void LoadContent()
		{
			//Load font shader and set the initial window size
			Font.Initialize(Content.Load<Effect>("ScreenMesh"));
			Font.OnResize(Window.ClientBounds.Width, Window.ClientBounds.Height);

			font = Content.Load<Font>("Arial");
		}

		protected override void Update(GameTime gameTime)
		{
			KeyboardState keyboardState = Keyboard.GetState();
			if (keyboardState.IsKeyDown(Keys.Escape))
				Exit();

			base.Update(gameTime);
		}
		
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			//Use sine wave to alternate sizes between 24 and 64 points
			float sin = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds);
			font.DrawString("Hello Monogame!", new Vector2(50, 75), 44 + 20 * sin);

			base.Draw(gameTime);
		}
	}
}
