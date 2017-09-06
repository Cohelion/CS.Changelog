using Newtonsoft.Json;
using System.IO;

namespace CS.Changelog.Exporters
{
	/// <summary>
	/// A change lo exported for JSON formatting.
	/// </summary>
	/// <seealso cref="CS.Changelog.Exporters.IChangelogExporter" />
	public class JsonChangelogExporter : IChangelogExporter {

		/// <summary>
		/// Exports the specified <paramref name="changes">changeset</paramref> to a MarkDown <paramref name="file" />.
		/// </summary>
		/// <param name="changes">The changes to export.</param>
		/// <param name="file">The file to create of append, depending on <see cref="ExportOptions.Append" />.</param>
		/// <param name="options">The options for exporting.</param>
		public void Export(ChangeSet changes, FileInfo file, ExportOptions options)
		{
			ChangeLog log;

			if (file.Exists && options.Append)
			{
				//Prepend content by reading entire file and then deleting the file

				using (var s = file.OpenText())
				{
					var originalContent = s.ReadToEnd();
					log = JsonConvert.DeserializeObject<ChangeLog>(originalContent);
				}

				if (options.Reverse)
					log.Insert(0, changes);
				else
					log.Add(changes);

				file.Delete();

			}
			else
			{
				log = new ChangeLog
				{
					changes
				};
			}

			using (var w = file.CreateText())
			{
				var s = new JsonSerializer
				{
					Formatting = Formatting.Indented
				};
				s.Serialize(w, log);
			}
		}
	}
}