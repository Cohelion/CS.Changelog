using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS.Changelog.Exporters.Tests
{
	/// <summary>
	/// Tests <see cref="MarkDownChangelogExporter"/>
	/// </summary>
	[TestClass()]
	public class MarkdownChangelogExporterTests : ChangelogExporterTestsBase<MarkDownChangelogExporter>
	{
		/// <summary>Gets the exporter.</summary>
		/// <returns></returns>
		protected override MarkDownChangelogExporter GetExporter()
		{
			return new MarkDownChangelogExporter();
		}
	}
}