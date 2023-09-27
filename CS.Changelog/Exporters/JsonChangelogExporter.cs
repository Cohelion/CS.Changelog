using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Linq;

namespace CS.Changelog.Exporters
{
	/// <summary>
	/// A change log exported for JSON formatting.
	/// </summary>
	/// <seealso cref="IChangelogExporter" />
	public class JsonChangelogExporter : IChangelogExporter, IChangelogDeserializer
	{
		/// <summary>
		/// Gets a value indicating whether the change log exporter supports writing to a file.
		/// </summary>
		/// <value>
		///   <c>true</c>.
		/// </value>
		public bool SupportsWritingToFile => true;

		/// <summary>
		/// Gets or sets the indent character.
		/// </summary>
		/// <value>
		/// The indent character.
		/// </value>
		public static char IndentChar { get; set; } = '\t';

		/// <summary>
		/// Deserializes the specified data to a <see cref="ChangeLog" />
		/// </summary>
		/// <param name="data">The data in JSON format.</param>
		/// <returns></returns>
		public ChangeLog Deserialize(string data)
		{
			return JsonConvert.DeserializeObject<ChangeLog>(data);
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

			if (file != null && file.Exists && options.Append)
			{
				//Append/Prepend content by reading entire file and then deleting the file
				using (var s = file.OpenText())
				{
					var originalContent = s.ReadToEnd();
					log = Deserialize(originalContent);
				}

				//Do not log a single commit more than once
				var loggedCommits = log
										.SelectMany(x => x)
										.Select(x => x.Hash)
										.Where(x => !string.IsNullOrWhiteSpace(x)) //it is possible that the serialized change log contains commit / entries without a hash
										.Distinct();


				changes?.RemoveAll(change =>
					loggedCommits.Any(h => h.Equals(change.Hash, System.StringComparison.OrdinalIgnoreCase))
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
			log.IssueTrackerUrl = string.IsNullOrEmpty(options?.IssueTrackerUrl) ? null : new Uri(options.IssueTrackerUrl);
			log.RepositoryUrl = string.IsNullOrEmpty(options?.RepositoryUrl) ? null : new Uri(options?.RepositoryUrl);

			var serializer = new JsonSerializer
			{
				Formatting = Formatting.Indented,
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
			};

			using var w = file?.CreateText();
			using var jtw = new JsonTextWriter(w);
			jtw.IndentChar = IndentChar;
			jtw.Indentation = 1;
			serializer.Serialize(jtw, log);
		}
	}
}
