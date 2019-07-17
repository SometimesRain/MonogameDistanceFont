using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceFont
{
	public class Font
	{
		public Dictionary<char, Glyph> Glyphs;
		public string Name;
		public float LineHeight;
		public float BaseSize;
		public float BaseLine;

		public Texture2D Texture;
		public Vector2 TextureDimensions;
		public float VerticalPadding;
		public float HorizontalPadding;

		private static Vector2 windowBounds;
		private static Effect effect;

		/// <summary>
		/// Initialize font rendering by supplying a shader.
		/// </summary>
		/// <param name="effect"></param>
		public static void Initialize(Effect effect)
		{
			Font.effect = effect;
		}

		/// <summary>
		/// Update shader matrix for rendering at current window size. Also updates the default bounds (when using overloads of DrawString without bounds parameter).
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public static void OnResize(int width, int height)
		{
			windowBounds.X = width;
			windowBounds.Y = height;

			Matrix translation = Matrix.CreateTranslation(width / -2f, height / -2f, 0);
			Matrix scale = Matrix.CreateScale(new Vector3(-2f / width, 2f / height, 1));
			Matrix rotation = Matrix.CreateRotationZ(MathHelper.Pi);

			effect.Parameters["WorldViewProjection"].SetValue(translation * rotation * scale);
		}

		/// <summary>
		/// Draw a string with the selected font.
		/// </summary>
		/// <param name="text">Text to be drawn.</param>
		/// <param name="position">Position at which to render the text. Origin is at the top-left corner.</param>
		/// <param name="size">Font size in points.</param>
		public void DrawString(string text, Vector2 position, float size)
		{
			DrawString(text, Color.Black, Color.Transparent, position, Vector2.Zero, size, 0, 0, 1.5f, 0);
		}

		/// <summary>
		/// Draw a string with the selected font.
		/// </summary>
		/// <param name="text">Text to be drawn.</param>
		/// <param name="color">Color of the text.</param>
		/// <param name="position">Position at which to render the text. Origin is at the top-left corner.</param>
		/// <param name="size">Font size in points.</param>
		public void DrawString(string text, Color color, Vector2 position, float size)
		{
			DrawString(text, color, Color.Transparent, position, Vector2.Zero, size, 0, 0, 1.5f, 0);
		}

		/// <summary>
		/// Draw a string with the selected font.
		/// </summary>
		/// <param name="text">Text to be drawn.</param>
		/// <param name="color">Color of the text.</param>
		/// <param name="position">Position at which to render the text. Origin is at the top-left corner.</param>
		/// <param name="size">Font size in points.</param>
		/// <param name="lineSpacing">Multiplies the linespacing. 1.5f recommended for paragraphs.</param>
		public void DrawString(string text, Color color, Vector2 position, float size, float lineSpacing)
		{
			DrawString(text, color, Color.Transparent, position, Vector2.Zero, size, 0, 0, lineSpacing, 0);
		}

		/// <summary>
		/// Draw a string with the selected font.
		/// </summary>
		/// <param name="text">Text to be drawn.</param>
		/// <param name="color">Color of the text.</param>
		/// <param name="position">Position at which to render the text. Origin is at the top-left corner.</param>
		/// <param name="bounds">Area which the text is allowed to occupy. Breaks the line before crossing the bounds. Set to 0 for a default value.</param>
		/// <param name="size">Font size in points.</param>
		/// <param name="lineSpacing">Multiplies the linespacing. 1.5f recommended for paragraphs.</param>
		public void DrawString(string text, Color color, Vector2 position, Vector2 bounds, float size, float lineSpacing)
		{
			DrawString(text, color, Color.Transparent, position, bounds, size, 0, 0, lineSpacing, 0);
		}

		/// <summary>
		/// Draw a string with the selected font.
		/// </summary>
		/// <param name="text">Text to be drawn.</param>
		/// <param name="color">Color of the text.</param>
		/// <param name="outlineColor">Color of the outline.</param>
		/// <param name="position">Position at which to render the text. Origin is at the top-left corner.</param>
		/// <param name="size">Font size in points.</param>
		/// <param name="outlineWeight">Weight of the outline. 0.15f creates a 15% outline.</param>
		/// <param name="lineSpacing">Multiplies the linespacing. 1.5f recommended for paragraphs.</param>
		public void DrawString(string text, Color color, Color outlineColor, Vector2 position, float size, float outlineWeight, float lineSpacing)
		{
			DrawString(text, color, outlineColor, position, Vector2.Zero, size, outlineWeight, 0, lineSpacing, 0);
		}

		/// <summary>
		/// Draw a string with the selected font.
		/// </summary>
		/// <param name="text">Text to be drawn.</param>
		/// <param name="color">Color of the text.</param>
		/// <param name="outlineColor">Color of the outline.</param>
		/// <param name="position">Position at which to render the text. Origin is at the top-left corner.</param>
		/// <param name="bounds">Area which the text is allowed to occupy. Breaks the line before crossing the bounds. Set to 0 for a default value.</param>
		/// <param name="size">Font size in points.</param>
		/// <param name="outlineWeight">Weight of the outline. 0.15f creates a 15% outline.</param>
		/// <param name="lineSpacing">Multiplies the linespacing. 1.5f recommended for paragraphs.</param>
		public void DrawString(string text, Color color, Color outlineColor, Vector2 position, Vector2 bounds, float size, float outlineWeight, float lineSpacing)
		{
			DrawString(text, color, outlineColor, position, bounds, size, outlineWeight, 0, lineSpacing, 0);
		}

		/// <summary>
		/// Draw a string with the selected font.
		/// </summary>
		/// <param name="text">Text to be drawn.</param>
		/// <param name="color">Color of the text.</param>
		/// <param name="outlineColor">Color of the outline.</param>
		/// <param name="position">Position at which to render the text. Origin is at the top-left corner.</param>
		/// <param name="bounds">Area which the text is allowed to occupy. Breaks the line before crossing the bounds. Set to 0 for a default value.</param>
		/// <param name="size">Font size in points.</param>
		/// <param name="outlineWeight">Weight of the outline. 0.15f creates a 15% outline.</param>
		/// <param name="outlineGlow">Weight of the outline glow. 0.15f creates a 15% outline glow.</param>
		/// <param name="lineSpacing">Multiplies the linespacing. 1.5f recommended for paragraphs.</param>
		/// <param name="charSpacing">Character spacing. Default value: 0.</param>
		public void DrawString(string text, Color color, Color outlineColor, Vector2 position, Vector2 bounds, float size, float outlineWeight, float outlineGlow, float lineSpacing, float charSpacing)
		{
			//Negative values set the distance inwards from window bounds
			//Zero sets the bounds to be exactly at the window bounds
			if (bounds.X <= 0) bounds.X += windowBounds.X;
			if (bounds.Y <= 0) bounds.Y += windowBounds.Y;

			//Turn fontsize to multiplier and set texture
			size /= BaseSize;
			effect.Parameters["Texture"].SetValue(Texture);
			
			//Calculate weights
			float edgeTransition = -0.08f * Math.Min(size, 1.5f) + 0.16f;
			effect.Parameters["FontWeight"].SetValue(new Vector2(0.47f, edgeTransition));
			effect.Parameters["OutlineWeight"].SetValue(new Vector2(0.47f + outlineWeight, edgeTransition + outlineGlow));
			
			//Set colors
			effect.Parameters["FontColor"].SetValue(color.ToVector4());
			effect.Parameters["OutlineColor"].SetValue(outlineColor.ToVector4());

			//Initialize mesh
			VertexPositionTexture[] vertices = new VertexPositionTexture[text.Length * 4];
			short[] indices = new short[text.Length * 6];

			int i;
			int lastSpace = 0;
			float lineStartX = position.X;
			for (i = 0; i < text.Length; i++)
			{
				if (text[i] == '\r') continue; //Ignore carriage return
				if (text[i] == '-') lastSpace = i; //Can break line on hyphen

				if (text[i] == '\n')
				{
					//Break line
					position.X = lineStartX;
					position.Y += BaseLine * lineSpacing * size;
					lastSpace = 0; //No spaces yet on this line
				}
				else if (text[i] == ' ')
				{
					//"Rendering" space is done by simply moving the cursor
					Glyph glyph = Glyphs[' '];
					float kern = i + 1 < text.Length && glyph.Kerning.ContainsKey(text[i + 1]) ? glyph.Kerning[text[i + 1]] : 0;
					position.X += (glyph.Advance + kern + charSpacing) * size;

					lastSpace = i; //Can break line here
				}
				else
				{
					Glyph glyph = Glyphs[text[i]];

					//Top left and bottom left coordinate of this glyph
					Vector2 TL = position + glyph.CursorOffset * size;
					Vector2 BR = TL + glyph.TextureSize * size;
					
					//Are rest of the characters out of bounds?
					if (TL.Y >= bounds.Y + glyph.CursorOffset.Y)
						break;

					//Does this character exceed the line bounds?
					if (BR.X >= bounds.X)
					{
						if (lastSpace == 0)
							i = i - 1; //No spaces -> break line here (and repeat this iteration)
						else
							i = lastSpace; //Has space -> break line after space (and repeat everything after)

						//Break line
						position.X = lineStartX;
						position.Y += BaseLine * lineSpacing * size;
						lastSpace = 0; //No spaces yet on this line
					}
					else
					{
						//Top left and bottom left texture coordinates
						Vector2 TexTL = glyph.TexturePos / TextureDimensions;
						Vector2 TexBR = (glyph.TexturePos + glyph.TextureSize) / TextureDimensions;

						//Vertices of this character quad
						vertices[i * 4 + 0] = new VertexPositionTexture(new Vector3(TL.X, TL.Y, 0), new Vector2(TexTL.X, TexTL.Y)); //Top left
						vertices[i * 4 + 1] = new VertexPositionTexture(new Vector3(BR.X, TL.Y, 0), new Vector2(TexBR.X, TexTL.Y)); //Top right
						vertices[i * 4 + 2] = new VertexPositionTexture(new Vector3(TL.X, BR.Y, 0), new Vector2(TexTL.X, TexBR.Y)); //Bottom left
						vertices[i * 4 + 3] = new VertexPositionTexture(new Vector3(BR.X, BR.Y, 0), new Vector2(TexBR.X, TexBR.Y)); //Bottom right

						//Indices of this character quad
						indices[i * 6 + 0] = (short)(i * 4 + 0);
						indices[i * 6 + 1] = (short)(i * 4 + 1);
						indices[i * 6 + 2] = (short)(i * 4 + 2);
						indices[i * 6 + 3] = (short)(i * 4 + 2);
						indices[i * 6 + 4] = (short)(i * 4 + 1);
						indices[i * 6 + 5] = (short)(i * 4 + 3);

						//Move cursor to the position of the next character
						float kern = i + 1 < text.Length && glyph.Kerning.ContainsKey(text[i + 1]) ? glyph.Kerning[text[i + 1]] : 0;
						position.X += (glyph.Advance + kern + charSpacing) * size;
					}
				}
			}

			//Nothing to render
			if (i == 0) return;

			//Render all characters in this mesh
			effect.CurrentTechnique.Passes[0].Apply();
			Texture.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, i * 2);
		}
	}
}
