using System.IO;

namespace CS.Changelog.Exporters
{
	/// <summary>
	/// Interface defining methods to be implemented by changelog exporters
	/// </summary>
	public interface IChangelogExporter
	{
		/// <summary>Exports the specified <paramref name="changes">changeset</paramref> to a <paramref name="file"/>.</summary>
		/// <param name="changes">The changes to export.</param>
		/// <param name="file">The file to create of append, depending on <see cref="ExportOptions.Append"/>.</param>
		/// <param name="options">The options for exporting.</param>
		void Export(ChangeSet changes, FileInfo file = null, ExportOptions options = null);
	}
}