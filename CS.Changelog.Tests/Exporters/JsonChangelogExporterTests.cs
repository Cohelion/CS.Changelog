using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS.Changelog.Exporters.Tests
{
	/// <summary>
	/// Tests <see cref="JsonChangelogExporter"/>
	/// </summary>
	[TestClass()]
	public class JsonChangelogExporterTests : ChangelogExporterTestsBase<JsonChangelogExporter>
	{
		/// <summary>Gets the exporter.</summary>
		/// <returns></returns>
		protected override JsonChangelogExporter GetExporter()
		{
			return new JsonChangelogExporter();
		}
	}
}