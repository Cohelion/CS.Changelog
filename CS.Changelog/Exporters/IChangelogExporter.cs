using System.IO;

namespace CS.Changelog.Exporters
{
	/// <summary>
	/// Interface defining methods to be implemented by changelog exporters
	/// </summary>
	public interface IChangelogExporter
	{
		/// <summary>
		/// Gets a value indicating whether the change log exporter supports writing to a file.
		/// </summary>
		/// <value><c>true</c> if the change log exporter supports writing to a file; otherwise, <c>false</c>.</value>
		bool SupportsWritingToFile { get; }

		/// <summary>
		/// Gets a value indicating whether the change log exporter supports deserializing an existing change log, and therefore append intelligently.
		/// </summary>
		/// <value><c>true</c> if the change log exporter supports deserializing; otherwise, <c>false</c>.</value>
		bool SupportsDeserializing { get; }

		/// <summary>Exports the specified <paramref name="changes">changeset</paramref> to a <paramref name="file"/>.</summary>
		/// <param name="changes">The changes to export.</param>
		/// <param name="file">The file to create of append, depending on <see cref="ExportOptions.Append"/>.</param>
		/// <param name="options">The options for exporting.</param>
		void Export(ChangeSet changes, FileInfo file, ExportOptions options = null);
	}
}