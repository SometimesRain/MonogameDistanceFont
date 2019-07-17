using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DistanceFont.Pipeline
{
	public class FontReader : ContentTypeReader<Font>
	{
		protected override Font Read(ContentReader input, Font font)
		{
			//Don't load the same resource multiple times (if Content.Load is called more than once)
			if (font != null)
				return font;

			//Read binary file
			font = new Font();
			font.Name = input.ReadString();
			font.BaseSize = input.ReadSingle();
			font.VerticalPadding = input.ReadSingle();
			font.HorizontalPadding = input.ReadSingle();

			int numGlyphs = input.ReadInt32();
			font.Glyphs = new Dictionary<char, Glyph>(numGlyphs);
			for (int i = 0; i < numGlyphs; i++)
			{
				Glyph glyph = new Glyph();
				glyph.Char = input.ReadChar();
				glyph.TexturePos = input.ReadVector2();
				glyph.TextureSize = input.ReadVector2();
				glyph.CursorOffset = input.ReadVector2();
				glyph.Advance = input.ReadSingle();

				int numKernings = input.ReadInt32();
				glyph.Kerning = new Dictionary<char, float>(numKernings);
				for (int j = 0; j < numKernings; j++)
					glyph.Kerning.Add(input.ReadChar(), input.ReadSingle());

				font.Glyphs.Add(glyph.Char, glyph);
			}

			font.LineHeight = input.ReadSingle();
			font.BaseLine = input.ReadSingle();

			byte[] alphas = input.ReadBytes(input.ReadInt32());
			font.TextureDimensions = input.ReadVector2();

			//Use a reflection hack to access GraphicsDevice (Monogame uses the same internal property in its Texture2DReader)
			GraphicsDevice graphicsDevice = (GraphicsDevice)typeof(ContentReader).GetProperty("GraphicsDevice", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(input);
			font.Texture = new Texture2D(graphicsDevice, (int)font.TextureDimensions.X, (int)font.TextureDimensions.Y, false, SurfaceFormat.Alpha8);
			font.Texture.SetData(alphas);

			return font;
		}
	}
}
