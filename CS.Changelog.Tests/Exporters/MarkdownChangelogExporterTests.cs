

namespace CS.Changelog.Exporters.Tests
{
	/// <summary>
	/// Tests <see cref="MarkDownChangelogExporter"/>
	/// </summary>
	public class MarkdownChangelogExporterTests : ChangelogExporterTestsBase<MarkDownChangelogExporter>
	{
		/// <summary>Gets the exporter.</summary>
		/// <returns>A <see cref="MarkDownChangelogExporter"/>.</returns>
		protected override MarkDownChangelogExporter GetExporter()
		{
			return new MarkDownChangelogExporter();
		}
	}
}