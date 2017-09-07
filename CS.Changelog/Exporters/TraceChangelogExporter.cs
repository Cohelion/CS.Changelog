using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CS.Changelog.Exporters
{
	/// <summary>
	/// <see cref="IChangelogExporter"/> for exporting to the diagnostics trace.
	/// </summary>
	/// <seealso cref="IChangelogExporter" />
	public class TraceChangelogExporter : IChangelogExporter
	{
		/// <summary>
		/// Exports the specified <paramref name="changes">changeset</paramref> to a console window, ignoring <paramref name="file"/>.
		/// </summary>
		/// <param name="changes">The changes to export.</param>
		/// <param name="file">Ignored, there is no file output.</param>
		/// <param name="options">The options for exporting.</param>
		public void Export(ChangeSet changes, FileInfo file = null, ExportOptions options= null)
		{
			foreach (var group in changes
						.GroupBy(x => x.Category)
						.Select(x => new { Category = x.Key, Entries = x.ToArray() }))
			{

				Trace.WriteLine($"[{group.Category}]");

				//Group by exact change log message
				foreach (var entry in group.Entries.GroupBy(x=>x.Message).Select(x=> new { Message = x.Key, Commits = x.Select(y=>y.Hash)}))
					Trace.WriteLine($" - {entry.Message} ({string.Join(",",entry.Commits.Select(x=>x.Substring(0, 8)))})");

			}
		}
	}
}