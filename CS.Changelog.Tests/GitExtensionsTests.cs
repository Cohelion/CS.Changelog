
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xunit;

namespace CS.Changelog.Tests
{
	/// <summary>
	/// Tests <see cref="GitExtensions"/>
	/// </summary>
	public class GitExtensionsTests
	{
		/// <summary>Tests <see cref="GitExtensions.GetHistory"/></summary>
		[SkippableFact]
		public void GenerateChangeLogTest()
		{
			var solutionPath = Directory
								.GetParent(Assembly.GetExecutingAssembly().Location)
								.Parent.Parent.Parent.FullName;

			Skip.IfNot(
				Directory.Exists(Path.Combine(solutionPath, ".git")),
				$"Path {solutionPath} is not a git repository");

			var log = GitExtensions.GetHistory(solutionPath);

			Trace.WriteLine(log);
		}

		/// <summary>
		/// The path to this repository
		/// </summary>
		/// <remarks>This was fun while developing locally, anywhere else this is totally useless.</remarks>
		internal const string thisrepopath = @"C:\Users\robert\Documents\Source\Cohelion\cs.changelog";
		/// <summary>Tests <see cref="GitExtensions.GetHistory"/> using <see cref="thisrepopath"/>.</summary>
		[Fact(Skip = "ignoreonbuildserver")]
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
			Skip.IfNot(Directory.Exists(path)
				, $"Path {path} does not exist");

			Skip.IfNot(Directory.Exists(Path.Combine(path, ".git"))
				, $"Path {path} is not a git repository");

			var log = GitExtensions.GetHistory(path);

			return log;
		}

		/// <summary>
		/// The path to the Swissport Cargo DCM repository
		/// </summary>
		/// <remarks>This was fun while developing locally, anywhere else this is totally useless.</remarks>
		internal const string swissportcargodcmrepopath = @"C:\Users\robert\Documents\Source\Cohelion\swissport-dcm";

		/// <summary>Tests <see cref="GitExtensions.GetHistory"/> using <see cref="swissportcargodcmrepopath"/>.</summary>
		[SkippableFact(Skip = "ignoreonbuildserver")]
		public void GenerateChangeLogTestSwissport()
		{
			var log = GetSwissportRepoChangelog();

			Trace.WriteLine(
				string.IsNullOrEmpty(log)
				? "empty log"
				: log);
		}

		/// <summary>Gets the swissport repo changelog.</summary>
		/// <returns>The changes for <see cref="swissportcargodcmrepopath"/> obtained using <see cref="GitExtensions.GetHistory"/></returns>
		internal static string GetSwissportRepoChangelog()
		{
			var path = swissportcargodcmrepopath;

			Skip.IfNot(Directory.Exists(path)
				, $"Path {path} does not exist");

			Skip.IfNot(Directory.Exists(Path.Combine(path, ".git"))
				, $"Path {path} is not a git repository");

			var log = GitExtensions.GetHistory(path);
			return log;
		}
	}
}
