using CS.Changelog.Utils;
using System.IO;

namespace CS.Changelog.Exporters
{
	/// <summary>
	/// Simple extensions for <see cref="System.IO"/>.
	/// </summary>
	internal static class IOExtensions {

		/// <summary>Asserts the existence of a directory by possibly creating the entire tree.</summary>
		/// <param name="directory">The directory to create.</param>
		public static void AssertExistence(this DirectoryInfo directory) {
			directory.Parent?.AssertExistence();

			if (!directory.Exists)
			{
				$"Creating {directory}".Dump();
				directory.Create();
			}
		}
	}
}