using System.Diagnostics;
using System.Text;

namespace CS.Changelog
{
	/// <summary>
	/// Extensions for obtaining the history from Git.
	/// </summary>
	public static class GitExtensions
	{
		/// <summary>Gets the history as a string that can be parsed using <see cref="Parsing.Parse(string, ParseOptions)"/>.</summary>
		/// <returns>The log message in custom prettyprint format</returns>
		/// <exception cref="System.Exception">An error ocurred whele reading the log.
		/// </exception>
		public static string GetHistory(
			string workingDirectory,
			string pathToGit = "git")
		{
			const string gitGetStartArgument = @"describe --tags --abbrev=0";

			string start;

			var psi = new ProcessStartInfo(pathToGit)
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				WorkingDirectory = workingDirectory,
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden
			};

			psi.Arguments = gitGetStartArgument;
			using (var p = Process.Start(psi))
			{
				p.WaitForExit();

				start = p.StandardOutput.ReadToEnd().Trim();

				if (p.ExitCode != 0)
					throw new System.Exception(p.StandardError.ReadToEnd());
			}

			var gitGetChangelogArgument = $@"log {start}..HEAD --pretty=format:""% H '%cI' % B""";

			var result = new StringBuilder();

			psi.Arguments = gitGetChangelogArgument;
			using (var p = Process.Start(psi))
			{
				while (!p.StandardOutput.EndOfStream || !p.HasExited)
				{
					var line = p.StandardOutput.ReadLine();
					if (!string.IsNullOrWhiteSpace(line))
						result.AppendLine(line);
				}

				p.WaitForExit();

				if (p.ExitCode != 0)
					throw new System.Exception(p.StandardError.ReadToEnd());
			}

			return result.ToString();
		}
	}
}