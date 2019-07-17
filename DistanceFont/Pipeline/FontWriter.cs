using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceFont.Pipeline
{
	[ContentTypeWriter]
	public class FontWriter : ContentTypeWriter<FontData>
	{
		protected override void Write(ContentWriter output, FontData data)
		{
			//Write binary .xnb file
			output.Write(data.Name);
			output.Write(data.BaseSize);
			output.Write(data.VerticalPadding);
			output.Write(data.HorizontalPadding);

			output.Write(data.Glyphs.Count);
			foreach (Glyph glyph in data.Glyphs.Values)
			{
				output.Write(glyph.Char);
				output.Write(glyph.TexturePos);
				output.Write(glyph.TextureSize);
				output.Write(glyph.CursorOffset);
				output.Write(glyph.Advance);

				output.Write(glyph.Kerning.Count);
				foreach (KeyValuePair<char, float> keyValuePair in glyph.Kerning)
				{
					output.Write(keyValuePair.Key);
					output.Write(keyValuePair.Value);
				}
			}

			output.Write(data.LineHeight);
			output.Write(data.BaseLine);

			output.Write(data.Alphas.Length);
			output.Write(data.Alphas);
			output.Write(data.TextureDimensions);
		}

		public override string GetRuntimeType(TargetPlatform targetPlatform)
		{
			return typeof(Font).AssemblyQualifiedName;
		}

		public override string GetRuntimeReader(TargetPlatform targetPlatform)
		{
			return typeof(FontReader).AssemblyQualifiedName;
		}
	}
}
