using System.IO;

namespace CS.Changelog.Exporters
{
	internal static class IOExtensions {
		public static void AssertExistence(this DirectoryInfo directory) {
			directory.Parent?.AssertExistence();

			if (!directory.Exists)
				directory.Create();
		}
	}
}