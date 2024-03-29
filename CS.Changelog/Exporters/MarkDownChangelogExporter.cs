﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace CS.Changelog.Exporters
{
    /// <summary>
    /// <see cref="IChangelogExporter"/> for exporting to <see cref="OutputFormat.MarkDown"/>.
    /// </summary>
    /// <seealso cref="IChangelogExporter" />
    public class MarkDownChangelogExporter : IChangelogExporter
    {
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
			//	If changes are empty there's nothing to export, return.
			if (changes == null) return;

            options = options ?? new ExportOptions();
            StringBuilder result = WriteChanges(changes, options);

            string originalContent = null;
            if (file != null && file.Exists && options.Append && options.Reverse)
            {
                //Prepend content by reading entire file and then deleting the file
                using (var s = file.OpenText())
                    originalContent = s.ReadToEnd();

                file.Delete();
            }

            using (var w = file?.AppendText())
            {
                w.Write(result);

                if (!string.IsNullOrWhiteSpace(originalContent))
                    w.Write(originalContent);
            }
        }

        /// <summary>Writes the changes to s <see cref="StringBuilder"/>.</summary>
        /// <param name="changes">The changes to write.</param>
        /// <param name="options">The options (for formatting text mainly).</param>
        /// <returns>A <see cref="StringBuilder"/> containing the MarkDown.</returns>
        internal static StringBuilder WriteChanges(ChangeSet changes, ExportOptions options)
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
                                            .Where(x => !x.Ignore)
                                            .GroupBy(x => x.Message, StringComparer.InvariantCultureIgnoreCase)
                                            .Select(x =>
                                                new
                                                {
                                                    Message = x.Key,
                                                    Commits = x.Select(y => y.Hash)
                                                               .Where(y => !string.IsNullOrWhiteSpace(y))
                                                }
                                            )
                                        )
                {
                    //Change log messages are grouped by message, commits are appended
                    var hashes = entry.Commits
                                            .Select(x => options.LinkHash
                                                                ? $"[{(options.ShortHash ? x.Substring(0, 8) : x)}]({string.Format(CultureInfo.InvariantCulture, options.RepositoryUrl, x)})"
                                                                : options.ShortHash
                                                                    ? x.Substring(0, 8)
                                                                    : x);

                    var message = string.IsNullOrWhiteSpace(entry.Message)
                        ? string.Empty
                        : options.ResolveIssueNumbers
                            ? options.IssueNumberRegex.Replace(entry.Message, $"[$0]({string.Format(CultureInfo.InvariantCulture, options.IssueTrackerUrl, "$0")})")
                            : entry.Message;

                    message = message
                                .Replace(@"_", @"\_", StringComparison.OrdinalIgnoreCase)
                                .Replace(@"#", @"\#", StringComparison.OrdinalIgnoreCase);

                    result.AppendLine($@"- {message}{(hashes.Any()
                                                        ? $" ({string.Join(", ", hashes)})"
                                                        : string.Empty)}");
                }
            }

            return result;
        }
    }
}
