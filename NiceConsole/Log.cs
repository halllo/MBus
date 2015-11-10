using System;

namespace NiceConsole
{
	public static class Log
	{
		public static void Info(string text)
		{
			It(text, ConsoleColor.Gray);
		}
		public static void Error(string text)
		{
			It(text, ConsoleColor.Red);
		}
		public static void Suucess(string text)
		{
			It(text, ConsoleColor.Green);
		}
		public static void Content(string text)
		{
			It(text, ConsoleColor.Cyan);
		}

		private static void It(string text, ConsoleColor? color = null)
		{
			if (color.HasValue)
			{
				Console.ForegroundColor = color.Value;
			}
			Console.WriteLine(text);
			Console.ResetColor();
		}
	}
}
