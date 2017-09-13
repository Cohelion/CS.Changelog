using CS.Changelog.Utils;
using System;
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
		/// Gets a value indicating whether the change log exporter supports deserializing an existing change log, and therefore append intelligently.
		/// </summary>
		/// <value>
		///   <c>false</c>
		/// </value>
		public bool SupportsDeserializing => false;
		/// <summary>
		/// Gets a value indicating whether the change log exporter supports writing to a file.
		/// </summary>
		/// <value>
		///   <c>false</c>.
		/// </value>
		public bool SupportsWritingToFile => false;

		/// <summary>
		/// Exports the specified <paramref name="changes">changeset</paramref> to a console window, ignoring <paramref name="file"/>.
		/// </summary>
		/// <param name="changes">The changes to export.</param>
		/// <param name="file">Ignored, there is no file output.</param>
		/// <param name="options">The options for exporting.</param>
		public void Export(ChangeSet changes, FileInfo file, ExportOptions options = null)
		{
			$"==({changes.Date:d}) {changes.Name}==".Dump();

			foreach (var group in changes
						.GroupBy(x => x.Category, StringComparer.InvariantCultureIgnoreCase)
						.Select(x => new { Category = x.Key, Entries = x.ToArray() }))
			{

				$"[{group.Category}]".Dump();

				foreach (var entry in group.Entries
										   .GroupBy(x => x.Message, StringComparer.InvariantCultureIgnoreCase)
										   .Select(x=> 
												new {
													Message = x.Key,
													Hashes = string.Join(",",x.Select(y=>y.Hash.Substring(0,8)))
											}))
					
					$" - {entry.Message} ({entry.Hashes})".Dump();

			}
		}
	}
}