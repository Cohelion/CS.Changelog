using System;
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
        /// Gets a value indicating whether the change log exporter supports deserializing an existing changelog, and therefore append intelligently.
        /// </summary>
        /// <value>
        ///   <c>false</c> if the change log exporter supports deserializing; otherwise, <c>false</c>.
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
        public void Export(ChangeSet changes, FileInfo file = null, ExportOptions options = null)
        {
            foreach (var group in changes
                        .GroupBy(x => x.Category, StringComparer.InvariantCultureIgnoreCase)
                        .Select(x => new { Category = x.Key, Entries = x.ToArray() }))
            {

                Trace.WriteLine($"[{group.Category}]");

                //Group by exact change log message
                foreach (var entry in group.Entries
                                        .GroupBy(x => x.Message, StringComparer.InvariantCultureIgnoreCase)
                                        .Select(x =>
                                            new
                                            {
                                                Message = x.Key,
                                                Commits = x.Select(y => y.Hash)
                                                           .Where(y => !string.IsNullOrWhiteSpace(y))
                                            })
                                        )

                    Trace.WriteLine($" - {entry.Message} ({string.Join(",", entry.Commits.Select(x => x.Substring(0, 8)))})");

            }
        }
    }
}