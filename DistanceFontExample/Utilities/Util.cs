using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceFontExample.Utilities
{
	public static class Util
	{
		public static Color ColorFromHSV(float h, float s, float v, float a)
		{
			//Achromatic
			if (s == 0) return new Color(v, v, v, a);

			//Divide to sectors 0-5
			h *= 6;

			//Decimal part of h stored in f
			int i = (int)h;
			float f = h - i;

			float p = v * (1 - s);
			float q = v * (1 - s * f);
			float t = v * (1 - s * (1 - f));

			if (i == 0)
				return new Color(v, t, p, a); //Red to yellow
			else if (i == 1)
				return new Color(q, v, p, a); //Yellow to green
			else if (i == 2)
				return new Color(p, v, t, a); //Green to teal
			else if (i == 3)
				return new Color(p, q, v, a); //Teal to blue
			else if (i == 4)
				return new Color(t, p, v, a); //Blue to magenta
			else
				return new Color(v, p, q, a); //Magenta to red
		}

		public static Color ColorFromHue(float hue)
		{
			return ColorFromHue(hue, 1);
		}
		public static Color ColorFromHue(float hue, float alpha)
		{
			float r = Math.Abs(hue * 6 - 3) - 1;
			float g = 2 - Math.Abs(hue * 6 - 2);
			float b = 2 - Math.Abs(hue * 6 - 4);

			return new Color(r, g, b, alpha);
		}
	}
}
