using DistanceFont.Util;
using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceFont.Pipeline
{
	[ContentImporter(".fnt", DefaultProcessor = "FontProcessor", DisplayName = "BMFont Importer - DistanceFont")]
	public class FontImporter : ContentImporter<FontRawContent>
	{
		private FontRawContent output;

		public override FontRawContent Import(string descriptionFile, ContentImporterContext context)
		{
			//==================================== Load texture ====================================
			//Replace file extension for the texture file
			string textureFile = descriptionFile.Substring(0, descriptionFile.Length - 4) + ".png";

			//See if texture file exists
			if (!File.Exists(textureFile))
				throw new InvalidContentException($"Associated texture not found ({textureFile})");

			//Load associated bitmap
			output.Bitmap = new Bitmap(textureFile);

			//================================== Load description ==================================
			//Load all lines of the description file and parse it into arguments that can be processed later in FontProcessor
			int numChars = 0;
			int numKernings = 0;
			output.Chars = new Dictionary<string, string>[0];
			output.Kernings = new Dictionary<string, string>[0];

			string[] lines = File.ReadAllLines(descriptionFile);
			for (int i = 0; i < lines.Length; i++)
			{
				//First word of the line is the identifier
				string id = lines[i].Substring(0, Math.Max(lines[i].IndexOf(' '), 0));

				//Info and common
				if (id == "info")
					output.Info = GetArguments(lines[i]);
				else if (id == "common")
					output.Common = GetArguments(lines[i]);
				//Characters
				else if (id == "chars")
					output.Chars = new Dictionary<string, string>[int.Parse(GetArguments(lines[i])["count"]) + 1];
				else if (id == "char")
					output.Chars[numChars++] = GetArguments(lines[i]);
				//Kernings
				else if (id == "kernings")
					output.Kernings = new Dictionary<string, string>[int.Parse(GetArguments(lines[i])["count"]) + 1];
				else if (id == "kerning")
					output.Kernings[numKernings++] = GetArguments(lines[i]);
			}

			return output;
		}

		private Dictionary<string, string> GetArguments(string line)
		{
			Dictionary<string, string> args = new Dictionary<string, string>();

			int equalsPos = 0;
			while (true)
			{
				//There is an argument at every equals sign
				equalsPos = line.NextIndexOf('=', equalsPos + 1);
				if (equalsPos == line.Length)
					break;

				//Argument begins at the last space relative to the equals sign
				int argBegin = line.ReverseIndexOf(' ', equalsPos) + 1;

				//Argument ends at the next space, or if it's a string, at a closing quote
				int argEnd = line[equalsPos + 1] == '\"' ? line.NextIndexOf('\"', equalsPos + 2) + 1 : line.NextIndexOf(' ', equalsPos + 1);

				args.Add(line.Substring(argBegin, equalsPos - argBegin), line.Substring(equalsPos + 1, argEnd - (equalsPos + 1)));
			}

			return args;
		}
	}

	public struct FontRawContent
	{
		public Dictionary<string, string> Info;
		public Dictionary<string, string> Common;
		public Dictionary<string, string>[] Chars;
		public Dictionary<string, string>[] Kernings;
		public Bitmap Bitmap;
	}
}
