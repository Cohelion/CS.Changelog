using Xunit;

namespace CS.Changelog.Exporters.Tests
{
	/// <summary>
	/// Tests <see cref="XmlChangelogExporter"/>
	/// </summary>
	/// <remarks>
	/// Xml serializer not properly implemented yet
	/// </remarks>
	public class XMLChangelogExporterTests : ChangelogExporterTestsBase<XmlChangelogExporter>
	{
		/// <summary>Gets the exporter.</summary>
		/// <returns>A <see cref="XmlChangelogExporter"/>.</returns>
		protected override XmlChangelogExporter GetExporter()
		{
			Skip.If(true, "Xmlseralization incomplete");

			return new XmlChangelogExporter();
		}
	}
}
