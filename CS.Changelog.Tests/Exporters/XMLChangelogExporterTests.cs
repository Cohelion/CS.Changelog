using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS.Changelog.Exporters.Tests
{
	/// <summary>
	/// Tests <see cref="XMLChangelogExporter"/>
	/// </summary>
	[TestClass()]
	public class XMLChangelogExporterTests : ChangelogExporterTestsBase<XMLChangelogExporter>
	{
		/// <summary>Gets the exporter.</summary>
		/// <returns></returns>
		protected override XMLChangelogExporter GetExporter()
		{

			Assert.Inconclusive("Xmlseralization incomplete");

			return new XMLChangelogExporter();

		}
	}
}