using System;

namespace CS.Changelog.Utils
{
	/// <summary>
	/// 
	/// </summary>
	public static partial class ConsoleExtensions
	{
		/// <summary>Dumps the specified <paramref name="trash"/> to a console window, either for development purposes or to use the programs' verbosity.</summary>
		/// <param name="trash">The 'trash', the object or text to dump.</param>
		/// <param name="color">The color in which to write to the console.</param>
		/// <param name="loglevel">The loglevel.</param>
		public static void Dump(this object trash, System.ConsoleColor? color = null, LogLevel loglevel = LogLevel.Info)
		{
			//if (loglevel <= Program._options.Verbosity)
				using ((ConsoleColor)color.GetValueOrDefault(loglevel.ToConsoleColor()))
					System.Console.WriteLine($"{trash}");
		}

		/// <summary>Converts <paramref name="level"/> to <see cref="System.ConsoleColor"/>.</summary>
		/// <param name="level">The level.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">level</exception>
		public static System.ConsoleColor ToConsoleColor(this LogLevel level)
		{
			switch (level)
			{
				case LogLevel.Error:
					return System.ConsoleColor.Red;
				case LogLevel.Warning:
					return System.ConsoleColor.Yellow;
				case LogLevel.Info:
					return System.ConsoleColor.White;
				case LogLevel.Debug:
					return System.ConsoleColor.Gray;
				default:
					throw new ArgumentOutOfRangeException(nameof(level), level, $"{level} is not a valid {typeof(LogLevel).Name}");
			}
		}
	}
}