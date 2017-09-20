using System;
using System.IO;
using System.Reflection;

namespace CS.Changelog.Exporters
{
	/// <summary>
	/// <see cref="IChangelogExporter"/> for exporting to <see cref="OutputFormat.Html"/>.
	/// </summary>
	/// <seealso cref="IChangelogExporter" />
	public class HtmlChangelogExporter : IChangelogExporter
	{

		/// <summary>
		/// Gets a value indicating whether the change log exporter supports deserializing an existing change log, and therefore append intelligently.
		/// </summary>
		/// <value>
		///   <c>false</c>
		/// </value>
		public bool SupportsDeserializing => false;

		/// <summary>
		/// Gets a value indicating whether the change log exporter supports writing to a file.
		/// </summary>
		/// <value>
		///   <c>true</c>.
		/// </value>
		public bool SupportsWritingToFile => true;

		/// <summary>
		/// Exports the specified <paramref name="changes">changeset</paramref> to a Html <paramref name="file" />.
		/// </summary>
		/// <param name="changes">The changes to export.</param>
		/// <param name="file">The file to create or overwrite. <see cref="ExportOptions.Append" /> is ignored.</param>
		/// <param name="options">The options for exporting.</param>
		/// <exception cref="NotImplementedException"></exception>
		public void Export(ChangeSet changes, FileInfo file, ExportOptions options = null)
		{
			var markdown = MarkDownChangelogExporter.WriteChanges(changes, options);

			var changesAsHtml = Markdig.Markdown.ToHtml(markdown.ToString());

			var html = $@"<!DOCTYPE html>
<html>
    <head>
        <title>Changelog for {changes.Name}</title>

		<!-- Custom CSS -->
		<style>
			/* Enable line-breaks withing change log messages */
			li {{
				white-space:pre-line;
			}}
		</style>

		<!-- CSS from markdownpad-github -->
        <style>
{Css}
        </style>
    </head>
    <body>
    {changesAsHtml}
    </body>
</html>";

			using (var w = file.CreateText())
				w.Write(html);
		}

		private static readonly Lazy<string> _Css = new Lazy<string>(GetCss);
		private static string Css
		{
			get
			{
				return _Css.Value;
			}
		}

		/// <summary>
		/// Gets the CSS from the embedded resource.
		/// </summary>
		/// <returns>The CSS as string to use for formatting MarkDown.</returns>
		public static string GetCss()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = $"{assembly.GetName().Name}.Exporters.Html.markdownpad-github.css";

			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			{
				var reader = new StreamReader(stream);
				string result = reader.ReadToEnd();
				return result;
			}
		}
	}
}