using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CS.Changelog.Exporters
{
	/// <summary>
	/// <see cref="IChangelogExporter"/> for exporting to MarkDown.
	/// </summary>
	/// <seealso cref="IChangelogExporter" />
	public class MarkDownChangelogExporter : IChangelogExporter
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
		///   <c>true</c>.
		/// </value>
		public bool SupportsWritingToFile => true;

		/// <summary>
		/// Exports the specified <paramref name="changes">changeset</paramref> to a Json <paramref name="file" />.
		/// </summary>
		/// <param name="changes">The changes to export.</param>
		/// <param name="file">The file to create of append, depending on <see cref="ExportOptions.Append" />.</param>
		/// <param name="options">The options for exporting.</param>
		public void Export(ChangeSet changes, FileInfo file, ExportOptions options = null)
		{
			options = options ?? new ExportOptions();

			var result = new StringBuilder();

			result.AppendLine($"# ({changes.Date:d}) {changes.Name} #");

			foreach (var group in changes
			.GroupBy(x => x.Category, StringComparer.InvariantCultureIgnoreCase)
			.Select(x => new { Category = x.Key, Entries = x.ToArray() }))
			{
				result.AppendLine();
				result.AppendLine($"## {group.Category} ##");

				foreach (var entry in group.Entries
											.GroupBy(x => x.Message, StringComparer.InvariantCultureIgnoreCase)
											.Select(x =>
												new
												{
													Message = x.Key,
													Commits = x.Select(y => y.Hash)
												}
											)
										)
				{
					//Change log messages are grouped by message, commits are appended
					var hashes = entry.Commits.Select(x => options.LinkHash ? $"[{(options.ShortHash ? x.Substring(0, 8) : x)}]({string.Format(options.RepositoryUrl, x)})" : options.ShortHash ? x.Substring(0, 8) : x);

					var message = string.IsNullOrWhiteSpace(entry.Message)
						? string.Empty
						: options.ResolveIssueNumbers
							? options.IssueNumberRegex.Replace(entry.Message, $"[$0]({string.Format(options.IssueTrackerUrl, "$0")})")
							: entry.Message;

					message = message
								.Replace(@"_", @"\_")
								.Replace(@"#", @"\#");

					result.AppendLine($"- {message} ({string.Join(", ", hashes)})");
				}
			}

			string originalContent = null;
			if (file.Exists && options.Append && options.Reverse)
			{
				//Prepend content by reading entire file and then deleting the file

				using (var s = file.OpenText())
					originalContent = s.ReadToEnd();

				file.Delete();
			}

			using (var w = file.AppendText())
			{
				w.Write(result);

				if (!string.IsNullOrWhiteSpace(originalContent))
					w.Write(originalContent);
			}
		}
	}
}