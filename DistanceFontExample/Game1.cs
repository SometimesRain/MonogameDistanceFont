using DistanceFont;
using DistanceFontExample.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace DistanceFontExample
{
	public enum Demo
	{
		Instructions,
		Outline,
		Paragraph,
		Effect,
		Input
	}
	public class Game1 : Game
	{
		private GraphicsDeviceManager graphics;
		private FpsCounter fpsCounter;

		private Font[] fonts;
		private int fontIndex;

		private Demo demo;
		private float scale = 24;
		private string input = "";

		public Game1(int width, int height)
		{
			//Set window bounds and the content directory
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = width;
			graphics.PreferredBackBufferHeight = height;
			Content.RootDirectory = "Content";

			//Enable shader support
			graphics.GraphicsProfile = GraphicsProfile.HiDef;

			//Set fullscreen mode to bordeless window
			graphics.HardwareModeSwitch = false;

			//Disable v-sync
			graphics.SynchronizeWithVerticalRetrace = false;

			//Show mouse cursor and disable framelimiting
			IsMouseVisible = true;
			IsFixedTimeStep = false;

			//Allow window resizing and set resize callback
			Window.ClientSizeChanged += (sender, e) => OnResize(Window.ClientBounds.Width, Window.ClientBounds.Height);
			Window.AllowUserResizing = true;
		}

		private void OnResize(int width, int height)
		{
			//Font shader needs to know of window size changes
			Font.OnResize(width, height);
		}

		private void ChangeDemo(Demo next)
		{
			//Subscribe to or unsubscribe from TextInput event (needed for input handling)
			if (demo == Demo.Input) Window.TextInput -= OnTextInput;
			if (next == Demo.Input) Window.TextInput += OnTextInput;

			//Set the initial font size for this demo
			if (next == Demo.Instructions)
				scale = 24;
			else if (next == Demo.Outline)
				scale = 36;
			else if (next == Demo.Paragraph)
				scale = 16;
			else if (next == Demo.Effect)
				scale = 60;
			else if (next == Demo.Input)
				scale = 48;

			//Clear input and change demo
			input = "";
			demo = next;
		}

		private void OnTextInput(object sender, TextInputEventArgs args)
		{
			//Backspace removes the last character
			if (args.Key == Keys.Back)
				input = input.Substring(0, MathHelper.Max(input.Length - 1, 0));
			//Enter adds a new line
			else if (args.Key == Keys.Enter)
				input += '\n';
			//Only add the character if it's supported by the font
			else if (fonts[fontIndex].Glyphs.ContainsKey(args.Character))
				input += args.Character;
		}

		protected override void Initialize()
		{
			//Initialize FPS counter
			fpsCounter = new FpsCounter();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			//Load font shader and set the initial window size
			Font.Initialize(Content.Load<Effect>("ScreenMesh"));
			OnResize(Window.ClientBounds.Width, Window.ClientBounds.Height);
			
			//Load a selection of fonts
			fonts = new Font[]
			{
				Content.Load<Font>("Arial"),
				Content.Load<Font>("Bahnschrift"),
				Content.Load<Font>("Cambria"),
				Content.Load<Font>("Candara"),
				Content.Load<Font>("Helvetica"),
				Content.Load<Font>("MyriadPro"),
				Content.Load<Font>("PalatinoLinotype"),
				Content.Load<Font>("Rosario"),
				Content.Load<Font>("Verdana")
			};
		}

		protected override void Update(GameTime gameTime)
		{
			//===================================== Input =====================================
			//Always update input first
			Input.Update();

			//==================================== Hotkeys ====================================
			//Exit application with Esc
			if (Input.KeyPressed(Keys.Escape))
				Exit();

			//Toggle fullscreen with Alt + Enter
			if (Input.KeyDown(Keys.LeftAlt) && Input.KeyPressed(Keys.Enter))
			{
				graphics.IsFullScreen = !graphics.IsFullScreen;
				graphics.ApplyChanges();
			}

			//Toggle v-sync with Alt + V
			if (Input.KeyDown(Keys.LeftAlt) && Input.KeyPressed(Keys.V))
			{
				graphics.SynchronizeWithVerticalRetrace = !graphics.SynchronizeWithVerticalRetrace;
				graphics.ApplyChanges();

				//Reset FPS counter to get more accurate reading
				fpsCounter.Reset();
			}

			//=================================== Font size ===================================
			if (Input.KeyDown(Keys.Up))   scale += scale * 0.4f * (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (Input.KeyDown(Keys.Down)) scale -= scale * 0.4f * (float)gameTime.ElapsedGameTime.TotalSeconds;

			//===================================== Font ======================================
			//Right/left for next/previous font
			if (Input.KeyPressed(Keys.Left))  fontIndex--;
			if (Input.KeyPressed(Keys.Right)) fontIndex++;

			//Bounds checking
			if (fontIndex < 0) fontIndex = fonts.Length - 1;
			if (fontIndex > fonts.Length - 1) fontIndex = 0;

			//===================================== Demos =====================================
			if (Input.KeyPressed(Keys.D1)) ChangeDemo(Demo.Instructions);
			if (Input.KeyPressed(Keys.D2)) ChangeDemo(Demo.Outline);
			if (Input.KeyPressed(Keys.D3)) ChangeDemo(Demo.Paragraph);
			if (Input.KeyPressed(Keys.D4)) ChangeDemo(Demo.Effect);
			if (Input.KeyPressed(Keys.D5)) ChangeDemo(Demo.Input);

			//==================================== Update =====================================
			fpsCounter.Update(gameTime);

			base.Update(gameTime);
		}
		
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			
			//Always draw FPS and font info
			fonts[fontIndex].DrawString($"Font: {fonts[fontIndex].Name} FPS: {fpsCounter.AverageFps}", Color.Black, new Vector2(5, 5), 12);
			
			//============================================================= Instructions =============================================================
			if (demo == Demo.Instructions)
			{
				//Basic font rendering demo
				string instruction = "Change font with left and right arrow keys.\nChange font size with up and down arrow keys.\nSelect a demo with number keys 1-5.";
				fonts[fontIndex].DrawString(instruction, Color.Black, new Vector2(50, 75), new Vector2(-50, 0), scale, 1.5f);
			}
			//=============================================================== Outline ================================================================
			else if (demo == Demo.Outline)
			{
				float spacing = scale * 1.5625f; //Custom linespacing
				Vector2 bounds = new Vector2(float.MaxValue, 0); //No x-boundary, default y-boundary

				//15% outline
				fonts[fontIndex].DrawString("Supports outlines", Color.Black, Color.Red, new Vector2(50, 75 + 0.0f * spacing), bounds, scale,  0.15f, 0.00f, 1.5f, 0);
				//5% outline and 10% glow
				fonts[fontIndex].DrawString("Supports outlines", Color.Black, Color.Red, new Vector2(50, 75 + 1.0f * spacing), bounds, scale,  0.05f, 0.10f, 1.5f, 0);

				//0% outline and 25% glow
				fonts[fontIndex].DrawString("Supports glow effect", Color.Black, Color.Red, new Vector2(50, 75 + 2.5f * spacing), bounds, scale,  0.00f, 0.25f, 1.5f, 0);
				//-5% outline and 30% glow (negative outline starts the glow within the character -> fainter glow)
				fonts[fontIndex].DrawString("Supports glow effect", Color.Black, Color.Red, new Vector2(50, 75 + 3.5f * spacing), bounds, scale, -0.05f, 0.30f, 1.5f, 0);

				//Text 50% alpha
				fonts[fontIndex].DrawString("Supports alpha", new Color(Color.Black, 0.5f), Color.Red, new Vector2(50, 75 + 5.0f * spacing), bounds, scale, 0.15f, 0.00f, 1.5f, 0);
				//Outline 50% alpha
				fonts[fontIndex].DrawString("Supports alpha", Color.Black, new Color(Color.Red, 0.5f), new Vector2(50, 75 + 6.0f * spacing), bounds, scale, 0.15f, 0.00f, 1.5f, 0);
			}
			//============================================================== Paragraph ===============================================================
			else if (demo == Demo.Paragraph)
			{
				//Long paragraph to test the performance
				string paragraph = "You can resize the window and all rendered paragraphs will be have linebreaks automatically inserted at the nearest word boundary. "
					+ "If a single word takes up the entire line, a linebreak is inserted at a hyphen (if one exists) or at the last character that does not fit on the line. "
					+ "The text is rendered using signed distance fields which means it scales really well up to a certain point. "
					+ "Performance-wise this is about as efficient as a regular spritefont so the versatility comes with no drawbacks. "
					+ "Every DrawString call turns into a single draw call which makes it really efficient even at rendering a large number of characters. "
					+ "This paragraph is long on purpose to test exactly that.\n\n"
					+ "Press Alt + Enter to toggle fullscreen. Use arrow keys to control font properties. Resize window to test automatic line wrapping.";
				fonts[fontIndex].DrawString(paragraph, Color.Black, new Vector2(10, 40), new Vector2(-10, 0), scale, 1.5f);
			}
			//================================================================ Effect ================================================================
			else if (demo == Demo.Effect)
			{
				float spacing = scale * 1.5625f; //Custom linespacing
				Func<float, float> getDecimalPart = (value) => { return value - (int)value; };

				float hue1 = getDecimalPart(0.5f * (float)gameTime.TotalGameTime.TotalSeconds);
				float hue2 = getDecimalPart(hue1 + 2/6f);

				fonts[fontIndex].DrawString("Special outline",             Color.Black, Util.ColorFromHue(hue1), new Vector2(50, 75 + 0 * spacing), new Vector2(-50, 0), scale, 0.15f, 1.5f);
				fonts[fontIndex].DrawString("Special outline", Util.ColorFromHue(hue2), Util.ColorFromHue(hue1), new Vector2(50, 75 + 2 * spacing), new Vector2(-50, 0), scale, 0.15f, 1.5f);
			}
			//================================================================ Input =================================================================
			else if (demo == Demo.Input)
			{
				if (input == "")
					fonts[fontIndex].DrawString("Type something", new Color(Color.Black, 0.5f), new Vector2(50, 75), new Vector2(-50, 0), scale, 1.5f);
				else
					fonts[fontIndex].DrawString(input, Color.Black, new Vector2(50, 75), new Vector2(-50, 0), scale, 1.5f);
			}

			base.Draw(gameTime);
		}
	}
}
