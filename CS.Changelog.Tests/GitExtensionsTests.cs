using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

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
			var log = GitExtensions.GetHistory();

			Trace.WriteLine(log);
		}
	}
}