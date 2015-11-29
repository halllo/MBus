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
		public static void Content(string prefix, string text)
		{
			It(prefix, ConsoleColor.White, newLine: false);
			It(text, ConsoleColor.Cyan);
		}

		private static void It(string text, ConsoleColor? color = null, bool newLine = true)
		{
			if (color.HasValue) Console.ForegroundColor = color.Value;

			if (newLine) Console.WriteLine(text);
			else Console.Write(text);

			Console.ResetColor();
		}
	}
}
