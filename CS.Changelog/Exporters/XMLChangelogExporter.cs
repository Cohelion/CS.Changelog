using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace CS.Changelog.Exporters
{
    /// <summary>
    /// A change log exported for JSON formatting.
    /// </summary>
    /// <seealso cref="CS.Changelog.Exporters.IChangelogExporter" />
    public class XmlChangelogExporter : IChangelogExporter, IChangelogDeserializer
    {
        /// <summary>
        /// Gets a value indicating whether the change log exporter supports writing to a file.
        /// </summary>
        /// <value>
        ///   <c>true</c>.
        /// </value>
        public bool SupportsWritingToFile => true;

        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(ChangeLog));

        /// <summary>
        /// Deserializes the specified data to a <see cref="ChangeLog" />
        /// </summary>
        /// <param name="data">The data in Xml format.</param>
        /// <returns></returns>
        [SuppressMessage("Security", "CA5369:Use XmlReader For Deserialize", Justification = "Postponing code change")]
        public ChangeLog Deserialize(string data)
        {
			using TextReader r = new StringReader(data);
			using var x = XmlReader.Create(r);
			return (ChangeLog)_serializer.Deserialize(x);
		}

        /// <summary>
        /// Exports the specified <paramref name="changes">changeset</paramref> to a MarkDown <paramref name="file" />.
        /// </summary>
        /// <param name="changes">The changes to export.</param>
        /// <param name="file">The file to create of append, depending on <see cref="ExportOptions.Append" />.</param>
        /// <param name="options">The options for exporting.</param>
        public void Export(ChangeSet changes, FileInfo file, ExportOptions options = null)
        {
            options ??= new ExportOptions();

            ChangeLog log;

            if (file.Exists && options.Append)
            {
                //Append/Prepend content by reading entire file and then deleting the file
                using (var s = file.OpenText())
                    log = Deserialize(s.ReadToEnd());//This can likely be done much more efficient.

                //Do not log a single commit more than once
                var loggedCommits = log
                                        .SelectMany(x => x)
                                        .Select(x => x.Hash)
                                        .Where(x => !string.IsNullOrWhiteSpace(x)) //it is possible that the serialized change log contains commit / entries without a hash
                                        .Distinct();

                changes.RemoveAll(change =>
                    loggedCommits.Any(h => h.Equals(change.Hash, System.StringComparison.InvariantCultureIgnoreCase))
                    );

                //Only append or write changeset when there are unlogged commits
                if (changes.Any())
                    //Append or insert changeset
                    if (options.Reverse)
                        log.Insert(0, changes);
                    else
                        log.Add(changes);

                file.Delete();

            }
            else
                log = new ChangeLog { changes };

            log.IssueNumberRegex = options?.IssueNumberRegex.ToString();
            log.IssueTrackerUrl = options?.IssueTrackerUrl;
            log.RepositoryUrl = options?.RepositoryUrl;

			using var w = file.CreateText();
			_serializer.Serialize(w, log);
		}
    }
}
