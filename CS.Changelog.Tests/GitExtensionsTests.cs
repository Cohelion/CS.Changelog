using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace CS.Changelog.Console.Tests
{
	[TestClass()]
	public class GitExtensionsTests
	{
		[TestMethod()]
		public void GenerateChangeLogTest()
		{
			var log = GitExtensions.GetHistory();

			Trace.WriteLine(log);
		}
	}
}