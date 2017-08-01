using CS.Changelog.Utils;
using System.IO;
using System.Linq;

namespace CS.Changelog.Exporters
{
	/// <summary>
	/// <see cref="IChangelogExporter"/> for exporting to the console output window.
	/// </summary>
	/// <seealso cref="IChangelogExporter" />
	public class ConsoleChangelogExporter : IChangelogExporter
	{
		/// <summary>
		/// Exports the specified <paramref name="changes">changeset</paramref> to a console window, ignoring <paramref name="file"/>.
		/// </summary>
		/// <param name="changes">The changes to export.</param>
		/// <param name="file">Ignored, there is no file output.</param>
		/// <param name="options">The options for exporting.</param>
		public void Export(ChangeSet changes, FileInfo file, ExportOptions options)
		{
			foreach (var group in changes
						.GroupBy(x => x.Category)
						.Select(x => new { Category = x.Key, Entries = x.ToArray() }))
			{

				$"[{group.Category}]".Dump();

				foreach (var entry in group.Entries)
					$" - {entry.Message} ({entry.Hash.Substring(0, 8)})".Dump();

			}
		}
	}
}