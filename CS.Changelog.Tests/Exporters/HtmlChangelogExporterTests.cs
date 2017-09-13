using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS.Changelog.Exporters.Tests
{
	/// <summary>
	/// Tests <see cref="HtmlChangelogExporter"/>
	/// </summary>
	[TestClass()]
	public class HtmlChangelogExporterTests : ChangelogExporterTestsBase<HtmlChangelogExporter>
	{
		/// <summary>Gets the exporter.</summary>
		/// <returns></returns>
		protected override HtmlChangelogExporter GetExporter()
		{
			return new HtmlChangelogExporter();
		}
	}
}