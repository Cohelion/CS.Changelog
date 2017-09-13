using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Linq;

namespace CS.Changelog.Exporters
{
	/// <summary>
	/// A change log exported for JSON formatting.
	/// </summary>
	/// <seealso cref="IChangelogExporter" />
	public class JsonChangelogExporter : IChangelogExporter
	{
		/// <summary>
		/// Gets a value indicating whether the change log exporter supports deserializing an existing change log, and therefore append intelligently.
		/// </summary>
		/// <value>
		///   <c>true</c>
		/// </value>
		public bool SupportsDeserializing => true;

		/// <summary>
		/// Gets a value indicating whether the change log exporter supports writing to a file.
		/// </summary>
		/// <value>
		///   <c>true</c>.
		/// </value>
		public bool SupportsWritingToFile => true;

		/// <summary>
		/// Exports the specified <paramref name="changes">changeset</paramref> to a MarkDown <paramref name="file" />.
		/// </summary>
		/// <param name="changes">The changes to export.</param>
		/// <param name="file">The file to create of append, depending on <see cref="ExportOptions.Append" />.</param>
		/// <param name="options">The options for exporting.</param>
		public void Export(ChangeSet changes, FileInfo file, ExportOptions options = null)
		{
			options = options ?? new ExportOptions();

			ChangeLog log;

			if (file.Exists && options.Append)
			{
				//Append/Prepend content by reading entire file and then deleting the file
				using (var s = file.OpenText())
				{
					var originalContent = s.ReadToEnd();
					log = JsonConvert.DeserializeObject<ChangeLog>(originalContent);
				}

				//Do not log a single commit more than once
				var loggedCommits = log
										.SelectMany(x => x)
										.Select(x => x.Hash)
										.Distinct();

				changes.RemoveAll(change =>
					loggedCommits.Any(h => h.Equals(change.Hash))
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

			var serializer = new JsonSerializer
			{
				Formatting = Formatting.Indented,
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			};

			using (var w = file.CreateText())
				serializer.Serialize(w, log);
		}
	}
}