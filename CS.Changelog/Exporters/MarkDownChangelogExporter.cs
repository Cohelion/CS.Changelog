using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CS.Changelog.Exporters
{

	/// <summary>
	/// <see cref="IChangelogExporter"/> for exporting to MarkDown.
	/// </summary>
	/// <seealso cref="CS.Changelog.Exporters.IChangelogExporter" />
	public class MarkDownChangelogExporter : IChangelogExporter
	{
		/// <summary>
		/// Exports the specified <paramref name="changes">changeset</paramref> to a Json <paramref name="file" />.
		/// </summary>
		/// <param name="changes">The changes to export.</param>
		/// <param name="file">The file to create of append, depending on <see cref="ExportOptions.Append" />.</param>
		/// <param name="options">The options for exporting.</param>
		public void Export(ChangeSet changes, FileInfo file, ExportOptions options)
		{
			var result = new StringBuilder();

			result.AppendLine($"# ({DateTime.Now:d}) #");
			result.AppendLine();

			foreach (var group in changes
			.GroupBy(x => x.Category)
			.Select(x => new { Category = x.Key, Entries = x.ToArray() }))
			{

				result.AppendLine($"## {group.Category} ##");
				result.AppendLine();

				foreach (var entry in group.Entries)
				{
					var displayHash = options.ShortHash ? entry.Hash.Substring(0, 8) : entry.Hash;
					var message = string.IsNullOrWhiteSpace(entry.Message)
						? string.Empty
						: options.ResolveIssueNumbers
							? options.IssueNumberRegex.Replace(entry.Message, $"[$0]({string.Format(options.IssueTrackerUrl, "$0")})")
							: entry.Message;

					message = message
								.Replace(@"_", @"\_")
								.Replace(@"#", @"\#");

					result.AppendLine($"- {message} ({(options.LinkHash ? $"[{displayHash}]({string.Format(options.RepositoryUrl, entry.Hash)})" : displayHash)})");
				}
			}

			result.AppendLine();

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