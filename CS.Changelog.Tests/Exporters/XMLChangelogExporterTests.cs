using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS.Changelog.Exporters.Tests
{
	/// <summary>
	/// Tests <see cref="XmlChangelogExporter"/>
	/// </summary>
	[TestClass(), Ignore] //Xml exporter not yet implemneted
	public class XMLChangelogExporterTests : ChangelogExporterTestsBase<XmlChangelogExporter>
	{
		/// <summary>Gets the exporter.</summary>
		/// <returns>A <see cref="XmlChangelogExporter"/>.</returns>
		protected override XmlChangelogExporter GetExporter()
		{
			Assert.Inconclusive("Xmlseralization incomplete");

			return new XmlChangelogExporter();

		}
	}
}