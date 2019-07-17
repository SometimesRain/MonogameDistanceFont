using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceFont.Util
{
	public static class StringExtensions
	{
		public static int NextIndexOf(this string s, char value, int startIndex)
		{
			for (int i = startIndex; i < s.Length; i++)
			{
				if (s[i] == value)
					return i;
			}
			return s.Length;
		}

		public static int ReverseIndexOf(this string s, char value, int startIndex)
		{
			for (int i = startIndex; i >= 0; i--)
			{
				if (s[i] == value)
					return i;
			}
			return -1;
		}
	}
}
