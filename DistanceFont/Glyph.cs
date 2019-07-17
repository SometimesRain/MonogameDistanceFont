using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceFont
{
	public struct Glyph
	{
		public char Char;
		public Vector2 TexturePos;
		public Vector2 TextureSize;
		public Vector2 CursorOffset;
		public float Advance;

		//Next character, X amount
		public Dictionary<char, float> Kerning;
	}
}
