using CS.Changelog.Utils;
using System.IO;

namespace CS.Changelog.Exporters
{
	internal static class IOExtensions {
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