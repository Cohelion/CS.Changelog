

namespace CS.Changelog.Exporters.Tests
{
	/// <summary>
	/// Tests <see cref="HtmlChangelogExporter"/>
	/// </summary>
	public class HtmlChangelogExporterTests : ChangelogExporterTestsBase<HtmlChangelogExporter>
	{
		/// <summary>Gets the exporter.</summary>
		/// <returns>A <see cref="HtmlChangelogExporter"/>.</returns>
		protected override HtmlChangelogExporter GetExporter()
		{
			return new HtmlChangelogExporter();
		}
	}
}