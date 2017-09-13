﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS.Changelog.Exporters.Tests
{
	/// <summary>
	/// Tests <see cref="ConsoleChangelogExporter"/>
	/// </summary>
	[TestClass()]
	public class ConsoleChangelogExporterTests : ChangelogExporterTestsBase<ConsoleChangelogExporter>
	{
		/// <summary>Gets the exporter.</summary>
		/// <returns></returns>
		protected override ConsoleChangelogExporter GetExporter()
		{
			return new ConsoleChangelogExporter();
		}
	}
}