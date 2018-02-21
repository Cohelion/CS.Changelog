using System;
using System.IO;

namespace CS.Changelog.Exporters
{
	/// <summary>
	/// Extensions for exporting a changeklog to multiple formats
	/// </summary>
	/// <seealso cref="IChangelogExporter"/>
	public static class ChangelogExportExtensions
	{
		/// <summary>Exports the specified format.</summary>
		/// <param name="changes">The changes to exort.</param>
		/// <param name="format">The format to export to.</param>
		/// <param name="targetFile">The target file (when applicable).</param>
		/// <param name="exportOptions">The export options.</param>
		/// <exception cref="NotImplementedException">When an export format (<paramref name="format"/>) is not supported yet</exception>
		/// <returns>A <see cref="FileInfo"/> referring to the exported file (when applicable).</returns>
		public static FileInfo Export(
			this ChangeSet changes,
			OutputFormat format,
			string targetFile,
			ExportOptions exportOptions)
		{
			IChangelogExporter exporter = null;

			var file = new FileInfo(targetFile);

			if (string.IsNullOrWhiteSpace(file.Extension))
				file = new FileInfo($"{targetFile}.{format.FileExtension()}");

			switch (format)
			{
				case OutputFormat.MarkDown:

					exporter = new MarkDownChangelogExporter();
					break;

				case OutputFormat.Console:

					exporter = new ConsoleChangelogExporter();
					break;

				case OutputFormat.JSON:

					exporter = new JsonChangelogExporter();
					break;

				case OutputFormat.XML:

					exporter = new XmlChangelogExporter();
					break;

				case OutputFormat.Html:

					exporter = new HtmlChangelogExporter();
					break;

				default:
					throw new NotImplementedException($"Export to {format} is not yet implemented");
			}

			file.Directory.AssertExistence();

			exporter.Export(changes, file, exportOptions);

			file.Refresh();

			return file;
		}
	}
}