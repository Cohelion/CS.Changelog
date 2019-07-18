using System;
using System.Diagnostics.CodeAnalysis;

namespace CS.Changelog
{
	/// <summary>
	/// Helper methods for generating output
	/// </summary>
	public static class OutputExtensions
	{

        /// <summary>Returns the file extension for the specified <see cref="OutputFormat"/>.</summary>
        /// <param name="format">The format.</param>
        /// <returns>A file extension, excluding the dot.</returns>
        /// <exception cref="NotImplementedException">When <paramref name="format"/> is not handled.</exception>
        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "I want file extensions to be in lowercase")]
        public static string FileExtension(this OutputFormat format)
		{
			switch (format)
			{
				case OutputFormat.Console:
					return string.Empty;
				case OutputFormat.MarkDown:
					return "md";
				case OutputFormat.JSON:
				case OutputFormat.XML:
				case OutputFormat.Html:
					return format.ToString().ToLowerInvariant();
				default:
					throw new NotImplementedException($"Format {format} has no default file extension");
			}
		}
	}
}