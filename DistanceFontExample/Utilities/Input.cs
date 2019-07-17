using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceFontExample.Utilities
{
	public static class Input
	{
		private static KeyboardState lastKeyboardState;
		private static KeyboardState keyboardState;

		static Input()
		{
			keyboardState = Keyboard.GetState();
		}

		public static void Update()
		{
			lastKeyboardState = keyboardState;
			keyboardState = Keyboard.GetState();
		}

		public static bool KeyDown(Keys key)
		{
			return keyboardState.IsKeyDown(key);
		}
		public static bool KeyUp(Keys key)
		{
			return keyboardState.IsKeyUp(key);
		}
		public static bool KeyPressed(Keys key)
		{
			return keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
		}
		public static bool KeyReleased(Keys key)
		{
			return keyboardState.IsKeyUp(key) && lastKeyboardState.IsKeyDown(key);
		}
	}
}
