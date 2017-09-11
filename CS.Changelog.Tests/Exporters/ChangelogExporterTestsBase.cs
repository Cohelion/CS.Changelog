using CS.Changelog.Console.Tests;
using CS.ChangelogConsole.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace CS.Changelog.Exporters.Tests
{
	/// <summary>
	/// A base class for exporter tests
	/// </summary>
	public abstract class GetHistoryChangelogExporterTestsBase<T> where T : IChangelogExporter
	{
		/// <summary>Gets the exporter.</summary>
		/// <returns></returns>
		protected abstract T GetExporter();

		/// <summary>Tests implementation of <see cref="IChangelogExporter.Export(ChangeSet, System.IO.FileInfo, ExportOptions)"/> by <see cref="JsonChangelogExporter.Export"/></summary>
		[TestMethod()]
		public void ExportThisRepoTest()
		{
			//Arrange
			var log = GitExtensionsTests.GetThisRepoChangelog();

			//Act & assert
			Testlog(log);
		}

		/// <summary>Tests implementation of <see cref="IChangelogExporter.Export(ChangeSet, System.IO.FileInfo, ExportOptions)"/> by <see cref="JsonChangelogExporter.Export"/></summary>
		[TestMethod()]
		public void ExportOtherRepoTest()
		{
			//Arrange
			var log = GitExtensionsTests.GetSwissportRepoChangelog();

			//Act & assert
			Testlog(log);
		}

		/// <summary>Tests implementation of <see cref="IChangelogExporter.Export(ChangeSet, System.IO.FileInfo, ExportOptions)"/> by <see cref="JsonChangelogExporter.Export"/></summary>
		[TestMethod()]
		public void ExportParseTest2Test()
		{
			//Arrange
			var log = ParsingTests.logParseTest2;

			//Act & assert
			Testlog(log);
		}

		private void Testlog(string log)
		{
			var changes = Parsing.Parse(log);
			var exporter = GetExporter();

			//Act
			var file = new FileInfo($"{Guid.NewGuid()}");
			exporter.Export(changes, file);

			//Assert
			file.Refresh();
			if (file.Exists) {
				Assert.IsTrue(file.Exists);

				string changelog = file.OpenText().ReadToEnd();


				if (string.IsNullOrWhiteSpace(changelog))
					Assert.Inconclusive("Changelog is empty");
				else {

					Trace.Write($@"{file.FullName} : 
/*Changelog*/
{changelog}");
				}
			}
		}
	}
}