using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS.Changelog.Exporters.Tests
{
	/// <summary>
	/// Tests <see cref="XMLChangelogExporter"/>
	/// </summary>
	[TestClass(), Ignore] //Xml exporter not yet implemneted
	public class XMLChangelogExporterTests : ChangelogExporterTestsBase<XMLChangelogExporter>
	{
		/// <summary>Gets the exporter.</summary>
		/// <returns>A <see cref="XMLChangelogExporter"/>.</returns>
		protected override XMLChangelogExporter GetExporter()
		{
			Assert.Inconclusive("Xmlseralization incomplete");

			return new XMLChangelogExporter();

		}
	}
}