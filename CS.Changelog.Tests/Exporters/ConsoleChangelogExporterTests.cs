

namespace CS.Changelog.Exporters.Tests
{
	/// <summary>
	/// Tests <see cref="ConsoleChangelogExporter"/>
	/// </summary>
	public class ConsoleChangelogExporterTests : ChangelogExporterTestsBase<ConsoleChangelogExporter>
	{
		/// <summary>Gets the exporter.</summary>
		/// <returns>A <see cref="ConsoleChangelogExporter"/>.</returns>
		protected override ConsoleChangelogExporter GetExporter()
		{
			return new ConsoleChangelogExporter();
		}
	}
}