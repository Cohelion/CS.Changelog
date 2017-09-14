using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CS.Changelog.Tests
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
								.Parent.Parent.Parent.FullName;

			if (!Directory.Exists(Path.Combine(solutionPath, ".git")))
				Assert.Inconclusive($"Path {solutionPath} is not a git repository");

			var log = GitExtensions.GetHistory(solutionPath);

			Trace.WriteLine(log);
		}

		/// <summary>
		/// The path to this repository
		/// </summary>
		/// <remarks>This was fun while developing locally, anywhere else this is totally useless.</remarks>
		internal const string thisrepopath = @"D:\Users\Robert\Source\CS.Changelog";
		/// <summary>Tests <see cref="GitExtensions.GetHistory"/> using <see cref="thisrepopath"/>.</summary>
		[TestMethod(), TestCategory("ignoreonbuildserver")]
		public void GenerateChangeLogTestCSChangelog()
		{
			var log = GetThisRepoChangelog();

			Trace.WriteLine(log);
		}

		/// <summary>Gets the this repo changelog.</summary>
		/// <returns>The changes for <see cref="thisrepopath"/> obtained using <see cref="GitExtensions.GetHistory"/></returns>
		internal static string GetThisRepoChangelog()
		{
			var path = thisrepopath;
			if (!Directory.Exists(path))
				Assert.Inconclusive($"Path {path} does not exist");

			if (!Directory.Exists(Path.Combine(path, ".git")))
				Assert.Inconclusive($"Path {path} is not a git repository");

			var log = GitExtensions.GetHistory(path);

			return log;
		}

		/// <summary>
		/// The path to the Swissport Cargo DCM repository
		/// </summary>
		/// <remarks>This was fun while developing locally, anywhere else this is totally useless.</remarks>
		internal const string swissportcargodcmrepopath = @"d:\Users\Robert\Source\SwissportCargoDCM";

		/// <summary>Tests <see cref="GitExtensions.GetHistory"/> using <see cref="swissportcargodcmrepopath"/>.</summary>
		[TestMethod(), TestCategory("ignoreonbuildserver")]
		public void GenerateChangeLogTestSwissport()
		{
			var log = GetSwissportRepoChangelog();

			Trace.WriteLine(log);
		}

		/// <summary>Gets the swissport repo changelog.</summary>
		/// <returns>The changes for <see cref="swissportcargodcmrepopath"/> obtained using <see cref="GitExtensions.GetHistory"/></returns>
		internal static string GetSwissportRepoChangelog()
		{
			var path = swissportcargodcmrepopath;

			if (!Directory.Exists(path))
				Assert.Inconclusive($"Path {path} does not exist");

			if (!Directory.Exists(Path.Combine(path, ".git")))
				Assert.Inconclusive($"Path {path} is not a git repository");

			var log = GitExtensions.GetHistory(path);
			return log;
		}
	}
}