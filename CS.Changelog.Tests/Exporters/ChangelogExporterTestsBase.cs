using CS.Changelog.Tests;
using System;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace CS.Changelog.Exporters.Tests
{
	/// <summary>A base class for exporter tests</summary>
	/// <typeparam name="T">The type of the change log exporter (implementing <see cref="IChangelogExporter"/>).</typeparam>
	public abstract class ChangelogExporterTestsBase<T> where T : IChangelogExporter
	{
		/// <summary>Gets the exporter.</summary>
		/// <returns>The implementation of <see cref="IChangelogExporter"/> being tested.</returns>
		protected abstract T GetExporter();

		/// <summary>Tests implementation of <see cref="IChangelogExporter.Export(ChangeSet, FileInfo, ExportOptions)"/> by <see cref="JsonChangelogExporter.Export"/></summary>
		[Fact]
		public void ExportThisRepoTest()
		{
			//Arrange
			var log = GitExtensionsTests.GetThisRepoChangelog();

			//Act & assert
			Testlog(log);
		}

		/// <summary>Tests implementation of <see cref="IChangelogExporter.Export(ChangeSet, FileInfo, ExportOptions)"/> by <see cref="JsonChangelogExporter.Export"/></summary>
		[Fact]
		public void ExportOtherRepoTest()
		{
			//Arrange
			var log = GitExtensionsTests.GetSwissportRepoChangelog();

			//Act & assert
			Testlog(log);
		}

		/// <summary>Tests implementation of <see cref="IChangelogExporter.Export(ChangeSet, FileInfo, ExportOptions)"/> by <see cref="JsonChangelogExporter.Export"/></summary>
		[Fact]
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
			var id = Guid.NewGuid();
			changes.Name = $"Funky release name : {id}";
			var exporter = GetExporter();

			//Act
			var file = new FileInfo($"Release_{id}");
			exporter.Export(changes, file);

			//Assert
			file.Refresh();
			if (!exporter.SupportsWritingToFile)
				return;

			Assert.True(file.Exists);

			string changelog;
			using (var r = file.OpenText())
				changelog = r.ReadToEnd();

			Assert.False(string.IsNullOrWhiteSpace(changelog), "Changelog is empty");

			Trace.Write($@"{file.FullName} : 
/*Changelog*/
{changelog}");

			if (!exporter.SupportsDeserializing())
				return;

			//When exporter supports deserializing, and the same chages are written exporter twice, this should result in no changes
			exporter.Export(changes, file);

			Assert.Empty(changes);

		}
	}
}