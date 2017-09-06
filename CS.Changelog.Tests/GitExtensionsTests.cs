using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CS.Changelog.Console.Tests
{
	/// <summary>
	/// Tests <see cref="GitExtensions"/>
	/// </summary>
	[TestClass()]
	public class GitExtensionsTests
	{
		/// <summary>Tests <see cref="GitExtensions.GetHistory"/></summary>
		[TestMethod()]
		public void GenerateChangeLogTest()
		{
			var solutionPath = Directory
	.GetParent(Assembly.GetExecutingAssembly().Location)
	.Parent.Parent.FullName;

			if (!Directory.Exists(Path.Combine(solutionPath, ".git")))
				Assert.Inconclusive($"Path {solutionPath} is not a git repository");

			var log = GitExtensions.GetHistory(solutionPath);

			Trace.WriteLine(log);
		}

		/// <summary>Tests <see cref="GitExtensions.GetHistory"/></summary>
		[TestMethod()]
		public void GenerateChangeLogTestSwissport()
		{
			const string path = @"d:\Users\Robert\Source\SwissportCargoDCM";

			if (!Directory.Exists(path))
				Assert.Inconclusive($"Path {path} does not exist");

			if (!Directory.Exists(Path.Combine(path, ".git")))
				Assert.Inconclusive($"Path {path} is not a git repository");

			var log = GitExtensions.GetHistory(path);

			Trace.WriteLine(log);
		}
	}
}