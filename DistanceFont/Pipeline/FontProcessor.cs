using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceFont.Pipeline
{
	[ContentProcessor(DisplayName = "DistanceFont.Pipeline.FontProcessor")]
	public class FontProcessor : ContentProcessor<FontRawContent, FontData>
	{
		private FontData output;

		public unsafe override FontData Process(FontRawContent input, ContentProcessorContext context)
		{
			//=================================== Process texture ==================================
			Bitmap bitmap = input.Bitmap;
			BitmapData bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
			byte* scan0 = (byte*)bitmapData.Scan0.ToPointer(); //ARGB

			output.Alphas = new byte[bitmap.Width * bitmap.Height];
			output.TextureDimensions = new Vector2(bitmap.Width, bitmap.Height);

			for (int i = 0; i < output.Alphas.Length; i++)
				output.Alphas[i] = scan0[i * 4 + 3];

			bitmap.UnlockBits(bitmapData);
			bitmap.Dispose();

			//================================= Process description ================================
			//Info
			output.Name = input.Info["face"].Trim('\"');
			output.BaseSize = float.Parse(input.Info["size"]) * 0.75f; //* 0.75f to get the correct size

			string[] padding = input.Info["padding"].Split(',');
			output.VerticalPadding = float.Parse(padding[0]) + float.Parse(padding[2]);
			output.HorizontalPadding = float.Parse(padding[1]) + float.Parse(padding[3]);

			//Common
			output.LineHeight = float.Parse(input.Common["lineHeight"]) - output.VerticalPadding;
			output.BaseLine = float.Parse(input.Common["base"]);

			//Chars
			output.Glyphs = new Dictionary<char, Glyph>(input.Chars.Length);
			for (int i = 0; i < input.Chars.Length; i++)
			{
				Glyph glyph = new Glyph();
				glyph.Kerning = new Dictionary<char, float>();

				glyph.Char = (char)int.Parse(input.Chars[i]["id"]);
				glyph.Advance = float.Parse(input.Chars[i]["xadvance"]) - output.HorizontalPadding;

				glyph.TexturePos.X = float.Parse(input.Chars[i]["x"]);
				glyph.TexturePos.Y = float.Parse(input.Chars[i]["y"]);

				glyph.TextureSize.X = float.Parse(input.Chars[i]["width"]);
				glyph.TextureSize.Y = float.Parse(input.Chars[i]["height"]);

				glyph.CursorOffset.X = float.Parse(input.Chars[i]["xoffset"]);
				glyph.CursorOffset.Y = float.Parse(input.Chars[i]["yoffset"]);

				output.Glyphs.Add(glyph.Char, glyph);
			}

			//Kernings
			for (int i = 0; i < input.Kernings.Length; i++)
			{
				Glyph glyph = output.Glyphs[(char)int.Parse(input.Kernings[i]["first"])];
				glyph.Kerning.Add((char)int.Parse(input.Kernings[i]["second"]), int.Parse(input.Kernings[i]["amount"]));
			}

			return output;
		}
	}

	public struct FontData
	{
		public string Name;
		public float BaseSize;
		public float VerticalPadding;
		public float HorizontalPadding;

		public Dictionary<char, Glyph> Glyphs;
		public float LineHeight;
		public float BaseLine;
		
		public byte[] Alphas;
		public Vector2 TextureDimensions;
	}
}
