using System;

namespace CS.Changelog.Utils
{
	public static partial class ConsoleExtensions
	{
		/// <summary>
		/// Trick for changing the console color and reverting to the original color upon disposal.
		/// </summary>
		/// <seealso cref="IDisposable" />
		public sealed class ConsoleColor : IDisposable
		{
			private readonly System.ConsoleColor _originalColor;
			/// <summary>
			/// Initializes a new instance of the <see cref="ConsoleColor"/> class, setting the console color and remembering the current color for reverting to upon disposing.
			/// </summary>
			/// <param name="color">The color.</param>
			public ConsoleColor(System.ConsoleColor color)
			{
				_originalColor = Console.ForegroundColor;
				Console.ForegroundColor = color;
			}

			/// <summary>
			/// Finalizes an instance of the <see cref="ConsoleColor"/> class.
			/// </summary>
			~ConsoleColor() {
				Dispose(false);
			}

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose()
			{
				Dispose(true);
			}

			void Dispose(bool disposing) {
				if (disposing) {
					Console.ForegroundColor = _originalColor;
				}
			}

			/// <summary>
			/// Performs an implicit conversion from <see cref="System.ConsoleColor"/> to <see cref="ConsoleColor"/>.
			/// </summary>
			/// <param name="color">The color.</param>
			/// <returns>The result of the conversion.</returns>
			public static implicit operator ConsoleColor(System.ConsoleColor color)
			{
				return new ConsoleColor(color);
			}
		}
	}
}